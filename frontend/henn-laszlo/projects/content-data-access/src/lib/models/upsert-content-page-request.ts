export interface UpsertContentPageRequest {
  readonly titleHu: string;
  readonly titleEn: string | null;
  readonly contentHu: string;
  readonly contentEn: string | null;
  readonly expectedVersion: string | null;
}