import { Component, OnInit, ElementRef, ViewChild } from "@angular/core";
import { Page } from "tns-core-modules/ui/page";
import { RouterExtensions } from "nativescript-angular/router";
import { HttpClient } from '@angular/common/http';
import { AppSettings } from '../../../app-settings';
import { Temp1Service } from '../../../shared/temp1.service';
import { AlertMobileService } from '../../shared/alert-mobile.service';
import { AuthMobileService } from '../../auth/auth-mobile.service';

@Component({
  selector: 'parentlogin',
  templateUrl: './parentlogin.component.html',
  styleUrls: ['./parentlogin.component.css']
})

export class ParentLoginComponent implements OnInit {

  isLoggingIn = true;
  user: any = { code: 'DEMO111' };
  guardian: any = { mobile: '7620345788', password: '7620345788' };
  processing = false;
  //@ViewChild("password") password: ElementRef;

  constructor(private page: Page, private routerExtensions: RouterExtensions, private http: HttpClient,
    private auth: AuthMobileService, private temp1: Temp1Service, private alertService: AlertMobileService) {

  }

  ngOnInit() {
  }

  submit() {
    if (!this.user.code) {
      this.alertService.alert("Please provide school code.");
      return;
    }

    this.processing = true;
    if (this.isLoggingIn) {
      this.login();
    } else {

    }
  }

  login() {
    this.http.get(AppSettings.API_ENDPOINT_MASTER + "api/authbycode/gettoken/" + this.user.code, {
    }).subscribe(response => {
      if ((<any>response).result) {
        let token = (<any>response).token;
        this.auth.setMasterToken(token);
        this.loginParentMobile();
      }
      else {
        this.processing = false;
        this.alertService.alert("no user available");
      }
    }, err => {
      this.processing = false;
      this.alertService.alert("Error: " + err.status);
    });
  }

  loginParentMobile() {
    this.http.post(AppSettings.API_ENDPOINT + "api/authmobile/parentlogin/", this.guardian, {
    }).subscribe(response => {
      let token = (<any>response).token;
      this.auth.setMobileToken(token);
      this.processing = false;
      // this.routerExtensions.navigate(["/liststudents"]);
      this.routerExtensions.navigate(["/listmessages"]);
    }, err => {
      this.processing = false;
      this.alertService.alert("Error: " + err.status);
    });
  }

  focusPassword() {
    //this.password.nativeElement.focus();
  }
}
