export interface ArtworkListItem {
  readonly id: string;
  readonly titleHu: string;
  readonly titleEn: string | null;
  readonly year: number | null;
  readonly techniqueHu: string | null;
  readonly techniqueEn: string | null;
  readonly widthCm: number | null;
  readonly heightCm: number | null;
  readonly isFeatured: boolean;
  readonly displayOrder: number;
}