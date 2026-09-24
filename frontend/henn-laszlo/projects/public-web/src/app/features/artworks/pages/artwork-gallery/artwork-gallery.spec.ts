import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ArtworkGallery } from './artwork-gallery';
import { provideRouter } from '@angular/router';
import { ArtworkApi } from 'artwork-data-access';
import { of } from 'rxjs';

describe('ArtworkGallery', () => {
  let component: ArtworkGallery;
  let fixture: ComponentFixture<ArtworkGallery>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ArtworkGallery],
      providers: [
        provideRouter([]),
        {
          provide: ArtworkApi,
          useValue: {
            getPublishedArtworks: () => of([]),
            getArtworkById: () => of(null),
            getArtworkImageUrl: () =>
              '/images/test.webp',
          },
        },
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(ArtworkGallery);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
