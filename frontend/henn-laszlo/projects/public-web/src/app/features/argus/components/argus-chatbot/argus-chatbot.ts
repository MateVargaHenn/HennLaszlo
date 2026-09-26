import {
  ChangeDetectionStrategy,
  Component,
  computed,
  DestroyRef,
  ElementRef,
  HostListener,
  inject,
  signal,
  viewChild,
  viewChildren,
} from '@angular/core';
import {
  FormControl,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { RouterLink } from '@angular/router';
import {
  finalize,
} from 'rxjs';
import {
  takeUntilDestroyed,
} from '@angular/core/rxjs-interop';

import {
  ArgusApi,
  ArgusSource,
  isRateLimitError,
} from '../../data-access/argus-api';

interface ArgusMessage {
  readonly id: number;
  readonly role: 'assistant' | 'user';
  readonly text: string;
  readonly source: ArgusSource | null;
  readonly isError: boolean;
}

@Component({
  selector: 'app-argus-chatbot',
  imports: [
    ReactiveFormsModule,
    RouterLink,
  ],
  templateUrl: './argus-chatbot.html',
  styleUrl: './argus-chatbot.css',
  changeDetection:
    ChangeDetectionStrategy.OnPush,
})
export class ArgusChatbot {
  private readonly api = inject(ArgusApi);
  private readonly destroyRef =
    inject(DestroyRef);

  private readonly questionInput =
    viewChild<ElementRef<HTMLInputElement>>(
      'questionInput',
    );

  private readonly launcher =
    viewChild<ElementRef<HTMLButtonElement>>(
      'launcher',
    );

  private readonly messageList =
    viewChild<ElementRef<HTMLDivElement>>(
      'messageList',
    );

  private nextMessageId = 1;

  protected readonly isOpen =
    signal(false);

  protected readonly isSending =
    signal(false);

  private readonly previousEntryId =
    signal<string | null>(null);

  protected readonly suggestedQuestions = [
    'Ki Henn László András?',
    'Mikor és hol született?',
    'Kik voltak a mesterei?',
    'Mi jellemzi a művészetét?',
    'Hol tekinthetők meg a művei?',
    'Hogyan léphetek kapcsolatba vele?',
  ] as const;

  private readonly suggestionButtons =
  viewChildren<
    ElementRef<HTMLButtonElement>
  >(
    'suggestionButton',
  );

  protected readonly showSuggestedQuestions =
    computed(() =>
      !this.messages().some(
        message => message.role === 'user',
      ),
    );

  protected readonly question =
    new FormControl(
      '',
      {
        nonNullable: true,
        validators: [
          Validators.required,
          Validators.maxLength(300),
        ],
      },
    );

  protected readonly messages =
    signal<readonly ArgusMessage[]>([
      {
        id: 0,
        role: 'assistant',
        text:
          'Üdvözlöm! Henn László András ' +
          'munkásságáról kérdezhet.',
        source: null,
        isError: false,
      },
    ]);

  protected toggle(): void {
    if (this.isOpen()) {
      this.close();
      return;
    }

    this.isOpen.set(true);

    setTimeout(() => {
      const firstSuggestion =
        this.suggestionButtons()[0]
          ?.nativeElement;

      const target =
        firstSuggestion ??
        this.questionInput()
          ?.nativeElement;

      target?.focus();
    });
  }

  protected prepareSuggestedQuestion(
  suggestedQuestion: string,
): void {
  if (this.isSending()) {
    return;
  }

  /*
   * A pointerdown hamarabb fut le, mint az
   * input blur eseménye. Így az input már
   * érvényes lesz, mire elveszíti a fókuszt.
   */
  this.question.setValue(
    suggestedQuestion,
  );
}

  protected close(): void {
    if (!this.isOpen()) {
      return;
    }

    this.isOpen.set(false);

    setTimeout(() => {
      this.launcher()
        ?.nativeElement
        .focus();
    });
  }

protected submit(
  event: Event,
): void {
  event.preventDefault();

  this.sendCurrentQuestion();
}

protected askSuggestedQuestion(
  suggestedQuestion: string,
): void {
  if (this.isSending()) {
    return;
  }

  this.question.setValue(
    suggestedQuestion,
  );

  this.sendCurrentQuestion();

  setTimeout(() => {
    this.questionInput()
      ?.nativeElement
      .focus();
  });
}

private sendCurrentQuestion(): void {
  const question =
    this.question.value.trim();

  if (
    !question ||
    this.question.invalid ||
    this.isSending()
  ) {
    this.question.markAsTouched();
    return;
  }

  this.addMessage({
    role: 'user',
    text: question,
    source: null,
    isError: false,
  });

  this.question.reset();
  this.isSending.set(true);

  this.api
    .ask(question)
    .pipe(
      finalize(() => {
        this.isSending.set(false);
        this.scrollToEnd();
      }),
      takeUntilDestroyed(
        this.destroyRef,
      ),
    )
    .subscribe({
      next: response => {
        this.addMessage({
          role: 'assistant',
          text: response.answer,
          source: response.source,
          isError: false,
        });
      },
      error: error => {
        this.addMessage({
          role: 'assistant',
          text: isRateLimitError(error)
            ? 'Túl sok kérdés érkezett rövid ' +
              'idő alatt. Kérlek, próbáld ' +
              'meg egy perc múlva.'
            : 'Argus jelenleg nem érhető el. ' +
              'Kérlek, próbáld meg később.',
          source: null,
          isError: true,
        });
      },
    });
}

  @HostListener('document:keydown.escape')
  protected handleEscape(): void {
    this.close();
  }

  private addMessage(
    message: Omit<ArgusMessage, 'id'>,
  ): void {
    this.messages.update(messages => [
      ...messages,
      {
        ...message,
        id: this.nextMessageId++,
      },
    ]);

    this.scrollToEnd();
  }

  private scrollToEnd(): void {
    setTimeout(() => {
      const element =
        this.messageList()
          ?.nativeElement;

      if (element) {
        element.scrollTop =
          element.scrollHeight;
      }
    });
  }
}