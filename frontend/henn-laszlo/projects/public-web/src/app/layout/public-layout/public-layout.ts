import {
  ChangeDetectionStrategy,
  Component,
  effect,
  inject,
} from '@angular/core';
import { RouterOutlet } from '@angular/router';
import {
  PublicWebsiteSettingsStore,
} from 'content-data-access';

import { PublicFooter } from '../public-footer/public-footer';
import { PublicHeader } from '../public-header/public-header';
import { ArgusChatbot } from
  '../../features/argus/components/argus-chatbot/argus-chatbot';
import {
  SeoService,
} from '../../core/seo/seo.service';

@Component({
  selector: 'app-public-layout',
  imports: [
    RouterOutlet,
    PublicHeader,
    PublicFooter,
    ArgusChatbot,
  ],
  templateUrl: './public-layout.html',
  styleUrl: './public-layout.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PublicLayout {
  private readonly websiteSettings =
    inject(PublicWebsiteSettingsStore);

  private readonly seo = inject(SeoService);

  constructor() {
    effect(() => {
      const settings =
        this.websiteSettings.settings();

      this.seo.configureDefaults({
        siteName: settings.artistName,
        title: settings.defaultSeoTitle,
        description:
          settings.defaultSeoDescription,
      });
    });
  }
}
