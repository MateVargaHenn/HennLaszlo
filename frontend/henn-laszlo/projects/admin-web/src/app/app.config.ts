import { ApplicationConfig, provideBrowserGlobalErrorListeners } from '@angular/core';
import { provideRouter } from '@angular/router';
import { routes } from './app.routes';
import { provideHttpClient } from '@angular/common/http';
import {
  provideArtworkDataAccess,
} from 'artwork-data-access';
import { environment } from '../environments/environment';

export const appConfig: ApplicationConfig = {
  providers: [provideBrowserGlobalErrorListeners(), provideRouter(routes), provideHttpClient(), provideArtworkDataAccess({
  apiBaseUrl: environment.apiBaseUrl,
})],
};
