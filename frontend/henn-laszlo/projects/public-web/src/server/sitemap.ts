interface DatedItem {
  readonly updatedAtUtc: string;
}

interface SitemapArtwork
  extends DatedItem {
  readonly id: string;
}

interface SitemapArticle
  extends DatedItem {
  readonly slug: string;
}

interface SitemapVideo
  extends DatedItem {
  readonly id: string;
}

interface SitemapContentPage
  extends DatedItem {
  readonly key: string;
}

interface SitemapEntry {
  readonly path: string;
  readonly lastModified?: string;
}

const contentPages = [
  {
    key: 'about',
    path: '/bemutatkozas',
  },
  {
    key: 'exhibitions',
    path: '/kiallitasok',
  },
  {
    key: 'memberships-and-awards',
    path: '/tagsagok-es-dijak',
  },
  {
    key: 'writings',
    path: '/irasok',
  },
  {
    key: 'contact',
    path: '/kapcsolat',
  },
] as const;

export async function createSitemapXml():
  Promise<string> {
  const apiOrigin =
    (
      process.env[
        'INTERNAL_API_ORIGIN'
      ] ??
      'http://localhost:5270'
    ).replace(/\/+$/, '');

  const publicSiteUrl =
    new URL(
      process.env[
        'PUBLIC_SITE_URL'
      ] ??
      'http://localhost:4000',
    ).origin;

  const [
    artworks,
    articles,
    videos,
    loadedContentPages,
  ] =
    await Promise.all([
      fetchJson<
        readonly SitemapArtwork[]
      >(
        apiOrigin,
        '/api/artworks',
      ),
      fetchJson<
        readonly SitemapArticle[]
      >(
        apiOrigin,
        '/api/articles',
      ),
      fetchJson<
        readonly SitemapVideo[]
      >(
        apiOrigin,
        '/api/videos',
      ),
      loadContentPages(
        apiOrigin,
      ),
    ]);

  const artworkLastModified =
    latestDate(
      artworks.map(
        artwork =>
          artwork.updatedAtUtc,
      ),
    );

  const articleLastModified =
    latestDate(
      articles.map(
        article =>
          article.updatedAtUtc,
      ),
    );

  const videoLastModified =
    latestDate(
      videos.map(
        video =>
          video.updatedAtUtc,
      ),
    );

  const writingsPage =
    loadedContentPages.find(
      page =>
        page.key === 'writings',
    );

  const writingsLastModified =
    latestDate([
      articleLastModified,
      writingsPage?.updatedAtUtc,
    ]);

  const homeLastModified =
    latestDate([
      artworkLastModified,
      articleLastModified,
      videoLastModified,
      ...loadedContentPages.map(
        page =>
          page.updatedAtUtc,
      ),
    ]);

  const entries: SitemapEntry[] = [
    {
      path: '/',
      lastModified:
        homeLastModified,
    },
    {
      path: '/muvek',
      lastModified:
        artworkLastModified,
    },
    ...artworks.map(
      artwork => ({
        path:
          `/muvek/${
            encodeURIComponent(
              artwork.id,
            )
          }`,
        lastModified:
          normalizeDate(
            artwork.updatedAtUtc,
          ),
      }),
    ),
    {
      path: '/meghivok',
    },
    {
      path: '/videok',
      lastModified:
        videoLastModified,
    },
    ...loadedContentPages
      .filter(
        page =>
          page.key !== 'writings',
      )
      .map(page => ({
        path: page.path,
        lastModified:
          normalizeDate(
            page.updatedAtUtc,
          ),
      })),
    {
      path: '/irasok',
      lastModified:
        writingsLastModified,
    },
    ...articles.map(
      article => ({
        path:
          `/irasok/${
            encodeURIComponent(
              article.slug,
            )
          }`,
        lastModified:
          normalizeDate(
            article.updatedAtUtc,
          ),
      }),
    ),
  ];

  return renderSitemap(
    publicSiteUrl,
    entries,
  );
}

async function loadContentPages(
  apiOrigin: string,
): Promise<
  readonly (
    SitemapContentPage & {
      readonly path: string;
    }
  )[]
> {
  const pages =
    await Promise.all(
      contentPages.map(
        async definition => {
          const page =
            await fetchOptionalJson<
              SitemapContentPage
            >(
              apiOrigin,
              `/api/content-pages/${
                encodeURIComponent(
                  definition.key,
                )
              }`,
            );

          if (!page) {
            return null;
          }

          return {
            ...page,
            path: definition.path,
          };
        },
      ),
    );

  return pages.filter(
    (
      page,
    ): page is SitemapContentPage & {
      readonly path: typeof contentPages[number]['path'];
    } => page !== null,
  );
}

async function fetchJson<T>(
  apiOrigin: string,
  path: string,
): Promise<T> {
  const response =
    await fetch(
      new URL(
        path,
        `${apiOrigin}/`,
      ),
      {
        headers: {
          accept: 'application/json',
        },
      },
    );

  if (!response.ok) {
    throw new Error(
      `A sitemap API-hívása sikertelen: ` +
      `${path} (${response.status}).`,
    );
  }

  return await response.json() as T;
}

async function fetchOptionalJson<T>(
  apiOrigin: string,
  path: string,
): Promise<T | null> {
  const response =
    await fetch(
      new URL(
        path,
        `${apiOrigin}/`,
      ),
      {
        headers: {
          accept: 'application/json',
        },
      },
    );

  if (response.status === 404) {
    return null;
  }

  if (!response.ok) {
    throw new Error(
      `A sitemap API-hívása sikertelen: ` +
      `${path} (${response.status}).`,
    );
  }

  return await response.json() as T;
}

function latestDate(
  values:
    readonly (
      string | undefined
    )[],
): string | undefined {
  let latestTimestamp =
    Number.NEGATIVE_INFINITY;

  for (const value of values) {
    if (!value) {
      continue;
    }

    const timestamp =
      Date.parse(value);

    if (
      !Number.isNaN(timestamp) &&
      timestamp > latestTimestamp
    ) {
      latestTimestamp = timestamp;
    }
  }

  return Number.isFinite(
    latestTimestamp,
  )
    ? new Date(
        latestTimestamp,
      ).toISOString()
    : undefined;
}

function normalizeDate(
  value: string,
): string | undefined {
  return latestDate([value]);
}

function renderSitemap(
  publicSiteUrl: string,
  entries:
    readonly SitemapEntry[],
): string {
  const urls =
    entries.map(entry => {
      const location =
        new URL(
          entry.path,
          `${publicSiteUrl}/`,
        ).toString();

      const lastModified =
        entry.lastModified
          ? `\n    <lastmod>${
              escapeXml(
                entry.lastModified,
              )
            }</lastmod>`
          : '';

      return (
        '  <url>\n' +
        `    <loc>${
          escapeXml(location)
        }</loc>` +
        lastModified +
        '\n  </url>'
      );
    });

  return (
    '<?xml version="1.0" ' +
    'encoding="UTF-8"?>\n' +
    '<urlset xmlns="' +
    'http://www.sitemaps.org/' +
    'schemas/sitemap/0.9">\n' +
    `${urls.join('\n')}\n` +
    '</urlset>\n'
  );
}

function escapeXml(
  value: string,
): string {
  return value
    .replaceAll('&', '&amp;')
    .replaceAll('<', '&lt;')
    .replaceAll('>', '&gt;')
    .replaceAll('"', '&quot;')
    .replaceAll("'", '&apos;');
}