export interface AdminArticleDetails {
  readonly id: string;
  readonly slug: string;
  readonly titleHu: string;
  readonly titleEn: string | null;
  readonly summaryHu: string | null;
  readonly summaryEn: string | null;
  readonly contentHu: string;
  readonly contentEn: string | null;
  readonly isPublished: boolean;
  readonly displayOrder: number;
  readonly createdAtUtc: string;
  readonly updatedAtUtc: string;
}