import {
  afterNextRender,
  DestroyRef,
  Directive,
  ElementRef,
  inject,
  Renderer2,
} from '@angular/core';

@Directive({
  selector: '[appRevealOnScroll]',
})
export class RevealOnScroll {
  private readonly destroyRef = inject(DestroyRef);

  private readonly element =
    inject<ElementRef<HTMLElement>>(ElementRef);

  private readonly renderer = inject(Renderer2);

  constructor() {
    afterNextRender(() => {
    const prefersReducedMotion =
		typeof window.matchMedia === 'function' &&
		window.matchMedia(
			'(prefers-reduced-motion: reduce)',
		).matches;	
		if (
		prefersReducedMotion ||
		!('IntersectionObserver' in window)
		) {
		return;
	}

      const element = this.element.nativeElement;

      this.renderer.addClass(
        element,
        'reveal-on-scroll',
      );

      const observer = new IntersectionObserver(
        entries => {
          if (!entries[0]?.isIntersecting) {
            return;
          }

          this.renderer.addClass(
            element,
            'reveal-on-scroll--visible',
          );

          observer.disconnect();
        },
        {
          threshold: 0.15,
          rootMargin: '0px 0px -8% 0px',
        },
      );

      observer.observe(element);

      this.destroyRef.onDestroy(() =>
        observer.disconnect(),
      );
    });
  }
}