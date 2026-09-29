import {
  ChangeDetectionStrategy,
  Component,
  effect,
  inject,
} from '@angular/core';
import { DatePipe } from '@angular/common';
import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import {
  AdminWebsiteSettingsStore,
} from 'content-data-access';

@Component({
  selector: 'app-admin-website-settings',
  imports: [
    DatePipe,
    ReactiveFormsModule,
  ],
  templateUrl: './admin-website-settings.html',
  styleUrl: './admin-website-settings.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AdminWebsiteSettings {
  protected readonly store =
    inject(AdminWebsiteSettingsStore);

  private readonly optionalUrlPattern =
    /^https?:\/\/\S+$/i;

  protected readonly form = new FormGroup({
    artistName: new FormControl('', {
      nonNullable: true,
      validators: [
        Validators.required,
        Validators.maxLength(200),
      ],
    }),
    artistSubtitle: new FormControl('', {
      nonNullable: true,
      validators: [
        Validators.required,
        Validators.maxLength(300),
      ],
    }),
    heroDescription: new FormControl('', {
      nonNullable: true,
      validators: [
        Validators.required,
        Validators.maxLength(1000),
      ],
    }),
    defaultSeoTitle: new FormControl('', {
      nonNullable: true,
      validators: [
        Validators.required,
        Validators.maxLength(200),
      ],
    }),
    defaultSeoDescription: new FormControl('', {
      nonNullable: true,
      validators: [
        Validators.required,
        Validators.maxLength(500),
      ],
    }),
    facebookUrl: new FormControl('', {
      nonNullable: true,
      validators: [
        Validators.maxLength(2048),
        Validators.pattern(
          this.optionalUrlPattern,
        ),
      ],
    }),
    instagramUrl: new FormControl('', {
      nonNullable: true,
      validators: [
        Validators.maxLength(2048),
        Validators.pattern(
          this.optionalUrlPattern,
        ),
      ],
    }),
    youtubeUrl: new FormControl('', {
      nonNullable: true,
      validators: [
        Validators.maxLength(2048),
        Validators.pattern(
          this.optionalUrlPattern,
        ),
      ],
    }),
  });

  constructor() {
    effect(() => {
      const settings = this.store.settings();

      if (!settings) {
        return;
      }

      this.form.reset(
        {
          artistName: settings.artistName,
          artistSubtitle: settings.artistSubtitle,
          heroDescription: settings.heroDescription,
          defaultSeoTitle: settings.defaultSeoTitle,
          defaultSeoDescription:
            settings.defaultSeoDescription,
          facebookUrl: settings.facebookUrl ?? '',
          instagramUrl: settings.instagramUrl ?? '',
          youtubeUrl: settings.youtubeUrl ?? '',
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

    try {
      await this.store.save({
        artistName: value.artistName.trim(),
        artistSubtitle:
          value.artistSubtitle.trim(),
        heroDescription:
          value.heroDescription.trim(),
        defaultSeoTitle:
          value.defaultSeoTitle.trim(),
        defaultSeoDescription:
          value.defaultSeoDescription.trim(),
        facebookUrl: this.normalizeOptionalUrl(
          value.facebookUrl,
        ),
        instagramUrl: this.normalizeOptionalUrl(
          value.instagramUrl,
        ),
        youtubeUrl: this.normalizeOptionalUrl(
          value.youtubeUrl,
        ),
      });

      this.form.markAsPristine();
    }
    catch {
      // A store eltárolja a megjelenítendő hibát.
    }
  }

  private normalizeOptionalUrl(
    value: string,
  ): string | null {
    const normalized = value.trim();

    return normalized
      ? normalized
      : null;
  }
}
