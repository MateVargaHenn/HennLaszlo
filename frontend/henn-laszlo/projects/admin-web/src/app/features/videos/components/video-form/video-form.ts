import { ChangeDetectionStrategy, Component, effect, input, output } from '@angular/core';
import {
  AbstractControl,
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  ValidationErrors,
  ValidatorFn,
  Validators,
} from '@angular/forms';
import type { AdminVideoDetails, CreateVideoRequest } from 'video-data-access';

const httpsUrlValidator: ValidatorFn = (control: AbstractControl): ValidationErrors | null => {
  const value = `${control.value ?? ''}`.trim();

  if (!value) {
    return null;
  }

  try {
    const url = new URL(value);

    return url.protocol === 'https:' ? null : { httpsUrl: true };
  } catch {
    return { httpsUrl: true };
  }
};

@Component({
  selector: 'app-video-form',
  imports: [ReactiveFormsModule],
  templateUrl: './video-form.html',
  styleUrl: './video-form.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class VideoForm {
  readonly mode = input<'create' | 'edit'>('create');

  readonly initialValue = input<AdminVideoDetails | null>(null);

  readonly isSubmitting = input(false);

  readonly submitError = input<string | null>(null);

  readonly submitted = output<CreateVideoRequest>();

  readonly cancelled = output<void>();

  protected readonly maximumYear = new Date().getFullYear() + 1;

  protected readonly form = new FormGroup({
    titleHu: new FormControl('', {
      nonNullable: true,
      validators: [Validators.required, Validators.pattern(/\S/), Validators.maxLength(250)],
    }),

    titleEn: new FormControl<string | null>(null, Validators.maxLength(250)),

    year: new FormControl<number | null>(null, [
      Validators.min(1),
      Validators.max(this.maximumYear),
    ]),

    descriptionHu: new FormControl<string | null>(null, Validators.maxLength(4000)),

    descriptionEn: new FormControl<string | null>(null, Validators.maxLength(4000)),

    videoUrl: new FormControl('', {
      nonNullable: true,
      validators: [Validators.required, Validators.maxLength(2048), httpsUrlValidator],
    }),

    displayOrder: new FormControl(0, {
      nonNullable: true,
      validators: [Validators.min(0)],
    }),
  });

  constructor() {
    effect(() => {
      const video = this.initialValue();

      if (!video) {
        return;
      }

      this.form.patchValue({
        titleHu: video.titleHu,
        titleEn: video.titleEn,
        year: video.year,
        descriptionHu: video.descriptionHu,
        descriptionEn: video.descriptionEn,
        videoUrl: video.videoUrl,
        displayOrder: video.displayOrder,
      });
    });
  }

  protected onSubmit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const value = this.form.getRawValue();

    this.submitted.emit({
      titleHu: value.titleHu.trim(),
      titleEn: this.normalizeText(value.titleEn),
      year: value.year,
      descriptionHu: this.normalizeText(value.descriptionHu),
      descriptionEn: this.normalizeText(value.descriptionEn),
      videoUrl: value.videoUrl.trim(),
      displayOrder: value.displayOrder,
    });
  }

  protected cancel(): void {
    this.cancelled.emit();
  }

  private normalizeText(value: string | null): string | null {
    const normalizedValue = value?.trim();

    return normalizedValue ? normalizedValue : null;
  }
}
