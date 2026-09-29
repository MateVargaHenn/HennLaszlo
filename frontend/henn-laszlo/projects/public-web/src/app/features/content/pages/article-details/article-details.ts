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

import {
  SsrResponseService,
} from '../../../../core/http/ssr-response.service';

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
  private readonly response = inject(SsrResponseService);

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
        const slug =
          this.route.snapshot.paramMap
            .get('slug')
            ?.trim() ?? '';

        const canonicalPath =
          `/irasok/${
            encodeURIComponent(slug)
          }`;

        if (
          this.articleStore.isNotFound()
        ) {
          this.response.setNotFound();

          this.seo.updatePage({
            title:
              'Az írás nem található',
            description:
              'A keresett írás nem található.',
            canonicalPath,
            type: 'article',
            robots:
              'noindex, nofollow, noarchive',
          });

          return;
        }

        if (this.articleStore.error()) {
          this.response.setStatus(500);

          this.seo.updatePage({
            title:
              'Az írás nem érhető el',
            description:
              'A keresett írás jelenleg nem érhető el.',
            canonicalPath,
            type: 'article',
            robots:
              'noindex, follow',
          });

          return;
        }

        const article =
          this.articleStore.article();

        if (!article) {
          return;
        }

        // Innentől marad a már elkészített
        // description + canonicalUrl +
        // structuredData kód.
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