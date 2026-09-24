import {
  ComponentFixture,
  TestBed,
} from '@angular/core/testing';
import { provideRouter } from '@angular/router';
import { ArtworkApi } from 'artwork-data-access';
import { of } from 'rxjs';

import {
  AdminCreateArtwork,
} from './admin-create-artwork';

describe('AdminCreateArtwork', () => {
  let component: AdminCreateArtwork;
  let fixture:
    ComponentFixture<AdminCreateArtwork>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AdminCreateArtwork],
      providers: [
        provideRouter([]),
        {
          provide: ArtworkApi,
          useValue: {
            getAdminArtworks: () =>
              of([]),
          },
        },
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(
      AdminCreateArtwork,
    );

    component = fixture.componentInstance;

    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});