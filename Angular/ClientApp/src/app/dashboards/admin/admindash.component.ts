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
  selector: 'app-admindash',
  templateUrl: './admindash.component.html',
  styleUrls: ['./admindash.component.css'],
  animations: [FadeAnimation]
})

export class AdminDashboardComponent implements OnInit {
  isLoading: boolean = false;
  isLoading1: boolean = false;
  constructor(private router: Router, private http: HttpClient, private route: ActivatedRoute,
    private snackbar: CustomSnackbarService, private titleService: Title, private auth: AuthService) { }

  month: number = (new Date()).getMonth() + 1;
  year: number = (new Date()).getFullYear();
  batchid = this.auth.getBatchId();

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

  GotoStudents() {
    this.router.navigate(["students"]);
  }

  gotoCollectionsReport() {
    let params = [0, this.batchid, this.month, this.year];
    this.router.navigate(["collectionsreport/" + params.join()]);
  }

  gotoSaleriesReport() {
    let params = [this.month, this.year];
    this.router.navigate(["salariesreport/" + params.join()]);
  }

  gotoExpenseReport() {
    //let datefrom = new Date(this.year, this.month, 1);
    //let dateto = new Date(this.year, this.month + 1, 0);
    //let params = [datefrom.toDateString(), dateto.toDateString()];
    //this.router.navigate(["dailyexpensereport/" + params.join()]);
    this.router.navigate(["dailyexpensereport/0"]);
  }
}

