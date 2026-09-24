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
} from 'content-data-access';

import {
  SeoService,
} from '../../../../core/seo/seo.service';

import {
  RevealOnScroll,
} from '../../../../shared/directives/reveal-on-scroll';

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

  protected readonly store =
    inject(PublishedContentPageStore);

  private readonly seoData: Record<
    ContentPageKey,
    ContentPageSeoData
  > = {
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
      if (this.store.error()) {
        this.seo.updatePage({
          ...seoData,
          type: 'website',
          robots: 'noindex, follow',
        });

        return;
      }

      const contentPage = this.store.contentPage();

      if (!contentPage) {
        return;
      }

      this.seo.updatePage({
        title: contentPage.titleHu,
        description: seoData.description,
        canonicalPath: seoData.canonicalPath,
        type: 'website',
      });
    });
  }

  private isContentPageKey(
    value: unknown,
  ): value is ContentPageKey {
    return value === 'exhibitions' ||
      value === 'memberships-and-awards' ||
      value === 'writings';
  }
}