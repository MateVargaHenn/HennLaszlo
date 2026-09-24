import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ArticleDetails } from './article-details';
import {
  provideRouter,
} from '@angular/router';
import { ContentApi } from 'content-data-access';
import { of } from 'rxjs';

describe('ArticleDetails', () => {
  let component: ArticleDetails;
  let fixture: ComponentFixture<ArticleDetails>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ArticleDetails],
      providers: [
        provideRouter([]),
        {
          provide: ContentApi,
          useValue: {
            getPublishedContentPage: () => of(null),
            getPublishedArticleBySlug: () => of(null),
          },
        },
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(ArticleDetails);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

await TestBed.configureTestingModule({
  imports: [ArticleDetails],
  providers: [
    provideRouter([]),
  ],
}).compileComponents();
