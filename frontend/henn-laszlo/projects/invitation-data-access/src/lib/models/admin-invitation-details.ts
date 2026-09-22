export interface AdminInvitationDetails {
  readonly id: string;
  readonly titleHu: string;
  readonly titleEn: string | null;
  readonly year: number | null;
  readonly altTextHu: string | null;
  readonly altTextEn: string | null;
  readonly hasImage: boolean;
  readonly isPublished: boolean;
  readonly displayOrder: number;
}