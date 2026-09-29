import { ContentPageKey } from './content-page-key';

export interface PublishedContentPageDetails {
  readonly key: ContentPageKey;
  readonly titleHu: string;
  readonly titleEn: string | null;
  readonly contentHu: string;
  readonly contentEn: string | null;
  readonly createdAtUtc: string;
  readonly updatedAtUtc: string;
}