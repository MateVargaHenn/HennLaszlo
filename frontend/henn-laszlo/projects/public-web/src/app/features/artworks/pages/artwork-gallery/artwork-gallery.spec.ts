import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ArtworkGallery } from './artwork-gallery';

describe('ArtworkGallery', () => {
  let component: ArtworkGallery;
  let fixture: ComponentFixture<ArtworkGallery>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ArtworkGallery],
    }).compileComponents();

    fixture = TestBed.createComponent(ArtworkGallery);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
