import {
  ComponentFixture,
  TestBed,
} from '@angular/core/testing';
import { provideRouter } from '@angular/router';
import { ContentApi } from 'content-data-access';
import { of } from 'rxjs';

import {
  ArticleEditor,
} from './article-editor';

describe('ArticleEditor', () => {
  let component: ArticleEditor;
  let fixture:
    ComponentFixture<ArticleEditor>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ArticleEditor],
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
      ArticleEditor,
    );

    component = fixture.componentInstance;

    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});