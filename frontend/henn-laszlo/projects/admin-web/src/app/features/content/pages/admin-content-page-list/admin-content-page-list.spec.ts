import { ComponentFixture, TestBed } from '@angular/core/testing';
import { AdminContentPageList } from './admin-content-page-list';

describe('AdminContentPageList', () => {
  let component: AdminContentPageList;
  let fixture: ComponentFixture<AdminContentPageList>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AdminContentPageList],
    }).compileComponents();

    fixture = TestBed.createComponent(AdminContentPageList);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
