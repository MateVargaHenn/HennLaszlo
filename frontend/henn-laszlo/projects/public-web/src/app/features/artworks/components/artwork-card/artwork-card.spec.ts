import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ArtworkCard } from './artwork-card';

describe('ArtworkCard', () => {
  let component: ArtworkCard;
  let fixture: ComponentFixture<ArtworkCard>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ArtworkCard],
    }).compileComponents();

    fixture = TestBed.createComponent(ArtworkCard);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
