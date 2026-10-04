import {
  computed,
  inject,
  Injectable,
  resource,
  signal,
} from '@angular/core';
import { firstValueFrom } from 'rxjs';

import {
  ContentRevisionDetails,
} from '../models/content-revision-details';
import {
  ContentRevisionListItem,
} from '../models/content-revision-list-item';
import {
  ContentRevisionTarget,
} from '../models/content-revision-target';
import { ContentApi } from '../services/content-api';

interface RevisionTarget {
  readonly target: ContentRevisionTarget;
  readonly targetId: string;
}

interface RevisionDetailsRequest
  extends RevisionTarget {
  readonly revisionId: string;
}

@Injectable({
  providedIn: 'root',
})
export class ContentRevisionsStore {
  private readonly contentApi =
    inject(ContentApi);

  private readonly targetState =
    signal<RevisionTarget | undefined>(
      undefined,
    );

  private readonly selectedRevisionIdState =
    signal<string | undefined>(undefined);

  private readonly restoring =
    signal(false);

  private readonly restoreErrorState =
    signal<unknown | undefined>(undefined);

  private readonly revisionsResource =
    resource({
      params: () => this.targetState(),

      loader: ({ params }) =>
        firstValueFrom(
          this.contentApi.getContentRevisions(
            params.target,
            params.targetId,
          ),
        ),
    });

  private readonly revisionDetailsResource =
    resource({
      params:
        (): RevisionDetailsRequest | undefined => {
          const target = this.targetState();
          const revisionId =
            this.selectedRevisionIdState();

          if (!target || !revisionId) {
            return undefined;
          }

          return {
            ...target,
            revisionId,
          };
        },

      loader: ({ params }) =>
        firstValueFrom(
          this.contentApi.getContentRevision(
            params.target,
            params.targetId,
            params.revisionId,
          ),
        ),
    });

  readonly revisions =
    computed<
      readonly ContentRevisionListItem[]
    >(() => {
      if (
        !this.revisionsResource.hasValue()
      ) {
        return [];
      }

      return this.revisionsResource.value();
    });

  readonly selectedRevision =
    computed<ContentRevisionDetails | null>(
      () => {
        if (
          !this.revisionDetailsResource
            .hasValue()
        ) {
          return null;
        }

        return this.revisionDetailsResource
          .value();
      },
    );

  readonly isLoading =
    this.revisionsResource.isLoading;

  readonly loadError =
    this.revisionsResource.error;

  readonly isRevisionLoading =
    this.revisionDetailsResource.isLoading;

  readonly revisionLoadError =
    this.revisionDetailsResource.error;

  readonly isRestoring =
    this.restoring.asReadonly();

  readonly restoreError =
    this.restoreErrorState.asReadonly();

  load(
    target: ContentRevisionTarget,
    targetId: string,
  ): void {
    const normalizedTargetId =
      targetId.trim();

    const currentTarget =
      this.targetState();

    this.selectedRevisionIdState.set(
      undefined,
    );

    this.restoreErrorState.set(undefined);

    if (
      currentTarget?.target === target &&
      currentTarget.targetId ===
        normalizedTargetId
    ) {
      this.revisionsResource.reload();
      return;
    }

    this.targetState.set({
      target,
      targetId: normalizedTargetId,
    });
  }

  selectRevision(revisionId: string): void {
    const normalizedRevisionId =
      revisionId.trim();

    if (
      this.selectedRevisionIdState() ===
      normalizedRevisionId
    ) {
      this.revisionDetailsResource.reload();
      return;
    }

    this.selectedRevisionIdState.set(
      normalizedRevisionId,
    );
  }

  closeRevision(): void {
    this.selectedRevisionIdState.set(
      undefined,
    );

    this.restoreErrorState.set(undefined);
  }

  reload(): void {
    this.revisionsResource.reload();
  }

  async restore(
    revisionId: string,
    expectedVersion: string,
  ): Promise<void> {
    const target = this.targetState();

    if (!target) {
      throw new Error(
        'Nincs kiválasztva verziózható tartalom.',
      );
    }

    if (!expectedVersion.trim()) {
      throw new Error(
        'A visszaállításhoz hiányzik a tartalom verziója.',
      );
    }

    if (this.restoring()) {
      throw new Error(
        'Már folyamatban van egy visszaállítás.',
      );
    }

    this.restoring.set(true);
    this.restoreErrorState.set(undefined);

    try {
      await firstValueFrom(
        this.contentApi.restoreContentRevision(
          target.target,
          target.targetId,
          revisionId,
          expectedVersion,
        ),
      );

      this.selectedRevisionIdState.set(
        undefined,
      );

      this.revisionsResource.reload();
    }
    catch (error) {
      this.restoreErrorState.set(error);
      throw error;
    }
    finally {
      this.restoring.set(false);
    }
  }

  clear(): void {
    this.targetState.set(undefined);

    this.selectedRevisionIdState.set(
      undefined,
    );

    this.restoreErrorState.set(undefined);
  }
}
