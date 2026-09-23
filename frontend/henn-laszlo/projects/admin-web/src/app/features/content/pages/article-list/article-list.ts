import { DatePipe } from '@angular/common';
import {
  ChangeDetectionStrategy,
  Component,
  inject,
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
export class ArticleList {
  protected readonly articlesStore =
    inject(AdminArticlesStore);
}