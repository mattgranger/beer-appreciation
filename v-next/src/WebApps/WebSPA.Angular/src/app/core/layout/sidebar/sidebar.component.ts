import { Component, OnInit, Input, ViewEncapsulation, Output, EventEmitter } from '@angular/core';
import { SidebarOptions } from '../sidebar-options';

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.scss']
})
export class SidebarComponent implements OnInit {
@Input()
options: SidebarOptions;
@Input()
open = true;
@Input()
navItems: any[];

constructor() { }

  ngOnInit() {
  }
}
