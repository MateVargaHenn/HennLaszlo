import {
  AngularNodeAppEngine,
  createNodeRequestHandler,
  isMainModule,
  writeResponseToNodeResponse,
} from '@angular/ssr/node';
import express from 'express';
import { join } from 'node:path';
import {
  createSitemapXml,
} from './server/sitemap';

const browserDistFolder = join(import.meta.dirname, '../browser');

const app = express();
const angularApp =
  new AngularNodeAppEngine();

app.get(
  '/sitemap.xml',
  async (_request, response) => {
    try {
      const sitemap =
        await createSitemapXml();

      response
        .status(200)
        .set(
          'Content-Type',
          'application/xml; charset=utf-8',
        )
        .set(
          'Cache-Control',
          'public, max-age=300, ' +
          'stale-while-revalidate=3600',
        )
        .send(sitemap);
    } catch (error) {
      console.error(
        'A sitemap előállítása sikertelen.',
        error,
      );

      response
        .status(503)
        .type('text/plain')
        .send(
          'A sitemap átmenetileg nem érhető el.',
        );
    }
  },
);

/**
 * Serve static files from /browser
 */
app.use(
  express.static(
    browserDistFolder,
    {
      maxAge: '1y',
      index: false,
      redirect: false,
    },
  ),
);

/**
 * Handle all other requests by rendering the Angular application.
 */
app.use((req, res, next) => {
  angularApp
    .handle(req)
    .then((response) => (response ? writeResponseToNodeResponse(response, res) : next()))
    .catch(next);
});

/**
 * Start the server if this module is the main entry point, or it is ran via PM2.
 * The server listens on the port defined by the `PORT` environment variable, or defaults to 4000.
 */
if (isMainModule(import.meta.url) || process.env['pm_id']) {
  const port = process.env['PORT'] || 4000;
  app.listen(port, (error) => {
    if (error) {
      throw error;
    }

    console.log(`Node Express server listening on http://localhost:${port}`);
  });
}

/**
 * Request handler used by the Angular CLI (for dev-server and during build) or Firebase Cloud Functions.
 */
export const reqHandler = createNodeRequestHandler(app);
