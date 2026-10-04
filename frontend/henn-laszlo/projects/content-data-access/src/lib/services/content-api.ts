import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { CONTENT_DATA_ACCESS_CONFIG } from '../config/content-data-access-config';
import { AdminContentPageDetails } from '../models/admin-content-page-details';
import { AdminContentPageListItem } from '../models/admin-content-page-list-item';
import { ContentPageKey } from '../models/content-page-key';
import { PublishedContentPageDetails } from '../models/published-content-page-details';
import { UpsertContentPageRequest } from '../models/upsert-content-page-request';
import { UpsertContentPageResponse } from '../models/upsert-content-page-response';
import { AdminArticleDetails } from '../models/admin-article-details';
import { AdminArticleListItem } from '../models/admin-article-list-item';
import { CreateArticleRequest } from '../models/create-article-request';
import { CreateArticleResponse } from '../models/create-article-response';
import { PublishedArticleDetails } from '../models/published-article-details';
import { PublishedArticleListItem } from '../models/published-article-list-item';
import { UpdateArticleRequest } from '../models/update-article-request';
import { AdminWebsiteSettings } from '../models/admin-website-settings';
import { PublicWebsiteSettings } from '../models/public-website-settings';
import { UpdateWebsiteSettingsRequest } from '../models/update-website-settings-request';
import {
  ContentRevisionDetails,
} from '../models/content-revision-details';
import {
  ContentRevisionListItem,
} from '../models/content-revision-list-item';
import {
  ContentRevisionTarget,
} from '../models/content-revision-target';

@Injectable({
  providedIn: 'root',
})
export class ContentApi {
  private readonly http = inject(HttpClient);
  private readonly config =
    inject(CONTENT_DATA_ACCESS_CONFIG);

  private readonly apiBaseUrl =
    this.config.apiBaseUrl.replace(/\/+$/, '');

  getPublishedContentPage(
    key: ContentPageKey,
  ): Observable<PublishedContentPageDetails> {
    return this.http.get<PublishedContentPageDetails>(
      `${this.apiBaseUrl}/api/content-pages/${key}`,
    );
  }

  getAdminContentPages():
    Observable<readonly AdminContentPageListItem[]> {
    return this.http.get<readonly AdminContentPageListItem[]>(
      `${this.apiBaseUrl}/api/admin/content-pages`,
    );
  }

  getAdminContentPage(
    key: ContentPageKey,
  ): Observable<AdminContentPageDetails> {
    return this.http.get<AdminContentPageDetails>(
      `${this.apiBaseUrl}/api/admin/content-pages/${key}`,
    );
  }

  upsertContentPage(
    key: ContentPageKey,
    request: UpsertContentPageRequest,
  ): Observable<UpsertContentPageResponse> {
    return this.http.put<UpsertContentPageResponse>(
      `${this.apiBaseUrl}/api/admin/content-pages/${key}`,
      request,
    );
  }

  publishContentPage(
    key: ContentPageKey,
  ): Observable<void> {
    return this.http.put<void>(
      `${this.apiBaseUrl}/api/admin/content-pages/${key}/publish`,
      null,
    );
  }

  unpublishContentPage(
    key: ContentPageKey,
  ): Observable<void> {
    return this.http.put<void>(
      `${this.apiBaseUrl}/api/admin/content-pages/${key}/unpublish`,
      null,
    );
  }

  getPublicWebsiteSettings():
    Observable<PublicWebsiteSettings> {
    return this.http.get<PublicWebsiteSettings>(
      `${this.apiBaseUrl}/api/website-settings`,
    );
  }

  getAdminWebsiteSettings():
    Observable<AdminWebsiteSettings> {
    return this.http.get<AdminWebsiteSettings>(
      `${this.apiBaseUrl}/api/admin/website-settings`,
    );
  }

  updateWebsiteSettings(
    request: UpdateWebsiteSettingsRequest,
  ): Observable<void> {
    return this.http.put<void>(
      `${this.apiBaseUrl}/api/admin/website-settings`,
      request,
    );
  }

  getPublishedArticles():
  Observable<readonly PublishedArticleListItem[]> {
  return this.http.get<
    readonly PublishedArticleListItem[]
  >(
    `${this.apiBaseUrl}/api/articles`,
  );
}

getPublishedArticleBySlug(
  slug: string,
): Observable<PublishedArticleDetails> {
  return this.http.get<PublishedArticleDetails>(
    `${this.apiBaseUrl}/api/articles/${
      encodeURIComponent(slug)
    }`,
  );
}

getAdminArticles():
  Observable<readonly AdminArticleListItem[]> {
  return this.http.get<
    readonly AdminArticleListItem[]
  >(
    `${this.apiBaseUrl}/api/admin/articles`,
  );
}

getAdminArticleById(
  articleId: string,
): Observable<AdminArticleDetails> {
  return this.http.get<AdminArticleDetails>(
    `${this.apiBaseUrl}/api/admin/articles/${articleId}`,
  );
}

createArticle(
  request: CreateArticleRequest,
): Observable<CreateArticleResponse> {
  return this.http.post<CreateArticleResponse>(
    `${this.apiBaseUrl}/api/admin/articles`,
    request,
  );
}

updateArticle(
  articleId: string,
  request: UpdateArticleRequest,
): Observable<void> {
  return this.http.put<void>(
    `${this.apiBaseUrl}/api/admin/articles/${articleId}`,
    request,
  );
}

publishArticle(
  articleId: string,
): Observable<void> {
  return this.http.put<void>(
    `${this.apiBaseUrl}/api/admin/articles/${articleId}/publish`,
    null,
  );
}

unpublishArticle(
  articleId: string,
): Observable<void> {
  return this.http.put<void>(
    `${this.apiBaseUrl}/api/admin/articles/${articleId}/unpublish`,
    null,
  );
}

deleteArticle(
  articleId: string,
): Observable<void> {
  return this.http.delete<void>(
    `${this.apiBaseUrl}/api/admin/articles/${articleId}`,
  );
}

getContentRevisions(
  target: ContentRevisionTarget,
  targetId: string,
): Observable<readonly ContentRevisionListItem[]> {
  return this.http.get<
    readonly ContentRevisionListItem[]
  >(
    `${this.apiBaseUrl}/api/admin/` +
    `${target}/${encodeURIComponent(targetId)}/revisions`,
  );
}

getContentRevision(
  target: ContentRevisionTarget,
  targetId: string,
  revisionId: string,
): Observable<ContentRevisionDetails> {
  return this.http.get<ContentRevisionDetails>(
    `${this.apiBaseUrl}/api/admin/` +
    `${target}/${encodeURIComponent(targetId)}/` +
    `revisions/${encodeURIComponent(revisionId)}`,
  );
}

restoreContentRevision(
  target: ContentRevisionTarget,
  targetId: string,
  revisionId: string,
  expectedVersion: string,
): Observable<void> {
  return this.http.post<void>(
    `${this.apiBaseUrl}/api/admin/` +
    `${target}/${encodeURIComponent(targetId)}/` +
    `revisions/${encodeURIComponent(revisionId)}/restore`,
    { expectedVersion },
  );
}

  uploadContentFile(
    file: Blob,
    fileName: string,
  ): Observable<{ readonly id: string }> {
    const formData = new FormData();
    formData.append('file', file, fileName);

    return this.http.post<{ readonly id: string }>(
      `${this.apiBaseUrl}/api/admin/files`,
      formData,
    );
  }

  getContentFileUrl(fileId: string): string {
    return `${this.apiBaseUrl}/api/content-files/${encodeURIComponent(fileId)}`;
  }
}
