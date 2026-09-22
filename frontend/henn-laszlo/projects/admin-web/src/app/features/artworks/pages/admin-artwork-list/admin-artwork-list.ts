import { DatePipe } from '@angular/common';
import {
  ChangeDetectionStrategy,
  Component,
  DestroyRef,
  inject,
  signal,
} from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { RouterLink } from '@angular/router';
import {
  AdminArtworksStore,
  ArtworkApi,
  type AdminArtworkListItem,
} from 'artwork-data-access';
import { finalize } from 'rxjs';

@Component({
  selector: 'app-admin-artwork-list',
  imports: [
    DatePipe,
    RouterLink,
  ],
  providers: [
    AdminArtworksStore,
  ],
  templateUrl: './admin-artwork-list.html',
  styleUrl: './admin-artwork-list.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AdminArtworkList {
  private readonly artworkApi = inject(ArtworkApi);
  private readonly destroyRef = inject(DestroyRef);

  protected readonly store =
    inject(AdminArtworksStore);

  protected readonly artworkPendingDeletion =
    signal<AdminArtworkListItem | null>(null);

  protected readonly deletingArtworkId =
    signal<string | null>(null);

  protected readonly deleteError =
    signal<string | null>(null);

  protected requestDelete(
    artwork: AdminArtworkListItem
  ): void {
    if (artwork.isPublished) {
      return;
    }

    this.deleteError.set(null);
    this.artworkPendingDeletion.set(artwork);
  }

  protected cancelDelete(): void {
    if (this.deletingArtworkId()) {
      return;
    }

    this.artworkPendingDeletion.set(null);
    this.deleteError.set(null);
  }

  protected confirmDelete(): void {
    const artwork = this.artworkPendingDeletion();

    if (!artwork || this.deletingArtworkId()) {
      return;
    }

    this.deletingArtworkId.set(artwork.id);
    this.deleteError.set(null);

    this.artworkApi
      .deleteArtwork(artwork.id)
      .pipe(
        takeUntilDestroyed(this.destroyRef),
        finalize(() => {
          this.deletingArtworkId.set(null);
        })
      )
      .subscribe({
        next: () => {
          this.artworkPendingDeletion.set(null);
          this.store.reload();
        },
        error: () => {
          this.deleteError.set(
            'A mű törlése nem sikerült.'
          );
        },
      });
  }

  protected publishArtwork(
    artworkId: string
  ): void {
    void this.store.publish(artworkId);
  }

  protected unpublishArtwork(
    artworkId: string
  ): void {
    void this.store.unpublish(artworkId);
  }
}