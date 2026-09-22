import {
  computed,
  inject,
  Injectable,
  Signal,
} from '@angular/core';
import {
  rxResource,
} from '@angular/core/rxjs-interop';

import {
  InvitationListItem,
} from '../models/invitation-list-item';
import {
  InvitationApi,
} from '../services/invitation-api';

@Injectable()
export class PublishedInvitationsStore {
  private readonly invitationApi =
    inject(InvitationApi);

  private readonly resource = rxResource({
    defaultValue:
      [] as readonly InvitationListItem[],

    stream: () =>
      this.invitationApi.getPublishedInvitations(),
  });

  readonly invitations:
    Signal<readonly InvitationListItem[]> =
      computed(() => this.resource.value());

  readonly isLoading: Signal<boolean> =
    computed(() => this.resource.isLoading());

  readonly hasError: Signal<boolean> =
    computed(
      () => this.resource.error() !== undefined
    );

  reload(): void {
    this.resource.reload();
  }

  getImageUrl(invitationId: string): string {
    return this.invitationApi
      .getInvitationImageUrl(invitationId);
  }
}