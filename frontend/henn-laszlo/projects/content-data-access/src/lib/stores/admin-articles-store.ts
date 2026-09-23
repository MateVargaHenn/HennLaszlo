import {
  computed,
  inject,
  Injectable,
  resource,
} from '@angular/core';
import { firstValueFrom } from 'rxjs';
import {
  AdminArticleListItem,
} from '../models/admin-article-list-item';
import { ContentApi } from '../services/content-api';

@Injectable({
  providedIn: 'root',
})
export class AdminArticlesStore {
  private readonly contentApi = inject(ContentApi);

  private readonly articlesResource = resource({
    loader: () =>
      firstValueFrom(
        this.contentApi.getAdminArticles(),
      ),
  });

 readonly articles = computed<
  readonly AdminArticleListItem[]
>(() => {
  if (!this.articlesResource.hasValue()) {
    return [];
  }

  return this.articlesResource.value();
});

  readonly isLoading =
    this.articlesResource.isLoading;

  readonly error =
    this.articlesResource.error;

  reload(): void {
    this.articlesResource.reload();
  }
}