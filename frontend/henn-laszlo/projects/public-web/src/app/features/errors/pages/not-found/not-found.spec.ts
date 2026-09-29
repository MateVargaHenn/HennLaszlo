import {
  RESPONSE_INIT,
} from '@angular/core';
import {
  ComponentFixture,
  TestBed,
} from '@angular/core/testing';
import {
  provideRouter,
} from '@angular/router';

import {
  NotFound,
} from './not-found';

describe('NotFound', () => {
  let fixture:
    ComponentFixture<NotFound>;

  const responseInit:
    ResponseInit = {};

  beforeEach(async () => {
    await TestBed
      .configureTestingModule({
        imports: [NotFound],
        providers: [
          provideRouter([]),
          {
            provide:
              RESPONSE_INIT,
            useValue:
              responseInit,
          },
        ],
      })
      .compileComponents();

    fixture =
      TestBed.createComponent(
        NotFound,
      );

    fixture.detectChanges();
  });

  it(
    'should set the response status to 404',
    () => {
      expect(responseInit.status)
        .toBe(404);
    },
  );
});