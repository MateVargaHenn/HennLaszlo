import {
  computed,
  inject,
  Injectable,
  resource,
} from '@angular/core';
import { firstValueFrom } from 'rxjs';
import {
  PublicWebsiteSettings,
} from '../models/public-website-settings';
import { ContentApi } from '../services/content-api';

const fallbackSettings: PublicWebsiteSettings = {
  artistName: 'Henn László András',
  artistSubtitle:
    'Galyasi Miklós nívódíjas festőművész, grafikus',
  heroDescription:
    'Válogatás az alkotó festményeiből, ' +
    'kiállításaiból és több évtizedes ' +
    'művészi munkásságából.',
  defaultSeoTitle:
    'Henn László András | Festőművész és grafikus',
  defaultSeoDescription:
    'Henn László András Galyasi Miklós ' +
    'nívódíjas festőművész és grafikus ' +
    'hivatalos oldala: művek, kiállítások, ' +
    'meghívók, videók és írások.',
  facebookUrl: null,
  instagramUrl: null,
  youtubeUrl: 'https://www.youtube.com/@LA_Henn',
  emailAddress: 'hennlaa@gmail.com',
};

@Injectable({
  providedIn: 'root',
})
export class PublicWebsiteSettingsStore {
  private readonly contentApi = inject(ContentApi);

  private readonly settingsResource = resource({
    loader: () =>
      firstValueFrom(
        this.contentApi.getPublicWebsiteSettings(),
      ),
  });

  readonly settings =
    computed<PublicWebsiteSettings>(
      () => {
        if (!this.settingsResource.hasValue()) {
          return fallbackSettings;
        }

        return this.settingsResource.value();
      },
    );

  readonly isLoading =
    this.settingsResource.isLoading;

  readonly error =
    this.settingsResource.error;

  reload(): void {
    this.settingsResource.reload();
  }
}
