import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';

import { provideVideoDataAccess } from '../config/video-data-access-config';
import { VideoApi } from './video-api';

describe('VideoApi', () => {
  let api: VideoApi;
  let http: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        provideHttpClient(),
        provideHttpClientTesting(),
        provideVideoDataAccess({
          apiBaseUrl: 'https://api.example.test/',
        }),
      ],
    });

    api = TestBed.inject(VideoApi);
    http = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    http.verify();
  });

  it('should load published videos', () => {
    api.getPublishedVideos().subscribe((videos) => {
      expect(videos).toEqual([]);
    });

    const request = http.expectOne('https://api.example.test/api/videos');

    expect(request.request.method).toBe('GET');

    request.flush([]);
  });

  it('should create an admin video', () => {
    const body = {
      titleHu: 'Próbavideó',
      titleEn: null,
      year: 2026,
      descriptionHu: null,
      descriptionEn: null,
      videoUrl: 'https://youtu.be/dQw4w9WgXcQ',
      displayOrder: 0,
    };

    api.createVideo(body).subscribe((response) => {
      expect(response).toEqual({ id: 'video-1' });
    });

    const request = http.expectOne('https://api.example.test/api/admin/videos');

    expect(request.request.method).toBe('POST');
    expect(request.request.body).toEqual(body);

    request.flush({ id: 'video-1' });
  });
});
