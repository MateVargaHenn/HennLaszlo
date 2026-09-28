/*
 * Public API Surface of video-data-access
 */

export {
  VIDEO_DATA_ACCESS_CONFIG,
  provideVideoDataAccess,
} from './lib/config/video-data-access-config';

export type { VideoDataAccessConfig } from './lib/config/video-data-access-config';

export type { VideoListItem } from './lib/models/video-list-item';

export { VideoApi } from './lib/services/video-api';

export { PublishedVideosStore } from './lib/stores/published-videos-store';

export type { AdminVideoListItem } from './lib/models/admin-video-list-item';

export { AdminVideosStore } from './lib/stores/admin-video-store';

export type { CreateVideoRequest } from './lib/models/create-video-request';

export type { CreateVideoResponse } from './lib/models/create-video-response';

export * from './lib/models/admin-video-details';
export * from './lib/models/update-video-request';
export * from './lib/stores/admin-video-details-store';
