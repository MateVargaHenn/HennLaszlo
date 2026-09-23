export interface PublishedArticleListItem {
  readonly id: string;
  readonly slug: string;
  readonly titleHu: string;
  readonly titleEn: string | null;
  readonly summaryHu: string | null;
  readonly summaryEn: string | null;
}