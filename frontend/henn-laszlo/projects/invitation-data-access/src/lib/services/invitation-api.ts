import {
  inject,
  Injectable,
} from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import {
  INVITATION_DATA_ACCESS_CONFIG,
} from '../config/invitation-data-access-config';
import {
  InvitationListItem,
} from '../models/invitation-list-item';
import {
  AdminInvitationListItem,
} from '../models/admin-invitation-list-item';

import {
  CreateInvitationRequest,
} from '../models/create-invitation-request';
import {
  CreateInvitationResponse,
} from '../models/create-invitation-response';
import {
  UploadFileResponse,
} from '../models/upload-file-response';
import { UpdateInvitationRequest } from '../models/update-invitation-request';
import { AdminInvitationDetails } from '../models/admin-invitation-details';

@Injectable({
  providedIn: 'root',
})
export class InvitationApi {
  private readonly http = inject(HttpClient);

  private readonly config = inject(
    INVITATION_DATA_ACCESS_CONFIG
  );

  private readonly apiBaseUrl =
    this.config.apiBaseUrl.replace(/\/$/, '');

  getPublishedInvitations():
    Observable<readonly InvitationListItem[]> {
    return this.http.get<readonly InvitationListItem[]>(
      `${this.apiBaseUrl}/api/invitations`
    );
  }

  getInvitationImageUrl(
    invitationId: string
  ): string {
    return `${this.apiBaseUrl}/api/invitations/${invitationId}/image`;
  }

  getAdminInvitations():
    Observable<readonly AdminInvitationListItem[]> {
    return this.http.get<
      readonly AdminInvitationListItem[]
    >(
      `${this.apiBaseUrl}/api/admin/invitations`
    );
  }

  getAdminInvitationImageUrl(
    invitationId: string
  ): string {
    return `${this.apiBaseUrl}/api/admin/invitations/${invitationId}/image`;
  }

  createInvitation(
    request: CreateInvitationRequest
  ): Observable<CreateInvitationResponse> {
    return this.http.post<CreateInvitationResponse>(
      `${this.apiBaseUrl}/api/admin/invitations`,
      request
    );
  }

  uploadFile(
    file: File
  ): Observable<UploadFileResponse> {
    const formData = new FormData();

    formData.append(
      'file',
      file,
      file.name
    );

    return this.http.post<UploadFileResponse>(
      `${this.apiBaseUrl}/api/admin/files`,
      formData
    );
  }

  attachImage(
    invitationId: string,
    fileId: string
  ): Observable<void> {
    return this.http.put<void>(
      `${this.apiBaseUrl}/api/admin/invitations/${invitationId}/image`,
      {
        fileId,
      }
    );
  }

  publishInvitation(
    invitationId: string
  ): Observable<void> {
    return this.http.put<void>(
      `${this.apiBaseUrl}/api/admin/invitations/${invitationId}/publish`,
      null
    );
  }

  unpublishInvitation(
    invitationId: string
  ): Observable<void> {
    return this.http.put<void>(
      `${this.apiBaseUrl}/api/admin/invitations/${invitationId}/unpublish`,
      null
    );
  }

  getAdminInvitationById(
  invitationId: string
  ): Observable<AdminInvitationDetails> {
    return this.http.get<AdminInvitationDetails>(
      `${this.apiBaseUrl}/api/admin/invitations/${invitationId}`
    );
  }

  updateInvitation(
    invitationId: string,
    request: UpdateInvitationRequest
  ): Observable<void> {
    return this.http.put<void>(
      `${this.apiBaseUrl}/api/admin/invitations/${invitationId}`,
      request
    );
  }

  deleteInvitation(
    invitationId: string
  ): Observable<void> {
    return this.http.delete<void>(
      `${this.apiBaseUrl}/api/admin/invitations/${invitationId}`
    );
  }
}