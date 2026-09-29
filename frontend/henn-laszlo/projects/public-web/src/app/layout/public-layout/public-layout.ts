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
import {
  StructuredDataService,
} from '../../core/seo/structured-data.service';

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

  private readonly seo =
    inject(SeoService);

  private readonly structuredData =
    inject(StructuredDataService);

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
      const siteUrl =
        this.seo.toAbsoluteUrl('/');

      const personId =
        `${siteUrl}#person`;

      const websiteId =
        `${siteUrl}#website`;

      const sameAs = [
        settings.facebookUrl,
        settings.instagramUrl,
        settings.youtubeUrl,
      ].filter(
        (url): url is string =>
          Boolean(url),
      );

      this.structuredData.setGroup(
        'site',
        [
          {
            '@type': 'WebSite',
            '@id': websiteId,
            url: siteUrl,
            name: settings.artistName,
            description:
              settings.defaultSeoDescription,
            inLanguage: 'hu-HU',
            about: {
              '@id': personId,
            },
          },
          {
            '@type': 'Person',
            '@id': personId,
            name: settings.artistName,
            url: siteUrl,
            jobTitle:
              'Festőművész és grafikus',
            description:
              settings.defaultSeoDescription,
            ...(sameAs.length > 0
              ? { sameAs }
              : {}),
          },
        ],
      );
    });
  }
}
