import {
  ChangeDetectionStrategy,
  Component,
  effect,
  inject,
  input,
} from '@angular/core';
import { RouterLink } from '@angular/router';
import { ArtworkDetails as ArtworkDetailsModel, ArtworkDetailsStore } from 'artwork-data-access';
import { RevealOnScroll } from '../../../../shared/directives/reveal-on-scroll';
import {
  SeoService,
} from '../../../../core/seo/seo.service';
import {
  SsrResponseService,
} from '../../../../core/http/ssr-response.service';

@Component({
  selector: 'app-artwork-details',
  imports: [RouterLink,RevealOnScroll],
  providers: [ArtworkDetailsStore],
  templateUrl: './artwork-details.html',
  styleUrl: './artwork-details.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ArtworkDetails {
  private readonly seo =
  inject(SeoService);
  readonly artworkId = input.required<string>();

  protected readonly store =
    inject(ArtworkDetailsStore);

  private readonly response =
    inject(SsrResponseService);

  constructor() {
    effect(() => {
      this.store.setArtworkId(
        this.artworkId(),
      );
    });

    effect(() => {
      const artwork =
        this.store.artwork();

      if (artwork) {
        this.updateArtworkSeo(
          artwork,
        );

        return;
      }

      const canonicalPath =
        `/muvek/${
          encodeURIComponent(
            this.artworkId(),
          )
        }`;

      if (this.store.isNotFound()) {
        this.response.setNotFound();

        this.seo.updatePage({
          title:
            'A mű nem található',
          description:
            'A keresett műalkotás nem található.',
          canonicalPath,
          type: 'website',
          robots:
            'noindex, nofollow, noarchive',
        });

        return;
      }

      if (this.store.error()) {
        this.response.setStatus(500);

        this.seo.updatePage({
          title:
            'A mű nem érhető el',
          description:
            'A keresett műalkotás adatlapja jelenleg nem érhető el.',
          canonicalPath,
          type: 'website',
          robots:
            'noindex, follow',
        });
      }
    });
  }
  
  private updateArtworkSeo(
    artwork: ArtworkDetailsModel,
  ): void {
    const title =
      artwork.year !== null
        ? `${artwork.titleHu} (${artwork.year})`
        : artwork.titleHu;

    const description =
      this.createArtworkDescription(
        artwork,
      );

    const canonicalPath =
      `/muvek/${
        encodeURIComponent(
          artwork.id,
        )
      }`;

    const canonicalUrl =
      this.seo.toAbsoluteUrl(
        canonicalPath,
      );

    const imagePath =
      this.store.getImageUrl(
        artwork.id,
      );

    const imageUrl =
      this.seo.toAbsoluteUrl(
        imagePath,
      );

    const personId =
      `${
        this.seo.toAbsoluteUrl('/')
      }#person`;

    this.seo.updatePage({
      title,
      description,
      canonicalPath,
      type: 'website',
      imagePath,
      imageAlt: artwork.titleHu,
      breadcrumbs: [
        {
          name: 'Kezdőlap',
          path: '/',
        },
        {
          name: 'Művek',
          path: '/muvek',
        },
        {
          name: artwork.titleHu,
          path: canonicalPath,
        },
      ],
      structuredData: {
        '@type': 'VisualArtwork',
        '@id':
          `${canonicalUrl}#artwork`,
        url: canonicalUrl,
        name: artwork.titleHu,
        description,
        image: imageUrl,
        inLanguage: 'hu-HU',
        creator: {
          '@id': personId,
        },
        ...(artwork.year !== null
          ? {
              dateCreated:
                artwork.year.toString(),
            }
          : {}),
        ...(artwork.techniqueHu
          ? {
              artMedium:
                artwork.techniqueHu,
            }
          : {}),
        ...(
          artwork.widthCm !== null &&
          artwork.heightCm !== null
            ? {
                width: {
                  '@type':
                    'QuantitativeValue',
                  value:
                    artwork.widthCm,
                  unitText: 'cm',
                },
                height: {
                  '@type':
                    'QuantitativeValue',
                  value:
                    artwork.heightCm,
                  unitText: 'cm',
                },
              }
            : {}
        ),
      },
    });
  }

  private createArtworkDescription(
    artwork: ArtworkDetailsModel,
  ): string {
    const details: string[] = [];

    if (artwork.year !== null) {
      details.push(
        artwork.year.toString(),
      );
    }

    if (artwork.techniqueHu) {
      details.push(
        artwork.techniqueHu,
      );
    }

    if (
      artwork.widthCm !== null &&
      artwork.heightCm !== null
    ) {
      details.push(
        `${artwork.widthCm} × ` +
        `${artwork.heightCm} cm`,
      );
    }

    const fallback =
      `„${artwork.titleHu}” című alkotás` +
      (
        details.length > 0
          ? ` – ${details.join(', ')}`
          : ''
      ) +
      '. Henn László András műve.';

    return this.truncateDescription(
      artwork.descriptionHu?.trim() ||
        fallback,
    );
  }

  private truncateDescription(
    description: string,
  ): string {
    const maximumLength = 160;

    if (
      description.length <= maximumLength
    ) {
      return description;
    }

    return `${description
      .slice(0, maximumLength - 1)
      .trimEnd()}…`;
  }
}