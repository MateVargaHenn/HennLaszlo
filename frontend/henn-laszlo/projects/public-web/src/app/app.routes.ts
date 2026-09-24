import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    loadComponent: () =>
      import('./layout/public-layout/public-layout')
        .then(component => component.PublicLayout),

    children: [
      {
        path: '',
        pathMatch: 'full',
        loadComponent: () =>
          import('./features/home/pages/home/home')
            .then(component => component.Home),
        title: 'Henn László András | Festőművész',
      },
      {
        path: 'bemutatkozas',
        data: {
          contentPageKey: 'about',
        },
        loadComponent: () =>
          import(
            './features/content/pages/content-page/content-page'
          ).then(
            component => component.ContentPage,
          ),
      },
      {
        path: 'kapcsolat',
        data: {
          contentPageKey: 'contact',
        },
        loadComponent: () =>
          import(
            './features/content/pages/content-page/content-page'
          ).then(
            component => component.ContentPage,
          ),
      },
      {
        path: 'muvek',
        loadComponent: () =>
          import(
            './features/artworks/pages/artwork-gallery/artwork-gallery'
          ).then(component => component.ArtworkGallery),
        title: 'Művek | Henn László András',
      },
      {
        path: 'muvek/:artworkId',
        loadComponent: () =>
          import(
            './features/artworks/pages/artwork-details/artwork-details'
          ).then(component => component.ArtworkDetails),
        title: 'Mű adatlap | Henn László András',
      },
      {
        path: 'meghivok',
        loadComponent: () =>
          import(
            './features/invitations/pages/invitation-gallery/invitation-gallery'
          ).then(component => component.InvitationGallery),
        title: 'Meghívók | Henn László András',
      },
      {
        path: 'kiallitasok',
        data: {
          contentPageKey: 'exhibitions',
        },
        loadComponent: () =>
          import(
            './features/content/pages/content-page/content-page'
          ).then(
            (component) => component.ContentPage,
          ),
      },
      {
        path: 'tagsagok-es-dijak',
        data: {
          contentPageKey: 'memberships-and-awards',
        },
        loadComponent: () =>
          import(
            './features/content/pages/content-page/content-page'
          ).then(
            (component) => component.ContentPage,
          ),
      },
      {
        path: 'irasok',
        loadComponent: () =>
          import(
            './features/content/pages/writings-page/writings-page'
          ).then(
            (component) => component.WritingsPage,
          ),
      },
      {
        path: 'irasok/:slug',
        loadComponent: () =>
          import(
            './features/content/pages/article-details/article-details'
          ).then(
            (component) => component.ArticleDetails,
          ),
      },
    ],
  },
  {
    path: '**',
    redirectTo: '',
  },
];