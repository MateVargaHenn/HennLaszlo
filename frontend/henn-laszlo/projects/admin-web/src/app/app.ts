import {
  ChangeDetectionStrategy,
  Component
} from '@angular/core';

import {
  RouterOutlet
} from '@angular/router';
import { AdminHeader } from './layout/admin-header/admin-header';

@Component({
  selector: 'app-root',
  imports: [AdminHeader, RouterOutlet],
  templateUrl: './app.html',
  styleUrl: './app.css',
  changeDetection:
    ChangeDetectionStrategy.OnPush,
})
export class App {
 
}