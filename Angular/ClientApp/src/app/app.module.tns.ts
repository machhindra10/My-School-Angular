import { NgModule, NgModuleFactoryLoader, NO_ERRORS_SCHEMA } from '@angular/core';
import { NativeScriptModule } from 'nativescript-angular/nativescript.module';
import { AppRoutingModule } from './app-routing.module.tns';
import { AppComponent } from './app/app.component';
import { LandingComponent } from './mobileapp/landing/landing.component.tns';
// Uncomment and add to NgModule imports if you need to use two-way binding
import { NativeScriptFormsModule } from 'nativescript-angular/forms';

// Uncomment and add to NgModule imports  if you need to use the HTTP wrapper
import { NativeScriptHttpClientModule } from 'nativescript-angular/http-client';
import { ParentLoginComponent } from './mobileapp/parents/login/parentlogin.component.tns';
import { ListStudentsComponent } from './mobileapp/parents/students/liststudents.component.tns';
import { Temp1Service } from './shared/temp1.service';
import { CustomSnackbarService } from './shared/snackbar.service';
import { TokenMobileInterceptor } from './mobileapp/auth/token-mobile.interceptor';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { MatSnackBarModule } from '@angular/material';
import { JwtModule, JwtHelperService } from '@auth0/angular-jwt';

import 'nativescript-localstorage';
import { AlertMobileService } from './mobileapp/shared/alert-mobile.service';
import { AuthMobileService } from './mobileapp/auth/auth-mobile.service';
import { ListMessagesComponent } from './mobileapp/parents/messages/listmessages.component.tns';
import { MessageDetailsComponent } from './mobileapp/parents/messagedetails/messagedetails.component.tns';

export function getToken(): string {
  return "";
}

@NgModule({
  declarations: [
    AppComponent, LandingComponent, ParentLoginComponent, ListStudentsComponent, ListMessagesComponent,
    MessageDetailsComponent
  ],
  imports: [[JwtModule.forRoot({
    config: {
      tokenGetter: getToken
    }
  })],
    NativeScriptModule, AppRoutingModule, NativeScriptHttpClientModule,
    NativeScriptFormsModule, MatSnackBarModule
  ],
  providers: [JwtHelperService, AuthMobileService, Temp1Service, CustomSnackbarService, AlertMobileService,
    { provide: HTTP_INTERCEPTORS, useClass: TokenMobileInterceptor, multi: true },],
  bootstrap: [AppComponent],
  schemas: [NO_ERRORS_SCHEMA]
})
/*
Pass your application module to the bootstrapModule function located in main.ts to start your app
*/
export class AppModule { }
