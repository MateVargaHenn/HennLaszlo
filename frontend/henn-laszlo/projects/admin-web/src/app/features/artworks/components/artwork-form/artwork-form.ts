import {
  ChangeDetectionStrategy,
  Component,
  ElementRef,
  input,
  OnDestroy,
  output,
  signal,
  viewChild,
} from '@angular/core';
import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { CreateArtworkRequest } from 'artwork-data-access';

export interface ArtworkFormSubmission {
  readonly artwork: CreateArtworkRequest;
  readonly imageFile: File | null;
}

@Component({
  selector: 'app-artwork-form',
  imports: [ReactiveFormsModule],
  templateUrl: './artwork-form.html',
  styleUrl: './artwork-form.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ArtworkForm implements OnDestroy {
  private readonly fileInput =
    viewChild<ElementRef<HTMLInputElement>>('fileInput');

  private previewObjectUrl: string | null = null;

  readonly isSubmitting = input(false);
  readonly submitError = input<string | null>(null);

  readonly submitted =
    output<ArtworkFormSubmission>();

  readonly cancelled = output<void>();

  readonly imageFile = signal<File | null>(null);
  readonly imagePreviewUrl = signal<string | null>(null);
  readonly imageError = signal<string | null>(null);

  protected readonly maximumYear =
    new Date().getFullYear() + 1;

  protected readonly form = new FormGroup({
    titleHu: new FormControl('', {
      nonNullable: true,
      validators: [
        Validators.required,
        Validators.pattern(/\S/),
        Validators.maxLength(200),
      ],
    }),

    titleEn: new FormControl<string | null>(
      null,
      Validators.maxLength(200)
    ),

    year: new FormControl<number | null>(
      null,
      [
        Validators.min(1000),
        Validators.max(this.maximumYear),
      ]
    ),

    techniqueHu: new FormControl<string | null>(
      null,
      Validators.maxLength(200)
    ),

    techniqueEn: new FormControl<string | null>(
      null,
      Validators.maxLength(200)
    ),

    widthCm: new FormControl<number | null>(
      null,
      Validators.min(0.01)
    ),

    heightCm: new FormControl<number | null>(
      null,
      Validators.min(0.01)
    ),
  });

  protected onSubmit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const value = this.form.getRawValue();

    this.submitted.emit({
      artwork: {
        titleHu: value.titleHu.trim(),
        titleEn: this.normalizeText(value.titleEn),
        year: value.year,
        techniqueHu: this.normalizeText(
          value.techniqueHu
        ),
        techniqueEn: this.normalizeText(
          value.techniqueEn
        ),
        widthCm: value.widthCm,
        heightCm: value.heightCm,
      },
      imageFile: this.imageFile(),
    });
  }

  protected onFileSelected(event: Event): void {
    const inputElement =
      event.target as HTMLInputElement;

    this.setImage(inputElement.files?.[0] ?? null);
  }

  protected onFileDropped(event: DragEvent): void {
    event.preventDefault();

    this.setImage(
      event.dataTransfer?.files?.[0] ?? null
    );
  }

  protected allowFileDrop(event: DragEvent): void {
    event.preventDefault();
  }

  protected removeImage(): void {
    this.releasePreviewUrl();

    this.imageFile.set(null);
    this.imagePreviewUrl.set(null);
    this.imageError.set(null);

    const inputElement =
      this.fileInput()?.nativeElement;

    if (inputElement) {
      inputElement.value = '';
    }
  }

  protected cancel(): void {
    this.cancelled.emit();
  }

  ngOnDestroy(): void {
    this.releasePreviewUrl();
  }

  private setImage(file: File | null): void {
    if (!file) {
      return;
    }

    if (!file.type.startsWith('image/')) {
      this.imageError.set(
        'Kizárólag képfájl választható.'
      );
      return;
    }

    this.releasePreviewUrl();
    const previewUrl = URL.createObjectURL(file);

    this.previewObjectUrl = previewUrl;
    this.imageFile.set(file);
    this.imagePreviewUrl.set(previewUrl);
    this.imageError.set(null);
  }

private releasePreviewUrl(): void {
  if (!this.previewObjectUrl) {
    return;
  }

  URL.revokeObjectURL(this.previewObjectUrl);
  this.previewObjectUrl = null;
}

  private normalizeText(
    value: string | null
  ): string | null {
    const normalizedValue = value?.trim();

    return normalizedValue
      ? normalizedValue
      : null;
  }
}