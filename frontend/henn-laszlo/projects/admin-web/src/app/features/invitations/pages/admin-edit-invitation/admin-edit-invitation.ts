import {
  ChangeDetectionStrategy,
  Component,
  computed,
  inject,
  signal,
} from '@angular/core';
import {
  ActivatedRoute,
  Router,
} from '@angular/router';
import { firstValueFrom } from 'rxjs';

import {
  AdminInvitationDetailsStore,
  InvitationApi,
} from 'invitation-data-access';

import type {
  UpdateInvitationRequest,
} from 'invitation-data-access';

import {
  InvitationForm,
} from '../../components/invitation-form/invitation-form';

import type {
  InvitationFormSubmission,
} from '../../components/invitation-form/invitation-form';

@Component({
  selector: 'app-admin-edit-invitation',
  imports: [InvitationForm],
  templateUrl: './admin-edit-invitation.html',
  styleUrl: './admin-edit-invitation.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [AdminInvitationDetailsStore],
})
export class AdminEditInvitation {
  private readonly route =
    inject(ActivatedRoute);

  private readonly router =
    inject(Router);

  private readonly invitationApi =
    inject(InvitationApi);

  protected readonly store =
    inject(AdminInvitationDetailsStore);

  private readonly invitationId =
    this.route.snapshot.paramMap.get(
      'invitationId'
    ) ?? '';

  protected readonly isSubmitting =
    signal(false);

  protected readonly submitError =
    signal<string | null>(null);

  protected readonly existingImageUrl =
    computed(() => {
      const invitation = this.store.invitation();

      if (!invitation?.hasImage) {
        return null;
      }

      return this.store.getImageUrl(
        invitation.id
      );
    });

  constructor() {
    if (!this.invitationId) {
      void this.router.navigate([
        '/invitations',
      ]);

      return;
    }

    this.store.load(this.invitationId);
  }

  protected async save(
    submission: InvitationFormSubmission
  ): Promise<void> {
    if (this.isSubmitting()) {
      return;
    }

    this.isSubmitting.set(true);
    this.submitError.set(null);

    const request: UpdateInvitationRequest = {
      titleHu: submission.invitation.titleHu,
      titleEn: submission.invitation.titleEn,
      year: submission.invitation.year,
      altTextHu:
        submission.invitation.altTextHu,
      altTextEn:
        submission.invitation.altTextEn,
      displayOrder:
        submission.invitation.displayOrder,
    };

    try {
      await firstValueFrom(
        this.invitationApi.updateInvitation(
          this.invitationId,
          request
        )
      );

      if (submission.imageFile) {
        const uploadedFile =
          await firstValueFrom(
            this.invitationApi.uploadFile(
              submission.imageFile
            )
          );

        await firstValueFrom(
          this.invitationApi.attachImage(
            this.invitationId,
            uploadedFile.id
          )
        );
      }

      await this.router.navigate([
        '/invitations',
      ]);
    }
    catch {
      this.submitError.set(
        'A meghívó módosításait nem sikerült menteni.'
      );
    }
    finally {
      this.isSubmitting.set(false);
    }
  }

  protected cancel(): void {
    void this.router.navigate([
      '/invitations',
    ]);
  }
}