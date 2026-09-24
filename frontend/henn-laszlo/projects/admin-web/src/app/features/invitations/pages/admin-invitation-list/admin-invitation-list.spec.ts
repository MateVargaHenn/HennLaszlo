import {
  ComponentFixture,
  TestBed,
} from '@angular/core/testing';
import { provideRouter } from '@angular/router';
import {
  InvitationApi,
} from 'invitation-data-access';
import { of } from 'rxjs';

import {
  AdminInvitationList,
} from './admin-invitation-list';

describe('AdminInvitationList', () => {
  let component: AdminInvitationList;
  let fixture:
    ComponentFixture<AdminInvitationList>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AdminInvitationList],
      providers: [
        provideRouter([]),
        {
          provide: InvitationApi,
          useValue: {
            getAdminInvitations: () =>
              of([]),

            getAdminInvitationImageUrl: () =>
              '/images/test.webp',
          },
        },
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(
      AdminInvitationList,
    );

    component = fixture.componentInstance;

    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});