import { ComponentFixture, TestBed } from '@angular/core/testing';
import { InvitationGallery } from './invitation-gallery';

describe('InvitationGallery', () => {
  let component: InvitationGallery;
  let fixture: ComponentFixture<InvitationGallery>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [InvitationGallery],
    }).compileComponents();

    fixture = TestBed.createComponent(InvitationGallery);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
