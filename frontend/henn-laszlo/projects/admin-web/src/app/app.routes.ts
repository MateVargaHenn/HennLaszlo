import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    redirectTo: 'artworks',
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