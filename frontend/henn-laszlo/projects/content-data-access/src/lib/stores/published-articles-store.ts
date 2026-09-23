import {
  computed,
  inject,
  Injectable,
  resource,
} from '@angular/core';
import { firstValueFrom } from 'rxjs';
import { PublishedArticleListItem } from '../models/published-article-list-item';
import { ContentApi } from '../services/content-api';

@Injectable({
  providedIn: 'root',
})
export class PublishedArticlesStore {
  private readonly contentApi = inject(ContentApi);

  private readonly articlesResource = resource({
    loader: () =>
      firstValueFrom(
        this.contentApi.getPublishedArticles(),
      ),
  });

  readonly articles = computed<
    readonly PublishedArticleListItem[]
  >(() => {
    if (!this.articlesResource.hasValue()) {
      return [];
    }

    return this.articlesResource.value();
  });

  readonly error =
    this.articlesResource.error;

  readonly isLoading =
    this.articlesResource.isLoading;

  reload(): void {
    this.articlesResource.reload();
  }
}