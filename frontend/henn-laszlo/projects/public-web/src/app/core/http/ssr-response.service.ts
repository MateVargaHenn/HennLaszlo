import {
  inject,
  Injectable,
  RESPONSE_INIT,
} from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class SsrResponseService {
  private readonly responseInit =
    inject(
      RESPONSE_INIT,
      {
        optional: true,
      },
    );

  setStatus(status: number): void {
    if (this.responseInit) {
      this.responseInit.status =
        status;
    }
  }

  setNotFound(): void {
    this.setStatus(404);
  }
}