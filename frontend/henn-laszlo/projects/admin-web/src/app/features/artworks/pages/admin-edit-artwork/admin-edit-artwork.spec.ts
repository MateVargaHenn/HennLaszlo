import { ComponentFixture, TestBed } from '@angular/core/testing';
import { AdminEditArtwork } from './admin-edit-artwork';

describe('AdminEditArtwork', () => {
  let component: AdminEditArtwork;
  let fixture: ComponentFixture<AdminEditArtwork>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AdminEditArtwork],
    }).compileComponents();

    fixture = TestBed.createComponent(AdminEditArtwork);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
