import {
  ComponentFixture,
  TestBed,
} from '@angular/core/testing';
import {
  ActivatedRoute,
  provideRouter,
} from '@angular/router';
import { ContentApi } from 'content-data-access';
import { of } from 'rxjs';
import { vi } from 'vitest';

import { ContentPage } from './content-page';

describe('ContentPage', () => {
  let component: ContentPage;
  let fixture: ComponentFixture<ContentPage>;

  const getPublishedContentPage =
    vi.fn(() => of(null));

  beforeEach(async () => {
    getPublishedContentPage.mockClear();

    await TestBed.configureTestingModule({
      imports: [ContentPage],
      providers: [
        provideRouter([]),
        {
          provide: ContentApi,
          useValue: {
            getPublishedContentPage,
          },
        },
      ],
    }).compileComponents();

    const route =
      TestBed.inject(ActivatedRoute);

    Object.defineProperty(
      route.snapshot,
      'data',
      {
        configurable: true,
        value: {
          contentPageKey: 'about',
        },
      },
    );

    fixture =
      TestBed.createComponent(ContentPage);

    component = fixture.componentInstance;
    fixture.detectChanges();

    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should load the about content page', () => {
    expect(
      getPublishedContentPage,
    ).toHaveBeenCalledWith('about');
  });
});