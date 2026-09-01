import {
  computed,
  inject,
  Injectable,
  signal,
} from '@angular/core';
import { rxResource } from '@angular/core/rxjs-interop';
import { ArtworkApi } from '../services/artwork-api';

@Injectable()
export class ArtworkDetailsStore {
  private readonly artworkApi = inject(ArtworkApi);

  private readonly artworkId =
    signal<string | undefined>(undefined);

  private readonly resource = rxResource({
    params: () => {
      const artworkId = this.artworkId();

      return artworkId
        ? { artworkId }
        : undefined;
    },

    stream: ({ params }) =>
      this.artworkApi.getArtworkById(params.artworkId),
  });

  readonly artwork = computed(() =>
    this.resource.hasValue()
      ? this.resource.value()
      : null
  );

  readonly isLoading = computed(() =>
    this.resource.isLoading()
  );

  readonly error = computed(() =>
    this.resource.error()
  );

  setArtworkId(artworkId: string): void {
    if (this.artworkId() !== artworkId) {
      this.artworkId.set(artworkId);
    }
  }

  reload(): void {
    this.resource.reload();
  }

  getImageUrl(artworkId: string): string {
    return this.artworkApi.getArtworkImageUrl(artworkId);
  }
}