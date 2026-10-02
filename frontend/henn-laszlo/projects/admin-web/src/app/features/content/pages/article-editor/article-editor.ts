import {
  ChangeDetectionStrategy,
  Component,
  effect,
  inject,
  signal,
  DestroyRef
} from '@angular/core';
import {
  takeUntilDestroyed,
} from '@angular/core/rxjs-interop';
import {
  debounceTime,
} from 'rxjs';

import {
  LocalDraftStorage,
} from '../../../../core/drafts/local-draft-storage';
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
  AdminArticleEditorStore,
  AdminArticlesStore,
  ContentRevisionsStore,
} from 'content-data-access';
import type {
  CreateArticleRequest,
  UpdateArticleRequest,
} from 'content-data-access';
import {
  EditorComponent,
} from '@tinymce/tinymce-angular';

import {
  environment,
} from '../../../../../environments/environment';
import {
  richTextEditorConfig,
} from '../../config/rich-text-editor-config';
import {
  ContentEditorBase,
} from '../../../../core/editor/content-editor-base';
import {
  ContentRevisionHistory,
} from '../../components/content-revision-history/content-revision-history';

interface ArticleDraftValue {
  readonly slug: string;
  readonly titleHu: string;
  readonly titleEn: string;
  readonly summaryHu: string;
  readonly summaryEn: string;
  readonly contentHu: string;
  readonly contentEn: string;
}

@Component({
  selector: 'app-article-editor',
  imports: [
    ReactiveFormsModule,
    RouterLink,
    EditorComponent,
    ContentRevisionHistory,
  ],
  providers: [
    ContentRevisionsStore,
  ],
  templateUrl: './article-editor.html',
  styleUrl: './article-editor.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ArticleEditor 
  extends ContentEditorBase {
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);

  private readonly listStore =
    inject(AdminArticlesStore);

  protected readonly store =
    inject(AdminArticleEditorStore);

  private readonly articleId =
    this.route.snapshot.paramMap.get('articleId');

  private readonly draftKey =
    this.articleId
      ? `article:${this.articleId}`
      : 'article:new';

  private hasCheckedDraft = false;

  protected readonly isEditMode =
    this.articleId !== null;

  protected readonly isDeleteConfirmationOpen =
    signal(false);

  protected readonly revisionsStore =
    inject(ContentRevisionsStore);

  protected override readonly form = new FormGroup({
    slug: new FormControl('', {
      nonNullable: true,
      validators: [
        Validators.required,
        Validators.maxLength(200),
        Validators.pattern(
          /^[a-z0-9]+(?:-[a-z0-9]+)*$/,
        ),
      ],
    }),

    titleHu: new FormControl('', {
      nonNullable: true,
      validators: [
        Validators.required,
        Validators.maxLength(250),
      ],
    }),

    titleEn: new FormControl('', {
      nonNullable: true,
      validators: [
        Validators.maxLength(250),
      ],
    }),

    summaryHu: new FormControl('', {
      nonNullable: true,
      validators: [
        Validators.maxLength(1000),
      ],
    }),

    summaryEn: new FormControl('', {
      nonNullable: true,
      validators: [
        Validators.maxLength(1000),
      ],
    }),

    contentHu: new FormControl('', {
      nonNullable: true,
      validators: [
        Validators.required,
      ],
    }),

    contentEn: new FormControl('', {
      nonNullable: true,
    }),
  });

  protected readonly tinyMceApiKey =
    environment.tinyMceApiKey;

  protected readonly editorConfig =
    richTextEditorConfig;

  private readonly destroyRef =
    inject(DestroyRef);

  private readonly draftStorage =
    inject(LocalDraftStorage);

  constructor() {
    super();
    this.form.valueChanges
      .pipe(
        debounceTime(1000),
        takeUntilDestroyed(this.destroyRef),
      )
      .subscribe(() => {
        if (!this.form.dirty) {
          return;
        }

        this.draftStorage.save<ArticleDraftValue>(
          this.draftKey,
          this.form.getRawValue(),
        );
      });

      if (this.articleId) {
        this.form.controls.slug.disable({
          emitEvent: false,
        });

        this.store.load(this.articleId);

        this.revisionsStore.load(
          'articles',
          this.articleId,
        );
      }
      else {
        this.store.clear();
        this.revisionsStore.clear();
        this.restoreDraft();
      }

    effect(() => {
      const article = this.store.article();

      if (
        !this.articleId ||
        !article ||
        article.id !== this.articleId
      ) {
        return;
      }

      if (article.contentEn?.trim()) {
        this.isEnglishContentEditorVisible.set(
          true,
        );
      }

      this.form.reset(
        {
          slug: article.slug,
          titleHu: article.titleHu,
          titleEn: article.titleEn ?? '',
          summaryHu: article.summaryHu ?? '',
          summaryEn: article.summaryEn ?? '',
          contentHu: article.contentHu,
          contentEn: article.contentEn ?? ''
        },
        {
          emitEvent: false,
        },
      );

      this.restoreDraft();
    });
  }

  protected async save(): Promise<void> {
    this.form.markAllAsTouched();

    if (
      this.form.invalid ||
      this.store.isSaving()
    ) {
      return;
    }

    const value = this.form.getRawValue();

    const commonRequest: UpdateArticleRequest = {
      titleHu: value.titleHu.trim(),
      titleEn: this.normalizeOptionalText(
        value.titleEn,
      ),
      summaryHu: this.normalizeOptionalText(
        value.summaryHu,
      ),
      summaryEn: this.normalizeOptionalText(
        value.summaryEn,
      ),
      contentHu: value.contentHu.trim(),
      contentEn: this.normalizeOptionalText(
        value.contentEn,
      )
    };

    try {
      if (this.articleId) {
        await this.store.update(
          this.articleId,
          commonRequest,
        );

        this.listStore.reload();

        this.form.markAsPristine();

        this.draftStorage.remove(
          this.draftKey,
        );

        await this.router.navigate([
          '/content/articles',
        ]);

        return;
      }

      const createRequest: CreateArticleRequest = {
        slug: value.slug.trim(),
        ...commonRequest,
      };

      const createdArticleId =
        await this.store.create(createRequest);

      this.listStore.reload();

      this.form.markAsPristine();

      this.draftStorage.remove(
        this.draftKey,
      );

      await this.router.navigate([
        '/content/articles',
        createdArticleId,
        'edit',
      ]);
    }
    catch {
      // A store eltárolja a megjelenítendő hibát.
    }
  }

  protected async publish(): Promise<void> {
    if (
      !this.articleId ||
      this.store.isSaving()
    ) {
      return;
    }

    if (this.form.dirty) {
      window.alert(
        'A publikálás előtt mentsd el a módosításokat.',
      );

      return;
    }

    try {
      await this.store.publish(this.articleId);
      this.listStore.reload();
    }
    catch {
      // A store eltárolja a megjelenítendő hibát.
    }
  }

  protected async unpublish(): Promise<void> {
    if (
      !this.articleId ||
      this.store.isSaving()
    ) {
      return;
    }

    try {
      await this.store.unpublish(this.articleId);
      this.listStore.reload();
    }
    catch {
      // A store eltárolja a megjelenítendő hibát.
    }
  }

  protected handleRevisionRestored(): void {
  if (!this.articleId) {
    return;
  }

  this.form.markAsPristine();

  this.draftStorage.remove(
    this.draftKey,
  );

  this.store.load(this.articleId);
  this.listStore.reload();
}

  protected requestDeleteArticle(): void {
    if (
      !this.articleId ||
      this.store.isSaving()
    ) {
      return;
    }

    const article = this.store.article();

    if (!article || article.isPublished) {
      return;
    }

    this.isDeleteConfirmationOpen.set(true);
  }

  protected cancelDeleteArticle(): void {
    if (this.store.isSaving()) {
      return;
    }

    this.isDeleteConfirmationOpen.set(false);
  }

  protected async confirmDeleteArticle():
    Promise<void> {
    if (
      !this.articleId ||
      this.store.isSaving()
    ) {
      return;
    }

    const article = this.store.article();

    if (!article || article.isPublished) {
      return;
    }

    try {
      await this.store.delete(
        this.articleId,
      );

      this.isDeleteConfirmationOpen.set(
        false,
      );

      this.listStore.reload();

      this.form.markAsPristine();

      this.draftStorage.remove(
        this.draftKey,
      );

      await this.router.navigate([
        '/content/articles',
      ]);
    }
    catch {
      // A store eltárolja a hibát.
    }
  }

  private restoreDraft(): void {
  if (this.hasCheckedDraft) {
    return;
  }

  this.hasCheckedDraft = true;

  const draft =
    this.draftStorage.load<ArticleDraftValue>(
      this.draftKey,
    );

  if (!draft) {
    return;
  }

  if (
    JSON.stringify(draft.value) ===
    JSON.stringify(this.form.getRawValue())
  ) {
    this.draftStorage.remove(
      this.draftKey,
    );

    return;
  }

  const savedAt =
    new Date(
      draft.savedAtUtc,
    ).toLocaleString('hu-HU');

  const shouldRestore = window.confirm(
    'Találtunk egy nem mentett piszkozatot ' +
    `(${savedAt}). Visszaállítod?`,
  );

  if (!shouldRestore) {
    this.draftStorage.remove(
      this.draftKey,
    );

    return;
  }

  this.form.patchValue(
      draft.value,
      {
        emitEvent: false,
      },
    );

    this.form.markAsDirty();

    if (draft.value.contentEn.trim()) {
      this.isEnglishContentEditorVisible.set(
        true,
      );
    }
  }

  private normalizeOptionalText(
    value: string | null,
  ): string | null {
    const normalized = value?.trim();

    return normalized
      ? normalized
      : null;
  }
}