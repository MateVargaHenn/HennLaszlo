import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideRouter } from '@angular/router';
import { of } from 'rxjs';
import { VideoApi } from 'video-data-access';

import { AdminVideoList } from './admin-video-list';

describe('AdminVideoList', () => {
  let component: AdminVideoList;
  let fixture: ComponentFixture<AdminVideoList>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AdminVideoList],
      providers: [
        provideRouter([]),
        {
          provide: VideoApi,
          useValue: {
            getAdminVideos: () => of([]),
          },
        },
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(AdminVideoList);
    component = fixture.componentInstance;

    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
