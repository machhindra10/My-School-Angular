import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from "@angular/router";
import { NgForm } from '@angular/forms';

import { AppSettings } from '../../../app/app-settings';
import { AuthService } from '../../../app/app-auth/auth.service';  
import { Title } from '@angular/platform-browser';
import { concat, Observable } from 'rxjs';
import { CustomSnackbarService } from '../../../app/shared/snackbar.service';

@Component({
  selector: 'login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  constructor(private router: Router, private http: HttpClient, private auth: AuthService,
    private titleService: Title, private route: ActivatedRoute, private snackbar: CustomSnackbarService) { }

  returnUrl: string;
  invalidLogin: boolean;
  invalidemail: boolean = false;
  requestsent: boolean = false;
  isLoading = false;
  isTokenExpired: boolean = true;
  setupcount: any = {};
  credentials: any = {};

  ngOnInit() {
    this.titleService.setTitle('Login');
    this.returnUrl = this.route.snapshot.queryParams['returnUrl'];    

    if (!this.auth.isMasterTokenAuthenticated()) {
      this.auth.removeMasterToken();
      this.router.navigate(["servicelogin"]);
    }

    this.credentials.username = this.auth.getEmailFromMasterToken();
    this.credentials.password = null;
  }

  reset() {
    this.auth.removeMasterToken();
    this.router.navigate(["servicelogin"]);
  }

  login(form: NgForm) {

    this.requestsent = false;
    this.invalidemail = false;
    this.invalidLogin = false;

    if (!this.auth.isMasterTokenAuthenticated()) {
      this.reset();
      return;
    }
    if (form.invalid) { return; }    

    this.isLoading = true;
    this.http.post(AppSettings.API_ENDPOINT + "api/auth/login", this.credentials, {
    }).subscribe(response => {
      let token = (<any>response).token;
      this.auth.setToken(token);
      let rrtoken = (<any>response).rrtoken;
      this.auth.setRRToken(rrtoken)
      this.invalidLogin = false;
      this.isLoading = false;
      this.router.navigate(["authenticating"], { queryParams: { returnUrl: this.returnUrl } });      
    }, err => {
      this.invalidLogin = true;
      console.log(err.message);
      this.isLoading = false;
    });
  }

  forgotpassword() {
    if (this.credentials.username == null || this.credentials.username == "") {
      return;
    }
    this.isLoading = true;
    this.http.get(AppSettings.API_ENDPOINT + "api/users/forgotpassword/" + this.credentials.username, {
    }).subscribe(response => {
      if ((<any>response).result) {
        this.requestsent = true;
        this.isLoading = false;
      }
      else {
        this.invalidemail = true;
        this.isLoading = false;
      }
    }, err => {
      console.log(err.message);
      this.isLoading = false;
    });
  }
}
