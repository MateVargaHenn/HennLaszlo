import {
  computed,
  inject,
  Injectable,
  resource,
  signal,
} from '@angular/core';
import {
  HttpErrorResponse,
} from '@angular/common/http';
import { firstValueFrom } from 'rxjs';
import { ContentPageKey } from '../models/content-page-key';
import { PublishedContentPage } from '../models/published-content-page';
import { ContentApi } from '../services/content-api';

@Injectable({
  providedIn: 'root',
})
export class PublishedContentPageStore {
  private readonly contentApi = inject(ContentApi);

  private readonly contentPageKey =
    signal<ContentPageKey | undefined>(undefined);

  private readonly contentPageResource = resource({
    params: () => this.contentPageKey(),

    loader: ({ params }) =>
      firstValueFrom(
        this.contentApi.getPublishedContentPage(params),
      ),
  });

  readonly contentPage =
    computed<PublishedContentPage | null>(() => {
      if (!this.contentPageResource.hasValue()) {
        return null;
      }

      return this.contentPageResource.value();
    });

  readonly isLoading =
    this.contentPageResource.isLoading;

  readonly error =
    this.contentPageResource.error;

  readonly isNotFound = computed(() => {
    const error = this.contentPageResource.error();

    return error instanceof HttpErrorResponse &&
      error.status === 404;
  });

  load(key: ContentPageKey): void {
    if (this.contentPageKey() === key) {
      this.contentPageResource.reload();
      return;
    }

    this.contentPageKey.set(key);
  }

  reload(): void {
    this.contentPageResource.reload();
  }
}