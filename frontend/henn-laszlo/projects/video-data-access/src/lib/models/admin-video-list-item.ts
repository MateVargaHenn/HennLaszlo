export interface AdminVideoListItem {
  readonly id: string;
  readonly titleHu: string;
  readonly titleEn: string | null;
  readonly year: number | null;
  readonly videoUrl: string;
  readonly isPublished: boolean;
  readonly displayOrder: number;
  readonly createdAtUtc: string;
}
