import { ComponentFixture, TestBed } from '@angular/core/testing';
import { Home } from './home';
import { provideRouter } from '@angular/router';
import { ArtworkApi } from 'artwork-data-access';
import {
  PublicWebsiteSettingsStore,
} from 'content-data-access';
import { of } from 'rxjs';

describe('Home', () => {
  let component: Home;
  let fixture: ComponentFixture<Home>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Home],
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
        {
          provide: PublicWebsiteSettingsStore,
          useValue: {
            settings: () => ({
              artistName: 'Teszt Művész',
              artistSubtitle: 'Festőművész',
              heroDescription:
                'Teszt kezdőlapi leírás.',
              defaultSeoTitle: 'Tesztoldal',
              defaultSeoDescription:
                'Teszt SEO-leírás.',
              facebookUrl: null,
              instagramUrl: null,
              youtubeUrl: null,
            }),
          },
        },
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(Home);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should display the configured artist data', () => {
    fixture.detectChanges();

    const content =
      fixture.nativeElement.textContent;

    expect(content).toContain('Teszt Művész');
    expect(content).toContain('Festőművész');
    expect(content).toContain(
      'Teszt kezdőlapi leírás.',
    );
  });
});
