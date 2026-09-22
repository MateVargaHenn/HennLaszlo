import {
  computed,
  inject,
  Injectable,
  signal,
  Signal,
} from '@angular/core';
import {
  firstValueFrom,
  Observable,
} from 'rxjs';
import {
  rxResource,
} from '@angular/core/rxjs-interop';

import {
  AdminInvitationListItem,
} from '../models/admin-invitation-list-item';
import {
  InvitationApi,
} from '../services/invitation-api';

@Injectable()
export class AdminInvitationsStore {
  private readonly invitationApi =
    inject(InvitationApi);

  private readonly resource = rxResource({
    defaultValue:
      [] as readonly AdminInvitationListItem[],

    stream: () =>
      this.invitationApi.getAdminInvitations(),
  });

  readonly invitations:
    Signal<readonly AdminInvitationListItem[]> =
      computed(() => this.resource.value());

  readonly isLoading: Signal<boolean> =
    computed(() => this.resource.isLoading());

  readonly hasError: Signal<boolean> =
    computed(
      () => this.resource.error() !== undefined
    );

  readonly publishedCount = computed(() =>
    this.invitations().filter(
      invitation => invitation.isPublished
    ).length
  );

  readonly draftCount = computed(() =>
    this.invitations().filter(
      invitation => !invitation.isPublished
    ).length
  );

  readonly deletionInProgressId =
  signal<string | null>(null);

readonly deletionError =
  signal<string | null>(null);

  reload(): void {
    this.resource.reload();
  }

  getImageUrl(invitationId: string): string {
    return this.invitationApi
      .getAdminInvitationImageUrl(invitationId);
  }

  private readonly publicationInProgressId =
    signal<string | null>(null);

  readonly publicationError =
    signal<string | null>(null);

  isChangingPublication(
    invitationId: string
  ): boolean {
    return this.publicationInProgressId() ===
      invitationId;
  }

  publish(
    invitationId: string
  ): Promise<void> {
    return this.changePublication(
      invitationId,
      this.invitationApi.publishInvitation(
        invitationId
      )
    );
  }

  unpublish(
    invitationId: string
  ): Promise<void> {
    return this.changePublication(
      invitationId,
      this.invitationApi.unpublishInvitation(
        invitationId
      )
    );
  }

  

  private async changePublication(
    invitationId: string,
    operation: Observable<void>
  ): Promise<void> {
    if (this.publicationInProgressId()) {
      return;
    }

    this.publicationInProgressId.set(
      invitationId
    );

    this.publicationError.set(null);

    try {
      await firstValueFrom(operation);
      this.resource.reload();
    }
    catch {
      this.publicationError.set(
        'Az állapot módosítása nem sikerült.'
      );
    }
    finally {
      this.publicationInProgressId.set(null);
    }
  }

  isDeleting(
    invitationId: string
  ): boolean {
    return this.deletionInProgressId() ===
      invitationId;
  }

  async deleteInvitation(
    invitationId: string
  ): Promise<boolean> {
    if (this.deletionInProgressId()) {
      return false;
    }

    this.deletionInProgressId.set(
      invitationId
    );

    this.deletionError.set(null);

    try {
      await firstValueFrom(
        this.invitationApi.deleteInvitation(
          invitationId
        )
      );

      this.resource.reload();

      return true;
    }
    catch {
      this.deletionError.set(
        'A meghívót nem sikerült törölni.'
      );

      return false;
    }
    finally {
      this.deletionInProgressId.set(null);
    }
  }
}