import { ComponentFixture, TestBed } from '@angular/core/testing';
import {
  PublicWebsiteSettingsStore,
} from 'content-data-access';
import { PublicFooter } from './public-footer';

describe('PublicFooter', () => {
  let component: PublicFooter;
  let fixture: ComponentFixture<PublicFooter>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PublicFooter],
      providers: [
        {
          provide: PublicWebsiteSettingsStore,
          useValue: {
            settings: () => ({
              artistName: 'Teszt Művész',
              artistSubtitle: 'Festőművész',
              facebookUrl:
                'https://facebook.com/test',
              instagramUrl: null,
              youtubeUrl:
                'https://youtube.com/@test',
            }),
          },
        },
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(PublicFooter);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should display settings and social links', () => {
    fixture.detectChanges();

    const content =
      fixture.nativeElement.textContent;

    const socialLinks =
      fixture.nativeElement.querySelectorAll(
        'nav[aria-label="Közösségi oldalak"] a',
      );

    expect(content).toContain('Teszt Művész');
    expect(content).toContain('Festőművész');
    expect(socialLinks.length).toBe(2);
  });
});
