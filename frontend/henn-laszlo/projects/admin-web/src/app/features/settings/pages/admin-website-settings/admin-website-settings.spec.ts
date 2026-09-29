import {
  ComponentFixture,
  TestBed,
} from '@angular/core/testing';
import { ContentApi } from 'content-data-access';
import { of } from 'rxjs';

import {
  AdminWebsiteSettings,
} from './admin-website-settings';

describe('AdminWebsiteSettings', () => {
  let component: AdminWebsiteSettings;
  let fixture:
    ComponentFixture<AdminWebsiteSettings>;

  const updateWebsiteSettings = vi.fn()
    .mockReturnValue(of(void 0));

  beforeEach(async () => {
    updateWebsiteSettings.mockClear();

    await TestBed.configureTestingModule({
      imports: [AdminWebsiteSettings],
      providers: [
        {
          provide: ContentApi,
          useValue: {
            getAdminWebsiteSettings: () =>
              of({
                id: 'settings-1',
                artistName: 'Henn László András',
                artistSubtitle:
                  'Festőművész, grafikus',
                heroDescription:
                  'Kortárs képzőművészeti alkotások.',
                defaultSeoTitle:
                  'Henn László András | Festőművész',
                defaultSeoDescription:
                  'Henn László András hivatalos weboldala.',
                facebookUrl: null,
                instagramUrl: null,
                youtubeUrl:
                  'https://youtube.com/@hennlaszlo',
                updatedAtUtc:
                  '2026-09-29T10:00:00Z',
              }),
            updateWebsiteSettings,
          },
        },
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(
      AdminWebsiteSettings,
    );

    component = fixture.componentInstance;

    fixture.detectChanges();
    await fixture.whenStable();
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should save the website settings', async () => {
    const form =
      fixture.nativeElement.querySelector(
        'form',
      ) as HTMLFormElement | null;

    expect(form).not.toBeNull();

    form!.dispatchEvent(
      new Event('submit'),
    );

    await fixture.whenStable();

    expect(updateWebsiteSettings)
      .toHaveBeenCalledWith({
        artistName: 'Henn László András',
        artistSubtitle:
          'Festőművész, grafikus',
        heroDescription:
          'Kortárs képzőművészeti alkotások.',
        defaultSeoTitle:
          'Henn László András | Festőművész',
        defaultSeoDescription:
          'Henn László András hivatalos weboldala.',
        facebookUrl: null,
        instagramUrl: null,
        youtubeUrl:
          'https://youtube.com/@hennlaszlo',
      });
  });
});
