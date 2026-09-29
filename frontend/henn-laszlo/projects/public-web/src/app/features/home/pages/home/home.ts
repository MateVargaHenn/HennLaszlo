import {
  ChangeDetectionStrategy,
  Component,
  computed,
  inject,
} from '@angular/core';
import { RouterLink } from '@angular/router';
import { PublishedArtworksStore } from 'artwork-data-access';
import {
  PublicWebsiteSettingsStore,
} from 'content-data-access';
import { ArtworkCard } from '../../../artworks/components/artwork-card/artwork-card';
import {
  RevealOnScroll,
} from '../../../../shared/directives/reveal-on-scroll';
import {
  SeoService,
} from '../../../../core/seo/seo.service';

@Component({
  selector: 'app-home',
  imports: [RouterLink, ArtworkCard, RevealOnScroll],
  templateUrl: './home.html',
  styleUrl: './home.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class Home {
  private readonly seo = inject(SeoService);
  protected readonly artworkStore =
    inject(PublishedArtworksStore);

  protected readonly websiteSettings =
    inject(PublicWebsiteSettingsStore);

  constructor() {
    this.seo.updatePage({
      canonicalPath: '/',
      type: 'website',
      includeSiteName: false,
    });
  }
protected readonly featuredArtworks = computed(() =>
  this.artworkStore
    .artworks()
    .filter(artwork => artwork.isFeatured)
    .sort((a, b) => a.displayOrder - b.displayOrder)
);

  protected readonly selectedArtworks = computed(() =>
  this.artworkStore.artworks().slice(0, 3)
  );

}
