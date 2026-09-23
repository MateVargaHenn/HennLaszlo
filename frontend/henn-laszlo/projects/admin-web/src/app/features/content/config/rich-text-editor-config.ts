import type {
  EditorComponent,
} from '@tinymce/tinymce-angular';


export const richTextEditorConfig:
  EditorComponent['init'] = {
    height: 520,
    min_height: 420,

    menubar:
      'edit view insert format tools table help',

    plugins: [
      'advlist',
      'autolink',
      'lists',
      'link',
      'charmap',
      'searchreplace',
      'visualblocks',
      'code',
      'fullscreen',
      'preview',
      'anchor',
      'insertdatetime',
      'table',
      'wordcount',
      'help',
    ],

    toolbar:
      'undo redo | ' +
      'blocks fontfamily fontsize | ' +
      'bold italic underline strikethrough | ' +
      'forecolor backcolor | ' +
      'alignleft aligncenter alignright alignjustify | ' +
      'bullist numlist outdent indent | ' +
      'blockquote | ' +
      'link unlink anchor | ' +
      'table | ' +
      'searchreplace charmap insertdatetime | ' +
      'removeformat | ' +
      'code preview fullscreen help',

    toolbar_mode: 'wrap',
    toolbar_sticky: true,

    block_formats:
      'Bekezdés=p; ' +
      'Címsor 2=h2; ' +
      'Címsor 3=h3; ' +
      'Címsor 4=h4; ' +
      'Előformázott=pre',

    font_size_formats:
      '12px 14px 16px 18px 20px 24px 30px 36px',

    browser_spellcheck: true,
    contextmenu: 'link table',
    paste_data_images: false,

    statusbar: true,
    resize: true,
    branding: false,
    promotion: false,

    content_style: `
      body {
        box-sizing: border-box;
        margin: 0;
        padding: 24px;
        color: #1c1917;
        font-family: Arial, sans-serif;
        font-size: 16px;
        line-height: 1.7;
      }

      h2,
      h3,
      h4 {
        font-family: Georgia, serif;
      }

      blockquote {
        margin: 24px 0;
        border-left: 4px solid #0891b2;
        background: #f5f5f4;
        padding: 16px 20px;
        color: #57534e;
        font-style: italic;
      }

      a {
        color: #0e7490;
        text-decoration: underline;
      }
    `,
  };