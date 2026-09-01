import { ComponentFixture, TestBed } from '@angular/core/testing';
import { AdminArtworkList } from './admin-artwork-list';

describe('AdminArtworkList', () => {
  let component: AdminArtworkList;
  let fixture: ComponentFixture<AdminArtworkList>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AdminArtworkList],
    }).compileComponents();

    fixture = TestBed.createComponent(AdminArtworkList);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
