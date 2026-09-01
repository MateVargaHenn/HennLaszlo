import {
  ChangeDetectionStrategy,
  Component,
  computed,
  inject,
} from '@angular/core';
import { RouterLink } from '@angular/router';
import { PublishedArtworksStore } from 'artwork-data-access';
import { ArtworkCard } from '../../../artworks/components/artwork-card/artwork-card';

@Component({
  selector: 'app-home',
  imports: [RouterLink, ArtworkCard],
  templateUrl: './home.html',
  styleUrl: './home.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class Home {
  protected readonly artworkStore =
    inject(PublishedArtworksStore);

  protected readonly featuredArtwork = computed(() => {
    const artworks = this.artworkStore.artworks();

    return (
      artworks.find(artwork => artwork.isFeatured) ??
      artworks[0] ??
      null
    );
  });

  protected readonly selectedArtworks = computed(() =>
  this.artworkStore.artworks().slice(0, 3)
  );

}