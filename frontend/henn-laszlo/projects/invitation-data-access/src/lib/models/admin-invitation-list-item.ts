export interface AdminInvitationListItem {
  readonly id: string;
  readonly titleHu: string;
  readonly titleEn: string | null;
  readonly year: number | null;
  readonly hasImage: boolean;
  readonly isPublished: boolean;
  readonly displayOrder: number;
  readonly createdAtUtc: string;
}