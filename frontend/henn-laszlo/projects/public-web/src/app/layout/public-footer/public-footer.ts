import {
  ChangeDetectionStrategy,
  Component,
  inject,
} from '@angular/core';
import {
  PublicWebsiteSettingsStore,
} from 'content-data-access';
import {
  APP_VERSION,
} from '../../core/version/app-version';

@Component({
  selector: 'app-public-footer',
  templateUrl: './public-footer.html',
  styleUrl: './public-footer.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PublicFooter {
  protected readonly websiteSettings =
    inject(PublicWebsiteSettingsStore);

  protected readonly currentYear =
    new Date().getFullYear();
  protected readonly appVersion =
    APP_VERSION;
}
