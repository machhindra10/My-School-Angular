import { Component, OnInit, ViewEncapsulation, ViewChild } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { ActivatedRoute, Router } from '@angular/router';
import { NgForm, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AppSettings } from '../../../app/app-settings';
import { CustomSnackbarService } from '../../../app/shared/snackbar.service';
import { FadeAnimation } from '../../../app/shared/animations';
import { Title } from '@angular/platform-browser';
import { MatStepper } from '@angular/material';
import { AuthService } from '../../../app/app-auth/auth.service';
import { delay } from 'rxjs/operators';
import { Temp1Service } from '../../../app/shared/temp1.service';

@Component({
  selector: 'gotoapp',
  templateUrl: './gotoapp.component.html',
  styleUrls: ['./gotoapp.component.css']
})
export class GotoApplicationComponent implements OnInit {
  constructor(private router: Router, private http: HttpClient, private route: ActivatedRoute,
    private snackbar: CustomSnackbarService, private titleService: Title, private _formBuilder: FormBuilder,
    private auth: AuthService, private temp1: Temp1Service) { }

  isLoading: boolean = false;
  token = this.route.snapshot.paramMap.get("token");

  userid: number = 0;
  user: any = {};
  userroles: any = [];
  invalid: boolean = false;
  notready: boolean = false;
  checking: boolean = true;
  org: any;

  ngOnInit() {
    this.titleService.setTitle("Redirecting...");
    this.checkUserExists();
  }

  gotoapp() {
    this.auth.removeMasterToken();
    this.router.navigate(["servicelogin"]);
  }

  gotoAddFirstUser() {
    this.router.navigate(["createuser/" + this.token]);
  }


  checkUserExists() {
    this.isLoading = true;
    this.temp1.ignoreHeader = true;
    this.http.get(AppSettings.API_ENDPOINT_MASTER + "api/organisations/getbytoken/" + this.token, {
      //headers: { 'ignore': 'true' }
    }).toPromise().then(response => {
      this.temp1.ignoreHeader = false;
      if ((<any>response).result) {
        if ((<any>response).usercount == 0) {
          this.auth.setMasterToken((<any>response).token);
          this.org = (<any>response).org;

          //this.saveApplicatinSettings();
          this.GetRoles();
          //this.gotoAddFirstUser();
          ////this.createuser = true;
        }
        else {
          this.temp1.ignoreHeader = false;
          this.gotoapp();
        }
      }
      else {
        this.temp1.ignoreHeader = false;
        if ((<any>response).invalid) {
          this.invalid = true;
          this.checking = false;
        } else {
          this.notready = true;
          this.checking = false;
        }
      }


    }, err => {
      this.temp1.ignoreHeader = false;
      console.log(err.message);
      this.isLoading = false;
    });
    this.temp1.ignoreHeader = false;
  }

  GetRoles() {
    //this.isLoading = true;
    this.http.get(AppSettings.API_ENDPOINT + "api/roles/getrolesenabled", {
    }).subscribe(response => {
      this.userroles = response;
      this.user.roleId = this.userroles.filter(ur => ur.isadmin == true)[0].id;
      this.isLoading = false;
      this.gotoAddFirstUser();
    }, err => {
      console.log(err)
    });
  }

  saveApplicatinSettings() {

    let setting: any = {
      appname: this.org.orgname,
      address: this.org.address,
      country: this.org.country,
      currency: this.org.currency,
      timezoneid: this.org.timezoneid,
      token: this.org.token,
      userid: this.userid
    };
    this.http.post(AppSettings.API_ENDPOINT + "api/settings/", setting, {
    }).toPromise().then(response => {
      if ((<any>response).result) {
        this.gotoAddFirstUser();
      }
    }, err => {
      console.log(err)
    });
  }
}
