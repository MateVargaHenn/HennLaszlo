import { ComponentFixture, TestBed } from '@angular/core/testing';
import { AdminInvitationList } from './admin-invitation-list';

describe('AdminInvitationList', () => {
  let component: AdminInvitationList;
  let fixture: ComponentFixture<AdminInvitationList>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AdminInvitationList],
    }).compileComponents();

    fixture = TestBed.createComponent(AdminInvitationList);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
