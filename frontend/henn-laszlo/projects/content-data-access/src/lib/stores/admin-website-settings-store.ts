import {
  computed,
  inject,
  Injectable,
  resource,
  signal,
} from '@angular/core';
import { firstValueFrom } from 'rxjs';
import {
  AdminWebsiteSettings,
} from '../models/admin-website-settings';
import {
  UpdateWebsiteSettingsRequest,
} from '../models/update-website-settings-request';
import { ContentApi } from '../services/content-api';

@Injectable({
  providedIn: 'root',
})
export class AdminWebsiteSettingsStore {
  private readonly contentApi = inject(ContentApi);

  private readonly settingsResource = resource({
    loader: () =>
      firstValueFrom(
        this.contentApi.getAdminWebsiteSettings(),
      ),
  });

  private readonly saving = signal(false);

  private readonly saveErrorState =
    signal<unknown | undefined>(undefined);

  private readonly savedState = signal(false);

  readonly settings =
    computed<AdminWebsiteSettings | null>(
      () => {
        if (
          !this.settingsResource.hasValue()
        ) {
          return null;
        }

        return this.settingsResource.value();
      },
    );

  readonly isLoading =
    this.settingsResource.isLoading;

  readonly loadError =
    this.settingsResource.error;

  readonly isSaving = this.saving.asReadonly();

  readonly saveError =
    this.saveErrorState.asReadonly();

  readonly savedSuccessfully =
    this.savedState.asReadonly();

  reload(): void {
    this.settingsResource.reload();
  }

  async save(
    request: UpdateWebsiteSettingsRequest,
  ): Promise<void> {
    if (this.saving()) {
      return;
    }

    this.saving.set(true);
    this.saveErrorState.set(undefined);
    this.savedState.set(false);

    try {
      await firstValueFrom(
        this.contentApi.updateWebsiteSettings(request),
      );

      this.savedState.set(true);
      this.reload();
    }
    catch (error) {
      this.saveErrorState.set(error);
      throw error;
    }
    finally {
      this.saving.set(false);
    }
  }
}
