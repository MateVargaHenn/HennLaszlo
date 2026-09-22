import {
  computed,
  inject,
  Injectable,
  signal,
} from '@angular/core';
import { firstValueFrom } from 'rxjs';
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
    return this.artworkApi.getAdminArtworkImageUrl(
      artworkId
    );
  }

  private readonly publicationInProgressId =
  signal<string | null>(null);

  readonly publicationError =
    signal<string | null>(null);

  isChangingPublication(
    artworkId: string
  ): boolean {
    return this.publicationInProgressId() === artworkId;
  }

  publish(artworkId: string): Promise<boolean> {
  return this.changePublication(
    artworkId,
    true
  );
}

  unpublish(artworkId: string): Promise<boolean> {
    return this.changePublication(
      artworkId,
      false
    );
  }

  private async changePublication(
    artworkId: string,
    publish: boolean
  ): Promise<boolean> {
    this.publicationInProgressId.set(artworkId);
    this.publicationError.set(null);
    try {
      if (publish) {
        await firstValueFrom(this.artworkApi.publishArtwork(artworkId));
      } else {
        await firstValueFrom(this.artworkApi.unpublishArtwork(artworkId));
      }
      this.reload();
      return true;
    } catch (error) {
      this.publicationError.set(
        error instanceof Error ? error.message : String(error)
      );
      return false;
    } finally {
      this.publicationInProgressId.set(null);
    }
  }
}