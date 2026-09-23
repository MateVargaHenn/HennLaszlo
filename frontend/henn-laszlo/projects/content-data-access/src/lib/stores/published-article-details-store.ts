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
import { PublishedArticleDetails } from '../models/published-article-details';
import { ContentApi } from '../services/content-api';

@Injectable({
  providedIn: 'root',
})
export class PublishedArticleDetailsStore {
  private readonly contentApi = inject(ContentApi);

  private readonly articleSlug =
    signal<string | undefined>(undefined);

  private readonly articleResource = resource({
    params: () => this.articleSlug(),

    loader: ({ params }) =>
      firstValueFrom(
        this.contentApi
          .getPublishedArticleBySlug(params),
      ),
  });

readonly article =
  computed<PublishedArticleDetails | null>(() => {
    if (!this.articleResource.hasValue()) {
      return null;
    }

    return this.articleResource.value();
  });

  readonly isLoading =
    this.articleResource.isLoading;

  readonly error =
    this.articleResource.error;

  readonly isNotFound = computed(() => {
    const error = this.articleResource.error();

    return error instanceof HttpErrorResponse &&
      error.status === 404;
  });

  load(slug: string): void {
    const normalizedSlug = slug.trim();

    if (this.articleSlug() === normalizedSlug) {
      this.articleResource.reload();
      return;
    }

    this.articleSlug.set(normalizedSlug);
  }

  reload(): void {
    this.articleResource.reload();
  }
}