import { EnvironmentProviders, InjectionToken, makeEnvironmentProviders } from '@angular/core';

export interface VideoDataAccessConfig {
  readonly apiBaseUrl: string;
}

export const VIDEO_DATA_ACCESS_CONFIG = new InjectionToken<VideoDataAccessConfig>(
  'VIDEO_DATA_ACCESS_CONFIG',
);

export function provideVideoDataAccess(config: VideoDataAccessConfig): EnvironmentProviders {
  return makeEnvironmentProviders([
    {
      provide: VIDEO_DATA_ACCESS_CONFIG,
      useValue: config,
    },
  ]);
}
