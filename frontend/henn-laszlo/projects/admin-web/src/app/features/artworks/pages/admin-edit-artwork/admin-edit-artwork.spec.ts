import {
  ComponentFixture,
  TestBed,
} from '@angular/core/testing';
import {
  ActivatedRoute,
  convertToParamMap,
  provideRouter,
} from '@angular/router';
import { ArtworkApi } from 'artwork-data-access';
import { of } from 'rxjs';

import {
  AdminEditArtwork,
} from './admin-edit-artwork';

describe('AdminEditArtwork', () => {
  let component: AdminEditArtwork;
  let fixture:
    ComponentFixture<AdminEditArtwork>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AdminEditArtwork],
      providers: [
        provideRouter([]),
        {
          provide: ArtworkApi,
          useValue: {
            getAdminArtworkById: () =>
              of(null),

            getAdminArtworkImageUrl: () =>
              '/images/test.webp',
          },
        },
      ],
    }).compileComponents();

    const route =
      TestBed.inject(ActivatedRoute);

    Object.defineProperty(
      route.snapshot,
      'paramMap',
      {
        value: convertToParamMap({
          artworkId: 'test-artwork',
        }),
      },
    );

    fixture = TestBed.createComponent(
      AdminEditArtwork,
    );

    component = fixture.componentInstance;

    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});