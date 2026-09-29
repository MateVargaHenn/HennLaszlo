import { Routes } from '@angular/router';
import {
  adminAuthGuard,
} from './core/auth/admin-auth.guard';

export const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    redirectTo: 'artworks',
  },
  {
    path: 'login',
    loadComponent: () =>
      import(
        './features/auth/pages/admin-login/admin-login'
      ).then(
        (component) => component.AdminLogin,
      ),
    title: 'Belépés | Henn László Admin',
  },
  {
    path: 'artworks/new',
    loadComponent: () =>
      import(
        './features/artworks/pages/admin-create-artwork/admin-create-artwork'
      ).then(component => component.AdminCreateArtwork),
    canActivate: [adminAuthGuard],
    title: 'Új mű | Henn László Admin',
  },
  {
  path: 'artworks/:artworkId/edit',
  loadComponent: () =>
    import(
      './features/artworks/pages/admin-edit-artwork/admin-edit-artwork'
    ).then(component => component.AdminEditArtwork),
  canActivate: [adminAuthGuard],
  title: 'Mű szerkesztése | Henn László Admin',
  },
  {
    path: 'artworks',
    loadComponent: () =>
      import(
        './features/artworks/pages/admin-artwork-list/admin-artwork-list'
      ).then(component => component.AdminArtworkList),
    canActivate: [adminAuthGuard],
    title: 'Művek kezelése | Henn László Admin',
  },
  {
    path: 'invitations',
    loadComponent: () =>
      import(
        './features/invitations/pages/admin-invitation-list/admin-invitation-list'
      ).then(component => component.AdminInvitationList),
    canActivate: [adminAuthGuard],
    title: 'Meghívók kezelése | Henn László Admin',
  },
  {
    path: 'invitations/new',
    loadComponent: () =>
      import(
        './features/invitations/pages/admin-create-invitation/admin-create-invitation'
      ).then(component => component.AdminCreateInvitation),
    canActivate: [adminAuthGuard],
    title: 'Új meghívó | Henn László Admin',
  },
  {
    path: 'invitations/:invitationId/edit',
    loadComponent: () =>
      import(
        './features/invitations/pages/admin-edit-invitation/admin-edit-invitation'
      ).then(component => component.AdminEditInvitation),
    canActivate: [adminAuthGuard],
    title: 'Meghívó szerkesztése | Henn László Admin',
  },
  {
    path: 'videos',
    loadComponent: () =>
      import(
        './features/videos/pages/admin-video-list/admin-video-list'
      ).then(component => component.AdminVideoList),
    canActivate: [adminAuthGuard],
    title: 'Videók kezelése | Henn László Admin',
  },
  {
    path: 'videos/new',
    loadComponent: () =>
      import(
        './features/videos/pages/admin-create-video/admin-create-video'
      ).then(component => component.AdminCreateVideo),
    canActivate: [adminAuthGuard],
    title: 'Új videó | Henn László Admin',
  },
  {
    path: 'videos/:videoId/edit',
    loadComponent: () =>
      import(
        './features/videos/pages/admin-edit-video/admin-edit-video'
      ).then(component => component.AdminEditVideo),
    canActivate: [adminAuthGuard],
    title: 'Videó szerkesztése | Henn László Admin',
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
    canActivate: [adminAuthGuard],
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
    canActivate: [adminAuthGuard],
  },
  {
    path: 'content/articles',
    loadComponent: () =>
      import(
        './features/content/pages/article-list/article-list'
      ).then(
        (component) => component.ArticleList,
      ),
    canActivate: [adminAuthGuard],
  },
  {
    path: 'content/articles/new',
    loadComponent: () =>
      import(
        './features/content/pages/article-editor/article-editor'
      ).then(
        (component) => component.ArticleEditor,
      ),
    canActivate: [adminAuthGuard],
  },
  {
    path: 'content/articles/:articleId/edit',
    loadComponent: () =>
      import(
        './features/content/pages/article-editor/article-editor'
      ).then(
        (component) => component.ArticleEditor,
      ),
    canActivate: [adminAuthGuard],
  },
  {
    path: 'settings',
    loadComponent: () =>
      import(
        './features/settings/pages/admin-website-settings/admin-website-settings'
      ).then(
        (component) =>
          component.AdminWebsiteSettings,
      ),
    canActivate: [adminAuthGuard],
    title: 'Webbeállítások | Henn László Admin',
  },
  {
    path: '**',
    redirectTo: 'artworks',
  },
];
