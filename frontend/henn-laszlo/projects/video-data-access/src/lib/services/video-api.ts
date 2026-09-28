import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { VIDEO_DATA_ACCESS_CONFIG } from '../config/video-data-access-config';
import { VideoListItem } from '../models/video-list-item';
import { AdminVideoListItem } from '../models/admin-video-list-item';

import { CreateVideoRequest } from '../models/create-video-request';
import { CreateVideoResponse } from '../models/create-video-response';
import { UpdateVideoRequest } from '../models/update-video-request';
import { AdminVideoDetails } from '../models/admin-video-details';

@Injectable({
  providedIn: 'root',
})
export class VideoApi {
  private readonly http = inject(HttpClient);

  private readonly config = inject(VIDEO_DATA_ACCESS_CONFIG);

  private readonly apiBaseUrl = this.config.apiBaseUrl.replace(/\/$/, '');

  getPublishedVideos(): Observable<readonly VideoListItem[]> {
    return this.http.get<readonly VideoListItem[]>(`${this.apiBaseUrl}/api/videos`);
  }

  getAdminVideos(): Observable<readonly AdminVideoListItem[]> {
    return this.http.get<readonly AdminVideoListItem[]>(`${this.apiBaseUrl}/api/admin/videos`);
  }

  createVideo(request: CreateVideoRequest): Observable<CreateVideoResponse> {
    return this.http.post<CreateVideoResponse>(`${this.apiBaseUrl}/api/admin/videos`, request);
  }

  publishVideo(videoId: string): Observable<void> {
    return this.http.put<void>(`${this.apiBaseUrl}/api/admin/videos/${videoId}/publish`, null);
  }

  unpublishVideo(videoId: string): Observable<void> {
    return this.http.put<void>(`${this.apiBaseUrl}/api/admin/videos/${videoId}/unpublish`, null);
  }

  getAdminVideoById(videoId: string): Observable<AdminVideoDetails> {
    return this.http.get<AdminVideoDetails>(`${this.apiBaseUrl}/api/admin/videos/${videoId}`);
  }

  updateVideo(videoId: string, request: UpdateVideoRequest): Observable<void> {
    return this.http.put<void>(`${this.apiBaseUrl}/api/admin/videos/${videoId}`, request);
  }

  deleteVideo(videoId: string): Observable<void> {
    return this.http.delete<void>(`${this.apiBaseUrl}/api/admin/videos/${videoId}`);
  }
}
