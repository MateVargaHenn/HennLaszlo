import {
  computed,
  inject,
  Injectable,
} from '@angular/core';
import { rxResource } from '@angular/core/rxjs-interop';
import { ArtworkApi } from '../services/artwork-api';
import { AdminArtworkListItem } from '../models/admin-artwork-list-item';

@Injectable({
  providedIn: 'root',
})
export class AdminArtworksStore {
  private readonly artworkApi = inject(ArtworkApi);

  private readonly resource = rxResource<
    readonly AdminArtworkListItem[],
    unknown
  >({
    defaultValue: [],
    stream: () =>
      this.artworkApi.getAdminArtworks(),
  });

  readonly artworks = computed(() =>
    this.resource.value()
  );

  readonly isLoading = computed(() =>
    this.resource.isLoading()
  );

  readonly error = computed(() =>
    this.resource.error()
  );

  readonly publishedCount = computed(() =>
    this.artworks().filter(
      artwork => artwork.isPublished
    ).length
  );

  readonly draftCount = computed(() =>
    this.artworks().filter(
      artwork => !artwork.isPublished
    ).length
  );

  reload(): void {
    this.resource.reload();
  }

  getImageUrl(artworkId: string): string {
    return this.artworkApi.getArtworkImageUrl(
      artworkId
    );
  }
}