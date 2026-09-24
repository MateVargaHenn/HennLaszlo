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

describe('AdminEditContentPage', () => {
  let component: AdminEditContentPage;
  let fixture:
    ComponentFixture<AdminEditContentPage>;

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

            getAdminContentPage: () =>
              of(null),
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
          key: 'exhibitions',
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
});