import {
  Directive,
  HostListener,
  signal,
} from '@angular/core';
import {
  FormGroup,
} from '@angular/forms';

import {
  PendingChangesAware,
} from './pending-changes.guard';

@Directive()
export abstract class ContentEditorBase
  implements PendingChangesAware {
  protected abstract readonly form:
    FormGroup;

  protected readonly isEnglishContentEditorVisible = 
    signal(false);

  public hasUnsavedChanges(): boolean {
    return this.form.dirty;
  }

  @HostListener(
    'window:beforeunload',
    ['$event'],
  )
  protected handleBeforeUnload(
    event: BeforeUnloadEvent,
  ): void {
    if (!this.hasUnsavedChanges()) {
      return;
    }

    event.preventDefault();
    event.returnValue =
      'Nem mentett módosítások vannak.';
  }

  protected enableEnglishContentEditor():
    void {
    this.isEnglishContentEditorVisible.set(
      true,
    );
  }
}