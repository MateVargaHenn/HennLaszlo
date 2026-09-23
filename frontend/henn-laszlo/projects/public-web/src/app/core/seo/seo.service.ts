import {
  DOCUMENT,
} from '@angular/common';
import {
  inject,
  Injectable,
} from '@angular/core';
import {
  Meta,
  Title,
} from '@angular/platform-browser';

export interface SeoPageData {
  readonly title: string;
  readonly description: string;
  readonly canonicalPath: string;
  readonly type?: 'website' | 'article';
  readonly robots?: 'index, follow' | 'noindex, follow';
}

@Injectable({
  providedIn: 'root',
})
export class SeoService {
  private readonly document = inject(DOCUMENT);
  private readonly meta = inject(Meta);
  private readonly title = inject(Title);

  private readonly siteName =
    'Henn László András';

  private readonly siteUrl =
    'https://hennlaszlo.hu';

  updatePage(data: SeoPageData): void {
    const documentTitle =
      `${data.title} | ${this.siteName}`;

    const canonicalUrl = new URL(
      data.canonicalPath,
      this.siteUrl,
    ).toString();

    this.title.setTitle(documentTitle);

    this.meta.updateTag({
      name: 'description',
      content: data.description,
    });

    this.meta.updateTag({
    name: 'robots',
    content: data.robots ?? 'index, follow',
    });

    this.meta.updateTag({
      property: 'og:title',
      content: documentTitle,
    });

    this.meta.updateTag({
      property: 'og:description',
      content: data.description,
    });

    this.meta.updateTag({
      property: 'og:type',
      content: data.type ?? 'website',
    });

    this.meta.updateTag({
      property: 'og:url',
      content: canonicalUrl,
    });

    this.meta.updateTag({
      property: 'og:site_name',
      content: this.siteName,
    });

    this.meta.updateTag({
      property: 'og:locale',
      content: 'hu_HU',
    });

    this.meta.updateTag({
      name: 'twitter:card',
      content: 'summary',
    });

    this.meta.updateTag({
      name: 'twitter:title',
      content: documentTitle,
    });

    this.meta.updateTag({
      name: 'twitter:description',
      content: data.description,
    });

    this.updateCanonicalUrl(canonicalUrl);
  }

  private updateCanonicalUrl(
    canonicalUrl: string,
  ): void {
    let canonicalElement =
      this.document.head.querySelector<HTMLLinkElement>(
        'link[rel="canonical"]',
      );

    if (!canonicalElement) {
      canonicalElement =
        this.document.createElement('link');

      canonicalElement.rel = 'canonical';

      this.document.head.appendChild(
        canonicalElement,
      );
    }

    canonicalElement.href = canonicalUrl;
  }
}