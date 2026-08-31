import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    loadComponent: () =>
      import(
        './features/artworks/pages/artwork-gallery/artwork-gallery'
      ).then(component => component.ArtworkGallery),
    title: 'Művek | Henn László András',
  },
];