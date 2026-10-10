export interface UpdateInvitationRequest {
  readonly titleHu: string;
  readonly titleEn: string | null;
  readonly year: number | null;
  readonly altTextHu: string | null;
  readonly altTextEn: string | null;
  readonly displayOrder: number;
  readonly exhibitionStartsAt: string | null;
  readonly exhibitionEndsAt: string | null;
  readonly locationHu: string | null;
  readonly locationEn: string | null;
}