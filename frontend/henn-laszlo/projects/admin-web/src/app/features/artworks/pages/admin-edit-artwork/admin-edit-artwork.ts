import {
  ChangeDetectionStrategy,
  Component,
  inject,
} from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AdminArtworkDetailsStore } from 'artwork-data-access';
import { Router } from '@angular/router';
import { UpdateArtworkRequest } from 'artwork-data-access';
import { ArtworkForm,
  ArtworkFormSubmission } from '../../components/artwork-form/artwork-form';

@Component({
  selector: 'app-admin-edit-artwork',
  imports: [ArtworkForm],
  providers: [AdminArtworkDetailsStore],
  templateUrl: './admin-edit-artwork.html',
  styleUrl: './admin-edit-artwork.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AdminEditArtwork {
  protected readonly store =
    inject(AdminArtworkDetailsStore);

  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);

  constructor() {
    const artworkId =
      this.route.snapshot.paramMap.get('artworkId');

    if (!artworkId) {
      throw new Error(
        'A mű azonosítója hiányzik az útvonalból.'
      );
    }

    this.store.load(artworkId);
  }

protected async save(
  submission: ArtworkFormSubmission
): Promise<void> {
  const request: UpdateArtworkRequest = {
    titleHu: submission.artwork.titleHu,
    titleEn: submission.artwork.titleEn,
    year: submission.artwork.year,
    techniqueHu: submission.artwork.techniqueHu,
    techniqueEn: submission.artwork.techniqueEn,
    widthCm: submission.artwork.widthCm,
    heightCm: submission.artwork.heightCm,
    descriptionHu:
      submission.artwork.descriptionHu,
    descriptionEn:
      submission.artwork.descriptionEn,
    isFeatured:
      submission.artwork.isFeatured,
    displayOrder:
      submission.artwork.displayOrder,
  };

  const succeeded = await this.store.update(
    request,
    submission.imageFile
  );

  if (succeeded) {
    await this.router.navigate(['/artworks']);
  }
}

protected async cancel(): Promise<void> {
  await this.router.navigate(['/artworks']);
}
}