import { Component, OnInit, ElementRef, ViewChild } from "@angular/core";
import { Page, Observable } from "tns-core-modules/ui/page";
import { RouterExtensions } from "nativescript-angular/router";
import { HttpClient } from '@angular/common/http';
import { AppSettings } from '../../../app-settings';
import { AlertMobileService } from '../../shared/alert-mobile.service';
import { AuthMobileService } from '../../auth/auth-mobile.service';
import { ActivatedRoute, Router } from '@angular/router';
import { registerElement } from 'nativescript-angular/element-registry';
import { CardView } from 'nativescript-cardview';
//registerElement('CardView', () => CardView);


@Component({
  selector: 'messagedetails',
  moduleId: module.id,
  templateUrl: './messagedetails.component.html',
  styleUrls: ['./messagedetails.component.css']
})

export class MessageDetailsComponent implements OnInit {

  isLoggingIn = true;
  message: any;
  processing = false;
  messageid: number = parseInt(this.route.snapshot.paramMap.get("id"));
  constructor(private page: Page, private routerExtensions: RouterExtensions, private http: HttpClient,
    private auth: AuthMobileService, private alertService: AlertMobileService, private route: ActivatedRoute,
  ) { }

  ngOnInit() {
    this.load();
  }

  load() {
    this.processing = true;

    this.http.get(AppSettings.API_ENDPOINT + "api/messagesguardians/getmessagebyid/" + this.messageid, {
    }).toPromise().then(response => {
      this.message = response;
      this.processing = false;
    }, err => {
      this.processing = false;
      this.alertService.alert("error: " + err);
    });
  }

  public onNavBtnTap() {
    // This code will be called only in Android.
    this.routerExtensions.navigate(["/listmessages"]);
  }

}
