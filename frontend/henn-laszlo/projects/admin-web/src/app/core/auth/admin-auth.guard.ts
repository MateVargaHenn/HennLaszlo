import { inject } from '@angular/core';
import {
  CanActivateFn,
  Router,
} from '@angular/router';
import {
  catchError,
  map,
  of,
} from 'rxjs';
import {
  AdminAuthService,
} from './admin-auth.service';

export const adminAuthGuard:
  CanActivateFn = (_route, state) => {
    const authService =
      inject(AdminAuthService);

    const router = inject(Router);

    if (authService.isAuthenticated()) {
      return true;
    }

    const loginUrl = router.createUrlTree(
      ['/login'],
      {
        queryParams: {
          returnUrl: state.url,
        },
      },
    );

    return authService.restoreSession().pipe(
      map((isAuthenticated) =>
        isAuthenticated
          ? true
          : loginUrl,
      ),
      catchError(() => of(loginUrl)),
    );
  };