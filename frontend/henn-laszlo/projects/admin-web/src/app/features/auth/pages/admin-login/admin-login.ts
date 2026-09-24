import {
  ChangeDetectionStrategy,
  Component,
  DestroyRef,
  inject,
  signal,
} from '@angular/core';
import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { HttpErrorResponse } from '@angular/common/http';
import {
  ActivatedRoute,
  Router,
} from '@angular/router';
import { takeUntilDestroyed } from
  '@angular/core/rxjs-interop';
import { finalize } from 'rxjs';
import {
  AdminAuthService,
} from '../../../../core/auth/admin-auth.service';

@Component({
  selector: 'app-admin-login',
  imports: [ReactiveFormsModule],
  templateUrl: './admin-login.html',
  styleUrl: './admin-login.css',
  changeDetection:
    ChangeDetectionStrategy.OnPush,
})
export class AdminLogin {
  private readonly authService =
    inject(AdminAuthService);

  private readonly router = inject(Router);
  private readonly route = inject(ActivatedRoute);
  private readonly destroyRef =
    inject(DestroyRef);

  protected readonly isSubmitting =
    signal(false);

  protected readonly loginError =
    signal<string | null>(null);

  protected readonly form = new FormGroup({
    username: new FormControl('', {
      nonNullable: true,
      validators: [Validators.required],
    }),
    password: new FormControl('', {
      nonNullable: true,
      validators: [Validators.required],
    }),
  });

  protected submit(): void {
    if (
      this.form.invalid ||
      this.isSubmitting()
    ) {
      this.form.markAllAsTouched();
      return;
    }

    this.isSubmitting.set(true);
    this.loginError.set(null);

    this.authService
      .login(this.form.getRawValue())
      .pipe(
        takeUntilDestroyed(this.destroyRef),
        finalize(() =>
          this.isSubmitting.set(false),
        ),
      )
      .subscribe({
        next: () => {
          void this.router.navigateByUrl(
            this.getReturnUrl(),
          );
        },
        error: (error: unknown) => {
          this.loginError.set(
            this.getErrorMessage(error),
          );
        },
      });
  }

  private getReturnUrl(): string {
    const returnUrl =
      this.route.snapshot.queryParamMap.get(
        'returnUrl',
      );

    if (
      returnUrl?.startsWith('/') &&
      !returnUrl.startsWith('//')
    ) {
      return returnUrl;
    }

    return '/artworks';
  }

  private getErrorMessage(
    error: unknown,
  ): string {
    if (error instanceof HttpErrorResponse) {
      if (error.status === 401) {
        return 'Hibás felhasználónév vagy jelszó.';
      }

      if (error.status === 429) {
        return 'Túl sok belépési próbálkozás. Próbáld újra egy perc múlva.';
      }
    }

    return 'A belépés nem sikerült.';
  }
}