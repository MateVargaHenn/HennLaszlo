import { DOCUMENT } from '@angular/common';
import {
  inject,
  Injectable,
} from '@angular/core';
import {
  Meta,
  Title,
} from '@angular/platform-browser';
import {
  SEO_SITE_URL,
} from './seo.config';
import {
  StructuredData,
  StructuredDataService,
} from './structured-data.service';
import {
  SeoBreadcrumb,
} from '../../shared/interfaces/seo-breadcrumb.interface';

interface SeoPageMetadata {
  readonly title?: string;
  readonly description?: string;
  readonly canonicalPath: string;
  readonly type?: 'website' | 'article';
  readonly robots?: string;
  readonly imagePath?: string;
  readonly imageAlt?: string;
  readonly includeSiteName?: boolean;
  readonly breadcrumbs?:
  readonly SeoBreadcrumb[];
  readonly structuredData?:
  | StructuredData
  | readonly StructuredData[];
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
  private readonly structuredData =
  inject(StructuredDataService);

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
    inject(SEO_SITE_URL)
      .replace(/\/+$/, '');

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
        this.toAbsoluteUrl(
          metadata.canonicalPath,
        );

    const imageUrl =
      this.toAbsoluteUrl(
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

        this.updateBreadcrumbs(
      metadata,
      pageTitle,
      canonicalUrl,
    );

    if (metadata.structuredData) {
      this.structuredData.setGroup(
        'page',
        metadata.structuredData,
      );
    } else {
      this.structuredData.removeGroup(
        'page',
      );
    }
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

  toAbsoluteUrl(
    pathOrUrl: string,
  ): string {
    return new URL(
      pathOrUrl,
      `${this.siteUrl}/`,
    ).toString();
  }

  private updateBreadcrumbs(
    metadata: SeoPageMetadata,
    pageTitle: string,
    canonicalUrl: string,
  ): void {
    if (
      metadata.canonicalPath === '/'
    ) {
      this.structuredData.removeGroup(
        'breadcrumb',
      );

      return;
    }

    const breadcrumbs =
      metadata.breadcrumbs ?? [
        {
          name: 'Kezdőlap',
          path: '/',
        },
        {
          name: pageTitle,
          path:
            metadata.canonicalPath,
        },
      ];

    if (breadcrumbs.length < 2) {
      this.structuredData.removeGroup(
        'breadcrumb',
      );

      return;
    }

    this.structuredData.setGroup(
      'breadcrumb',
      {
        '@type': 'BreadcrumbList',
        '@id':
          `${canonicalUrl}#breadcrumb`,
        itemListElement:
          breadcrumbs.map(
            (breadcrumb, index) => ({
              '@type': 'ListItem',
              position: index + 1,
              name: breadcrumb.name,
              item:
                this.toAbsoluteUrl(
                  breadcrumb.path,
                ),
            }),
          ),
      },
    );
  }
}
