import { DOCUMENT } from '@angular/common';
import {
  inject,
  Injectable,
} from '@angular/core';

export type StructuredData =
  Readonly<Record<string, unknown>>;

@Injectable({
  providedIn: 'root',
})
export class StructuredDataService {
  private readonly document =
    inject(DOCUMENT);

  private readonly groups =
    new Map<
      string,
      readonly StructuredData[]
    >();

  setGroup(
    key: string,
    data:
      | StructuredData
      | readonly StructuredData[],
  ): void {
    const entries =
      Array.isArray(data)
        ? data
        : [data];

    this.groups.set(key, entries);
    this.render();
  }

  removeGroup(key: string): void {
    this.groups.delete(key);
    this.render();
  }

  private render(): void {
    const graph =
      [...this.groups.values()]
        .flat();

    let script =
      this.document
        .querySelector<HTMLScriptElement>(
          'script#app-structured-data',
        );

    if (graph.length === 0) {
      script?.remove();
      return;
    }

    if (!script) {
      script =
        this.document.createElement(
          'script',
        );

      script.type =
        'application/ld+json';

      script.id = 'app-structured-data';

      this.document.head.appendChild(
        script,
      );
    }

    script.textContent =
      JSON.stringify({
        '@context':
          'https://schema.org',
        '@graph': graph,
      }).replaceAll(
        '<',
        String.raw`\u003c`,
      );
  }
}