import { Component, OnInit, Input, Inject, Output, EventEmitter, ViewEncapsulation } from '@angular/core';
import { LayoutDirectionality } from '../layout-directionality';
import { RippleOptions } from 'src/app/shared/ripple-options';
import { Directionality } from '@angular/cdk/bidi';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit {

  @Output() toggleFullscreen = new EventEmitter();
  @Output() toggleTheme = new EventEmitter();
  @Output() toggleSidebar = new EventEmitter();
  @Output() toggleOffsidebar = new EventEmitter();

  constructor(public rippleOptions: RippleOptions,
              @Inject(Directionality) public dir: LayoutDirectionality) { }

  ngOnInit() {
  }

  onToggleFullscreen() {
    this.toggleFullscreen.emit();
  }

  onToggleTheme() {
    this.toggleTheme.emit();
  }

  onToggleSidebar() {
    this.toggleSidebar.emit();
  }

  onToggleOffsidebar() {
    this.toggleOffsidebar.emit();
  }
}
