import { DatePipe } from '@angular/common';
import { ChangeDetectionStrategy, Component, inject, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { AdminVideosStore } from 'video-data-access';
import type { AdminVideoListItem } from 'video-data-access';

@Component({
  selector: 'app-admin-video-list',
  imports: [DatePipe, RouterLink],
  providers: [AdminVideosStore],
  templateUrl: './admin-video-list.html',
  styleUrl: './admin-video-list.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AdminVideoList {
  protected readonly store = inject(AdminVideosStore);

  protected readonly videoPendingDeletion = signal<AdminVideoListItem | null>(null);

  protected publishVideo(videoId: string): void {
    void this.store.publish(videoId);
  }

  protected unpublishVideo(videoId: string): void {
    void this.store.unpublish(videoId);
  }

  protected requestDeletion(video: AdminVideoListItem): void {
    if (video.isPublished) {
      return;
    }

    this.videoPendingDeletion.set(video);
  }

  protected cancelDeletion(): void {
    const video = this.videoPendingDeletion();

    if (video && this.store.isDeleting(video.id)) {
      return;
    }

    this.videoPendingDeletion.set(null);
  }

  protected async confirmDeletion(): Promise<void> {
    const video = this.videoPendingDeletion();

    if (!video) {
      return;
    }

    const deleted = await this.store.deleteVideo(video.id);

    if (deleted) {
      this.videoPendingDeletion.set(null);
    }
  }
}
