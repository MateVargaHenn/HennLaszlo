import {
  ChangeDetectionStrategy,
  Component,
} from '@angular/core';

@Component({
  selector: 'app-public-footer',
  templateUrl: './public-footer.html',
  styleUrl: './public-footer.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PublicFooter {
  protected readonly currentYear =
    new Date().getFullYear();
}