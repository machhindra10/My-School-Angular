import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute, Router } from '@angular/router';
import { NgForm } from '@angular/forms';
import { AppSettings } from '../../app-settings';
import { CustomSnackbarService } from '../../shared/snackbar.service';
import { FadeAnimation } from '../../shared/animations';
import { Title } from '@angular/platform-browser';
import { PrintService } from '../../../app/reports/print.service';
import { Location } from '@angular/common';


@Component({
  selector: 'app-details',
  templateUrl: './details.component.html',
  styleUrls: ['./details.component.css'],
  animations: [FadeAnimation]
})

export class StudentDetailsComponent implements OnInit {
  isLoading: boolean = false;
  isLoading3: boolean = false;
  isLoading1: boolean = false;
  constructor(private router: Router, private http: HttpClient, private route: ActivatedRoute,
    private snackbar: CustomSnackbarService, private titleService: Title, public printService: PrintService,
    private location: Location) { }

  reloadchild: boolean = false;
  isadmitted: boolean = false;
  fees: any;
  studentid: number = parseInt(this.route.snapshot.paramMap.get("id"));
  breakpoint: number;
  tstudentpayment = { 'studentid': this.studentid, 'description': '', 'mode': '', 'chtrno': '', 'amount': null };
  paymentmodes;

  ngOnInit() {
    this.titleService.setTitle("Student Details");
    this.breakpoint = (window.innerWidth <= 700) ? 1 : 3;

    if (this.studentid > 0) {
      this.GetStudentAdmissionStatus();
    }
  }

  onAfterSelect(id: number) {
    this.studentid = id;
    if (this.studentid > 0) {
      this.router.navigate(["studentdetails/" + this.studentid]);
      this.GetStudentAdmissionStatus();
    }
  }

  onAfterFeesUpdate(result: boolean) {
    if (result) {
      this.reloadchild = !this.reloadchild;
    }
  }

  edit() {
    if (this.studentid > 0) {
      this.router.navigate(["register/" + this.studentid]);
    }
  }

  selectAnotherStudent() {
    this.studentid = 0;
    this.router.navigate(["studentdetails/0"]);
  }

  Save(form: NgForm) {
    if (form.invalid)
      return;

    if (this.studentid <= 0) {
      return;
    }
    this.tstudentpayment.studentid = this.studentid;
    this.http.post(AppSettings.API_ENDPOINT + "api/studentpayments", this.tstudentpayment, {
    }).toPromise().then(response => {
      this.snackbar.open("Student payment successful!");
      this.reloadchild = !this.reloadchild;
      this.Clear();
    }, err => {
      console.log(err);
      this.snackbar.open("error student payment!");
    });
  }

  Clear() {
    this.tstudentpayment.amount = null;
    this.tstudentpayment.chtrno = null;
    this.tstudentpayment.description = null;
    this.tstudentpayment.mode = null;

  }

  GetStudentAdmissionStatus() {
    this.isLoading3 = true;
    this.http.get(AppSettings.API_ENDPOINT + "api/studentadmissions/checkstudentadmitted/" + this.studentid, {
    }).toPromise().then(response => {
      this.isadmitted = (<any>response);
      this.GetPaymentModes();
    }, err => {
      console.log(err.status)
    });
  }

  GetPaymentModes() {
    if (this.isadmitted) {
      this.http.get(AppSettings.API_ENDPOINT + "api/paymentmodes", {
      }).toPromise().then(response => {
        this.paymentmodes = (<any>response);
        this.isLoading = false;
      }, err => {
        console.log(err.status);
        this.isLoading = false;
      });
    }
    this.isLoading3 = false;
  }

  Back() {
    this.location.back();
    this.studentid = 0;
  }

  onResize(event) {
    this.breakpoint = (event.target.innerWidth <= 700) ? 1 : 3;
  }

}

