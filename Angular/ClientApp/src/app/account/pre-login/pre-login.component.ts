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
  selector: 'pre-login',
  templateUrl: './pre-login.component.html',
  styleUrls: ['./pre-login.component.css']
})
export class PreLoginComponent implements OnInit {
  constructor(private router: Router, private http: HttpClient, private auth: AuthService,
    private titleService: Title, private route: ActivatedRoute, private snackbar: CustomSnackbarService) { }

  returnUrl: string;
  invalidLogin: boolean;
  isLoading = false;
  isTokenExpired: boolean = true;
  setupcount: any = {};
  credentials: any = {};

  ngOnInit() {
    this.titleService.setTitle('Login');
    this.returnUrl = this.route.snapshot.queryParams['returnUrl'];
  }

  login(form: NgForm) {
    if (form.invalid) { return; }
    this.isLoading = true;
    
    //console.log(this.credentials);
    this.http.post(AppSettings.API_ENDPOINT_MASTER + "api/authprelogin/login", this.credentials, {
    }).subscribe(response => {
      if ((<any>response).result) {
        let token = (<any>response).token;
        this.auth.setMasterToken(token);
        this.invalidLogin = false;
        this.isLoading = false;
        this.router.navigate(["login"]);
      }
      else {
        this.router.navigate(["subscriptionexpired"]);
      }
    }, err => {
      this.invalidLogin = true;
      //console.log(err.message);
      this.isLoading = false;
    });
  } 
}
