import { ComponentFixture, TestBed } from '@angular/core/testing';
import { InvitationDataAccess } from './invitation-data-access';

describe('InvitationDataAccess', () => {
  let component: InvitationDataAccess;
  let fixture: ComponentFixture<InvitationDataAccess>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [InvitationDataAccess],
    }).compileComponents();

    fixture = TestBed.createComponent(InvitationDataAccess);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
