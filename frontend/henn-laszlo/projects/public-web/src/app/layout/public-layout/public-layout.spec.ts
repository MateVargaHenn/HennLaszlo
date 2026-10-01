import { ComponentFixture, TestBed } from '@angular/core/testing';
import { PublicLayout } from './public-layout';
import {
  provideRouter,
} from '@angular/router';
import { provideHttpClient } from '@angular/common/http';
import {
  PublicWebsiteSettingsStore,
} from 'content-data-access';

describe('PublicLayout', () => {
  let component: PublicLayout;
  let fixture: ComponentFixture<PublicLayout>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PublicLayout],
      providers: [
        provideRouter([]),
        provideHttpClient(),
        {
          provide: PublicWebsiteSettingsStore,
          useValue: {
            settings: () => ({
              artistName: 'Teszt Művész',
              artistSubtitle: 'Festőművész',
              heroDescription: 'Teszt leírás.',
              defaultSeoTitle: 'Tesztoldal',
              defaultSeoDescription:
                'Teszt SEO-leírás.',
              facebookUrl: null,
              instagramUrl: null,
              youtubeUrl: null,
              emailAddress: null,
            }),
          },
        },
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(PublicLayout);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
