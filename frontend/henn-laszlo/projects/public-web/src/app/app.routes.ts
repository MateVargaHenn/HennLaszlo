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
    ],
  },
  {
    path: '**',
    redirectTo: '',
  },
];