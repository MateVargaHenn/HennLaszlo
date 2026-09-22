import {
  afterNextRender,
  ChangeDetectionStrategy,
  Component,
  ElementRef,
  input,
  output,
  viewChild,
} from '@angular/core';

@Component({
  selector: 'app-image-lightbox',
  imports: [],
  templateUrl: './image-lightbox.html',
  styleUrl: './image-lightbox.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ImageLightbox {
  private readonly dialog =
    viewChild.required<
      ElementRef<HTMLDialogElement>
    >('dialog');

  readonly imageUrl =
    input.required<string>();

  readonly alt =
    input.required<string>();

  readonly title =
    input<string | null>(null);

  readonly closed =
    output<void>();

  constructor() {
    afterNextRender(() => {
      const dialog =
        this.dialog().nativeElement;

      if (!dialog.open) {
        dialog.showModal();
      }
    });
  }

  protected close(): void {
    const dialog =
      this.dialog().nativeElement;

    if (dialog.open) {
      dialog.close();
    }
  }

  protected onCancel(
    event: Event
  ): void {
    event.preventDefault();
    this.close();
  }

  protected onClosed(): void {
    this.closed.emit();
  }
}