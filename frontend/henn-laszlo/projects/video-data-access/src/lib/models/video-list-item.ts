export interface VideoListItem {
  readonly id: string;
  readonly titleHu: string;
  readonly titleEn: string | null;
  readonly year: number | null;
  readonly descriptionHu: string | null;
  readonly descriptionEn: string | null;
  readonly videoUrl: string;
  readonly displayOrder: number;
  readonly createdAtUtc: string;
  readonly updatedAtUtc: string;
}
