import { computed, inject, Injectable, Signal, signal } from '@angular/core';
import { rxResource } from '@angular/core/rxjs-interop';
import { of } from 'rxjs';

import { AdminVideoDetails } from '../models/admin-video-details';
import { VideoApi } from '../services/video-api';

@Injectable()
export class AdminVideoDetailsStore {
  private readonly videoApi = inject(VideoApi);

  private readonly videoId = signal<string | null>(null);

  private readonly resource = rxResource<AdminVideoDetails | null, string | null>({
    params: () => this.videoId(),

    stream: ({ params: videoId }) => {
      if (!videoId) {
        return of(null);
      }

      return this.videoApi.getAdminVideoById(videoId);
    },

    defaultValue: null,
  });

  readonly video: Signal<AdminVideoDetails | null> = computed(() => this.resource.value());

  readonly isLoading = this.resource.isLoading;

  readonly hasError = computed(() => this.resource.error() !== undefined);

  load(videoId: string): void {
    this.videoId.set(videoId);
  }

  reload(): void {
    this.resource.reload();
  }
}
