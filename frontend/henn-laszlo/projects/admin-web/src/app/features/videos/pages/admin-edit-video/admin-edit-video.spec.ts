import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ActivatedRoute, convertToParamMap, provideRouter } from '@angular/router';
import { of } from 'rxjs';
import { VideoApi } from 'video-data-access';

import { AdminEditVideo } from './admin-edit-video';

describe('AdminEditVideo', () => {
  let component: AdminEditVideo;
  let fixture: ComponentFixture<AdminEditVideo>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AdminEditVideo],
      providers: [
        provideRouter([]),
        {
          provide: VideoApi,
          useValue: {
            getAdminVideoById: () => of(null),
          },
        },
      ],
    }).compileComponents();

    const route = TestBed.inject(ActivatedRoute);

    Object.defineProperty(route.snapshot, 'paramMap', {
      value: convertToParamMap({
        videoId: 'test-video',
      }),
    });

    fixture = TestBed.createComponent(AdminEditVideo);
    component = fixture.componentInstance;

    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
