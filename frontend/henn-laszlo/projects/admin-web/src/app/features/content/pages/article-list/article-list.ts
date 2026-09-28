import { DatePipe } from '@angular/common';
import {
  ChangeDetectionStrategy,
  Component,
  inject,
  OnDestroy,
  signal,
} from '@angular/core';
import { RouterLink } from '@angular/router';
import {
  AdminArticlesStore,
} from 'content-data-access';

@Component({
  selector: 'app-article-list',
  imports: [
    DatePipe,
    RouterLink,
  ],
  templateUrl: './article-list.html',
  styleUrl: './article-list.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ArticleList
  implements OnDestroy {
  protected readonly articlesStore =
    inject(AdminArticlesStore);

  protected readonly copiedArticleId =
    signal<string | null>(null);

  protected readonly copyError =
    signal<string | null>(null);

  private copyFeedbackTimer:
    number | undefined;

  protected async copyArticleLink(
    articleId: string,
    slug: string,
  ): Promise<void> {
    const articlePath =
      `/irasok/${slug}`;

    try {
      await navigator.clipboard.writeText(
        articlePath,
      );

      this.copyError.set(null);
      this.copiedArticleId.set(articleId);

      window.clearTimeout(
        this.copyFeedbackTimer,
      );

      this.copyFeedbackTimer =
        window.setTimeout(() => {
          this.copiedArticleId.set(null);
          this.copyFeedbackTimer =
            undefined;
        }, 2000);
    } catch {
      this.copiedArticleId.set(null);
      this.copyError.set(
        'A hivatkozás másolása nem sikerült.',
      );
    }
  }

  ngOnDestroy(): void {
    window.clearTimeout(
      this.copyFeedbackTimer,
    );
  }
}