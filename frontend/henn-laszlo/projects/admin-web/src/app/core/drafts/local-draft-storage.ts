import {
  isPlatformBrowser,
} from '@angular/common';
import {
  inject,
  Injectable,
  PLATFORM_ID,
} from '@angular/core';

export interface LocalDraft<T> {
  readonly savedAtUtc: string;
  readonly value: T;
}

interface DraftEnvelope {
  readonly savedAtUtc: string;
  readonly value: unknown;
}

@Injectable({
  providedIn: 'root',
})
export class LocalDraftStorage {
  private static readonly StoragePrefix =
    'henn-laszlo:admin-draft:v1:';

  private static readonly RetentionMilliseconds =
    7 * 24 * 60 * 60 * 1000;

  private readonly isBrowser =
    isPlatformBrowser(
      inject(PLATFORM_ID),
    );

  save<T>(
    key: string,
    value: T,
  ): void {
    if (!this.isBrowser) {
      return;
    }

    const draft: LocalDraft<T> = {
      savedAtUtc: new Date().toISOString(),
      value,
    };

    try {
      localStorage.setItem(
        this.createStorageKey(key),
        JSON.stringify(draft),
      );
    }
    catch {
      // A localStorage nem elérhető vagy megtelt.
    }
  }

  load<T>(
    key: string,
  ): LocalDraft<T> | null {
    if (!this.isBrowser) {
      return null;
    }

    try {
      const serialized = localStorage.getItem(
        this.createStorageKey(key),
      );

      if (!serialized) {
        return null;
      }

      const parsed: unknown =
        JSON.parse(serialized);

      if (!this.isDraftEnvelope(parsed)) {
        this.remove(key);
        return null;
      }

      const savedAt =
        Date.parse(parsed.savedAtUtc);

      if (
        !Number.isFinite(savedAt) ||
        Date.now() - savedAt >
          LocalDraftStorage
            .RetentionMilliseconds
      ) {
        this.remove(key);
        return null;
      }

      return {
        savedAtUtc: parsed.savedAtUtc,
        value: parsed.value as T,
      };
    }
    catch {
      this.remove(key);
      return null;
    }
  }

  remove(key: string): void {
    if (!this.isBrowser) {
      return;
    }

    try {
      localStorage.removeItem(
        this.createStorageKey(key),
      );
    }
    catch {
      // A localStorage nem elérhető.
    }
  }

  private createStorageKey(
    key: string,
  ): string {
    return (
      LocalDraftStorage.StoragePrefix +
      key
    );
  }

  private isDraftEnvelope(
    value: unknown,
  ): value is DraftEnvelope {
    return (
      typeof value === 'object' &&
      value !== null &&
      'savedAtUtc' in value &&
      typeof value.savedAtUtc === 'string' &&
      'value' in value
    );
  }
}