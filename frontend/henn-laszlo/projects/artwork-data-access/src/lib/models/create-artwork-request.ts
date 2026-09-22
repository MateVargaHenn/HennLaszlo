export interface CreateArtworkRequest {
  readonly titleHu: string;
  readonly titleEn: string | null;
  readonly year: number | null;
  readonly techniqueHu: string | null;
  readonly techniqueEn: string | null;
  readonly widthCm: number | null;
  readonly heightCm: number | null;
  readonly descriptionHu: string | null;
  readonly descriptionEn: string | null;
  readonly isFeatured: boolean;
  readonly displayOrder: number;
}