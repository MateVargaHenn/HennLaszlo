import {
  ChangeDetectionStrategy,
  Component,
  DestroyRef,
  inject,
  signal,
} from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import {
  EMPTY,
  finalize,
  map,
  Observable,
  of,
  switchMap,
  tap,
} from 'rxjs';
import {
  AdminArtworksStore,
  ArtworkApi,
} from 'artwork-data-access';
import {
  ArtworkForm,
  ArtworkFormSubmission,
} from '../../components/artwork-form/artwork-form';

@Component({
  selector: 'app-admin-create-artwork',
  imports: [
    RouterLink,
    ArtworkForm,
  ],
  templateUrl: './admin-create-artwork.html',
  styleUrl: './admin-create-artwork.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AdminCreateArtwork {
  private readonly artworkApi = inject(ArtworkApi);
  private readonly artworksStore =
    inject(AdminArtworksStore);

  private readonly router = inject(Router);
  private readonly destroyRef = inject(DestroyRef);

  private createdArtworkId: string | null = null;
  private uploadedFileId: string | null = null;
  private pendingImageFile: File | null = null;

  protected readonly isSubmitting = signal(false);
  protected readonly submitError =
    signal<string | null>(null);

  protected save(
    submission: ArtworkFormSubmission
  ): void {
    if (this.isSubmitting()) {
      return;
    }

    if (
      this.pendingImageFile !== submission.imageFile
    ) {
      this.pendingImageFile = submission.imageFile;
      this.uploadedFileId = null;
    }

    this.isSubmitting.set(true);
    this.submitError.set(null);

    this.saveArtwork(submission)
      .pipe(
        takeUntilDestroyed(this.destroyRef),

        finalize(() => {
          this.isSubmitting.set(false);
        })
      )
      .subscribe({
        next: () => {
          this.artworksStore.reload();

          void this.router.navigate(['/artworks']);
        },

        error: () => {
          this.submitError.set(
            this.createdArtworkId
              ? 'A vázlat létrejött, de a kép mentése nem sikerült. Kérlek, próbáld újra.'
              : 'A mű mentése nem sikerült. Kérlek, próbáld újra.'
          );
        },
      });
  }

  protected cancel(): void {
    void this.router.navigate(['/artworks']);
  }

  private saveArtwork(
    submission: ArtworkFormSubmission
  ): Observable<string> {
    return this.getOrCreateArtwork(submission)
      .pipe(
        switchMap(artworkId =>
          this.attachImageIfSelected(
            artworkId,
            submission.imageFile
          )
        )
      );
  }

  private getOrCreateArtwork(
    submission: ArtworkFormSubmission
  ): Observable<string> {
    if (this.createdArtworkId) {
      return of(this.createdArtworkId);
    }

    return this.artworkApi
      .createArtwork(submission.artwork)
      .pipe(
        tap(response => {
          this.createdArtworkId =
            response.id;
        }),

        map(response => response.id)
      );
  }

  private attachImageIfSelected(
    artworkId: string,
    imageFile: File | null
  ): Observable<string> {
    if (!imageFile) {
      return of(artworkId);
    }

    return this.getOrUploadFile(imageFile)
      .pipe(
        switchMap(fileId =>
          this.artworkApi
            .attachImage(artworkId, fileId)
        ),

        map(() => artworkId)
      );
  }

  private getOrUploadFile(
    imageFile: File
  ): Observable<string> {
    if (this.uploadedFileId) {
      return of(this.uploadedFileId);
    }

    return this.artworkApi
      .uploadFile(imageFile)
      .pipe(
        tap(response => {
          this.uploadedFileId = response.id;
        }),

        map(response => response.id)
      );
  }
}