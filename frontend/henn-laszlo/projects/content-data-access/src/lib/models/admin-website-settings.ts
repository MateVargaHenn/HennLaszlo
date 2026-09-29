import {
  PublicWebsiteSettings,
} from './public-website-settings';

export interface AdminWebsiteSettings
  extends PublicWebsiteSettings {
  readonly id: string;
  readonly updatedAtUtc: string;
}
