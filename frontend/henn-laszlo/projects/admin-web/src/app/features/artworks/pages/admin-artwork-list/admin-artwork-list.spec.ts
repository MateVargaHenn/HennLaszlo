import {
  ComponentFixture,
  TestBed,
} from '@angular/core/testing';
import { provideRouter } from '@angular/router';
import { ArtworkApi } from 'artwork-data-access';
import { of } from 'rxjs';

import {
  AdminArtworkList,
} from './admin-artwork-list';

describe('AdminArtworkList', () => {
  let component: AdminArtworkList;
  let fixture:
    ComponentFixture<AdminArtworkList>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AdminArtworkList],
      providers: [
        provideRouter([]),
        {
          provide: ArtworkApi,
          useValue: {
            getAdminArtworks: () =>
              of([]),

            getAdminArtworkImageUrl: () =>
              '/images/test.webp',
          },
        },
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(
      AdminArtworkList,
    );

    component = fixture.componentInstance;

    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});