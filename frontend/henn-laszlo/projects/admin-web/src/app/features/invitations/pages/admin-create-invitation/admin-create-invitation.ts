import {
  ChangeDetectionStrategy,
  Component,
  inject,
  signal,
} from '@angular/core';
import {
  Router,
  RouterLink,
} from '@angular/router';
import {
  firstValueFrom,
} from 'rxjs';
import {
  InvitationApi,
} from 'invitation-data-access';

import {
  InvitationForm,
  InvitationFormSubmission,
} from '../../components/invitation-form/invitation-form';

@Component({
  selector: 'app-admin-create-invitation',
  imports: [
    InvitationForm,
    RouterLink,
  ],
  templateUrl: './admin-create-invitation.html',
  styleUrl: './admin-create-invitation.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AdminCreateInvitation {
  private readonly invitationApi =
    inject(InvitationApi);

  private readonly router =
    inject(Router);

  private createdInvitationId:
    string | null = null;

  private uploadedFileId:
    string | null = null;

  private selectedFile:
    File | null = null;

  protected readonly isSubmitting =
    signal(false);

  protected readonly submitError =
    signal<string | null>(null);

  protected async save(
    submission: InvitationFormSubmission
  ): Promise<void> {
    if (this.isSubmitting()) {
      return;
    }

    this.isSubmitting.set(true);
    this.submitError.set(null);

    try {
      const invitationId =
        await this.ensureInvitationCreated(
          submission
        );

      if (submission.imageFile) {
        const fileId =
          await this.ensureFileUploaded(
            submission.imageFile
          );

        await firstValueFrom(
          this.invitationApi.attachImage(
            invitationId,
            fileId
          )
        );
      }

      await this.router.navigate([
        '/invitations',
      ]);
    }
    catch {
      const message = this.createdInvitationId
        ? 'A vázlat létrejött, de a kép mentése nem sikerült. Az adatok elvesztése nélkül újrapróbálhatod.'
        : 'A meghívó mentése nem sikerült. Próbáld újra.';

      this.submitError.set(message);
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

  private async ensureInvitationCreated(
    submission: InvitationFormSubmission
  ): Promise<string> {
    if (this.createdInvitationId) {
      return this.createdInvitationId;
    }

    const response = await firstValueFrom(
      this.invitationApi.createInvitation(
        submission.invitation
      )
    );

    this.createdInvitationId = response.id;

    return response.id;
  }

  private async ensureFileUploaded(
    file: File
  ): Promise<string> {
    if (
      this.selectedFile === file &&
      this.uploadedFileId
    ) {
      return this.uploadedFileId;
    }

    const response = await firstValueFrom(
      this.invitationApi.uploadFile(file)
    );

    this.selectedFile = file;
    this.uploadedFileId = response.id;

    return response.id;
  }
}