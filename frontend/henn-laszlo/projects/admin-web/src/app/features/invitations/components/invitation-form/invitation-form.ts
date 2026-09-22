import {
  ChangeDetectionStrategy,
  Component,
  ElementRef,
  input,
  OnDestroy,
  output,
  signal,
  viewChild,
  computed,
  effect,
} from '@angular/core';
import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import {
  AdminInvitationDetails,
  CreateInvitationRequest,
} from 'invitation-data-access';

export interface InvitationFormSubmission {
  readonly invitation: CreateInvitationRequest;
  readonly imageFile: File | null;
}

@Component({
  selector: 'app-invitation-form',
  imports: [ReactiveFormsModule],
  templateUrl: './invitation-form.html',
  styleUrl: './invitation-form.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class InvitationForm implements OnDestroy {

  constructor() {
    effect(() => {
      const invitation = this.initialValue();

      if (!invitation) {
        return;
      }

      this.form.patchValue({
        titleHu: invitation.titleHu,
        titleEn: invitation.titleEn,
        year: invitation.year,
        altTextHu: invitation.altTextHu,
        altTextEn: invitation.altTextEn,
        displayOrder: invitation.displayOrder,
      });
    });
  }

  readonly mode =
    input<'create' | 'edit'>('create');

  readonly initialValue =
    input<AdminInvitationDetails | null>(null);

  readonly existingImageUrl =
    input<string | null>(null);

  protected readonly displayedImageUrl =
    computed(
      () =>
        this.imagePreviewUrl() ??
        this.existingImageUrl()
    );


  private readonly fileInput =
    viewChild<ElementRef<HTMLInputElement>>(
      'fileInput'
    );

  private previewObjectUrl: string | null = null;

  readonly isSubmitting = input(false);

  readonly submitError =
    input<string | null>(null);

  readonly submitted =
    output<InvitationFormSubmission>();

  readonly cancelled = output<void>();

  readonly imageFile =
    signal<File | null>(null);

  readonly imagePreviewUrl =
    signal<string | null>(null);

  readonly imageError =
    signal<string | null>(null);

  protected readonly maximumYear =
    new Date().getFullYear() + 1;

  protected readonly form = new FormGroup({
    titleHu: new FormControl('', {
      nonNullable: true,
      validators: [
        Validators.required,
        Validators.pattern(/\S/),
        Validators.maxLength(250),
      ],
    }),

    titleEn: new FormControl<string | null>(
      null,
      Validators.maxLength(250)
    ),

    year: new FormControl<number | null>(
      null,
      [
        Validators.min(1),
        Validators.max(this.maximumYear),
      ]
    ),

    altTextHu: new FormControl<string | null>(
      null,
      Validators.maxLength(500)
    ),

    altTextEn: new FormControl<string | null>(
      null,
      Validators.maxLength(500)
    ),

    displayOrder: new FormControl(0, {
      nonNullable: true,
      validators: [
        Validators.min(0),
      ],
    }),
  });

  protected onSubmit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const value = this.form.getRawValue();

    this.submitted.emit({
      invitation: {
        titleHu: value.titleHu.trim(),
        titleEn: this.normalizeText(value.titleEn),
        year: value.year,
        altTextHu:
          this.normalizeText(value.altTextHu),
        altTextEn:
          this.normalizeText(value.altTextEn),
        displayOrder: value.displayOrder,
      },
      imageFile: this.imageFile(),
    });
  }

  protected onFileSelected(event: Event): void {
    const inputElement =
      event.target as HTMLInputElement;

    this.setSelectedImage(
      inputElement.files?.[0] ?? null
    );
  }

  protected onFileDropped(
    event: DragEvent
  ): void {
    event.preventDefault();

    this.setSelectedImage(
      event.dataTransfer?.files?.[0] ?? null
    );
  }

  protected allowFileDrop(
    event: DragEvent
  ): void {
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

  private setSelectedImage(
    file: File | null
  ): void {
    if (!file) {
      return;
    }

    const allowedTypes = [
      'image/jpeg',
      'image/png',
      'image/webp',
    ];

    if (!allowedTypes.includes(file.type)) {
      this.imageError.set(
        'JPG, PNG vagy WebP kép választható.'
      );
      return;
    }

    this.releasePreviewUrl();

    const previewUrl =
      URL.createObjectURL(file);

    this.previewObjectUrl = previewUrl;
    this.imageFile.set(file);
    this.imagePreviewUrl.set(previewUrl);
    this.imageError.set(null);
  }

  private releasePreviewUrl(): void {
    if (!this.previewObjectUrl) {
      return;
    }

    URL.revokeObjectURL(
      this.previewObjectUrl
    );

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