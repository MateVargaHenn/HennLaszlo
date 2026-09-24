import { DOCUMENT } from '@angular/common';
import { TestBed } from '@angular/core/testing';
import {
  Meta,
  Title,
} from '@angular/platform-browser';

import { SeoService } from './seo.service';

describe('SeoService', () => {
  let document: Document;
  let meta: Meta;
  let service: SeoService;
  let title: Title;

  beforeEach(() => {
    TestBed.configureTestingModule({});

    document = TestBed.inject(DOCUMENT);
    meta = TestBed.inject(Meta);
    service = TestBed.inject(SeoService);
    title = TestBed.inject(Title);
  });

  it('should update page metadata with the default image', () => {
    service.updatePage({
      title: 'Tesztoldal',
      description: 'Tesztleírás.',
      canonicalPath: '/teszt',
    });

    expect(title.getTitle()).toBe(
      'Tesztoldal | Henn László András',
    );

    expect(
      meta.getTag(
        'property="og:image"',
      )?.content,
    ).toBe(
      'https://hennlaszlo.hu/video/medistacio-poster.webp',
    );

    expect(
      meta.getTag(
        'name="twitter:card"',
      )?.content,
    ).toBe('summary_large_image');

    const canonical =
      document.head
        .querySelector<HTMLLinkElement>(
          'link[rel="canonical"]',
        );

    expect(canonical?.href).toBe(
      'https://hennlaszlo.hu/teszt',
    );
  });
});