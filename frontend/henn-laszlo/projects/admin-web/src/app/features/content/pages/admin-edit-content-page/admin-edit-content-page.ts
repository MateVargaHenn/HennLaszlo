import {
  ChangeDetectionStrategy,
  Component,
  computed,
  effect,
  inject,
} from '@angular/core';
import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import {
  ActivatedRoute,
  Router,
  RouterLink,
} from '@angular/router';
import {
  AdminContentPageDetailsStore,
  AdminContentPagesStore,
  ContentPageKey,
  ContentRevisionsStore,
} from 'content-data-access';

import {
  EditorComponent,
} from '@tinymce/tinymce-angular';

import { ContentFileUpload } from '../../services/content-file-upload';

import {
  environment,
} from '../../../../../environments/environment';
import { ContentEditorBase } from '../../../../core/editor/content-editor-base';

import {
  ContentRevisionHistory,
} from '../../components/content-revision-history/content-revision-history';

import {
  HttpErrorResponse,
} from '@angular/common/http';

@Component({
  selector: 'app-admin-edit-content-page',
  imports: [
    ReactiveFormsModule,
    RouterLink,
    EditorComponent,
    ContentRevisionHistory
  ],
  providers: [
    ContentFileUpload,
    AdminContentPageDetailsStore,
    AdminContentPagesStore,
    ContentRevisionsStore,
  ],
  templateUrl: './admin-edit-content-page.html',
  styleUrl: './admin-edit-content-page.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AdminEditContentPage
  extends ContentEditorBase {
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);

  private readonly listStore =
    inject(AdminContentPagesStore);

  protected readonly store =
    inject(AdminContentPageDetailsStore);

  protected readonly hasVersionConflict =
  computed(() => {
    const error = this.store.saveError();

    return (
      error instanceof HttpErrorResponse &&
      error.status === 409
    );
  });

  protected override readonly form = new FormGroup({
    titleHu: new FormControl('', {
      nonNullable: true,
      validators: [
        Validators.required,
        Validators.maxLength(250),
      ],
    }),

    titleEn: new FormControl<string | null>(null, {
      validators: [
        Validators.maxLength(250),
      ],
    }),

    contentHu: new FormControl('', {
      nonNullable: true,
      validators: [
        Validators.required,
      ],
    }),

    contentEn:
      new FormControl<string | null>('', {
        nonNullable: true,
      }),
  });

  protected readonly tinyMceApiKey =
    environment.tinyMceApiKey;

  protected readonly fileUploads = inject(ContentFileUpload);

  protected readonly editorConfig =
    this.fileUploads.createEditorConfig();

  protected readonly revisionsStore =
    inject(ContentRevisionsStore);

  private loadedRevisionTargetId:
    string | null = null;

  protected loadedVersion: string | undefined;

  constructor() {
    super();
    const key =
      this.route.snapshot.paramMap.get('key');

    if (!this.isContentPageKey(key)) {
      void this.router.navigate(['/content']);
      return;
    }

    this.store.load(key);

    effect(() => {
      const contentPage = this.store.contentPage();

    if (!contentPage || this.form.dirty) {
      return;
    }

    this.loadedVersion = contentPage.version;

      if (
        this.loadedRevisionTargetId !==
        contentPage.id
      ) {
        this.loadedRevisionTargetId =
          contentPage.id;

        this.revisionsStore.load(
          'content-pages',
          contentPage.id,
        );
      }

      if (contentPage.contentEn?.trim()) {
        this.isEnglishContentEditorVisible.set(
          true,
        );
      }

      this.form.reset(
        {
          titleHu: contentPage.titleHu,
          titleEn: contentPage.titleEn,
          contentHu: contentPage.contentHu,
          contentEn: contentPage.contentEn ?? '',
        },
        {
          emitEvent: false,
        },
      );
    });
  }

  public override hasUnsavedChanges(): boolean {
    return super.hasUnsavedChanges() || this.fileUploads.isUploading();
  }

  protected async save(): Promise<void> {
    this.form.markAllAsTouched();

    if (this.form.invalid || this.fileUploads.isUploading() || this.store.isSaving()) {
      return;
    }

    const expectedVersion = this.loadedVersion;

    if (!expectedVersion) {
      window.alert(
        'A tartalmi oldal verziója nem érhető el. ' +
        'Másold ki a módosításaidat, ' +
        'majd töltsd újra az oldalt.',
      );

      return;
    }

    const value = this.form.getRawValue();

    if (!this.fileUploads.canSave([
      value.contentHu,
      value.contentEn ?? '',
    ])) {
      return;
    }

    try {
      await this.store.save({
        titleHu: value.titleHu.trim(),
        titleEn: this.normalizeOptionalText(
          value.titleEn,
        ),
        contentHu: value.contentHu.trim(),
        contentEn: this.normalizeOptionalText(
          value.contentEn,
        ),
        expectedVersion,
      });

      this.listStore.reload();

      this.form.markAsPristine();

      await this.router.navigate(['/content']);
    }
    catch {
      // A store eltárolja a megjelenítendő hibát.
    }
  }

  protected handleRevisionRestored(): void {
    this.form.markAsPristine();

    this.store.reload();
    this.listStore.reload();
  }

  private normalizeOptionalText(
    value: string | null,
  ): string | null {
    const normalized = value?.trim();

    return normalized
      ? normalized
      : null;
  }

  private isContentPageKey(
    value: string | null,
  ): value is ContentPageKey {
    return value === 'about' ||
      value === 'contact' ||
      value === 'exhibitions' ||
      value === 'memberships-and-awards' ||
      value === 'writings';
  }
}
