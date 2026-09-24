import {
  computed,
  inject,
  Injectable,
  signal,
} from '@angular/core';
import {
  HttpClient,
  HttpErrorResponse,
} from '@angular/common/http';
import {
  catchError,
  map,
  Observable,
  of,
  switchMap,
  tap,
  throwError,
} from 'rxjs';

export interface AdminLoginRequest {
  readonly username: string;
  readonly password: string;
}

export interface AdminSession {
  readonly username: string;
}

@Injectable({
  providedIn: 'root',
})
export class AdminAuthService {
  private readonly http = inject(HttpClient);

  private readonly apiUrl =
    '/api/admin/auth';

  private readonly sessionState =
    signal<AdminSession | null>(null);

  readonly session =
    this.sessionState.asReadonly();

  readonly isAuthenticated = computed(
    () => this.sessionState() !== null,
  );

  login(
    request: AdminLoginRequest,
  ): Observable<void> {
    return this.issueCsrfToken().pipe(
      switchMap(() =>
        this.http.post<void>(
          `${this.apiUrl}/login`,
          request,
        ),
      ),
      switchMap(() => this.issueCsrfToken()),
      tap(() =>
        this.sessionState.set({
          username: request.username,
        }),
      ),
    );
  }

  restoreSession(): Observable<boolean> {
    return this.http
      .get<AdminSession>(
        `${this.apiUrl}/session`,
      )
      .pipe(
        tap((session) =>
          this.sessionState.set(session),
        ),
        switchMap(() =>
          this.issueCsrfToken(),
        ),
        map(() => true),
        catchError((error: unknown) => {
          this.sessionState.set(null);

          if (
            error instanceof HttpErrorResponse &&
            (error.status === 401 ||
              error.status === 403)
          ) {
            return of(false);
          }

          return throwError(() => error);
        }),
      );
  }

  logout(): Observable<void> {
    return this.issueCsrfToken().pipe(
      switchMap(() =>
        this.http.post<void>(
          `${this.apiUrl}/logout`,
          {},
        ),
      ),
      tap(() =>
        this.sessionState.set(null),
      ),
    );
  }

  private issueCsrfToken(): Observable<void> {
    return this.http.get<void>(
      `${this.apiUrl}/csrf`,
    );
  }
}