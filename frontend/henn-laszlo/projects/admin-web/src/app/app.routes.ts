import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    redirectTo: 'artworks',
  },
  {
    path: 'artworks/new',
    loadComponent: () =>
      import(
        './features/artworks/pages/admin-create-artwork/admin-create-artwork'
      ).then(component => component.AdminCreateArtwork),
    title: 'Új mű | Henn László Admin',
  },
  {
  path: 'artworks/:artworkId/edit',
  loadComponent: () =>
    import(
      './features/artworks/pages/admin-edit-artwork/admin-edit-artwork'
    ).then(component => component.AdminEditArtwork),
  title: 'Mű szerkesztése | Henn László Admin',
  },
  {
    path: 'artworks',
    loadComponent: () =>
      import(
        './features/artworks/pages/admin-artwork-list/admin-artwork-list'
      ).then(component => component.AdminArtworkList),
    title: 'Művek kezelése | Henn László Admin',
  },
  {
    path: '**',
    redirectTo: 'artworks',
  },
];