import {
  computed,
  inject,
  Injectable,
  resource,
  signal,
} from '@angular/core';
import { firstValueFrom } from 'rxjs';
import { AdminContentPageListItem } from '../models/admin-content-page-list-item';
import { ContentPageKey } from '../models/content-page-key';
import { ContentApi } from '../services/content-api';

@Injectable({
  providedIn: 'root',
})
export class AdminContentPagesStore {
  private readonly contentApi = inject(ContentApi);

  private readonly contentPagesResource = resource({
    loader: () =>
      firstValueFrom(
        this.contentApi.getAdminContentPages(),
      ),
  });

  private readonly changingPublicationKey =
    signal<ContentPageKey | null>(null);

  readonly contentPages = computed<
    readonly AdminContentPageListItem[]
  >(
    () => this.contentPagesResource.value() ?? [],
  );

  readonly publishedCount = computed(
    () =>
      this.contentPages()
        .filter((page) => page.isPublished)
        .length,
  );

  readonly draftCount = computed(
    () =>
      this.contentPages()
        .filter((page) => !page.isPublished)
        .length,
  );

  readonly isLoading =
    this.contentPagesResource.isLoading;

  readonly error = computed(() => {
    const error = this.contentPagesResource.error();

    return error instanceof Error
      ? error
      : undefined;
  });

  reload(): void {
    this.contentPagesResource.reload();
  }

  isChangingPublication(
    key: ContentPageKey,
  ): boolean {
    return this.changingPublicationKey() === key;
  }

  async publish(key: ContentPageKey): Promise<void> {
    if (this.changingPublicationKey() !== null) {
      return;
    }

    this.changingPublicationKey.set(key);

    try {
      await firstValueFrom(
        this.contentApi.publishContentPage(key),
      );

      this.reload();
    }
    finally {
      this.changingPublicationKey.set(null);
    }
  }

  async unpublish(
    key: ContentPageKey,
  ): Promise<void> {
    if (this.changingPublicationKey() !== null) {
      return;
    }

    this.changingPublicationKey.set(key);

    try {
      await firstValueFrom(
        this.contentApi.unpublishContentPage(key),
      );

      this.reload();
    }
    finally {
      this.changingPublicationKey.set(null);
    }
  }
}