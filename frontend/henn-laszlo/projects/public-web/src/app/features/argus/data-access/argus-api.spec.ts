import {
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
  ArgusApi,
  AskArgusResponse,
} from './argus-api';

describe('ArgusApi', () => {
  let api: ArgusApi;
  let http:
    HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        provideHttpClient(),
        provideHttpClientTesting(),
      ],
    });

    api = TestBed.inject(ArgusApi);

    http =
      TestBed.inject(
        HttpTestingController,
      );
  });

  afterEach(() => {
    http.verify();
  });

  it('should send the question', () => {
    const response: AskArgusResponse = {
      answer:
        'Henn László András ' +
        'festőművész és grafikus.',
      confidence: 0.95,
      isFallback: false,
      source: {
        title: 'Bemutatkozás',
        path: '/bemutatkozas',
      },
    };

    api
      .ask('Ki Henn László András?')
      .subscribe(result => {
        expect(result).toEqual(response);
      });

    const request =
      http.expectOne(candidate =>
        candidate.url.endsWith(
          '/api/chatbot/ask',
        ),
      );

    expect(request.request.method)
      .toBe('POST');

    expect(request.request.body)
      .toEqual({
        question:
          'Ki Henn László András?',
      });

    request.flush(response);
  });
});