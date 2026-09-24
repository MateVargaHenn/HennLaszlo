import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ContentPage } from './content-page';
import { provideRouter } from '@angular/router';
import { ContentApi } from 'content-data-access';
import { of } from 'rxjs';

describe('ContentPage', () => {
  let component: ContentPage;
  let fixture: ComponentFixture<ContentPage>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ContentPage],
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

    fixture = TestBed.createComponent(ContentPage);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

await TestBed.configureTestingModule({
  imports: [ContentPage],
  providers: [
    provideRouter([]),
  ],
}).compileComponents();