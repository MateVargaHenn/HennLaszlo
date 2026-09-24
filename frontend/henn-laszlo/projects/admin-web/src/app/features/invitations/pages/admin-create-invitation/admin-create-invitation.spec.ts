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
  AdminCreateInvitation,
} from './admin-create-invitation';

describe('AdminCreateInvitation', () => {
  let component: AdminCreateInvitation;
  let fixture:
    ComponentFixture<AdminCreateInvitation>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AdminCreateInvitation],
      providers: [
        provideRouter([]),
        {
          provide: InvitationApi,
          useValue: {
            createInvitation: () =>
              of({
                id: 'test-invitation',
              }),
          },
        },
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(
      AdminCreateInvitation,
    );

    component = fixture.componentInstance;

    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});