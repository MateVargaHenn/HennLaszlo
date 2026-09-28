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

  const writeText =
    vi.fn()
      .mockResolvedValue(undefined);

  Object.defineProperty(
    navigator,
    'clipboard',
    {
      configurable: true,
      value: {
        writeText,
      },
    },
  );

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ArticleList],
      providers: [
        provideRouter([]),
        {
          provide: ContentApi,
          useValue: {
              getAdminArticles: vi.fn()
                .mockReturnValue(
                  of([
                    {
                      id: 'article-1',
                      slug:
                        'fodor-jozsef-megnyitobeszede-2002',
                      titleHu:
                        'Fodor József megnyitóbeszéde',
                      titleEn:
                        'Opening speech by József Fodor',
                      isPublished: true,
                      updatedAtUtc:
                        '2026-09-28T10:00:00Z',
                    },
                  ]),
                ),
            },
        },
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(
      ArticleList,
    );

    component = fixture.componentInstance;
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it(
    'should copy the public article path',
    async () => {
      fixture.detectChanges();

      await fixture.whenStable();
      fixture.detectChanges();

      const copyButton =
        fixture.nativeElement.querySelector(
          '.article-copy-button',
        ) as HTMLButtonElement;

      expect(copyButton).not.toBeNull();

      copyButton.click();

      await Promise.resolve();
      fixture.detectChanges();

      expect(writeText)
        .toHaveBeenCalledWith(
          '/irasok/fodor-jozsef-megnyitobeszede-2002',
        );

      expect(copyButton.textContent)
        .toContain('Kimásolva');
    },
  );
});