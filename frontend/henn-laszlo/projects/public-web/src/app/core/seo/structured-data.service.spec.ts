import { DOCUMENT } from '@angular/common';
import {
  TestBed,
} from '@angular/core/testing';

import {
  StructuredDataService,
} from './structured-data.service';

describe(
  'StructuredDataService',
  () => {
    let document: Document;
    let service:
      StructuredDataService;

    beforeEach(() => {
      TestBed.configureTestingModule(
        {},
      );

      document =
        TestBed.inject(DOCUMENT);

      service =
        TestBed.inject(
          StructuredDataService,
        );

      document
        .querySelector(
          'script[data-app-structured-data]',
        )
        ?.remove();
    });

    it(
      'should combine structured data groups',
      () => {
        service.setGroup(
          'site',
          {
            '@type': 'WebSite',
            name: 'Tesztoldal',
          },
        );

        service.setGroup(
          'page',
          {
            '@type': 'Article',
            headline: 'Tesztírás',
          },
        );

        const script =
          document.querySelector(
            'script[data-app-structured-data]',
          );

        expect(script).not.toBeNull();

        const data =
          JSON.parse(
            script?.textContent ??
            '{}',
          );

        expect(data['@context'])
          .toBe(
            'https://schema.org',
          );

        expect(data['@graph'])
          .toHaveLength(2);
      },
    );
  },
);