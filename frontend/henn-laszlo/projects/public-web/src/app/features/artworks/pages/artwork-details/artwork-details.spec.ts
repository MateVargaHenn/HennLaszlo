import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ArtworkDetails } from './artwork-details';

describe('ArtworkDetails', () => {
  let component: ArtworkDetails;
  let fixture: ComponentFixture<ArtworkDetails>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ArtworkDetails],
    }).compileComponents();

    fixture = TestBed.createComponent(ArtworkDetails);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
