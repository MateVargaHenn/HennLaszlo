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
  readonly title?: string;
  readonly description?: string;
  readonly canonicalPath: string;
  readonly type?: 'website' | 'article';
  readonly robots?: string;
  readonly imagePath?: string;
  readonly imageAlt?: string;
  readonly includeSiteName?: boolean;
}

interface WebsiteSeoDefaults {
  readonly siteName: string;
  readonly title: string;
  readonly description: string;
}

@Injectable({
  providedIn: 'root',
})
export class SeoService {
  private readonly document = inject(DOCUMENT);
  private readonly meta = inject(Meta);
  private readonly title = inject(Title);

  private defaults: WebsiteSeoDefaults = {
    siteName: 'Henn László András',
    title:
      'Henn László András | Festőművész és grafikus',
    description:
      'Henn László András Galyasi Miklós ' +
      'nívódíjas festőművész és grafikus ' +
      'hivatalos oldala.',
  };

  private currentMetadata:
    SeoPageMetadata | null = null;

  private readonly siteUrl =
    'https://hennlaszlo.hu';

  private readonly defaultImagePath =
    '/video/medistacio-poster.webp';

  configureDefaults(
    defaults: WebsiteSeoDefaults,
  ): void {
    this.defaults = defaults;

    if (this.currentMetadata) {
      this.applyPageMetadata(
        this.currentMetadata,
      );
    }
  }

  updatePage(metadata: SeoPageMetadata): void {
    this.currentMetadata = metadata;
    this.applyPageMetadata(metadata);
  }

  private applyPageMetadata(
    metadata: SeoPageMetadata,
  ): void {
    const pageTitle =
      metadata.title ??
      this.defaults.title;

    const description =
      metadata.description ??
      this.defaults.description;

    const documentTitle =
      metadata.includeSiteName === false
        ? pageTitle
        : `${pageTitle} | ${this.defaults.siteName}`;

    const canonicalUrl =
      this.resolveUrl(metadata.canonicalPath);

    const imageUrl =
      this.resolveUrl(
        metadata.imagePath ??
        this.defaultImagePath,
      );

    const imageAlt =
      metadata.imageAlt ??
      `${this.defaults.siteName} festőművész és grafikus`;

    this.title.setTitle(documentTitle);

    this.updateName(
      'description',
      description,
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
      description,
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
      this.defaults.siteName,
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
      description,
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
