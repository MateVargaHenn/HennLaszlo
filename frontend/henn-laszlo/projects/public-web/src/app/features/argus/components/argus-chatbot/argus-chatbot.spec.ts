import {
  ComponentFixture,
  TestBed,
} from '@angular/core/testing';
import {
  provideHttpClient,
} from '@angular/common/http';
import {
  HttpTestingController,
  provideHttpClientTesting,
} from '@angular/common/http/testing';
import {
  provideRouter,
} from '@angular/router';

import { ArgusChatbot } from './argus-chatbot';

describe('ArgusChatbot', () => {
  let component: ArgusChatbot;
  let fixture:
    ComponentFixture<ArgusChatbot>;
  let http:
    HttpTestingController;

  beforeEach(async () => {
    await TestBed
      .configureTestingModule({
        imports: [ArgusChatbot],
        providers: [
          provideHttpClient(),
          provideHttpClientTesting(),
          provideRouter([]),
        ],
      })
      .compileComponents();

    fixture =
      TestBed.createComponent(
        ArgusChatbot,
      );

    component = fixture.componentInstance;

    http =
      TestBed.inject(
        HttpTestingController,
      );

    fixture.detectChanges();
  });

  afterEach(() => {
    http.verify();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should open the chatbot', () => {
    openChatbot();

    const dialog =
      fixture.nativeElement
        .querySelector(
          '#argus-dialog',
        );

    expect(dialog).toBeTruthy();
  });

  it(
    'should display the answer and source',
    () => {
      openChatbot();

      submitQuestion(
        'Ki Henn László András?',
      );

      const request =
        http.expectOne(candidate =>
          candidate.url.endsWith(
            '/api/chatbot/ask',
          ),
        );

      expect(request.request.body)
        .toEqual({
          question:
            'Ki Henn László András?',
        });

      request.flush({
        answer:
          'Henn László András ' +
          'festőművész és grafikus.',
        confidence: 0.95,
        isFallback: false,
        source: {
          title: 'Bemutatkozás',
          path: '/bemutatkozas',
        },
      });

      fixture.detectChanges();

      const text =
        fixture.nativeElement
          .textContent;

      expect(text).toContain(
        'Henn László András ' +
        'festőművész és grafikus.',
      );

      expect(text).toContain(
        'Forrás: Bemutatkozás',
      );
    },
  );

  it(
    'should display the rate limit error',
    () => {
      openChatbot();

      submitQuestion(
        'Ki Henn László András?',
      );

      const request =
        http.expectOne(candidate =>
          candidate.url.endsWith(
            '/api/chatbot/ask',
          ),
        );

      request.flush(
        null,
        {
          status: 429,
          statusText:
            'Too Many Requests',
        },
      );

      fixture.detectChanges();

      expect(
        fixture.nativeElement
          .textContent,
      ).toContain(
        'Túl sok kérdés érkezett',
      );
    },
  );

  it(
    'should close when Escape is pressed',
    () => {
      openChatbot();

      document.dispatchEvent(
        new KeyboardEvent(
          'keydown',
          {
            key: 'Escape',
          },
        ),
      );

      fixture.detectChanges();

      expect(
        fixture.nativeElement
          .querySelector(
            '#argus-dialog',
          ),
      ).toBeNull();
    },
  );

  it(
  'should send a suggested question',
  () => {
    openChatbot();

    const buttons =
      Array.from(
        fixture.nativeElement
          .querySelectorAll(
            '.argus-suggestion',
          ),
      ) as HTMLButtonElement[];

    const suggestedQuestion =
      buttons.find(button =>
        button.textContent?.includes(
          'Ki Henn László András?',
        ),
      );

    expect(suggestedQuestion)
      .toBeTruthy();

    suggestedQuestion!.click();
    fixture.detectChanges();

    const request =
      http.expectOne(candidate =>
        candidate.url.endsWith(
          '/api/chatbot/ask',
        ),
      );

    expect(request.request.body)
      .toEqual({
        question:
          'Ki Henn László András?',
      });

    expect(
      fixture.nativeElement
        .querySelectorAll(
          '.argus-suggestion',
        ).length,
    ).toBe(0);

    request.flush({
      answer:
        'Henn László András ' +
        'festőművész és grafikus.',
      confidence: 0.95,
      isFallback: false,
      source: null,
    });

    fixture.detectChanges();
  },
);

  function openChatbot(): void {
    const launcher =
      fixture.nativeElement.querySelector(
        '.argus-launcher',
      ) as HTMLButtonElement;

    launcher.click();
    fixture.detectChanges();
  }

  function submitQuestion(
    value: string,
  ): void {
    const input =
      fixture.nativeElement.querySelector(
        '#argus-question',
      ) as HTMLInputElement;

    input.value = value;

    input.dispatchEvent(
      new Event('input'),
    );

    fixture.detectChanges();

    const form =
      fixture.nativeElement.querySelector(
        '.argus-form',
      ) as HTMLFormElement;

    form.dispatchEvent(
      new Event(
        'submit',
        {
          bubbles: true,
          cancelable: true,
        },
      ),
    );

    fixture.detectChanges();
  }
});