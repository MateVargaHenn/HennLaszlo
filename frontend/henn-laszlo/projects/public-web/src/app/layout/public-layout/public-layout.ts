import {
  ChangeDetectionStrategy,
  Component,
} from '@angular/core';
import { RouterOutlet } from '@angular/router';

import { PublicFooter } from '../public-footer/public-footer';
import { PublicHeader } from '../public-header/public-header';
import { ArgusChatbot } from
  '../../features/argus/components/argus-chatbot/argus-chatbot';

@Component({
  selector: 'app-public-layout',
  imports: [
    RouterOutlet,
    PublicHeader,
    PublicFooter,
    ArgusChatbot,
  ],
  templateUrl: './public-layout.html',
  styleUrl: './public-layout.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PublicLayout {}