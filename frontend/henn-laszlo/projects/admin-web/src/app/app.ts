import {
  ChangeDetectionStrategy,
  Component,
  DestroyRef,
  inject,
  signal,
} from '@angular/core';
import { takeUntilDestroyed } from
  '@angular/core/rxjs-interop';
import {
  Router,
  RouterOutlet,
} from '@angular/router';
import { finalize } from 'rxjs';
import {
  AdminAuthService,
} from './core/auth/admin-auth.service';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet],
  templateUrl: './app.html',
  styleUrl: './app.css',
  changeDetection:
    ChangeDetectionStrategy.OnPush,
})
export class App {
  private readonly router = inject(Router);
  private readonly destroyRef =
    inject(DestroyRef);

  protected readonly authService =
    inject(AdminAuthService);

  protected readonly isLoggingOut =
    signal(false);

  protected readonly logoutError =
    signal<string | null>(null);

  protected logout(): void {
    if (this.isLoggingOut()) {
      return;
    }

    this.isLoggingOut.set(true);
    this.logoutError.set(null);

    this.authService
      .logout()
      .pipe(
        takeUntilDestroyed(this.destroyRef),
        finalize(() =>
          this.isLoggingOut.set(false),
        ),
      )
      .subscribe({
        next: () => {
          void this.router.navigate(['/login']);
        },
        error: () => {
          this.logoutError.set(
            'A kijelentkezés nem sikerült.',
          );
        },
      });
  }
}