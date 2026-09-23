import { DatePipe } from '@angular/common';
import {
  ChangeDetectionStrategy,
  Component,
  inject,
  signal,
} from '@angular/core';
import { RouterLink } from '@angular/router';
import {
  AdminContentPagesStore,
  ContentPageKey,
} from 'content-data-access';

@Component({
  selector: 'app-admin-content-page-list',
  imports: [
    DatePipe,
    RouterLink,
  ],
  templateUrl: './admin-content-page-list.html',
  styleUrl: './admin-content-page-list.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AdminContentPageList {
  protected readonly store =
    inject(AdminContentPagesStore);

  protected readonly actionError =
    signal<string | null>(null);

  protected async publishContentPage(
    key: ContentPageKey,
  ): Promise<void> {
    await this.changePublication(
      () => this.store.publish(key),
    );
  }

  protected async unpublishContentPage(
    key: ContentPageKey,
  ): Promise<void> {
    await this.changePublication(
      () => this.store.unpublish(key),
    );
  }

  private async changePublication(
    action: () => Promise<void>,
  ): Promise<void> {
    this.actionError.set(null);

    try {
      await action();
    }
    catch {
      this.actionError.set(
        'Az oldal állapotának módosítása sikertelen.',
      );
    }
  }
}