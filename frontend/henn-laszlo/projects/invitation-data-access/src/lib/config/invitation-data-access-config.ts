import {
  EnvironmentProviders,
  InjectionToken,
  makeEnvironmentProviders,
} from '@angular/core';

export interface InvitationDataAccessConfig {
  readonly apiBaseUrl: string;
}

export const INVITATION_DATA_ACCESS_CONFIG =
  new InjectionToken<InvitationDataAccessConfig>(
    'INVITATION_DATA_ACCESS_CONFIG'
  );

export function provideInvitationDataAccess(
  config: InvitationDataAccessConfig
): EnvironmentProviders {
  return makeEnvironmentProviders([
    {
      provide: INVITATION_DATA_ACCESS_CONFIG,
      useValue: config,
    },
  ]);
}