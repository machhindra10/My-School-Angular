import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from "@angular/router";
import { NgForm } from '@angular/forms';

import { AppSettings } from '../../app-settings';
import { AuthService } from '../../app-auth/auth.service';
import { Title } from '@angular/platform-browser';

@Component({
  selector: 'recovery',
  templateUrl: './recovery.component.html',
  styleUrls: ['./recovery.component.css']
})
export class PasswordRecoveryComponent implements OnInit {
  constructor(private router: Router, private http: HttpClient, private auth: AuthService,
    private titleService: Title, private route: ActivatedRoute) { }

  params = this.route.snapshot.paramMap.get("id");
  userid: number;
  passverificationcode: string = '';
  passnotmatch: boolean = false;
  success: boolean = false;
  isLoading = false;
  newpass: string;
  confirmpass: string;
  invalidrequest: boolean = false;

  ngOnInit() {
    this.titleService.setTitle('Password recovery');

    if ((this.params.split(',')[0]) != null) {
      this.userid = parseInt(this.params.split(',')[0]);
    }
    if ((this.params.split(',')[1]) != null) {
      this.passverificationcode = this.params.split(',')[1];
    }

    this.checkVerification();
  }

  checkVerification() {
    if (this.userid != null && this.passverificationcode != null) {
      this.isLoading = true;
      this.http.get(AppSettings.API_ENDPOINT + "api/users/checkpasswordverificationcode/" + this.userid + "/" + this.passverificationcode, {
      }).subscribe(response => {
        if (!(<any>response).result) {
          this.invalidrequest = true;
          this.isLoading = false;
        }
        else {
          this.isLoading = false;
        }
      }, err => {
        console.log(err.message);
        this.isLoading = false;
        this.invalidrequest = true;
      });
    }
    else {
      this.invalidrequest = true;
    }
  }

  login(form: NgForm) {
    if (form.invalid) { return; }
    this.isLoading = true;

    this.http.get(AppSettings.API_ENDPOINT + "api/users/recoverpassword/" + this.userid + "/" + this.newpass + "/" + this.confirmpass, {
    }).subscribe(response => {
      if ((<any>response).match) {
        if ((<any>response).result) {
          this.success = true;
        }
        this.isLoading = false;
      }
      else {
        this.passnotmatch = true;
        this.isLoading = false;
      }
    }, err => {
      console.log(err.message);
      this.isLoading = false;
    });
  }

}
