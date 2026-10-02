import {
  signal,
} from '@angular/core';
import {
  ComponentFixture,
  TestBed,
} from '@angular/core/testing';
import {
  ContentRevisionDetails,
  ContentRevisionListItem,
  ContentRevisionsStore,
} from 'content-data-access';

import {
  ContentRevisionHistory,
} from './content-revision-history';

const revisionDetails:
  ContentRevisionDetails = {
    id: 'revision-1',
    titleHu: 'Korábbi cím',
    titleEn: null,
    summaryHu: 'Korábbi összefoglaló',
    summaryEn: null,
    contentHu: '<p>Korábbi tartalom</p>',
    contentEn: null,
    createdAtUtc:
      '2026-10-01T12:00:00Z',
  };

function createStoreStub() {
  const selectedRevision =
    signal<ContentRevisionDetails | null>(
      null,
    );

  return {
    revisions:
      signal<
        readonly ContentRevisionListItem[]
      >([
        {
          id: revisionDetails.id,
          titleHu: revisionDetails.titleHu,
          createdAtUtc:
            revisionDetails.createdAtUtc,
        },
      ]),

    selectedRevision,

    isLoading: signal(false),
    loadError:
      signal<unknown | undefined>(
        undefined,
      ),

    isRevisionLoading: signal(false),
    revisionLoadError:
      signal<unknown | undefined>(
        undefined,
      ),

    isRestoring: signal(false),
    restoreError:
      signal<unknown | undefined>(
        undefined,
      ),

    reload: vi.fn(),

    selectRevision: vi.fn(
      (_revisionId: string) => {
        selectedRevision.set(
          revisionDetails,
        );
      },
    ),

    closeRevision: vi.fn(() => {
      selectedRevision.set(null);
    }),

    restore: vi.fn(
      async (_revisionId: string) =>
        undefined,
    ),
  };
}

describe('ContentRevisionHistory', () => {
  let fixture:
    ComponentFixture<
      ContentRevisionHistory
    >;

  let store:
    ReturnType<typeof createStoreStub>;

  beforeEach(async () => {
    store = createStoreStub();

    await TestBed.configureTestingModule({
      imports: [
        ContentRevisionHistory,
      ],
      providers: [
        {
          provide:
            ContentRevisionsStore,
          useValue: store,
        },
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(
      ContentRevisionHistory,
    );

    fixture.detectChanges();
  });

  it(
    'should open a selected revision',
    () => {
      const button = findButton(
        'Megtekintés',
      );

      expect(button).toBeTruthy();

      button!.click();
      fixture.detectChanges();

      expect(store.selectRevision)
        .toHaveBeenCalledWith(
          revisionDetails.id,
        );

      expect(
        fixture.nativeElement.textContent,
      ).toContain('Korábbi tartalom');
    },
  );

  it(
    'should restore a selected revision',
    async () => {
      store.selectedRevision.set(
        revisionDetails,
      );

      const restored = vi.fn();

      fixture.componentInstance.restored
        .subscribe(restored);

      fixture.detectChanges();

      findButton(
        'Verzió visszaállítása',
      )!.click();

      fixture.detectChanges();

      findButton('Visszaállítás')!
        .click();

      await fixture.whenStable();
      fixture.detectChanges();

      expect(store.restore)
        .toHaveBeenCalledWith(
          revisionDetails.id,
        );

      expect(restored)
        .toHaveBeenCalledOnce();
    },
  );

  it(
    'should prevent restoring with unsaved changes',
    () => {
      const alertSpy =
        vi.spyOn(window, 'alert')
          .mockImplementation(
            () => undefined,
          );

      fixture.componentRef.setInput(
        'hasUnsavedChanges',
        true,
      );

      store.selectedRevision.set(
        revisionDetails,
      );

      fixture.detectChanges();

      findButton(
        'Verzió visszaállítása',
      )!.click();

      fixture.detectChanges();

      expect(alertSpy)
        .toHaveBeenCalledOnce();

      expect(store.restore)
        .not.toHaveBeenCalled();

      alertSpy.mockRestore();
    },
  );

  function findButton(
    text: string,
    ): HTMLButtonElement | undefined {
    const buttons =
        fixture.nativeElement
        .querySelectorAll(
            'button',
        ) as NodeListOf<HTMLButtonElement>;

    return Array
        .from(buttons)
        .find(
        button =>
            button.textContent
            ?.trim() === text,
        );
    }
});