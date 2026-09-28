import {
  ComponentFixture,
  TestBed,
} from '@angular/core/testing';
import { provideRouter } from '@angular/router';

import { PublicHeader } from './public-header';

describe('PublicHeader', () => {
  let component: PublicHeader;
  let fixture: ComponentFixture<PublicHeader>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PublicHeader],
      providers: [
        provideRouter([]),
      ],
    }).compileComponents();

    fixture =
      TestBed.createComponent(PublicHeader);

    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should display every mobile navigation item', () => {
    const menuButton =
      fixture.nativeElement.querySelector(
        '[aria-controls="mobile-navigation"]',
      ) as HTMLButtonElement;

    menuButton.click();
    fixture.detectChanges();

    const links =
      fixture.nativeElement.querySelectorAll(
        '#mobile-navigation a',
      ) as NodeListOf<HTMLAnchorElement>;

    const labels =
      Array.from(
        links,
        link =>
          (link.textContent ?? '').trim(),
      );

    expect(labels).toEqual([
      'Kezdőlap',
      'Bemutatkozás',
      'Művek',
      'Kiállítások',
      'Tagságok és díjak',
      'Meghívók',
      'Videók',
      'Írások',
      'Kapcsolat',
    ]);
  });

  it('should toggle a desktop navigation group', () => {
    const groupButton =
      fixture.nativeElement.querySelector(
        '[aria-controls="desktop-career-navigation"]',
      ) as HTMLButtonElement;

    const group =
      fixture.nativeElement.querySelector(
        '#desktop-career-navigation',
      ) as HTMLDivElement;

    expect(group.hidden).toBe(true);

    groupButton.click();
    fixture.detectChanges();

    expect(group.hidden).toBe(false);
    expect(
      groupButton.getAttribute(
        'aria-expanded',
      ),
    ).toBe('true');
  });
});
