import { ComponentFixture, TestBed } from '@angular/core/testing';

import { VideoForm } from './video-form';

describe('VideoForm', () => {
  let component: VideoForm;
  let fixture: ComponentFixture<VideoForm>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [VideoForm],
    }).compileComponents();

    fixture = TestBed.createComponent(VideoForm);
    component = fixture.componentInstance;

    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
