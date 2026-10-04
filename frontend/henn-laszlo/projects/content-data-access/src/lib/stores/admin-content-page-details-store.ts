import {
  computed,
  inject,
  Injectable,
  resource,
  signal,
} from '@angular/core';
import { firstValueFrom } from 'rxjs';
import { AdminContentPageDetails } from '../models/admin-content-page-details';
import { ContentPageKey } from '../models/content-page-key';
import { UpsertContentPageRequest } from '../models/upsert-content-page-request';
import { ContentApi } from '../services/content-api';

@Injectable({
  providedIn: 'root',
})
export class AdminContentPageDetailsStore {
  private readonly contentApi = inject(ContentApi);

  private readonly contentPageKey =
    signal<ContentPageKey | undefined>(undefined);

  private readonly saving = signal(false);

  private readonly saveErrorState =
    signal<Error | undefined>(undefined);

  private readonly contentPageResource = resource({
    params: () => this.contentPageKey(),

    loader: ({ params }) =>
      firstValueFrom(
        this.contentApi.getAdminContentPage(params),
      ),
  });

  readonly contentPage =
    computed<AdminContentPageDetails | null>(
      () => this.contentPageResource.value() ?? null,
    );

  readonly isLoading =
    this.contentPageResource.isLoading;

  readonly error = computed(() => {
    const error = this.contentPageResource.error();

    return error instanceof Error
      ? error
      : undefined;
  });

  readonly isSaving = this.saving.asReadonly();

  readonly saveError =
    this.saveErrorState.asReadonly();

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

  async save(
    request: UpsertContentPageRequest,
  ): Promise<void> {
    const key = this.contentPageKey();

    if (!key) {
      throw new Error(
        'Nincs kiválasztva szerkeszthető tartalmi oldal.',
      );
    }

    if (this.saving()) {
      return;
    }

    this.saving.set(true);
    this.saveErrorState.set(undefined);

    try {
      await firstValueFrom(
        this.contentApi.upsertContentPage(
          key,
          request,
        ),
      );

      this.reload();
    }
    catch (error) {
      this.saveErrorState.set(
        error instanceof Error
          ? error
          : new Error('A mentés sikertelen.'),
      );

      throw error;
    }
    finally {
      this.saving.set(false);
    }
  }
}