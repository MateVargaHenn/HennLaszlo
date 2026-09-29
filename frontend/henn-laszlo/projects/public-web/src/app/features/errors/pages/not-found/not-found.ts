import {
  ChangeDetectionStrategy,
  Component,
  inject,
} from '@angular/core';
import {
  Router,
  RouterLink,
} from '@angular/router';

import {
  SsrResponseService,
} from '../../../../core/http/ssr-response.service';
import {
  SeoService,
} from '../../../../core/seo/seo.service';

@Component({
  selector: 'app-not-found',
  imports: [RouterLink],
  templateUrl:
    './not-found.html',
  changeDetection:
    ChangeDetectionStrategy.OnPush,
})
export class NotFound {
  private readonly router =
    inject(Router);

  private readonly seo =
    inject(SeoService);

  private readonly response =
    inject(SsrResponseService);

  constructor() {
    this.response.setNotFound();

    const canonicalPath =
      this.router.url
        .split('?')[0] ||
      '/';

    this.seo.updatePage({
      title:
        'Az oldal nem található',
      description:
        'A keresett oldal nem található Henn László András honlapján.',
      canonicalPath,
      type: 'website',
      robots:
        'noindex, nofollow, noarchive',
    });
  }
}