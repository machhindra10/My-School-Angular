import { Component, OnInit, SimpleChanges, OnChanges, Input } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute, Router } from '@angular/router';
import { NgForm } from '@angular/forms';
import { AppSettings } from '../../app-settings';
import { CustomSnackbarService } from '../../shared/snackbar.service';
import { FadeAnimation } from '../../shared/animations';
import { Title } from '@angular/platform-browser';

@Component({
  selector: 'app-admission',
  templateUrl: './admission.component.html',
  styleUrls: ['./admission.component.css'],
  animations: [FadeAnimation]
})

export class StudentAdmissionComponent implements OnInit {

  constructor(private router: Router, private http: HttpClient, private route: ActivatedRoute,
    private snackbar: CustomSnackbarService, private titleService: Title) { }

  isLoading: boolean = false;
  isLoading3: boolean = false;
  isLoading1: boolean = false;
  isadmitted: boolean = false;
  classes: any;
  @Input() studentid: number = parseInt(this.route.snapshot.paramMap.get("id"));
  breakpoint: number;
  tstudentadmission = { 'studentid': this.studentid, 'classid': null, 'datecreated': new Date() };

  ngOnInit() {
    this.titleService.setTitle("Student Admission");
    this.breakpoint = (window.innerWidth <= 700) ? 1 : 3;

    if (this.studentid > 0) {
      this.GetStudentAdmissionStatus();
    }
  }

  onAfterSelect(id: number) {
    this.studentid = id;
    if (this.studentid > 0) {
      this.GetStudentAdmissionStatus();
    }
  }

  selectAnotherStudent() {
    this.studentid = 0;
  }

  GetClasses() {
    if (!this.isadmitted) {
      //this.isLoading3 = true;
      this.http.get(AppSettings.API_ENDPOINT + "api/classes/getclassesenabled", {
      }).toPromise().then(response => {
        this.classes = response;
        this.isLoading = false;
      }, err => {
        console.log(err.status)
        this.isLoading = false;
      });
    }
    this.isLoading3 = false;
  }

  GetStudentAdmissionStatus() {
    this.isLoading3 = true;
    this.http.get(AppSettings.API_ENDPOINT + "api/studentadmissions/checkstudentadmitted/" + this.studentid, {
    }).toPromise().then(response => {
      this.isadmitted = (<any>response);
      this.GetClasses();
    }, err => {
      console.log(err.status)
    });
  }

  Save(form: NgForm) {
    if (form.invalid)
      return;
    if (this.studentid <= 0) {
      return;
    }

    this.tstudentadmission.studentid = this.studentid;
    this.http.post(AppSettings.API_ENDPOINT + "api/studentadmissions", this.tstudentadmission, {
    }).subscribe(response => {
      this.snackbar.open("Student admission successful!");
      this.router.navigate(["payment/" + this.studentid]);
    }, err => {
      console.log(err);
      this.snackbar.open("error student admitting!");
    });
  }

  Back() {
    this.router.navigate(["students"]);
  }

  onResize(event) {
    this.breakpoint = (event.target.innerWidth <= 700) ? 1 : 3;
  }

}

