import { Directionality} from '@angular/cdk/bidi';
import { OverlayContainer} from '@angular/cdk/overlay';
import { Component, ElementRef, ViewEncapsulation, ChangeDetectorRef, Inject, OnDestroy} from '@angular/core';
import { RippleOptions } from '../../shared/ripple-options';
import { LayoutDirectionality } from './layout-directionality';
import { SidebarOptions } from './sidebar-options';
import { MediaMatcher } from '@angular/cdk/layout';
import { FormGroup, FormBuilder } from '@angular/forms';

@Component({
  selector: 'app-layout',
  templateUrl: './layout.component.html',
  styleUrls: ['./layout.component.scss']
})
export class LayoutComponent implements OnDestroy {
  dark = false;
  sideOpened = true;
  offsideOpened = false;
  sidebarOptions = new SidebarOptions(64, false, 64);
  offsidebarOptions = new SidebarOptions(0, false, 0);
  mobileQuery: MediaQueryList;
  private mobileQueryListener: () => void;
  options: FormGroup;

  navItems = [
    {name: 'Beverages', route: '/beverages', icon: 'local_drink'},
    {name: 'Events', route: '/events', icon: 'event_note'}
  ];

  // tslint:disable-next-line:variable-name
  constructor(
    changeDetectorRef: ChangeDetectorRef,
    media: MediaMatcher,
    private element: ElementRef<HTMLElement>,
    private overlayContainer: OverlayContainer,
    public rippleOptions: RippleOptions,
    @Inject(Directionality) public dir: LayoutDirectionality,
    cdr: ChangeDetectorRef,
    fb: FormBuilder) {
      dir.change.subscribe(() => cdr.markForCheck());
      this.mobileQuery = media.matchMedia('(max-width: 600px)');
      this.mobileQueryListener = () => changeDetectorRef.detectChanges();
      this.mobileQuery.addListener(this.mobileQueryListener);

      this.options = fb.group({
        bottom: 0,
        fixed: false,
        top: 0
      });
  }

    toggleFullscreen() {
      // Cast to `any`, because the typings don't include the browser-prefixed methods.
      const elem = this.element.nativeElement.querySelector('.layout-content') as any;
      if (elem.requestFullscreen) {
        elem.requestFullscreen();
      } else if (elem.webkitRequestFullScreen) {
        elem.webkitRequestFullScreen();
      } else if (elem.mozRequestFullScreen) {
        elem.mozRequestFullScreen();
      } else if (elem.msRequestFullScreen) {
        elem.msRequestFullScreen();
      }
    }

    toggleTheme() {
      const darkThemeClass = 'demo-unicorn-dark-theme';

      this.dark = !this.dark;

      if (this.dark) {
        this.element.nativeElement.classList.add(darkThemeClass);
        this.overlayContainer.getContainerElement().classList.add(darkThemeClass);
      } else {
        this.element.nativeElement.classList.remove(darkThemeClass);
        this.overlayContainer.getContainerElement().classList.remove(darkThemeClass);
      }
    }

    toggleSidebar() {
      this.sideOpened = !this.sideOpened;
      console.log('Sidebar was toggled to ' + this.sideOpened);
    }

    toggleOffsidebar() {
      this.offsideOpened = !this.offsideOpened;
      console.log('Offsidebar was toggled to ' + this.offsideOpened);
    }

    isMobile() {
      return this.mobileQuery.matches;
    }

    getSidebarMode() {
      return this.mobileQuery.matches ? 'over' : 'side';
    }

    ngOnDestroy(): void {
      this.mobileQuery.removeListener(this.mobileQueryListener);
    }
}
