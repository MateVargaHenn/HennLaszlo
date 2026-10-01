import {
  ChangeDetectionStrategy,
  Component,
  effect,
  inject,
  ViewEncapsulation,
} from '@angular/core';
import {
  ActivatedRoute,
  RouterLink,
} from '@angular/router';
import {
  ContentPageKey,
  PublishedContentPageStore,
  PublicWebsiteSettingsStore,
} from 'content-data-access';

import {
  SeoService,
} from '../../../../core/seo/seo.service';

import {
  RevealOnScroll,
} from '../../../../shared/directives/reveal-on-scroll';

import {
  SsrResponseService,
} from '../../../../core/http/ssr-response.service';

interface ContentPageSeoData {
  readonly title: string;
  readonly description: string;
  readonly canonicalPath: string;
}

@Component({
  selector: 'app-content-page',
  imports: [
    RouterLink,
    RevealOnScroll,
  ],
  templateUrl: './content-page.html',
  styleUrl: './content-page.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
  encapsulation: ViewEncapsulation.None,
})
export class ContentPage {
  private readonly route = inject(ActivatedRoute);
  private readonly seo = inject(SeoService);
  private readonly response = inject(SsrResponseService);

  protected readonly store =
    inject(PublishedContentPageStore);

  private readonly seoData: Record<
    ContentPageKey,
    ContentPageSeoData
  > = {
    about: {
      title: 'Bemutatkozás',
      description:
        'Ismerje meg Henn László András festőművész és grafikus életútját, művészi pályáját és alkotói szemléletét.',
      canonicalPath: '/bemutatkozas',
    },
    contact: {
      title: 'Kapcsolat',
      description:
        'Henn László András festőművész és grafikus elérhetőségei és kapcsolatfelvételi adatai.',
      canonicalPath: '/kapcsolat',
    },
    exhibitions: {
      title: 'Kiállítások',
      description:
        'Henn László András festőművész egyéni és csoportos kiállításainak válogatott jegyzéke.',
      canonicalPath: '/kiallitasok',
    },

    'memberships-and-awards': {
      title: 'Tagságok és díjak',
      description:
        'Henn László András festőművész művészeti tagságai, elismerései és díjai.',
      canonicalPath: '/tagsagok-es-dijak',
    },

    writings: {
      title: 'Írások',
      description:
        'Henn László András festőművész írásai, kiállításmegnyitó beszédei és művészeti gondolatai.',
      canonicalPath: '/irasok',
    },
  };

  protected readonly websiteSettings =
    inject(PublicWebsiteSettingsStore);

  protected readonly isContactPage =
    this.route.snapshot
      .data['contentPageKey'] ===
    'contact';

  constructor() {
    const key =
      this.route.snapshot.data['contentPageKey'];

    if (!this.isContentPageKey(key)) {
      return;
    }

    const seoData = this.seoData[key];

    this.seo.updatePage({
      ...seoData,
      type: 'website',
    });

    this.store.load(key);

    effect(() => {
      if (this.store.isNotFound()) {
        this.response.setNotFound();

        this.seo.updatePage({
          title:
            `${seoData.title} – az oldal nem található`,
          description:
            'A keresett tartalmi oldal nem található.',
          canonicalPath:
            seoData.canonicalPath,
          type: 'website',
          robots:
            'noindex, nofollow, noarchive',
        });

        return;
      }

      if (this.store.error()) {
        this.response.setStatus(500);

        this.seo.updatePage({
          ...seoData,
          type: 'website',
          robots:
            'noindex, follow',
        });

        return;
      }

      const contentPage =
        this.store.contentPage();

      if (!contentPage) {
        return;
      }

      this.seo.updatePage({
        title:
          contentPage.titleHu,
        description:
          seoData.description,
        canonicalPath:
          seoData.canonicalPath,
        type: 'website',
      });
    });
  }

  private isContentPageKey(
    value: unknown,
  ): value is ContentPageKey {
    return value === 'about' ||
      value === 'contact' ||
      value === 'exhibitions' ||
      value === 'memberships-and-awards' ||
      value === 'writings';
  }
}