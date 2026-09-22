import { ComponentFixture, TestBed } from '@angular/core/testing';
import { AdminCreateArtwork } from './admin-create-artwork';

describe('AdminCreateArtwork', () => {
  let component: AdminCreateArtwork;
  let fixture: ComponentFixture<AdminCreateArtwork>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AdminCreateArtwork],
    }).compileComponents();

    fixture = TestBed.createComponent(AdminCreateArtwork);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
