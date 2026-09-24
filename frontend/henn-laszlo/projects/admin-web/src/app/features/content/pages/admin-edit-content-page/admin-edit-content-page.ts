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
  AdminContentPageDetailsStore,
  AdminContentPagesStore,
  ContentPageKey,
} from 'content-data-access';

import {
  EditorComponent,
} from '@tinymce/tinymce-angular';

import {
  richTextEditorConfig,
} from '../../config/rich-text-editor-config';

import {
  environment,
} from '../../../../../environments/environment';

@Component({
  selector: 'app-admin-edit-content-page',
  imports: [
    ReactiveFormsModule,
    RouterLink,
    EditorComponent,
  ],
  templateUrl: './admin-edit-content-page.html',
  styleUrl: './admin-edit-content-page.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AdminEditContentPage {
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);

  private readonly listStore =
    inject(AdminContentPagesStore);

  protected readonly store =
    inject(AdminContentPageDetailsStore);

  protected readonly form = new FormGroup({
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

  protected readonly editorConfig = 
    richTextEditorConfig;

  constructor() {
    const key =
      this.route.snapshot.paramMap.get('key');

    if (!this.isContentPageKey(key)) {
      void this.router.navigate(['/content']);
      return;
    }

    this.store.load(key);

    effect(() => {
      const contentPage = this.store.contentPage();

      if (!contentPage) {
        return;
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

  protected async save(): Promise<void> {
    this.form.markAllAsTouched();

    if (this.form.invalid || this.store.isSaving()) {
      return;
    }

    const value = this.form.getRawValue();

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
      });

      this.listStore.reload();

      await this.router.navigate(['/content']);
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

  private isContentPageKey(
    value: string | null,
  ): value is ContentPageKey {
    return value === 'about' ||
      value === 'exhibitions' ||
      value === 'memberships-and-awards' ||
      value === 'writings';
  }
}