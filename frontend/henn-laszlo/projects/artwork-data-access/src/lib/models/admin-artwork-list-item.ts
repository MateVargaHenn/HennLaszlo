export interface AdminArtworkListItem {
  readonly id: string;
  readonly titleHu: string;
  readonly titleEn: string | null;
  readonly year: number | null;
  readonly techniqueHu: string | null;
  readonly widthCm: number | null;
  readonly heightCm: number | null;
  readonly hasImage: boolean;
  readonly isPublished: boolean;
  readonly isFeatured: boolean;
  readonly displayOrder: number;
  readonly createdAtUtc: string;
}