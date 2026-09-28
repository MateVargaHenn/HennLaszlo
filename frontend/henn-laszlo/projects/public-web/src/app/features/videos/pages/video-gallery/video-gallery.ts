import { ChangeDetectionStrategy, Component, computed, inject } from '@angular/core';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
import { PublishedVideosStore } from 'video-data-access';
import type { VideoListItem } from 'video-data-access';

import { RevealOnScroll } from '../../../../shared/directives/reveal-on-scroll';

interface DisplayVideo {
  readonly video: VideoListItem;
  readonly embedUrl: SafeResourceUrl | null;
}

@Component({
  selector: 'app-video-gallery',
  imports: [RevealOnScroll],
  providers: [PublishedVideosStore],
  templateUrl: './video-gallery.html',
  styleUrl: './video-gallery.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class VideoGallery {
  protected readonly store = inject(PublishedVideosStore);

  private readonly sanitizer = inject(DomSanitizer);

  protected readonly videos = computed((): readonly DisplayVideo[] =>
    this.store.videos().map((video) => ({
      video,
      embedUrl: this.createYouTubeEmbedUrl(video.videoUrl),
    })),
  );

  private createYouTubeEmbedUrl(videoUrl: string): SafeResourceUrl | null {
    const videoId = this.getYouTubeVideoId(videoUrl);

    if (!videoId) {
      return null;
    }

    return this.sanitizer.bypassSecurityTrustResourceUrl(
      `https://www.youtube-nocookie.com/embed/${videoId}`,
    );
  }

  private getYouTubeVideoId(videoUrl: string): string | null {
    try {
      const url = new URL(videoUrl);
      const hostname = url.hostname.toLowerCase().replace(/^www\./, '');

      let candidate: string | null = null;

      if (hostname === 'youtu.be') {
        candidate = url.pathname.split('/').filter(Boolean)[0] ?? null;
      } else if (
        hostname === 'youtube.com' ||
        hostname === 'm.youtube.com' ||
        hostname === 'youtube-nocookie.com'
      ) {
        if (url.pathname === '/watch') {
          candidate = url.searchParams.get('v');
        } else {
          const segments = url.pathname.split('/').filter(Boolean);

          if (['embed', 'shorts', 'live'].includes(segments[0] ?? '')) {
            candidate = segments[1] ?? null;
          }
        }
      }

      return candidate && /^[A-Za-z0-9_-]{11}$/.test(candidate) ? candidate : null;
    } catch {
      return null;
    }
  }
}
