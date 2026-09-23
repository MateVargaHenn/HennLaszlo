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
    path: 'invitations',
    loadComponent: () =>
      import(
        './features/invitations/pages/admin-invitation-list/admin-invitation-list'
      ).then(component => component.AdminInvitationList),
    title: 'Meghívók kezelése | Henn László Admin',
  },
  {
    path: 'invitations/new',
    loadComponent: () =>
      import(
        './features/invitations/pages/admin-create-invitation/admin-create-invitation'
      ).then(component => component.AdminCreateInvitation),
    title: 'Új meghívó | Henn László Admin',
  },
  {
    path: 'invitations/:invitationId/edit',
    loadComponent: () =>
      import(
        './features/invitations/pages/admin-edit-invitation/admin-edit-invitation'
      ).then(component => component.AdminEditInvitation),
    title: 'Meghívó szerkesztése | Henn László Admin',
  },
  {
    path: 'content',
    loadComponent: () =>
      import(
        './features/content/pages/admin-content-page-list/admin-content-page-list'
      ).then(
        (component) =>
          component.AdminContentPageList,
      ),
  },
  {
    path: 'content/:key/edit',
    loadComponent: () =>
      import(
        './features/content/pages/admin-edit-content-page/admin-edit-content-page'
      ).then(
        (component) =>
          component.AdminEditContentPage,
      ),
  },
  {
    path: 'content/articles',
    loadComponent: () =>
      import(
        './features/content/pages/article-list/article-list'
      ).then(
        (component) => component.ArticleList,
      ),
  },
  {
    path: 'content/articles/new',
    loadComponent: () =>
      import(
        './features/content/pages/article-editor/article-editor'
      ).then(
        (component) => component.ArticleEditor,
      ),
  },
  {
    path: 'content/articles/:articleId/edit',
    loadComponent: () =>
      import(
        './features/content/pages/article-editor/article-editor'
      ).then(
        (component) => component.ArticleEditor,
      ),
  },
  {
    path: '**',
    redirectTo: 'artworks',
  },
];