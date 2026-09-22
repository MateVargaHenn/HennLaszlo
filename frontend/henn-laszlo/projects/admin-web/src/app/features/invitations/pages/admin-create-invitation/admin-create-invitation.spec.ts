import { ComponentFixture, TestBed } from '@angular/core/testing';
import { AdminCreateInvitation } from './admin-create-invitation';

describe('AdminCreateInvitation', () => {
  let component: AdminCreateInvitation;
  let fixture: ComponentFixture<AdminCreateInvitation>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AdminCreateInvitation],
    }).compileComponents();

    fixture = TestBed.createComponent(AdminCreateInvitation);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
