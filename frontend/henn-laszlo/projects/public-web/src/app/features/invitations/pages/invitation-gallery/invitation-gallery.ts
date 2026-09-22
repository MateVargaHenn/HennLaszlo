import {
  ChangeDetectionStrategy,
  Component,
  inject,
  signal,
} from '@angular/core';
import {
  PublishedInvitationsStore,
} from 'invitation-data-access';
import type {
  InvitationListItem,
} from 'invitation-data-access';

import {
  ImageLightbox,
} from '../../../../shared/components/image-lightbox/image-lightbox';

@Component({
  selector: 'app-invitation-gallery',
  imports: [ImageLightbox],
  providers: [PublishedInvitationsStore],
  templateUrl: './invitation-gallery.html',
  styleUrl: './invitation-gallery.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class InvitationGallery {
  protected readonly store =
    inject(PublishedInvitationsStore);

    protected readonly selectedInvitation =
      signal<InvitationListItem | null>(null);

    protected openInvitation(
      invitation: InvitationListItem
    ): void {
      this.selectedInvitation.set(invitation);
    }

    protected closeInvitation(): void {
      this.selectedInvitation.set(null);
    }
}