import { HttpErrorResponse } from '@angular/common/http';
import { TestBed } from '@angular/core/testing';
import { ContentApi } from 'content-data-access';
import { of, Subject, throwError } from 'rxjs';

import { ContentFileUpload } from './content-file-upload';

describe('ContentFileUpload', () => {
  let uploads: ContentFileUpload;
  let api: {
    uploadContentFile: ReturnType<typeof vi.fn>;
    getContentFileUrl: ReturnType<typeof vi.fn>;
  };

  beforeEach(() => {
    api = {
      uploadContentFile: vi.fn(() => of({ id: 'file-1' })),
      getContentFileUrl: vi.fn(id => `/api/content-files/${id}`),
    };
    TestBed.configureTestingModule({
      providers: [
        ContentFileUpload,
        { provide: ContentApi, useValue: api },
      ],
    });
    uploads = TestBed.inject(ContentFileUpload);
  });

  it.each([
    ['photo.jpg', 'image/jpeg', 'image'],
    ['photo.png', 'image/png', 'image'],
    ['photo.webp', 'image/webp', 'image'],
    ['photo.avif', 'image/avif', 'image'],
    ['document.pdf', 'application/pdf', 'file'],
  ] as const)('uploads %s through FileStorage', async (name, type, kind) => {
    const file = new Blob(['content'], { type });
    await expect(uploads.upload(file, name, kind))
      .resolves.toBe('/api/content-files/file-1');
    expect(api.uploadContentFile).toHaveBeenCalledWith(file, name);
    expect(uploads.isUploading()).toBe(false);
    expect(uploads.error()).toBeNull();
  });

  it('supplies the MIME type when the browser provides none', async () => {
    await uploads.upload(new Blob(['pdf']), 'document.PDF', 'file');
    expect(api.uploadContentFile.mock.calls[0]![0].type)
      .toBe('application/pdf');
  });

  it.each([
    ['script.svg', 'image/svg+xml', 'image'],
    ['page.html', 'text/html', 'file'],
    ['document.pdf', 'application/pdf', 'image'],
    ['photo.png', 'image/jpeg', 'image'],
  ] as const)('rejects unsupported or mismatched %s', async (name, type, kind) => {
    await expect(uploads.upload(new Blob(['x'], { type }), name, kind))
      .rejects.toThrow();
    expect(api.uploadContentFile).not.toHaveBeenCalled();
    expect(uploads.isUploading()).toBe(false);
  });

  it('rejects empty and oversized files before sending', async () => {
    await expect(uploads.upload(
      new Blob([], { type: 'application/pdf' }), 'empty.pdf', 'file',
    )).rejects.toThrow();
    const oversized = new Blob(
      [new Uint8Array(25 * 1024 * 1024 + 1)],
      { type: 'application/pdf' },
    );
    await expect(uploads.upload(oversized, 'large.pdf', 'file'))
      .rejects.toThrow();
    expect(api.uploadContentFile).not.toHaveBeenCalled();
  });

  it('keeps saving blocked until all concurrent uploads finish', async () => {
    const first = new Subject<{ id: string }>();
    const second = new Subject<{ id: string }>();
    api.uploadContentFile.mockReturnValueOnce(first).mockReturnValueOnce(second);
    const file = new Blob(['x'], { type: 'image/png' });
    const firstUpload = uploads.upload(file, 'first.png', 'image');
    const secondUpload = uploads.upload(file, 'second.png', 'image');
    expect(uploads.isUploading()).toBe(true);
    first.next({ id: 'first' });
    await firstUpload;
    expect(uploads.isUploading()).toBe(true);
    second.next({ id: 'second' });
    await secondUpload;
    expect(uploads.isUploading()).toBe(false);
  });

  it('reports a failed upload without returning a file URL', async () => {
    api.uploadContentFile.mockReturnValue(throwError(() =>
      new HttpErrorResponse({ status: 500 }),
    ));
    await expect(uploads.upload(
      new Blob(['x'], { type: 'application/pdf' }), 'document.pdf', 'file',
    )).rejects.toThrow('A fájl feltöltése nem sikerült.');
    expect(uploads.error()).toContain('A fájl feltöltése nem sikerült.');
    expect(uploads.isUploading()).toBe(false);
    expect(api.getContentFileUrl).not.toHaveBeenCalled();
  });

  it('enables image uploads and the PDF toolbar action', () => {
    const config = uploads.createEditorConfig()!;
    expect(config.file_picker_types).toBe('image file');
    expect(config.images_upload_handler).toBeTypeOf('function');
    expect(config.convert_urls).toBe(false);
    expect(config.toolbar).toContain('uploadfile');
  });

  it.each([
    '<p>Text</p><img src="blob:local-image">',
    "<img src='data:image/png;base64,xxx'>",
  ])('prevents saving an unresolved local image', content => {
    expect(uploads.canSave([content])).toBe(false);
    expect(uploads.error()).toContain('még nem fejeződött be');
  });

  it('allows saving persistent images and PDF links', () => {
    expect(uploads.canSave([
      '<img src="/api/content-files/file-1">',
      '<a href="/api/content-files/file-2">PDF</a>',
    ])).toBe(true);
  });
});
