import { ContentPageKey } from './content-page-key';

export interface AdminContentPageDetails {
  readonly id: string;
  readonly key: ContentPageKey;
  readonly titleHu: string;
  readonly titleEn: string | null;
  readonly contentHu: string;
  readonly contentEn: string | null;
  readonly isPublished: boolean;
  readonly updatedAtUtc: string;
}