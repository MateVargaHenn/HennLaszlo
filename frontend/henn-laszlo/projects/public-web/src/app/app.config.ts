import { ApplicationConfig, provideBrowserGlobalErrorListeners } from '@angular/core';
import { provideRouter, withComponentInputBinding, } from '@angular/router';
import { routes } from './app.routes';
import { provideClientHydration } from '@angular/platform-browser';
import { provideHttpClient } from '@angular/common/http';
import { provideArtworkDataAccess } from 'artwork-data-access';
import {
  provideInvitationDataAccess,
} from 'invitation-data-access';
import {
  provideContentDataAccess,
} from 'content-data-access';
import { environment } from '../environments/environment';

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideRouter(routes, withComponentInputBinding()),
    provideClientHydration(),
    provideHttpClient(),

    provideArtworkDataAccess({
      apiBaseUrl: environment.apiBaseUrl,
    }),

    provideInvitationDataAccess({
      apiBaseUrl: environment.apiBaseUrl,
    }),

    provideContentDataAccess({
      apiBaseUrl: environment.apiBaseUrl,
    }),
  ],
};
