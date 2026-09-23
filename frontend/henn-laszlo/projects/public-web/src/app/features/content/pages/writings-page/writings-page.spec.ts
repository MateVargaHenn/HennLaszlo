import { ComponentFixture, TestBed } from '@angular/core/testing';
import { WritingsPage } from './writings-page';

describe('WritingsPage', () => {
  let component: WritingsPage;
  let fixture: ComponentFixture<WritingsPage>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [WritingsPage],
    }).compileComponents();

    fixture = TestBed.createComponent(WritingsPage);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
