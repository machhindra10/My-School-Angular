import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from "@angular/router";
import { NgForm } from '@angular/forms';

import { AppSettings } from '../app-settings';
import { AuthService } from '../app-auth/auth.service';
import { Title } from '@angular/platform-browser';
import { concat, Observable } from 'rxjs';
import { CustomSnackbarService } from '../shared/snackbar.service';
import { getToken } from '../app.module';
import { MatDialog } from '@angular/material';
import { AddSettingsComponent } from '../setup/addsettings/addsettings.component';
import { AddFirstBatchComponent } from '../setup/addfirstbatch/addfirstbatch.component';

@Component({
  selector: 'landing',
  templateUrl: './landing.component.html',
  styleUrls: ['./landing.component.css']
})
export class LandingComponent implements OnInit {
  constructor(private router: Router, private http: HttpClient, private auth: AuthService,
    private titleService: Title, private route: ActivatedRoute, private snackbar: CustomSnackbarService,
    public dialog: MatDialog) { }

  returnUrl: string;
  isLoading = false;
  isTokenExpired: boolean = true;
  setupcount: any = {};

  ngOnInit() {
    this.titleService.setTitle('Authenticating');
    this.returnUrl = this.route.snapshot.queryParams['returnUrl'];
    this.getSettings();
  }

  getSettings() {
    this.isLoading = true;
    this.http.get(AppSettings.API_ENDPOINT + "api/settings/getsetupcount/", {
    }).subscribe(response => {
      this.setupcount = response;
      if (this.setupcount.settingscount == 0) {
        //this.router.navigate(["createsettings"]);
        this.openSettingsDialog();
      } else if (this.setupcount.batchescount == 0) {
        //this.router.navigate(["createbatch"]);
        this.openBatchDialog();
      }
      else {
        this.gotoapp();
      }
      this.isLoading = false;

    }, err => {
      console.log(err.message);
      this.isLoading = false;
    });
  }

  gotoapp() {
    let rolename = this.auth.getRoleName();

    if (this.returnUrl != null) {
      this.router.navigateByUrl(this.returnUrl);
    } else if (rolename.toLowerCase() == 'administrator') {
      this.router.navigateByUrl('admindashboard');
    } else if (rolename.toLowerCase() == 'teacher') {
      this.router.navigateByUrl('teacherdashboard');
    } else if (rolename.toLowerCase() == 'accountant') {
      this.router.navigateByUrl('accountantdashboard');
    } else {
      this.router.navigateByUrl('/');
    }
  }


  openSettingsDialog(): void {
    const dialogRef = this.dialog.open(AddSettingsComponent, {
      width: '800px'
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        if (this.setupcount.batchescount == 0) {
          this.openBatchDialog();
        }
        else {
          this.snackbar.open("Please login again!");
          this.router.navigate(["servicelogin"]);
        }
      }
    });
  }

  openBatchDialog(): void {
    const dialogRef = this.dialog.open(AddFirstBatchComponent, {
      width: '500px'
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.snackbar.open("Please login again!");
        this.router.navigate(["servicelogin"]);
      }
    });
  }

  //not using rightnow
  CheckSubscriptionExpired() {
    this.isLoading = true;
    this.getOrgToken().subscribe(token => {
      this.http.get(AppSettings.API_ENDPOINT_MASTER + "api/transactions/checksubscription/" + token, {
      }).subscribe(response => {
        if ((<any>response).isvalid) {
          this.isLoading = false;
        }
        else {
          this.isLoading = false;
          this.router.navigate(["subscriptionexpired"]);
        }
      }, err => {
        console.log(err.message);
        this.isLoading = false;
        this.router.navigate(["subscriptionexpired"]);
      });
    }, err => {
      console.log(err.message);
      this.isLoading = false;
      this.router.navigate(["subscriptionexpired"]);
    });
  }

  login() {
    //this.isLoading = true;
    //this.getOrgToken().subscribe(token => {
    //  this.http.get(AppSettings.API_ENDPOINT_MASTER + "api/transactions/checksubscription/" + token, {
    //  }).subscribe(response => {
    //    if ((<any>response).isvalid) {
    //      this.login1(form);
    //      this.isLoading = false;
    //    }
    //    else {
    //      this.isLoading = false;
    //      this.snackbar.open("Subscription is expired!");
    //      this.router.navigate(["subscriptionexpired"]);
    //    }
    //  }, err => {
    //    console.log(err.message);
    //    this.isLoading = false;
    //    this.snackbar.open("Subscription is expired!");
    //    this.router.navigate(["subscriptionexpired"]);
    //  });
    //});
  }

  getOrgToken(): any {
    let token = "";
    return this.http.get(AppSettings.API_ENDPOINT + "api/settings/getorgtoken/", {
    }).map(response => {
      token = (<any>response).tk;
      return token
    }, err => {
      console.log(err);
      return token;
    });
  }
}
