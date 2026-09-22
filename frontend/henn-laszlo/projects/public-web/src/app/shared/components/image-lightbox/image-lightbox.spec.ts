import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ImageLightbox } from './image-lightbox';

describe('ImageLightbox', () => {
  let component: ImageLightbox;
  let fixture: ComponentFixture<ImageLightbox>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ImageLightbox],
    }).compileComponents();

    fixture = TestBed.createComponent(ImageLightbox);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
