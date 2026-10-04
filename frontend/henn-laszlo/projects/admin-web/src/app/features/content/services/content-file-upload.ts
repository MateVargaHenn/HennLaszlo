import {
  computed,
  inject,
  Injectable,
  signal,
} from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import type { EditorComponent } from '@tinymce/tinymce-angular';
import { firstValueFrom } from 'rxjs';
import { ContentApi } from 'content-data-access';

import { richTextEditorConfig } from '../config/rich-text-editor-config';

type UploadKind = 'image' | 'file';

const maximumFileSize = 25 * 1024 * 1024;
const imageAccept = '.jpg,.jpeg,.png,.webp,.avif';
const documentAccept = '.pdf';
const allowedTypes: Readonly<Record<string, string>> = {
  jpg: 'image/jpeg',
  jpeg: 'image/jpeg',
  png: 'image/png',
  webp: 'image/webp',
  avif: 'image/avif',
  pdf: 'application/pdf',
};

@Injectable()
export class ContentFileUpload {
  private readonly api = inject(ContentApi);
  private readonly pendingCount = signal(0);
  private readonly errorState = signal<string | null>(null);

  readonly isUploading = computed(() => this.pendingCount() > 0);
  readonly error = this.errorState.asReadonly();

  canSave(contents: readonly string[]): boolean {
    if (this.isUploading()) {
      return false;
    }

    if (contents.some(content =>
      /<img\b[^>]*\bsrc\s*=\s*["'](?:blob:|data:)/i.test(content),
    )) {
      this.errorState.set(
        'A kép feltöltése még nem fejeződött be. ' +
        'Várj a feltöltésre, vagy töröld és illeszd be újra a képet.',
      );
      return false;
    }

    return true;
  }

  createEditorConfig(): NonNullable<EditorComponent['init']> {
    return {
      ...richTextEditorConfig,
      relative_urls: false,
      remove_script_host: false,
      convert_urls: false,
      automatic_uploads: true,
      file_picker_types: 'image file',
      images_file_types: 'jpg,jpeg,png,webp,avif',
      images_upload_handler: async (blobInfo, progress) => {
        try {
          const url = await this.upload(
            blobInfo.blob(), blobInfo.filename(), 'image',
          );
          progress(100);
          return url;
        }
        catch (error) {
          // Remove failed local images so a blob URL cannot be saved as content.
          throw { message: this.errorMessage(error), remove: true };
        }
      },
      file_picker_callback: (callback, _value, meta) => {
        this.pickFile(
          meta['filetype'] === 'image' ? 'image' : 'file',
          (url, name) => callback(url, {
            alt: name,
            text: name,
            title: name,
          }),
        );
      },
      setup: editor => {
        richTextEditorConfig?.setup?.(editor);
        editor.ui.registry.addButton('uploadfile', {
          icon: 'upload',
          tooltip: 'PDF feltöltése és beillesztése',
          onAction: () => {
            const bookmark = editor.selection.getBookmark(2, true);
            this.pickFile('file', (url, name) => {
              if (editor.removed) {
                return;
              }

              editor.selection.moveToBookmark(bookmark);
              editor.insertContent(editor.dom.createHTML(
                'a',
                { href: url },
                editor.dom.encode(name),
              ));
            });
          },
        });
      },
    };
  }

  async upload(
    file: Blob,
    fileName: string,
    kind: UploadKind,
  ): Promise<string> {
    this.errorState.set(null);
    this.pendingCount.update(count => count + 1);

    try {
      const extension = fileName.split('.').pop()?.toLowerCase() ?? '';
      const expectedType = allowedTypes[extension];

      if (
        !expectedType ||
        (kind === 'image' && !expectedType.startsWith('image/')) ||
        (kind === 'file' && expectedType !== 'application/pdf')
      ) {
        throw new Error(
          kind === 'image'
            ? 'JPG, PNG, WebP vagy AVIF képet válassz.'
            : 'PDF dokumentumot válassz.',
        );
      }

      if (file.size === 0 || file.size > maximumFileSize) {
        throw new Error('A fájl mérete 1 bájt és 25 MB között lehet.');
      }

      if (fileName.length > 255) {
        throw new Error('A fájlnév legfeljebb 255 karakter lehet.');
      }

      if (
        file.type &&
        file.type !== 'application/octet-stream' &&
        file.type !== expectedType
      ) {
        throw new Error('A fájltípus nem egyezik a kiterjesztéssel.');
      }

      // Some operating systems supply no MIME type for a selected PDF or AVIF.
      const typedFile = file.type === expectedType
        ? file
        : file.slice(0, file.size, expectedType);
      const response = await firstValueFrom(
        this.api.uploadContentFile(typedFile, fileName),
      );

      if (!response.id) {
        throw new Error('A feltöltött fájl azonosítója hiányzik.');
      }

      return this.api.getContentFileUrl(response.id);
    }
    catch (error) {
      const message = this.errorMessage(error);
      this.errorState.set(message);
      throw new Error(message);
    }
    finally {
      this.pendingCount.update(count => count - 1);
    }
  }

  private pickFile(
    kind: UploadKind,
    insert: (url: string, name: string) => void,
  ): void {
    const input = document.createElement('input');
    input.type = 'file';
    input.accept = kind === 'image' ? imageAccept : documentAccept;

    input.addEventListener('change', () => {
      const file = input.files?.[0];
      if (!file) {
        return;
      }

      void this.upload(file, file.name, kind)
        .then(url => insert(url, file.name))
        .catch(error => window.alert(this.errorMessage(error)));
    }, { once: true });

    input.click();
  }

  private errorMessage(error: unknown): string {
    if (error instanceof HttpErrorResponse) {
      if (error.status === 401 || error.status === 403) {
        return 'A feltöltéshez jelentkezz be újra az adminfelületre.';
      }
      if (error.status === 413) {
        return 'A fájl túl nagy. Legfeljebb 25 MB-os fájlt válassz.';
      }
      return 'A fájl feltöltése nem sikerült. Próbáld meg újra.';
    }

    return error instanceof Error
      ? error.message
      : 'A fájl feltöltése nem sikerült. Próbáld meg újra.';
  }
}
