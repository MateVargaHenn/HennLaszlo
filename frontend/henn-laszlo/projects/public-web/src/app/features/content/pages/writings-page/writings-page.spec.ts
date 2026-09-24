import { ComponentFixture, TestBed } from '@angular/core/testing';
import { WritingsPage } from './writings-page';
import { ContentApi } from 'content-data-access';
import { of } from 'rxjs';

describe('WritingsPage', () => {
  let component: WritingsPage;
  let fixture: ComponentFixture<WritingsPage>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [WritingsPage],
      providers: [
        {
          provide: ContentApi,
          useValue: {
            getPublishedContentPage: () => of(null),
            getPublishedArticleBySlug: () => of(null),
          },
        },
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(WritingsPage);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
