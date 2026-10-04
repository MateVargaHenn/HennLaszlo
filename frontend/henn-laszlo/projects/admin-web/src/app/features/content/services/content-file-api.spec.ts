import { provideHttpClient } from '@angular/common/http';
import {
  HttpTestingController,
  provideHttpClientTesting,
} from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { ContentApi, provideContentDataAccess } from 'content-data-access';

describe('Content file API', () => {
  let api: ContentApi;
  let http: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        provideHttpClient(),
        provideHttpClientTesting(),
        provideContentDataAccess({ apiBaseUrl: '' }),
      ],
    });
    api = TestBed.inject(ContentApi);
    http = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    http.verify();
    document.cookie = 'XSRF-TOKEN=; Max-Age=0; path=/';
  });

  it('uploads multipart data with the existing CSRF protection', () => {
    document.cookie = 'XSRF-TOKEN=upload-token; path=/';
    const file = new Blob(['pdf'], { type: 'application/pdf' });
    const uploaded = vi.fn();
    api.uploadContentFile(file, 'document.pdf').subscribe(uploaded);

    const request = http.expectOne('/api/admin/files');
    expect(request.request.method).toBe('POST');
    expect(request.request.headers.get('X-XSRF-TOKEN')).toBe('upload-token');
    expect(request.request.body).toBeInstanceOf(FormData);
    const part = (request.request.body as FormData).get('file') as File;
    expect(part.name).toBe('document.pdf');
    expect(part.type).toBe('application/pdf');
    expect(request.request.headers.has('Content-Type')).toBe(false);
    request.flush({ id: 'file-1' });
    expect(uploaded).toHaveBeenCalledWith({ id: 'file-1' });
  });

  it('uses the public content endpoint for embedded files', () => {
    expect(api.getContentFileUrl('file-1'))
      .toBe('/api/content-files/file-1');
  });
});
