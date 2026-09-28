import {
  ChangeDetectionStrategy,
  Component,
  ElementRef,
  HostListener,
  inject,
  signal,
} from '@angular/core';
import {
  RouterLink,
  RouterLinkActive,
} from '@angular/router';

type DesktopNavigationGroup =
  | 'career'
  | 'media';

@Component({
  selector: 'app-public-header',
  imports: [
    RouterLink,
    RouterLinkActive,
  ],
  templateUrl: './public-header.html',
  styleUrl: './public-header.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PublicHeader {
  private readonly hostElement =
    inject<ElementRef<HTMLElement>>(
      ElementRef,
    );

  protected readonly isMenuOpen =
    signal(false);

  protected readonly openDesktopGroup =
    signal<DesktopNavigationGroup | null>(
      null,
    );

  protected toggleMenu(): void {
    this.isMenuOpen.update(
      value => !value,
    );

    this.openDesktopGroup.set(null);
  }

  protected toggleDesktopGroup(
    group: DesktopNavigationGroup,
  ): void {
    this.openDesktopGroup.update(
      currentGroup =>
        currentGroup === group
          ? null
          : group,
    );
  }

  protected closeMenu(): void {
    this.isMenuOpen.set(false);
    this.openDesktopGroup.set(null);
  }

  @HostListener(
    'document:click',
    ['$event'],
  )
  protected closeOnOutsideClick(
    event: MouseEvent,
  ): void {
    if (
      !event
        .composedPath()
        .includes(
          this.hostElement.nativeElement,
        )
    ) {
      this.closeMenu();
    }
  }

  @HostListener('document:keydown.escape')
  protected closeOnEscape(): void {
    this.closeMenu();
  }
}
