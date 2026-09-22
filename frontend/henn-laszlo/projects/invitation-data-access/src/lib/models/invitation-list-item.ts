export interface InvitationListItem {
  readonly id: string;
  readonly titleHu: string;
  readonly titleEn: string | null;
  readonly year: number | null;
  readonly altTextHu: string | null;
  readonly altTextEn: string | null;
  readonly displayOrder: number;
}