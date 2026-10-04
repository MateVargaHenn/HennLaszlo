import { HttpErrorResponse } from '@angular/common/http';
import {
  computed,
  ChangeDetectionStrategy,
  Component,
  inject,
  input,
  output,
  signal,
} from '@angular/core';
import {
  ContentRevisionsStore,
} from 'content-data-access';

@Component({
  selector: 'app-content-revision-history',
  templateUrl:
    './content-revision-history.html',
  changeDetection:
    ChangeDetectionStrategy.OnPush,
})
export class ContentRevisionHistory {
  readonly hasUnsavedChanges =
    input(false);

  readonly expectedVersion =
    input<string | undefined>(undefined);

  readonly restored = output<void>();

  protected readonly store =
    inject(ContentRevisionsStore);

  protected readonly hasRestoreConflict =
    computed(() => {
      const error = this.store.restoreError();

      return error instanceof HttpErrorResponse &&
        error.status === 409;
    });

  protected readonly isRestoreConfirmationOpen =
      signal(false);

  protected openRevision(
    revisionId: string,
  ): void {
    this.isRestoreConfirmationOpen.set(
      false,
    );

    this.store.selectRevision(revisionId);
  }

  protected closeRevision(): void {
    if (this.store.isRestoring()) {
      return;
    }

    this.isRestoreConfirmationOpen.set(
      false,
    );

    this.store.closeRevision();
  }

  protected requestRestore(): void {
    if (
      !this.store.selectedRevision() ||
      this.store.isRestoring()
    ) {
      return;
    }

    if (this.hasUnsavedChanges()) {
      window.alert(
        'A verzió visszaállítása előtt mentsd el ' +
        'vagy vesd el a jelenlegi módosításokat.',
      );

      return;
    }

    if (!this.expectedVersion()?.trim()) {
      window.alert(
        'A tartalom verziója nem érhető el. ' +
        'Töltsd újra az oldalt a visszaállítás előtt.',
      );
      return;
    }

    this.isRestoreConfirmationOpen.set(
      true,
    );
  }

  protected cancelRestore(): void {
    if (this.store.isRestoring()) {
      return;
    }

    this.isRestoreConfirmationOpen.set(
      false,
    );
  }

  protected async confirmRestore():
    Promise<void> {
    const revision =
      this.store.selectedRevision();

    if (
      !revision ||
      this.store.isRestoring() ||
      this.hasRestoreConflict()
    ) {
      return;
    }

    // A megerősítés megnyitása óta is változhatott a form.
    if (this.hasUnsavedChanges()) {
      window.alert(
        'A verzió visszaállítása előtt mentsd el ' +
        'vagy vesd el a jelenlegi módosításokat.',
      );
      return;
    }

    const expectedVersion = this.expectedVersion();
    if (!expectedVersion?.trim()) {
      window.alert(
        'A tartalom verziója nem érhető el. ' +
        'Töltsd újra az oldalt a visszaállítás előtt.',
      );
      return;
    }

    try {
      await this.store.restore(
        revision.id,
        expectedVersion,
      );

      this.isRestoreConfirmationOpen.set(
        false,
      );

      this.restored.emit();
    }
    catch {
      // A store eltárolja a hibát.
    }
  }

  protected formatDate(
    value: string,
  ): string {
    return new Date(value)
      .toLocaleString('hu-HU');
  }
}
