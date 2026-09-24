import {
  ComponentFixture,
  TestBed,
} from '@angular/core/testing';
import { provideRouter } from '@angular/router';
import { ContentApi } from 'content-data-access';
import { of } from 'rxjs';

import {
  AdminContentPageList,
} from './admin-content-page-list';

describe('AdminContentPageList', () => {
  let component: AdminContentPageList;
  let fixture:
    ComponentFixture<AdminContentPageList>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AdminContentPageList],
      providers: [
        provideRouter([]),
        {
          provide: ContentApi,
          useValue: {
            getAdminContentPages: () =>
              of([]),
          },
        },
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(
      AdminContentPageList,
    );

    component = fixture.componentInstance;

    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});