import { computed, inject, Injectable, Signal } from '@angular/core';
import { rxResource } from '@angular/core/rxjs-interop';

import { VideoListItem } from '../models/video-list-item';
import { VideoApi } from '../services/video-api';

@Injectable()
export class PublishedVideosStore {
  private readonly videoApi = inject(VideoApi);

  private readonly resource = rxResource({
    defaultValue: [] as readonly VideoListItem[],

    stream: () => this.videoApi.getPublishedVideos(),
  });

  readonly videos: Signal<readonly VideoListItem[]> = computed(() => this.resource.value());

  readonly isLoading: Signal<boolean> = computed(() => this.resource.isLoading());

  readonly hasError: Signal<boolean> = computed(() => this.resource.error() !== undefined);

  reload(): void {
    this.resource.reload();
  }
}
