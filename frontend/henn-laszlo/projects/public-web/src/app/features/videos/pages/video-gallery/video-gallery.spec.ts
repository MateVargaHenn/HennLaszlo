import { ComponentFixture, TestBed } from '@angular/core/testing';
import { of } from 'rxjs';
import { VideoApi } from 'video-data-access';

import { VideoGallery } from './video-gallery';

describe('VideoGallery', () => {
  let component: VideoGallery;
  let fixture: ComponentFixture<VideoGallery>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [VideoGallery],
      providers: [
        {
          provide: VideoApi,
          useValue: {
            getPublishedVideos: () =>
              of([
                {
                  id: 'video-1',
                  titleHu: 'Próbavideó',
                  titleEn: null,
                  year: 2026,
                  descriptionHu: null,
                  descriptionEn: null,
                  videoUrl: 'https://youtu.be/dQw4w9WgXcQ',
                  displayOrder: 0,
                },
              ]),
          },
        },
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(VideoGallery);
    component = fixture.componentInstance;

    await fixture.whenStable();
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should embed a YouTube video', () => {
    const iframe = fixture.nativeElement.querySelector('iframe') as HTMLIFrameElement | null;

    expect(iframe).not.toBeNull();
    expect(iframe?.title).toBe('Próbavideó');
  });
});
