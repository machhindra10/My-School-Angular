import { Component, OnInit, ElementRef, ViewChild } from "@angular/core";
import { Page } from "tns-core-modules/ui/page";
import { RouterExtensions } from "nativescript-angular/router";
import { HttpClient } from '@angular/common/http';
import { AppSettings } from '../../../app-settings';
import { AlertMobileService } from '../../shared/alert-mobile.service';
import { AuthMobileService } from '../../auth/auth-mobile.service';


@Component({
  selector: 'liststudents',
  templateUrl: './liststudents.component.html',
  styleUrls: ['./liststudents.component.css']
})

export class ListStudentsComponent implements OnInit {

  isLoggingIn = true;
  students: any = [];
  processing = false;

  constructor(private page: Page, private routerExtensions: RouterExtensions, private http: HttpClient,
    private auth: AuthMobileService, private alertService: AlertMobileService) { }

  ngOnInit() {
    this.load();
  }

  load() {
    this.processing == true;
    let guardianid = this.auth.getGuardianId();
    this.http.get(AppSettings.API_ENDPOINT + "api/students/getbyguardianid/" + guardianid, {
    }).toPromise().then(response => {
      this.students = response;
      this.processing = false;
    }, err => {
      this.processing = false;
      this.alertService.alert("error: " + err);
    });
  }
}
