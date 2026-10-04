import { HttpErrorResponse } from '@angular/common/http';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import {
  ContentApi,
  ContentRevisionDetails,
  ContentRevisionTarget,
  ContentRevisionsStore,
} from 'content-data-access';
import { of, throwError } from 'rxjs';
import { describe, expect, it, vi } from 'vitest';

import { ContentRevisionHistory } from './content-revision-history';

const revision: ContentRevisionDetails = {
  id: 'revision-1',
  titleHu: 'Korábbi cím',
  titleEn: null,
  summaryHu: null,
  summaryEn: null,
  contentHu: '<p>Korábbi tartalom</p>',
  contentEn: null,
  createdAtUtc: '2026-10-01T12:00:00Z',
};

const targets: readonly ContentRevisionTarget[] = [
  'articles',
  'content-pages',
];

for (const target of targets) {
  describe(`ContentRevisionsStore restore: ${target}`, () => {
    async function setup(fail: boolean) {
      const conflict = new HttpErrorResponse({ status: 409 });
      const restore = vi.fn(() => fail
        ? throwError(() => conflict)
        : of(undefined));

      await TestBed.configureTestingModule({
        imports: [ContentRevisionHistory],
        providers: [
          ContentRevisionsStore,
          {
            provide: ContentApi,
            useValue: {
              getContentRevisions: vi.fn(() => of([
                {
                  id: revision.id,
                  titleHu: revision.titleHu,
                  createdAtUtc: revision.createdAtUtc,
                },
              ])),
              getContentRevision: vi.fn(() => of(revision)),
              restoreContentRevision: restore,
            },
          },
        ],
      }).compileComponents();

      const store = TestBed.inject(ContentRevisionsStore);
      const fixture = TestBed.createComponent(ContentRevisionHistory);
      fixture.componentRef.setInput('expectedVersion', 'loaded-version');
      store.load(target, 'target-1');
      store.selectRevision(revision.id);
      fixture.detectChanges();
      await fixture.whenStable();
      fixture.detectChanges();

      return { store, fixture, restore, conflict };
    }

    function button(
      fixture: ComponentFixture<ContentRevisionHistory>,
      label: string,
    ): HTMLButtonElement {
      const buttons = fixture.nativeElement.querySelectorAll(
        'button',
      ) as NodeListOf<HTMLButtonElement>;
      const found = Array.from(buttons).find(
        item => item.textContent?.trim() === label,
      );
      if (!found) {
        throw new Error(`Missing button: ${label}`);
      }
      return found;
    }

    it('should forward the loaded version and retain the preview on HTTP 409', async () => {
      const { store, fixture, restore, conflict } = await setup(true);
      const restored = vi.fn();
      fixture.componentInstance.restored.subscribe(restored);

      button(fixture, 'Verzió visszaállítása').click();
      fixture.detectChanges();
      button(fixture, 'Visszaállítás').click();
      await fixture.whenStable();
      fixture.detectChanges();

      expect(restore).toHaveBeenCalledExactlyOnceWith(
        target, 'target-1', revision.id, 'loaded-version',
      );
      expect(store.restoreError()).toBe(conflict);
      expect(store.selectedRevision()).toEqual(revision);
      expect(store.isRestoring()).toBe(false);
      expect(restored).not.toHaveBeenCalled();
      expect(fixture.nativeElement.textContent).toContain(
        'A visszaállítás nem történt meg.',
      );
    });

    it('should close the preview and emit restored after a successful restore', async () => {
      const { store, fixture, restore } = await setup(false);
      const restored = vi.fn();
      fixture.componentInstance.restored.subscribe(restored);

      button(fixture, 'Verzió visszaállítása').click();
      fixture.detectChanges();
      button(fixture, 'Visszaállítás').click();
      await fixture.whenStable();
      fixture.detectChanges();

      expect(restore).toHaveBeenCalledExactlyOnceWith(
        target, 'target-1', revision.id, 'loaded-version',
      );
      expect(store.restoreError()).toBeUndefined();
      expect(store.selectedRevision()).toBeNull();
      expect(store.isRestoring()).toBe(false);
      expect(restored).toHaveBeenCalledOnce();
    });
  });
}
