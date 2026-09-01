import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    loadComponent: () =>
      import(
        './layout/public-layout/public-layout'
      ).then(component => component.PublicLayout),
    children: [
      {
        path: '',
        pathMatch: 'full',
        redirectTo: 'muvek',
      },
      {
        path: 'muvek',
        loadComponent: () =>
          import(
            './features/artworks/pages/artwork-gallery/artwork-gallery'
          ).then(component => component.ArtworkGallery),
        title: 'Művek | Henn László András',
      },
    ],
  },
  {
    path: '**',
    redirectTo: 'muvek',
  },
];