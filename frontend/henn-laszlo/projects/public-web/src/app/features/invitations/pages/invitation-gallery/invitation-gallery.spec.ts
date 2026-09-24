import { ComponentFixture, TestBed } from '@angular/core/testing';
import { InvitationGallery } from './invitation-gallery';
import { InvitationApi } from 'invitation-data-access';
import { of } from 'rxjs';

describe('InvitationGallery', () => {
  let component: InvitationGallery;
  let fixture: ComponentFixture<InvitationGallery>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [InvitationGallery],
      providers: [
        {
          provide: InvitationApi,
          useValue: {
            getPublishedInvitations: () => of([]),
            getInvitationImageUrl: () =>
              '/images/test.webp',
          },
        },
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(InvitationGallery);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
