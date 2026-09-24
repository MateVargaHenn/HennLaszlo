import {
  ChangeDetectionStrategy,
  Component,
  computed,
  inject,
} from '@angular/core';
import { RouterLink } from '@angular/router';
import { PublishedArtworksStore } from 'artwork-data-access';
import { ArtworkCard } from '../../../artworks/components/artwork-card/artwork-card';
import {
  RevealOnScroll,
} from '../../../../shared/directives/reveal-on-scroll';

@Component({
  selector: 'app-home',
  imports: [RouterLink, ArtworkCard, RevealOnScroll],
  templateUrl: './home.html',
  styleUrl: './home.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class Home {
  protected readonly artworkStore =
    inject(PublishedArtworksStore);

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