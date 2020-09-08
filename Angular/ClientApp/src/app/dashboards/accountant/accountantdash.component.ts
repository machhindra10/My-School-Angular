import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute, Router } from '@angular/router';
import { NgForm } from '@angular/forms';
import { AppSettings } from '../../app-settings';
import { CustomSnackbarService } from '../../shared/snackbar.service';
import { FadeAnimation } from '../../shared/animations';
import { Title } from '@angular/platform-browser';
import { AuthService } from '../../../app/app-auth/auth.service';

@Component({
  selector: 'app-accountantdash',
  templateUrl: './accountantdash.component.html',
  styleUrls: ['./accountantdash.component.css'],
  animations: [FadeAnimation]
})

export class AccountantDashboardComponent implements OnInit {
  isLoading: boolean = false;
  isLoading1: boolean = false;
  constructor(private router: Router, private http: HttpClient, private route: ActivatedRoute,
    private snackbar: CustomSnackbarService, private titleService: Title, private auth: AuthService) { }

  currencycode = this.auth.getCurrencyCode();
  studentid: number = 0;
  dataCount: any = {};
  ngOnInit() {
    this.titleService.setTitle("Admin Dashboard");
    this.loadDashboardData();
  }

  loadDashboardData() {
    this.isLoading = true;
    this.http.get(AppSettings.API_ENDPOINT + "api/dashbaord/getadmindashboarddata/2", {
    }).toPromise().then(response => {
      this.dataCount = response;
      this.isLoading = false;
    }, err => {
      this.isLoading = false;
      console.log(err.status);
    });
  }

  onAfterSelect(id: number) {
    this.studentid = id;
  }

  selectAnotherStudent() {
    this.studentid = 0;
  }

  Back() {
    this.router.navigate(["students"]);
  }

}

