import { mergeApplicationConfig, ApplicationConfig } from '@angular/core';
import { provideServerRendering, withRoutes } from '@angular/ssr';
import { appConfig } from './app.config';
import { serverRoutes } from './app.routes.server';
import { HttpInterceptorFn, provideHttpClient, withFetch, withInterceptors } from '@angular/common/http';

// SSR can reach the API on the Compose network; the browser keeps same-origin /api URLs.
const internalApiInterceptor: HttpInterceptorFn = (request, next) => {
  if (!request.url.startsWith('/api/')) {
    return next(request);
  }

  const apiOrigin = process.env['INTERNAL_API_ORIGIN'] || 'http://localhost:5270';
  return next(request.clone({ url: `${apiOrigin}${request.url}` }));
};

const serverConfig: ApplicationConfig = {
  providers: [
    provideServerRendering(withRoutes(serverRoutes)),
    provideHttpClient(withFetch(), withInterceptors([internalApiInterceptor])),
  ],
};

export const config = mergeApplicationConfig(appConfig, serverConfig);
