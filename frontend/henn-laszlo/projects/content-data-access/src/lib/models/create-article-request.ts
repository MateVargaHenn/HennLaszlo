export interface CreateArticleRequest {
  readonly slug: string;
  readonly titleHu: string;
  readonly titleEn: string | null;
  readonly summaryHu: string | null;
  readonly summaryEn: string | null;
  readonly contentHu: string;
  readonly contentEn: string | null;
  readonly displayOrder: number;
}