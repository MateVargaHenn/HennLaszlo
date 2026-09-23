import {
  ChangeDetectionStrategy,
  Component,
  effect,
  inject,
  ViewEncapsulation,
} from '@angular/core';
import {
  PublishedContentPageStore,
} from 'content-data-access';

import {
  SeoService,
} from '../../../../core/seo/seo.service';

@Component({
  selector: 'app-writings-page',
  templateUrl: './writings-page.html',
  styleUrl: './writings-page.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
  encapsulation: ViewEncapsulation.None,
})
export class WritingsPage {
  private readonly seo = inject(SeoService);

  protected readonly contentPageStore =
    inject(PublishedContentPageStore);

  constructor() {
    this.seo.updatePage({
      title: 'Írások',
      description:
        'Henn László András festőművész írásai, kiállításmegnyitó beszédei és művészeti gondolatai.',
      canonicalPath: '/irasok',
      type: 'website',
    });

    this.contentPageStore.load('writings');

    effect(() => {
      const contentPage =
        this.contentPageStore.contentPage();

      if (!contentPage) {
        return;
      }

      this.seo.updatePage({
        title: contentPage.titleHu,
        description:
          'Henn László András festőművész írásai, kiállításmegnyitó beszédei és művészeti gondolatai.',
        canonicalPath: '/irasok',
        type: 'website',
      });
    });
  }
}