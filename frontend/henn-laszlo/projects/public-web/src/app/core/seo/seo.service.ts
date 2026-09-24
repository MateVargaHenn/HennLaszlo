import { DOCUMENT } from '@angular/common';
import {
  inject,
  Injectable,
} from '@angular/core';
import {
  Meta,
  Title,
} from '@angular/platform-browser';

interface SeoPageMetadata {
  readonly title: string;
  readonly description: string;
  readonly canonicalPath: string;
  readonly type?: 'website' | 'article';
  readonly robots?: string;
  readonly imagePath?: string;
  readonly imageAlt?: string;
  readonly includeSiteName?: boolean;
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

  private readonly defaultImagePath =
    '/video/medistacio-poster.webp';

  private readonly defaultImageAlt =
    'Henn László András festőművész és grafikus';

  updatePage(metadata: SeoPageMetadata): void {
    const documentTitle =
      metadata.includeSiteName === false
        ? metadata.title
        : `${metadata.title} | ${this.siteName}`;

    const canonicalUrl =
      this.resolveUrl(metadata.canonicalPath);

    const imageUrl =
      this.resolveUrl(
        metadata.imagePath ??
        this.defaultImagePath,
      );

    const imageAlt =
      metadata.imageAlt ??
      this.defaultImageAlt;

    this.title.setTitle(documentTitle);

    this.updateName(
      'description',
      metadata.description,
    );

    this.updateName(
      'robots',
      metadata.robots ?? 'index, follow',
    );

    this.updateProperty(
      'og:title',
      documentTitle,
    );

    this.updateProperty(
      'og:description',
      metadata.description,
    );

    this.updateProperty(
      'og:type',
      metadata.type ?? 'website',
    );

    this.updateProperty(
      'og:url',
      canonicalUrl,
    );

    this.updateProperty(
      'og:site_name',
      this.siteName,
    );

    this.updateProperty(
      'og:locale',
      'hu_HU',
    );

    this.updateProperty(
      'og:image',
      imageUrl,
    );

    this.updateProperty(
      'og:image:secure_url',
      imageUrl,
    );

    this.updateProperty(
      'og:image:alt',
      imageAlt,
    );

    this.updateName(
      'twitter:card',
      'summary_large_image',
    );

    this.updateName(
      'twitter:title',
      documentTitle,
    );

    this.updateName(
      'twitter:description',
      metadata.description,
    );

    this.updateName(
      'twitter:image',
      imageUrl,
    );

    this.updateName(
      'twitter:image:alt',
      imageAlt,
    );

    this.updateCanonicalUrl(canonicalUrl);
  }

  private updateName(
    name: string,
    content: string,
  ): void {
    this.meta.updateTag({
      name,
      content,
    });
  }

  private updateProperty(
    property: string,
    content: string,
  ): void {
    this.meta.updateTag({
      property,
      content,
    });
  }

  private updateCanonicalUrl(
    canonicalUrl: string,
  ): void {
    let canonicalLink =
      this.document.head.querySelector<HTMLLinkElement>(
        'link[rel="canonical"]',
      );

    if (!canonicalLink) {
      canonicalLink =
        this.document.createElement('link');

      canonicalLink.rel = 'canonical';

      this.document.head.appendChild(
        canonicalLink,
      );
    }

    canonicalLink.href = canonicalUrl;
  }

  private resolveUrl(pathOrUrl: string): string {
    return new URL(
      pathOrUrl,
      `${this.siteUrl}/`,
    ).toString();
  }
}