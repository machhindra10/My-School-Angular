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
  selector: 'app-addfirstuser',
  templateUrl: './addfirstuser.component.html',
  styleUrls: ['./addfirstuser.component.css'],
  animations: [FadeAnimation]
})

export class AddFirstUserComponent implements OnInit {

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
  createuser: boolean = false;
  org: any;

  ngOnInit() {
    this.titleService.setTitle("Create User");
    this.checkUserExists();
    
  }

  gotoapp() {
    this.auth.removeMasterToken();
    this.router.navigate(["servicelogin"]);
  }

 
  checkUserExists() {
    this.isLoading = true;
    this.temp1.ignoreHeader = true;
    this.http.get(AppSettings.API_ENDPOINT_MASTER + "api/organisations/getbytoken/" + this.token, {
      //headers: {  'ignore': 'true' }
    }).toPromise().then(response => {
      this.temp1.ignoreHeader = false;
      if ((<any>response).result) {
        if ((<any>response).usercount == 0) {
          this.auth.setMasterToken((<any>response).token);
          this.org = (<any>response).org;

          this.GetRoles();         

          this.createuser = true;          
        }
        else {
          this.gotoapp();
        }
      }
      else {
        if ((<any>response).invalid) {
          this.invalid = true;
        } else {
          this.notready = true;
        }        
      }
      this.isLoading = false;

      }, err => {
        this.temp1.ignoreHeader = false;
      console.log(err.message);
      this.isLoading = false;
      });
    this.temp1.ignoreHeader = false;
  }

  SaveUser(form: NgForm) {
    if (form.invalid)
      return;

    this.isLoading = true;
    this.user.photo = "/assets/user-dummy.png";
    this.http.post(AppSettings.API_ENDPOINT + "api/users/addfirstuser/", this.user, {
    }).toPromise().then(response => {
      if ((<any>response).result) {
        this.userid = (<any>response).user.id;        
        this.saveUsertoMaster();
        this.saveApplicatinSettings();
      }
    }, err => {
      console.log(err)
    });
  }

  saveUsertoMaster() {

    let orgid = this.auth.getOrgIdFromMasterToken();
    let masteruser = { orgid: orgid, email: this.user.email };
    this.http.post(AppSettings.API_ENDPOINT_MASTER + "api/users/", masteruser, {
    }).toPromise().then(response => {
      if ((<any>response).result) {
        this.gotoapp();
      }
    }, err => {
      console.log(err)
    });
  }

  GetRoles() {
    //this.isLoading = true;
    this.http.get(AppSettings.API_ENDPOINT + "api/roles/getrolesenabled", {
    }).subscribe(response => {
      this.userroles = response;
      this.user.roleId = this.userroles.filter(ur => ur.isadmin == true)[0].id;
      this.isLoading = false;
    }, err => {
      console.log(err);
      this.invalid = true;
      this.createuser = false;
    });    
  }

  saveApplicatinSettings() {

    let setting: any = {
      appname: this.org.orgname,
      address: this.org.address,
      state: this.org.state,
      country: this.org.country,
      currency: this.org.currency,
      timezoneid: this.org.timezoneid,
      token: this.org.token,
      userid: this.userid,
      logo: "../assets/school-logo.png"
    };
    this.http.post(AppSettings.API_ENDPOINT + "api/settings/", setting, {
    }).toPromise().then(response => {
      if ((<any>response).result) {        
      }
    }, err => {
      console.log(err)
    });
  }
}

