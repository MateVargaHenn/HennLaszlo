import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ImageLightbox } from './image-lightbox';
import { vi } from 'vitest';

describe('ImageLightbox', () => {
  let component: ImageLightbox;
  let fixture: ComponentFixture<ImageLightbox>;

  Object.defineProperty(
    HTMLDialogElement.prototype,
    'showModal',
    {
      configurable: true,
      value: vi.fn(),
    },
  );

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ImageLightbox],
    }).compileComponents();

    fixture = TestBed.createComponent(ImageLightbox);

    fixture.componentRef.setInput(
      'imageUrl',
      '/images/test.webp',
    );

    fixture.componentRef.setInput(
      'alt',
      'Tesztkép',
    );

    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
