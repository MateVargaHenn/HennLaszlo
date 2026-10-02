export interface ContentRevisionDetails {
  readonly id: string;
  readonly titleHu: string;
  readonly titleEn: string | null;
  readonly summaryHu: string | null;
  readonly summaryEn: string | null;
  readonly contentHu: string;
  readonly contentEn: string | null;
  readonly createdAtUtc: string;
}