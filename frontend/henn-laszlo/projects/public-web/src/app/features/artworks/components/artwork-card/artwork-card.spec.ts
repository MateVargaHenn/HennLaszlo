import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ArtworkCard } from './artwork-card';
import {
  provideRouter,
} from '@angular/router';

describe('ArtworkCard', () => {
  let component: ArtworkCard;
  let fixture: ComponentFixture<ArtworkCard>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ArtworkCard],
    }).compileComponents();

    fixture = TestBed.createComponent(ArtworkCard);

    fixture.componentRef.setInput(
      'artwork',
      {
        id: 'test-artwork',
        titleHu: 'Tesztmű',
        titleEn: null,
        year: 2026,
        techniqueHu: 'Olaj, vászon',
        techniqueEn: null,
        widthCm: 80,
        heightCm: 60,
        isFeatured: false,
        displayOrder: 1,
      },
    );

    fixture.componentRef.setInput(
      'imageUrl',
      '/images/test.webp',
    );

    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
await TestBed.configureTestingModule({
  imports: [ArtworkCard],
  providers: [
    provideRouter([]),
  ],
}).compileComponents();
