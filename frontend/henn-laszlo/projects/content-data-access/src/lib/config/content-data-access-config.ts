import {
  EnvironmentProviders,
  InjectionToken,
  makeEnvironmentProviders,
} from '@angular/core';

export interface ContentDataAccessConfig {
  readonly apiBaseUrl: string;
}

export const CONTENT_DATA_ACCESS_CONFIG =
  new InjectionToken<ContentDataAccessConfig>(
    'CONTENT_DATA_ACCESS_CONFIG',
  );

export function provideContentDataAccess(
  config: ContentDataAccessConfig,
): EnvironmentProviders {
  return makeEnvironmentProviders([
    {
      provide: CONTENT_DATA_ACCESS_CONFIG,
      useValue: config,
    },
  ]);
}