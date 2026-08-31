import { inject, Injectable } from '@angular/core';
import { rxResource } from '@angular/core/rxjs-interop';

import { ArtworkApi } from '../services/artwork-api';
import { ArtworkListItem } from '../models/artwork-list-item';

@Injectable({
  providedIn: 'root',
})
export class PublishedArtworksStore {
  private readonly artworkApi = inject(ArtworkApi);

  private readonly resource = rxResource<
  readonly ArtworkListItem[],
  unknown
>({
  defaultValue: [],
  stream: () =>
    this.artworkApi.getPublishedArtworks(),
});

  readonly artworks = this.resource.value;
  readonly isLoading = this.resource.isLoading;
  readonly error = this.resource.error;

  reload(): void {
    this.resource.reload();
  }

  getImageUrl(artworkId: string): string {
    return this.artworkApi.getArtworkImageUrl(
      artworkId,
    );
  }
}