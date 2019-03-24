import { Component, OnInit, Input } from '@angular/core';
import { SidebarOptions } from '../sidebar-options';

@Component({
  selector: 'app-offsidebar',
  templateUrl: './offsidebar.component.html',
  styleUrls: ['./offsidebar.component.scss']
})
export class OffsidebarComponent implements OnInit {
  @Input()
  options: SidebarOptions;

  constructor() { }

  ngOnInit() {
  }

}
