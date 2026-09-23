import { ContentPageKey } from './content-page-key';

export interface AdminContentPageListItem {
  readonly id: string;
  readonly key: ContentPageKey;
  readonly titleHu: string;
  readonly titleEn: string | null;
  readonly isPublished: boolean;
  readonly updatedAtUtc: string;
}