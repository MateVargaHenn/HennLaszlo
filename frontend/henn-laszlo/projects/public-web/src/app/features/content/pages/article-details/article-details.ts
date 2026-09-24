import {
  ChangeDetectionStrategy,
  Component,
  DestroyRef,
  effect,
  inject,
  ViewEncapsulation,
} from '@angular/core';
import {
  takeUntilDestroyed,
} from '@angular/core/rxjs-interop';
import {
  ActivatedRoute,
  RouterLink,
} from '@angular/router';
import {
  PublishedArticleDetailsStore,
} from 'content-data-access';
import {
  distinctUntilChanged,
  filter,
  map,
} from 'rxjs';

import {
  SeoService,
} from '../../../../core/seo/seo.service';

import {
  RevealOnScroll,
} from '../../../../shared/directives/reveal-on-scroll';

@Component({
  selector: 'app-article-details',
  imports: [
    RouterLink,
    RevealOnScroll,
  ],
  templateUrl: './article-details.html',
  styleUrl: './article-details.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
  encapsulation: ViewEncapsulation.None,
})
export class ArticleDetails {
  private readonly route = inject(ActivatedRoute);
  private readonly destroyRef = inject(DestroyRef);
  private readonly seo = inject(SeoService);

  protected readonly articleStore =
    inject(PublishedArticleDetailsStore);

  constructor() {
    this.route.paramMap
      .pipe(
        map((parameters) => parameters.get('slug')),
        filter(
          (slug): slug is string =>
            slug !== null,
        ),
        map((slug) => slug.trim()),
        filter((slug) => slug.length > 0),
        distinctUntilChanged(),
        takeUntilDestroyed(this.destroyRef),
      )
      .subscribe((slug) => {
        this.setLoadingSeo(slug);
        this.articleStore.load(slug);
      });

    effect(() => {
      const article = this.articleStore.article();

      if (!article) {
        return;
      }

      const description =
        article.summaryHu?.trim() ||
        `${article.titleHu}. Írás Henn László András festőművész honlapján.`;

      this.seo.updatePage({
        title: article.titleHu,
        description,
        canonicalPath:
          `/irasok/${encodeURIComponent(article.slug)}`,
        type: 'article',
      });
    });
  }

  private setLoadingSeo(slug: string): void {
    this.seo.updatePage({
      title: 'Írás',
      description:
        'Henn László András festőművész írása.',
      canonicalPath:
        `/irasok/${encodeURIComponent(slug)}`,
      type: 'article',
      robots: 'noindex, follow',
    });
  }
}