export interface UpdateWebsiteSettingsRequest {
  readonly artistName: string;
  readonly artistSubtitle: string;
  readonly heroDescription: string;
  readonly defaultSeoTitle: string;
  readonly defaultSeoDescription: string;
  readonly facebookUrl: string | null;
  readonly instagramUrl: string | null;
  readonly youtubeUrl: string | null;
  readonly emailAddress: string | null;
}
