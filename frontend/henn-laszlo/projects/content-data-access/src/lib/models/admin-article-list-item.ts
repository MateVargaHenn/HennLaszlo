export interface AdminArticleListItem {
  readonly id: string;
  readonly slug: string;
  readonly titleHu: string;
  readonly titleEn: string | null;
  readonly isPublished: boolean;
  readonly displayOrder: number;
  readonly updatedAtUtc: string;
}