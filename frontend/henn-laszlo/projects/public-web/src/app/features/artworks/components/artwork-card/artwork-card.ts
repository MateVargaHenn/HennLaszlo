import {
  ChangeDetectionStrategy,
  Component,
  input,
} from '@angular/core';
import { ArtworkListItem } from 'artwork-data-access';

@Component({
  selector: 'app-artwork-card',
  imports: [],
  templateUrl: './artwork-card.html',
  styleUrl: './artwork-card.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ArtworkCard {
  readonly artwork = input.required<ArtworkListItem>();
  readonly imageUrl = input.required<string>();
  readonly priority = input(false);
}