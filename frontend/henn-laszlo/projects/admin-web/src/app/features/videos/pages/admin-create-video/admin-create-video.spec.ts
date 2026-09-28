import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideRouter } from '@angular/router';
import { of } from 'rxjs';
import { VideoApi } from 'video-data-access';

import { AdminCreateVideo } from './admin-create-video';

describe('AdminCreateVideo', () => {
  let component: AdminCreateVideo;
  let fixture: ComponentFixture<AdminCreateVideo>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AdminCreateVideo],
      providers: [
        provideRouter([]),
        {
          provide: VideoApi,
          useValue: {
            createVideo: () => of({ id: 'test-video' }),
          },
        },
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(AdminCreateVideo);
    component = fixture.componentInstance;

    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
