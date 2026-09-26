import {
  inject,
  Injectable,
} from '@angular/core';
import {
  HttpClient,
  HttpErrorResponse,
} from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from
  '../../../../environments/environment';

export interface ArgusSource {
  readonly title: string;
  readonly path: string;
}

export interface AskArgusResponse {
  readonly answer: string;
  readonly confidence: number;
  readonly isFallback: boolean;
  readonly source: ArgusSource | null;
}

@Injectable({
  providedIn: 'root',
})
export class ArgusApi {
  private readonly http = inject(HttpClient);

  private readonly endpoint =
    `${environment.apiBaseUrl.replace(/\/+$/, '')}` +
    '/api/chatbot/ask';

  ask(
    question: string,
  ): Observable<AskArgusResponse> {
    return this.http.post<AskArgusResponse>(
      this.endpoint,
      { question },
    );
  }
}

export function isRateLimitError(
  error: unknown,
): boolean {
  return (
    error instanceof HttpErrorResponse &&
    error.status === 429
  );
}