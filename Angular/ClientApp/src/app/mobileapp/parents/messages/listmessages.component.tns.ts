import { Component, OnInit, ElementRef, ViewChild } from "@angular/core";
import { Page } from "tns-core-modules/ui/page";
import { RouterExtensions } from "nativescript-angular/router";
import { HttpClient } from '@angular/common/http';
import { AppSettings } from '../../../app-settings';
import { AlertMobileService } from '../../shared/alert-mobile.service';
import { AuthMobileService } from '../../auth/auth-mobile.service';
require("nativescript-statusbar");

import { registerElement } from 'nativescript-angular/element-registry';
import { StatusBar } from 'nativescript-statusbar';
registerElement('StatusBar', () => StatusBar);

@Component({
  selector: 'listmessages',
  templateUrl: './listmessages.component.html',
  styleUrls: ['./listmessages.component.css']
})

export class ListMessagesComponent implements OnInit {

  isLoggingIn = true;
  messages: any = [];
  processing = false;

  constructor(private page: Page, private routerExtensions: RouterExtensions, private http: HttpClient,
    private auth: AuthMobileService, private alertService: AlertMobileService) { }

  ngOnInit() {
    this.load();
  }

  load() {
    this.processing = true;
    let guardianid = this.auth.getGuardianId();
    this.http.get(AppSettings.API_ENDPOINT + "api/messagesguardians/getmessagesbyguardianid/" + guardianid, {
    }).toPromise().then(response => {
      this.messages = response;
      this.processing = false;
    }, err => {
      this.processing = false;
      this.alertService.alert("error: " + err);
    });
  }


  public onItemTap(args) {
    let messageid = this.messages[args.index].id;
    this.messages[args.index].read = "NA";
    this.routerExtensions.navigate(["/messagedetails/" + messageid]);
  }

  onRefresh(args) {
    this.load();
  }

  getUnreadCount() {
    let count = this.messages.filter(o => o.read == null).length;
    if (count == 0) {
      return '';
    }
    else {
      return '(' + count + ')';
    }

  }

}
