import {
  computed,
  inject,
  Injectable,
  signal,
} from '@angular/core';
import { firstValueFrom } from 'rxjs';
import { rxResource } from '@angular/core/rxjs-interop';
import { UpdateArtworkRequest } from '../models/update-artwork-request';
import { ArtworkApi } from '../services/artwork-api';

@Injectable()
export class AdminArtworkDetailsStore {
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
      this.artworkApi.getAdminArtworkById(
        params.artworkId
      ),
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

  readonly isSaving = signal(false);
  readonly saveError = signal<string | null>(null);

  load(artworkId: string): void {
    this.artworkId.set(artworkId);
  }
reload(): void {
  this.resource.reload();
}

getImageUrl(artworkId: string): string {
  return this.artworkApi
    .getAdminArtworkImageUrl(artworkId);
}


async update(
  request: UpdateArtworkRequest,
  imageFile: File | null
): Promise<boolean> {
    const artworkId = this.artworkId();

    if (!artworkId) {
      this.saveError.set(
        'A mű azonosítója nem található.'
      );

      return false;
    }

    this.isSaving.set(true);
    this.saveError.set(null);

    try {
      await firstValueFrom(
        this.artworkApi.updateArtwork(
          artworkId,
          request
        )
      );

      if (imageFile) {
        const uploadedFile = await firstValueFrom(
          this.artworkApi.uploadFile(imageFile)
        );

        await firstValueFrom(
          this.artworkApi.attachImage(
            artworkId,
            uploadedFile.id
          )
        );
      }

      return true;
    }
    catch {
      this.saveError.set(
        'A módosításokat nem sikerült elmenteni.'
      );

      return false;
    }
    finally {
      this.isSaving.set(false);
    }
  }
}