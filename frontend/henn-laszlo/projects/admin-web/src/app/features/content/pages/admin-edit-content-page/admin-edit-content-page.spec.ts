import {
  ComponentFixture,
  TestBed,
} from '@angular/core/testing';
import {
  ActivatedRoute,
  convertToParamMap,
  provideRouter,
} from '@angular/router';
import { ContentApi } from 'content-data-access';
import { of } from 'rxjs';

import {
  AdminEditContentPage,
} from './admin-edit-content-page';
import { vi } from 'vitest';

describe('AdminEditContentPage', () => {
  let component: AdminEditContentPage;
  let fixture:
    ComponentFixture<AdminEditContentPage>;

    const getAdminContentPage =
  vi.fn(() => of(null));

  getAdminContentPage.mockClear();

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AdminEditContentPage],
      providers: [
        provideRouter([]),
        {
          provide: ContentApi,
          useValue: {
            getAdminContentPages: () =>
              of([]),

            getAdminContentPage,
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
          key: 'contact',
        }),
      },
    );

    fixture = TestBed.createComponent(
      AdminEditContentPage,
    );

    component = fixture.componentInstance;

    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
  it('should load the about content page', () => {
  expect(
    getAdminContentPage,
  ).toHaveBeenCalledWith('contact');
});
});