import {
  CanDeactivateFn,
} from '@angular/router';

export interface PendingChangesAware {
  hasUnsavedChanges(): boolean;
}

export const pendingChangesGuard:
  CanDeactivateFn<PendingChangesAware> =
    (component) => {
      if (!component.hasUnsavedChanges()) {
        return true;
      }

      return window.confirm(
        'Nem mentett módosításaid vannak. ' +
        'Biztosan elhagyod az oldalt?',
      );
    };