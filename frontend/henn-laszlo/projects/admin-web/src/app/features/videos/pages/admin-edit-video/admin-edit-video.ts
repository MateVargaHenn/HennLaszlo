import { ChangeDetectionStrategy, Component, inject, signal } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { firstValueFrom } from 'rxjs';
import { AdminVideoDetailsStore, VideoApi } from 'video-data-access';
import type { CreateVideoRequest, UpdateVideoRequest } from 'video-data-access';

import { VideoForm } from '../../components/video-form/video-form';

@Component({
  selector: 'app-admin-edit-video',
  imports: [VideoForm],
  providers: [AdminVideoDetailsStore],
  templateUrl: './admin-edit-video.html',
  styleUrl: './admin-edit-video.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AdminEditVideo {
  private readonly route = inject(ActivatedRoute);

  private readonly router = inject(Router);

  private readonly videoApi = inject(VideoApi);

  protected readonly store = inject(AdminVideoDetailsStore);

  private readonly videoId = this.route.snapshot.paramMap.get('videoId') ?? '';

  protected readonly isSubmitting = signal(false);

  protected readonly submitError = signal<string | null>(null);

  constructor() {
    if (!this.videoId) {
      void this.router.navigate(['/videos']);
      return;
    }

    this.store.load(this.videoId);
  }

  protected async save(value: CreateVideoRequest): Promise<void> {
    if (this.isSubmitting()) {
      return;
    }

    const request: UpdateVideoRequest = value;

    this.isSubmitting.set(true);
    this.submitError.set(null);

    try {
      await firstValueFrom(this.videoApi.updateVideo(this.videoId, request));

      await this.router.navigate(['/videos']);
    } catch {
      this.submitError.set('A videó módosításait nem sikerült menteni.');
    } finally {
      this.isSubmitting.set(false);
    }
  }

  protected cancel(): void {
    void this.router.navigate(['/videos']);
  }
}
