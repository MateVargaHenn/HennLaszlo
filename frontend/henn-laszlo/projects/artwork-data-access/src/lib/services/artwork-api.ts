import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { ARTWORK_DATA_ACCESS_CONFIG } from '../config/artwork-data-access.config';
import { ArtworkListItem } from '../models/artwork-list-item';
import { ArtworkDetails } from '../models/artwork-details';
import { AdminArtworkListItem } from '../models/admin-artwork-list-item';
import { CreateArtworkRequest, CreateArtworkResponse } from '../../public-api';
import { UploadFileResponse } from '../models/upload-file-response';


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
  
  getArtworkById(
    artworkId: string
  ): Observable<ArtworkDetails> {
    return this.http.get<ArtworkDetails>(
    `${this.config.apiBaseUrl}/api/artworks/${encodeURIComponent(artworkId)}`
  );
  }

  getAdminArtworks():
    Observable<readonly AdminArtworkListItem[]> {
    return this.http.get<readonly AdminArtworkListItem[]>(
      `${this.config.apiBaseUrl}/api/admin/artworks`
    );
  }

  createArtwork(
    request: CreateArtworkRequest
  ): Observable<CreateArtworkResponse> {
    return this.http.post<CreateArtworkResponse>(
      `${this.config.apiBaseUrl}/api/admin/artworks`,
      request
    );
  }

  uploadFile(
    file: File
  ): Observable<UploadFileResponse> {
    const formData = new FormData();

    formData.append(
      'file',
      file,
      file.name
    );

    return this.http.post<UploadFileResponse>(
      `${this.config.apiBaseUrl}/api/admin/files`,
      formData
    );
  }

  attachImage(
    artworkId: string,
    fileId: string
  ): Observable<void> {
    if (!artworkId) {
      throw new Error(
        'A mű azonosítója hiányzik a kép csatolásához.'
      );
    }

    return this.http.put<void>(
      `${this.config.apiBaseUrl}/api/admin/artworks/${artworkId}/image`,
      { fileId }
    );
  }

  getAdminArtworkImageUrl(
    artworkId: string
  ): string {
    return `${this.config.apiBaseUrl}/api/admin/artworks/${artworkId}/image`;
  }
}