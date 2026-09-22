import {
  EnvironmentProviders,
  InjectionToken,
  makeEnvironmentProviders,
} from '@angular/core';

export interface ArtworkDataAccessConfig {
  readonly apiBaseUrl: string;
}

export const ARTWORK_DATA_ACCESS_CONFIG =
  new InjectionToken<ArtworkDataAccessConfig>(
    'ARTWORK_DATA_ACCESS_CONFIG',
  );

export function provideArtworkDataAccess(
  config: ArtworkDataAccessConfig,
): EnvironmentProviders {
  return makeEnvironmentProviders([
    {
      provide: ARTWORK_DATA_ACCESS_CONFIG,
      useValue: config,
    },
  ]);
}