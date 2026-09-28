import { ChangeDetectionStrategy, Component, inject, signal } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { firstValueFrom } from 'rxjs';
import type { CreateVideoRequest } from 'video-data-access';
import { VideoApi } from 'video-data-access';

import { VideoForm } from '../../components/video-form/video-form';

@Component({
  selector: 'app-admin-create-video',
  imports: [VideoForm, RouterLink],
  templateUrl: './admin-create-video.html',
  styleUrl: './admin-create-video.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AdminCreateVideo {
  private readonly videoApi = inject(VideoApi);

  private readonly router = inject(Router);

  protected readonly isSubmitting = signal(false);

  protected readonly submitError = signal<string | null>(null);

  protected async save(request: CreateVideoRequest): Promise<void> {
    if (this.isSubmitting()) {
      return;
    }

    this.isSubmitting.set(true);
    this.submitError.set(null);

    try {
      await firstValueFrom(this.videoApi.createVideo(request));

      await this.router.navigate(['/videos']);
    } catch {
      this.submitError.set('A videó mentése nem sikerült. Próbáld újra.');
    } finally {
      this.isSubmitting.set(false);
    }
  }

  protected cancel(): void {
    void this.router.navigate(['/videos']);
  }
}
