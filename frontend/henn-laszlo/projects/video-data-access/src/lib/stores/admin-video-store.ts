import { computed, inject, Injectable, signal, Signal } from '@angular/core';
import { firstValueFrom, Observable } from 'rxjs';
import { rxResource } from '@angular/core/rxjs-interop';

import { AdminVideoListItem } from '../models/admin-video-list-item';
import { VideoApi } from '../services/video-api';

@Injectable()
export class AdminVideosStore {
  private readonly videoApi = inject(VideoApi);

  private readonly resource = rxResource({
    defaultValue: [] as readonly AdminVideoListItem[],

    stream: () => this.videoApi.getAdminVideos(),
  });

  readonly videos: Signal<readonly AdminVideoListItem[]> = computed(() => this.resource.value());

  readonly isLoading: Signal<boolean> = computed(() => this.resource.isLoading());

  readonly hasError: Signal<boolean> = computed(() => this.resource.error() !== undefined);

  readonly publishedCount = computed(
    () => this.videos().filter((video) => video.isPublished).length,
  );

  readonly draftCount = computed(() => this.videos().filter((video) => !video.isPublished).length);

  readonly deletionInProgressId = signal<string | null>(null);

  readonly deletionError = signal<string | null>(null);

  reload(): void {
    this.resource.reload();
  }

  private readonly publicationInProgressId = signal<string | null>(null);

  readonly publicationError = signal<string | null>(null);

  isChangingPublication(videoId: string): boolean {
    return this.publicationInProgressId() === videoId;
  }

  publish(videoId: string): Promise<void> {
    return this.changePublication(videoId, this.videoApi.publishVideo(videoId));
  }

  unpublish(videoId: string): Promise<void> {
    return this.changePublication(videoId, this.videoApi.unpublishVideo(videoId));
  }
  private async changePublication(videoId: string, operation: Observable<void>): Promise<void> {
    if (this.publicationInProgressId()) {
      return;
    }

    this.publicationInProgressId.set(videoId);

    this.publicationError.set(null);

    try {
      await firstValueFrom(operation);
      this.resource.reload();
    } catch {
      this.publicationError.set('Az állapot módosítása nem sikerült.');
    } finally {
      this.publicationInProgressId.set(null);
    }
  }

  isDeleting(videoId: string): boolean {
    return this.deletionInProgressId() === videoId;
  }

  async deleteVideo(videoId: string): Promise<boolean> {
    if (this.deletionInProgressId()) {
      return false;
    }

    this.deletionInProgressId.set(videoId);

    this.deletionError.set(null);

    try {
      await firstValueFrom(this.videoApi.deleteVideo(videoId));

      this.resource.reload();

      return true;
    } catch {
      this.deletionError.set('A videót nem sikerült törölni.');

      return false;
    } finally {
      this.deletionInProgressId.set(null);
    }
  }
}
