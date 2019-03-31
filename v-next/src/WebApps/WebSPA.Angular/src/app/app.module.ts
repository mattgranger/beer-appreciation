import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { SharedModule } from './shared/shared.module';
import { CoreModule } from './core/core.module';
import { OverlayContainer, FullscreenOverlayContainer } from '@angular/cdk/overlay';
import { MAT_RIPPLE_GLOBAL_OPTIONS } from '@angular/material';
import { RippleOptions } from './shared/ripple-options';
import { Directionality } from '@angular/cdk/bidi';
import { LayoutDirectionality } from './core/layout/layout-directionality';
import { HomeComponent } from './pages/home/home.component';
import { LoginComponent } from './pages/login/login.component';
import { FlexLayoutModule } from '@angular/flex-layout';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    LoginComponent
  ],
  imports: [
    BrowserModule,
    FlexLayoutModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    SharedModule,
    CoreModule
  ],
  providers: [
    {provide: OverlayContainer, useClass: FullscreenOverlayContainer},
    {provide: MAT_RIPPLE_GLOBAL_OPTIONS, useExisting: RippleOptions},
    {provide: Directionality, useClass: LayoutDirectionality},
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
