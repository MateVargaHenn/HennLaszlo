import {
  ComponentFixture,
  TestBed,
} from '@angular/core/testing';
import {
  ActivatedRoute,
  convertToParamMap,
  provideRouter,
} from '@angular/router';
import {
  InvitationApi,
} from 'invitation-data-access';
import { of } from 'rxjs';

import {
  AdminEditInvitation,
} from './admin-edit-invitation';

describe('AdminEditInvitation', () => {
  let component: AdminEditInvitation;
  let fixture:
    ComponentFixture<AdminEditInvitation>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AdminEditInvitation],
      providers: [
        provideRouter([]),
        {
          provide: InvitationApi,
          useValue: {
            getAdminInvitationById: () =>
              of(null),

            getAdminInvitationImageUrl: () =>
              '/images/test.webp',
          },
        },
      ],
    }).compileComponents();

    const route =
      TestBed.inject(ActivatedRoute);

    Object.defineProperty(
      route.snapshot,
      'paramMap',
      {
        value: convertToParamMap({
          invitationId: 'test-invitation',
        }),
      },
    );

    fixture = TestBed.createComponent(
      AdminEditInvitation,
    );

    component = fixture.componentInstance;

    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});