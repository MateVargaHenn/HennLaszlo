import {
  ChangeDetectionStrategy,
  Component,
  effect,
  inject,
  input,
} from '@angular/core';
import { RouterLink } from '@angular/router';
import { ArtworkDetailsStore } from 'artwork-data-access';
import { RevealOnScroll } from '../../../../shared/directives/reveal-on-scroll';

@Component({
  selector: 'app-artwork-details',
  imports: [RouterLink,RevealOnScroll],
  providers: [ArtworkDetailsStore],
  templateUrl: './artwork-details.html',
  styleUrl: './artwork-details.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ArtworkDetails {
  readonly artworkId = input.required<string>();

  protected readonly store =
    inject(ArtworkDetailsStore);

  constructor() {
    effect(() => {
      this.store.setArtworkId(this.artworkId());
    });
  }
}