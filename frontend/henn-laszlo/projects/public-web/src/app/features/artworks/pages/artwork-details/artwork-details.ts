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
        this.updateArtworkSeo(artwork);
        return;
      }

      if (this.store.error()) {
        this.seo.updatePage({
          title: 'A mű nem érhető el',
          description:
            'A keresett műalkotás adatlapja jelenleg nem érhető el.',
          canonicalPath:
            `/muvek/${encodeURIComponent(
              this.artworkId(),
            )}`,
          type: 'website',
          robots: 'noindex, follow',
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

    this.seo.updatePage({
      title,
      description:
        this.createArtworkDescription(artwork),
      canonicalPath:
        `/muvek/${encodeURIComponent(
          artwork.id,
        )}`,
      type: 'website',
      imagePath:
        this.store.getImageUrl(artwork.id),
      imageAlt: artwork.titleHu,
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