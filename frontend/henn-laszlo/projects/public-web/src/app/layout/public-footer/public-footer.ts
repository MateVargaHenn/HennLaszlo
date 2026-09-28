import {
  ChangeDetectionStrategy,
  Component,
} from '@angular/core';
import {
  APP_VERSION,
} from '../../core/version/app-version';

@Component({
  selector: 'app-public-footer',
  templateUrl: './public-footer.html',
  styleUrl: './public-footer.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PublicFooter {
  protected readonly currentYear =
    new Date().getFullYear();
  protected readonly appVersion =
    APP_VERSION;
}