import {
  ChangeDetectionStrategy,
  Component,
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
  AdminArticleEditorStore,
  AdminArticlesStore,
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

@Component({
  selector: 'app-article-editor',
  imports: [
    ReactiveFormsModule,
    RouterLink,
    EditorComponent,
  ],
  templateUrl: './article-editor.html',
  styleUrl: './article-editor.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ArticleEditor {
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);

  private readonly listStore =
    inject(AdminArticlesStore);

  protected readonly store =
    inject(AdminArticleEditorStore);

  private readonly articleId =
    this.route.snapshot.paramMap.get('articleId');

  protected readonly isEditMode =
    this.articleId !== null;

  protected readonly form = new FormGroup({
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

    displayOrder: new FormControl(0, {
      nonNullable: true,
      validators: [
        Validators.required,
        Validators.min(0),
      ],
    }),
  });

  protected readonly tinyMceApiKey =
    environment.tinyMceApiKey;

  protected readonly editorConfig =
    richTextEditorConfig;

  constructor() {
    if (this.articleId) {
      this.form.controls.slug.disable({
        emitEvent: false,
      });

      this.store.load(this.articleId);
    }
    else {
      this.store.clear();
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

      this.form.reset(
        {
          slug: article.slug,
          titleHu: article.titleHu,
          titleEn: article.titleEn ?? '',
          summaryHu: article.summaryHu ?? '',
          summaryEn: article.summaryEn ?? '',
          contentHu: article.contentHu,
          contentEn: article.contentEn ?? '',
          displayOrder: article.displayOrder,
        },
        {
          emitEvent: false,
        },
      );
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
      ),
      displayOrder: value.displayOrder,
    };

    try {
      if (this.articleId) {
        await this.store.update(
          this.articleId,
          commonRequest,
        );

        this.listStore.reload();

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

  protected async deleteArticle(): Promise<void> {
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

    const confirmed = window.confirm(
      `Biztosan törlöd ezt az írást?\n\n${article.titleHu}`,
    );

    if (!confirmed) {
      return;
    }

    try {
      await this.store.delete(this.articleId);

      this.listStore.reload();

      await this.router.navigate([
        '/content/articles',
      ]);
    }
    catch {
      // A store eltárolja a megjelenítendő hibát.
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