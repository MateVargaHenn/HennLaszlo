import {
  ChangeDetectionStrategy,
  Component,
  inject,
} from '@angular/core';
import { PublishedArtworksStore } from 'artwork-data-access';

@Component({
  selector: 'app-artwork-gallery',
  templateUrl: './artwork-gallery.html',
  styleUrl: './artwork-gallery.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ArtworkGallery {
  protected readonly store =
    inject(PublishedArtworksStore);

  protected getDimensions(
    widthCm: number | null,
    heightCm: number | null,
  ): string | null {
    if (widthCm === null || heightCm === null) {
      return null;
    }

    return `${widthCm} × ${heightCm} cm`;
  }
}