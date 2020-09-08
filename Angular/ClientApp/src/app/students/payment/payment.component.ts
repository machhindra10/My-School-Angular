import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute, Router } from '@angular/router';
import { NgForm } from '@angular/forms';
import { AppSettings } from '../../app-settings';
import { CustomSnackbarService } from '../../shared/snackbar.service';
import { FadeAnimation } from '../../shared/animations';
import { Title } from '@angular/platform-browser';
import { PrintService } from '../../../app/reports/print.service';

@Component({
  selector: 'app-payment',
  templateUrl: './payment.component.html',
  styleUrls: ['./payment.component.css'],
  animations: [FadeAnimation]
})

export class StudentPaymentComponent implements OnInit {
  isLoading: boolean = false;
  isLoading3: boolean = false;
  isLoading1: boolean = false;
  constructor(private router: Router, private http: HttpClient, private route: ActivatedRoute,
    private snackbar: CustomSnackbarService, private titleService: Title, public printService: PrintService) { }

  reloadchild: boolean = false;
  isadmitted: boolean = false;
  fees: any;
  studentid: number = parseInt(this.route.snapshot.paramMap.get("id"));
  breakpoint: number;
  tstudentpayment = { 'studentid': this.studentid, 'description': '', 'mode': '', 'chtrno': '', 'amount': null, 'datecreated': new Date() };
  paymentmodes;

  ngOnInit() {
    this.titleService.setTitle("Student Payments");
    this.breakpoint = (window.innerWidth <= 700) ? 1 : 3;
    
    this.GetStudentAdmissionStatus();
  }

  onAfterSelect(id: number) {
    this.studentid = id;
    if (this.studentid > 0) {
      this.GetStudentAdmissionStatus();
    }
  }

  onAfterFeesUpdate(result: boolean) {
    if (result) {
      this.reloadchild = !this.reloadchild;
    }
  }

  selectAnotherStudent() {
    this.studentid = 0;
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
    this.tstudentpayment.datecreated = new Date();
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
      }, err => {
        console.log(err.status)
      });
    }
    this.isLoading3 = false;
  }
 
  Back() {
    this.router.navigate(["students"]);
  }

  onResize(event) {
    this.breakpoint = (event.target.innerWidth <= 700) ? 1 : 3;
  }

}

