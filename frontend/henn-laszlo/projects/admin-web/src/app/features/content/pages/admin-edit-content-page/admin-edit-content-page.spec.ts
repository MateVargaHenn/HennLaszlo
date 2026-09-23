import { ComponentFixture, TestBed } from '@angular/core/testing';
import { AdminEditContentPage } from './admin-edit-content-page';

describe('AdminEditContentPage', () => {
  let component: AdminEditContentPage;
  let fixture: ComponentFixture<AdminEditContentPage>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AdminEditContentPage],
    }).compileComponents();

    fixture = TestBed.createComponent(AdminEditContentPage);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
