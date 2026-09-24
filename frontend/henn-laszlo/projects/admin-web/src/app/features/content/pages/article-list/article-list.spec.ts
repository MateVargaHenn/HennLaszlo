import {
  ComponentFixture,
  TestBed,
} from '@angular/core/testing';
import { provideRouter } from '@angular/router';
import { ContentApi } from 'content-data-access';
import { of } from 'rxjs';

import { ArticleList } from './article-list';

describe('ArticleList', () => {
  let component: ArticleList;
  let fixture:
    ComponentFixture<ArticleList>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ArticleList],
      providers: [
        provideRouter([]),
        {
          provide: ContentApi,
          useValue: {
            getAdminArticles: () =>
              of([]),
          },
        },
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(
      ArticleList,
    );

    component = fixture.componentInstance;

    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});