import { DatePipe } from '@angular/common';
import {
  ChangeDetectionStrategy,
  Component,
  inject,
  signal,
} from '@angular/core';
import { RouterLink } from '@angular/router';
import {
  AdminInvitationsStore,
} from 'invitation-data-access';
import type {
  AdminInvitationListItem,
} from 'invitation-data-access';

@Component({
  selector: 'app-admin-invitation-list',
  imports: [
    DatePipe,
    RouterLink,
  ],
  providers: [AdminInvitationsStore],
  templateUrl: './admin-invitation-list.html',
  styleUrl: './admin-invitation-list.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AdminInvitationList {
  protected readonly store =
    inject(AdminInvitationsStore);

    protected publishInvitation(
      invitationId: string
    ): void {
      void this.store.publish(invitationId);
    }

    protected unpublishInvitation(
      invitationId: string
    ): void {
      void this.store.unpublish(invitationId);
    }

    protected readonly invitationPendingDeletion =
      signal<AdminInvitationListItem | null>(
        null
      );

    protected requestDeletion(
      invitation: AdminInvitationListItem
    ): void {
      if (invitation.isPublished) {
        return;
      }

      this.invitationPendingDeletion.set(
        invitation
      );
    }

    protected cancelDeletion(): void {
      const invitation =
        this.invitationPendingDeletion();

      if (
        invitation &&
        this.store.isDeleting(invitation.id)
      ) {
        return;
      }

      this.invitationPendingDeletion.set(null);
    }

    protected async confirmDeletion(): Promise<void> {
      const invitation =
        this.invitationPendingDeletion();

      if (!invitation) {
        return;
      }

      const deleted =
        await this.store.deleteInvitation(
          invitation.id
        );

      if (deleted) {
        this.invitationPendingDeletion.set(null);
      }
    }
}