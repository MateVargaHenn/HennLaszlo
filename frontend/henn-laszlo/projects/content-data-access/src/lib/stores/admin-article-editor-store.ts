import {
  computed,
  inject,
  Injectable,
  resource,
  signal,
} from '@angular/core';
import { firstValueFrom } from 'rxjs';
import {
  AdminArticleDetails,
} from '../models/admin-article-details';
import {
  CreateArticleRequest,
} from '../models/create-article-request';
import {
  UpdateArticleRequest,
} from '../models/update-article-request';
import { ContentApi } from '../services/content-api';

@Injectable({
  providedIn: 'root',
})
export class AdminArticleEditorStore {
  private readonly contentApi = inject(ContentApi);

  private readonly articleId =
    signal<string | undefined>(undefined);

  private readonly articleResource = resource({
    params: () => this.articleId(),

    loader: ({ params }) =>
      firstValueFrom(
        this.contentApi.getAdminArticleById(params),
      ),
  });

  private readonly saving =
    signal(false);

  private readonly mutationErrorState =
    signal<unknown | undefined>(undefined);

  readonly article =
    computed<AdminArticleDetails | null>(() => {
      if (!this.articleResource.hasValue()) {
        return null;
      }

      return this.articleResource.value();
    });

  readonly loadError =
    this.articleResource.error;

  readonly isLoading =
    this.articleResource.isLoading;


  readonly isSaving =
    this.saving.asReadonly();

  readonly mutationError =
    this.mutationErrorState.asReadonly();

  load(articleId: string): void {
    const normalizedArticleId = articleId.trim();

    if (this.articleId() === normalizedArticleId) {
      this.articleResource.reload();
      return;
    }

    this.articleId.set(normalizedArticleId);
  }

  clear(): void {
    this.articleId.set(undefined);
    this.mutationErrorState.set(undefined);
  }

  async create(
    request: CreateArticleRequest,
  ): Promise<string> {
    return this.executeMutation(async () => {
      const response = await firstValueFrom(
        this.contentApi.createArticle(request),
      );

      return response.id;
    });
  }

  async update(
    articleId: string,
    request: UpdateArticleRequest,
  ): Promise<void> {
    await this.executeMutation(() =>
      firstValueFrom(
        this.contentApi.updateArticle(
          articleId,
          request,
        ),
      ),
    );
  }

  async publish(articleId: string): Promise<void> {
    await this.executeMutation(() =>
      firstValueFrom(
        this.contentApi.publishArticle(articleId),
      ),
    );

    this.articleResource.reload();
  }

  async unpublish(articleId: string): Promise<void> {
    await this.executeMutation(() =>
      firstValueFrom(
        this.contentApi.unpublishArticle(articleId),
      ),
    );

    this.articleResource.reload();
  }

  async delete(articleId: string): Promise<void> {
    await this.executeMutation(() =>
      firstValueFrom(
        this.contentApi.deleteArticle(articleId),
      ),
    );

    this.clear();
  }

  private async executeMutation<TResult>(
    operation: () => Promise<TResult>,
  ): Promise<TResult> {
    this.saving.set(true);
    this.mutationErrorState.set(undefined);

    try {
      return await operation();
    }
    catch (error) {
      this.mutationErrorState.set(error);
      throw error;
    }
    finally {
      this.saving.set(false);
    }
  }
}