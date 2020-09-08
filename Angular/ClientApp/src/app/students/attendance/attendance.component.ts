import { Component, OnInit, ViewChild } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Router } from '@angular/router';
import { AppSettings } from '../../app-settings';
import { element } from 'protractor';
import { CustomSnackbarService } from '../../shared/snackbar.service';
import { LoaderService } from '../../shared/loader.service';
import { FadeAnimation } from '../../shared/animations';
import { Title } from '@angular/platform-browser';
import { MatDialog, MatTableDataSource, MatPaginator } from '@angular/material';
import { ConfirmDialog } from '../../../app/shared/confirm/confirm-dialog';
import { AuthService } from '../../../app/app-auth/auth.service';

@Component({
  selector: 'app-attendance',
  templateUrl: './attendance.component.html',
  styleUrls: ['./attendance.component.css'],
  animations: [FadeAnimation]
})

export class StudentAttendanceComponent implements OnInit {

  constructor(private http: HttpClient, private router: Router, private snackbar: CustomSnackbarService,
    private loaderService: LoaderService, private titleService: Title, public dialog: MatDialog, private auth: AuthService) { }

  breakpoint: boolean;
  isLoading: boolean = false;
  isLoading1: boolean = false;
  date1 = new Date();
  minDate = new Date((new Date()).getFullYear(), 5, 1);   
  maxDate = new Date((parseInt((new Date()).getFullYear().toString()) + 1), 5, 0);
  classid;
  classes: any = [];
  students: any = [];
  displayedColumns: string[] = ['name', 'present'];
  dataSource: any;


  ngOnInit() {
    this.titleService.setTitle("Attendance");
    this.GetClasses();
  }

  load() {
    
    if (this.classid != null && this.date1 != null) {
      this.isLoading1 = true;
      this.GetAttendance();
    }
  }
  GetAttendance() {
    
    this.http.put(AppSettings.API_ENDPOINT + "api/studentattendences/getbyclassid/" + this.classid, this.date1, {
    }).toPromise().then(response => {
      this.students = response;
      this.isLoading1 = false;
    }, err => {
      console.log(err)
      this.isLoading1 = false;
    });

  }

  GetClasses() {
    this.isLoading = true;
    this.http.get(AppSettings.API_ENDPOINT + "api/classes/getclassesenabled", {
    }).toPromise().then(response => {
      this.classes = response;
      this.isLoading = false;
    }, err => {
      console.log(err)
      this.isLoading = false;
    });

  }

  Update(element) {
    let ele = element.tStudentAttendence[0];
    if (ele == null) {
      ele = { id: 0, studentid: element.id, classid: this.classid, datecreated: this.date1, ispresent: true };
    }
    else {
      ele.ispresent = !ele.ispresent;
    }
    this.http.put(AppSettings.API_ENDPOINT + "api/studentattendences/update/" + ele.id, ele, {
    }).subscribe(response => {
      //this.snackbar.open("Updated");
      this.GetAttendance();
    }, err => {
      console.log(err.status)
    });
  }


}
