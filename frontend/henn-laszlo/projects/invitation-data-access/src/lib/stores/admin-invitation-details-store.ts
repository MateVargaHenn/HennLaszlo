import {
  computed,
  inject,
  Injectable,
  Signal,
  signal,
} from '@angular/core';
import { rxResource } from '@angular/core/rxjs-interop';
import { of } from 'rxjs';

import { AdminInvitationDetails } from '../models/admin-invitation-details';
import { InvitationApi } from '../services/invitation-api';

@Injectable()
export class AdminInvitationDetailsStore {
  private readonly invitationApi =
    inject(InvitationApi);

  private readonly invitationId =
    signal<string | null>(null);

  private readonly resource =
    rxResource<
      AdminInvitationDetails | null,
      string | null
    >({
      params: () => this.invitationId(),

      stream: ({ params: invitationId }) => {
        if (!invitationId) {
          return of(null);
        }

        return this.invitationApi
          .getAdminInvitationById(invitationId);
      },

      defaultValue: null,
    });

  readonly invitation:
    Signal<AdminInvitationDetails | null> =
      computed(() => this.resource.value());

  readonly isLoading =
    this.resource.isLoading;

  readonly hasError =
    computed(
      () => this.resource.error() !== undefined
    );

  load(invitationId: string): void {
    this.invitationId.set(invitationId);
  }

  reload(): void {
    this.resource.reload();
  }

  getImageUrl(invitationId: string): string {
    return this.invitationApi
      .getAdminInvitationImageUrl(invitationId);
  }
}