import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ArtworkDetails } from './artwork-details';
import { provideRouter } from '@angular/router';
import { ArtworkApi } from 'artwork-data-access';
import { of } from 'rxjs';

describe('ArtworkDetails', () => {
  let component: ArtworkDetails;
  let fixture: ComponentFixture<ArtworkDetails>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ArtworkDetails],
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

    fixture = TestBed.createComponent(ArtworkDetails);
    component = fixture.componentInstance;
    fixture.componentRef.setInput(
      'artworkId',
      'test-artwork',
    );
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
