import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { ARTWORK_DATA_ACCESS_CONFIG } from '../config/artwork-data-access.config';
import { ArtworkListItem } from '../models/artwork-list-item';

@Injectable({
  providedIn: 'root',
})
export class ArtworkApi {
  private readonly http = inject(HttpClient);
  private readonly config = inject(
    ARTWORK_DATA_ACCESS_CONFIG,
  );

  private readonly apiBaseUrl =
    this.config.apiBaseUrl.replace(/\/+$/, '');

  getPublishedArtworks(): Observable<
    readonly ArtworkListItem[]
  > {
    return this.http.get<readonly ArtworkListItem[]>(
      `${this.apiBaseUrl}/api/artworks`,
    );
  }

  getArtworkImageUrl(artworkId: string): string {
    return `${this.apiBaseUrl}/api/artworks/${encodeURIComponent(artworkId)}/image`;
  }
}