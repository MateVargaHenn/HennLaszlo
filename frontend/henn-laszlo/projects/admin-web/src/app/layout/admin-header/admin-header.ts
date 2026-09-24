import { Component, DestroyRef, inject, signal } from '@angular/core';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { AdminAuthService } from '../../core/auth/admin-auth.service';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { finalize } from 'rxjs/internal/operators/finalize';

@Component({
  imports: [RouterLink, RouterLinkActive],
  selector: 'app-admin-header',
  styleUrl: './admin-header.css',
  templateUrl: './admin-header.html',
})
export class AdminHeader {
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
