import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ArtworkForm } from './artwork-form';

describe('ArtworkForm', () => {
  let component: ArtworkForm;
  let fixture: ComponentFixture<ArtworkForm>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ArtworkForm],
    }).compileComponents();

    fixture = TestBed.createComponent(ArtworkForm);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
