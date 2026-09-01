import {
  ChangeDetectionStrategy,
  Component,
  inject,
} from '@angular/core';
import { DatePipe } from '@angular/common';
import { AdminArtworksStore } from 'artwork-data-access';

@Component({
  selector: 'app-admin-artwork-list',
  imports: [DatePipe],
  templateUrl: './admin-artwork-list.html',
  styleUrl: './admin-artwork-list.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AdminArtworkList {
  protected readonly store =
    inject(AdminArtworksStore);
}