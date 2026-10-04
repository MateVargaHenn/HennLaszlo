import { HttpErrorResponse } from '@angular/common/http';
import { Type } from '@angular/core';
import { TestBed } from '@angular/core/testing';
import {
  ActivatedRoute,
  convertToParamMap,
  provideRouter,
  Router,
} from '@angular/router';
import { ContentApi } from 'content-data-access';
import { of, throwError } from 'rxjs';
import { describe, expect, it, vi } from 'vitest';

import {
  LocalDraftStorage,
} from '../../../core/drafts/local-draft-storage';
import { ArticleEditor } from './article-editor/article-editor';
import {
  AdminEditContentPage,
} from './admin-edit-content-page/admin-edit-content-page';
import { FormGroup } from '@angular/forms';

type Editor =
  ArticleEditor | AdminEditContentPage;

const cases: {
  name: string;
  component: Type<Editor>;
  params: Record<string, string>;
  isArticle: boolean;
}[] = [
  {
    name: 'ArticleEditor',
    component: ArticleEditor,
    params: { articleId: 'article-1' },
    isArticle: true,
  },
  {
    name: 'AdminEditContentPage',
    component: AdminEditContentPage,
    params: { key: 'contact' },
    isArticle: false,
  },
];

for (const testCase of cases) {
  describe(`${testCase.name} conflict handling`, () => {
    it(
      'should send the loaded version and preserve edits on HTTP 409',
      async () => {
        const conflict = new HttpErrorResponse({
          status: 409,
          statusText: 'Conflict',
        });

        const rejectSave = vi.fn(() =>
          throwError(() => conflict),
        );

        const draftStorage = {
          load: vi.fn(() => undefined),
          save: vi.fn(),
          remove: vi.fn(),
        };

        await TestBed.configureTestingModule({
          imports: [testCase.component],
          providers: [
            provideRouter([]),
            {
              provide: ActivatedRoute,
              useValue: {
                snapshot: {
                  paramMap: convertToParamMap(
                    testCase.params,
                  ),
                },
              },
            },
            {
              provide: LocalDraftStorage,
              useValue: draftStorage,
            },
            {
              provide: ContentApi,
              useValue: {
                getAdminArticles: () => of([]),
                getAdminContentPages: () => of([]),
                getContentRevisions: () => of([]),

                getAdminArticleById: () => of({
                  id: 'article-1',
                  slug: 'test-article',
                  titleHu: 'Eredeti cím',
                  titleEn: null,
                  summaryHu: null,
                  summaryEn: null,
                  contentHu: '<p>Eredeti tartalom</p>',
                  contentEn: null,
                  isPublished: false,
                  createdAtUtc: '2026-10-01T12:00:00Z',
                  updatedAtUtc: '2026-10-01T12:00:00Z',
                  version: 'version-1',
                }),

                getAdminContentPage: () => of({
                  id: 'page-1',
                  key: 'contact',
                  titleHu: 'Eredeti cím',
                  titleEn: null,
                  contentHu: '<p>Eredeti tartalom</p>',
                  contentEn: null,
                  isPublished: false,
                  updatedAtUtc: '2026-10-01T12:00:00Z',
                  version: 'version-1',
                }),

                updateArticle: rejectSave,
                upsertContentPage: rejectSave,
              },
            },
          ],
        })
          .overrideComponent(testCase.component, {
            set: {
              template: `
                <form
                  [formGroup]="form"
                  (ngSubmit)="save()"
                ></form>
              `,
            },
          })
          .compileComponents();

        const fixture = TestBed.createComponent(
          testCase.component,
        );

        fixture.detectChanges();
        await fixture.whenStable();
        fixture.detectChanges();

        const component =
            fixture.componentInstance as unknown as {
                readonly form: FormGroup;
                readonly hasVersionConflict: () => boolean;
            };

        const form = component.form;

        form.patchValue(
          {
            titleHu: 'Saját módosított cím',
            contentHu: '<p>Saját módosított tartalom</p>',
          },
          { emitEvent: false },
        );
        form.markAsDirty();

        const editedValue = form.getRawValue();

        const navigate = vi
          .spyOn(TestBed.inject(Router), 'navigate')
          .mockResolvedValue(true);

        const formElement =
          fixture.nativeElement.querySelector(
            'form',
          ) as HTMLFormElement;

        formElement.dispatchEvent(
          new Event('submit', {
            bubbles: true,
            cancelable: true,
          }),
        );

        await fixture.whenStable();
        fixture.detectChanges();

        const expectedRequest = expect.objectContaining({
          expectedVersion: 'version-1',
          titleHu: 'Saját módosított cím',
          contentHu: '<p>Saját módosított tartalom</p>',
        });

        if (testCase.isArticle) {
          expect(rejectSave)
            .toHaveBeenCalledExactlyOnceWith(
              'article-1',
              expectedRequest,
            );
        }
        else {
          expect(rejectSave)
            .toHaveBeenCalledExactlyOnceWith(
              'contact',
              expectedRequest,
            );
        }

        expect(form.getRawValue()).toEqual(editedValue);
        expect(form.dirty).toBe(true);
        expect(component.hasVersionConflict()).toBe(true);
        expect(navigate).not.toHaveBeenCalled();
        expect(draftStorage.remove).not.toHaveBeenCalled();
      },
    );
  });
}