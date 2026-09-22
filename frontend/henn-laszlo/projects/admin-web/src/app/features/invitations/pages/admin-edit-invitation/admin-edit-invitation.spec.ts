import { ComponentFixture, TestBed } from '@angular/core/testing';
import { AdminEditInvitation } from './admin-edit-invitation';

describe('AdminEditInvitation', () => {
  let component: AdminEditInvitation;
  let fixture: ComponentFixture<AdminEditInvitation>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AdminEditInvitation],
    }).compileComponents();

    fixture = TestBed.createComponent(AdminEditInvitation);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
