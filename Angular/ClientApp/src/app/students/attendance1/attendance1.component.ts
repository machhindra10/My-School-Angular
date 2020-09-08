import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
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
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-attendance1',
  templateUrl: './attendance1.component.html',
  styleUrls: ['./attendance1.component.css'],
  animations: [FadeAnimation]
})

export class StudentAttendance1Component implements OnInit {

  constructor(private http: HttpClient, private router: Router, private snackbar: CustomSnackbarService,
    private loaderService: LoaderService, private titleService: Title, public dialog: MatDialog, private auth: AuthService) { }

  isLoading: boolean = false;
  isLoading1: boolean = false;
  year = (new Date()).getFullYear();
  daysinmonth: number;
  //date1 = new Date();
  //minDate = new Date(this.auth.getYear(), 5, 1);
  //maxDate = new Date((parseInt(this.auth.getYear().toString()) + 1), 5, 0);
  classid;
  batchid = this.auth.getBatchId();
  imonth = (new Date()).getMonth() + 1;
  classes: any = [];
  students: any = [];
  tmonths: any = [];
  currentdate: number = 0;
  currentMonth = (new Date()).getMonth() + 1;

  displayedColumns: string[] = ['name', '_1', '_2', '_3', '_4', '_5', '_6', '_7', '_8', '_9', '_10',
    '_11', '_12', '_13', '_14', '_15', '_16', '_17', '_18', '_19', '_20', '_21',
    '_22', '_23', '_24', '_25', '_26', '_27', '_28', '_29', '_30', '_31'];


  ngOnInit() {
    this.titleService.setTitle("Attendance1");
    this.GetClasses();
    this.GetMonths();
  }

  setCurrentDay() {
    if (this.imonth == (new Date().getMonth() + 1)) {
      this.currentdate = new Date().getDate();
    }
    else {
      this.currentdate = 0;
    }
  }

  getDaysInMonth(month, year) {
    return new Date(year, month, 0).getDate();
  }

  setColumnsToDisaplay(month, year) {
    this.daysinmonth = this.getDaysInMonth(month, year);
    if (this.daysinmonth == 28) {
      this.displayedColumns = ['name', '_1', '_2', '_3', '_4', '_5', '_6', '_7', '_8', '_9', '_10',
        '_11', '_12', '_13', '_14', '_15', '_16', '_17', '_18', '_19', '_20', '_21',
        '_22', '_23', '_24', '_25', '_26', '_27', '_28'];

    } else if (this.daysinmonth == 29) {
      this.displayedColumns = ['name', '_1', '_2', '_3', '_4', '_5', '_6', '_7', '_8', '_9', '_10',
        '_11', '_12', '_13', '_14', '_15', '_16', '_17', '_18', '_19', '_20', '_21',
        '_22', '_23', '_24', '_25', '_26', '_27', '_28', '_29'];

    } else if (this.daysinmonth == 30) {
      this.displayedColumns = ['name', '_1', '_2', '_3', '_4', '_5', '_6', '_7', '_8', '_9', '_10',
        '_11', '_12', '_13', '_14', '_15', '_16', '_17', '_18', '_19', '_20', '_21',
        '_22', '_23', '_24', '_25', '_26', '_27', '_28', '_29', '_30'];
    } else {
      this.displayedColumns = ['name', '_1', '_2', '_3', '_4', '_5', '_6', '_7', '_8', '_9', '_10',
        '_11', '_12', '_13', '_14', '_15', '_16', '_17', '_18', '_19', '_20', '_21',
        '_22', '_23', '_24', '_25', '_26', '_27', '_28', '_29', '_30', '_31'];
    }
  }

  UpdateStatus(element, i, columnName) {
    let value = this.students[i][columnName];
    
    if (value == null) {
      value = 'P';
    }
    else if (value == 'P') {
      value = 'A';
    }
    else if (value == 'A') {
      value = 'L';
    }
    else if (value == 'L') {
      value = 'P';
    }

    this.students[i][columnName] = value;

  }

  Save() {
    this.http.post(AppSettings.API_ENDPOINT + "api/studentattendence1/updateattendence/", this.students, {
    }).subscribe(response => {
      this.snackbar.open("Updated!");
    }, err => {
      console.log(err)
    });
  }

  @ViewChild('scrolldiv', { read: ElementRef }) public panel: ElementRef<any>;

  setScrollPosition() {
    let iday = (new Date()).getUTCDate();
    let tday = iday - 1;
    this.panel.nativeElement.scrollLeft = (38 * tday);
    //event.target.scrollLeft = 0;
    //console.log(this.panel.nativeElement.scrollLeft + " - " + tday);
  }

  load() {

    if (this.classid != null && this.imonth != null) {
      this.isLoading1 = true;
      this.setColumnsToDisaplay(this.imonth, this.year);
      this.GetStudents();
    }
  }

  GetStudents() {
    this.http.get(AppSettings.API_ENDPOINT + "api/studentattendence1/getstudents/" + this.classid + "/" + this.imonth + "/" + this.batchid, {
    }).toPromise().then(response => {
      this.students = response;
      this.isLoading1 = false;
      //this.setScrollPosition();
      this.setCurrentDay();
    }, err => {
      console.log(err)
      this.isLoading1 = false;
    });
  }

  GetMonths() {
    this.http.get(AppSettings.API_ENDPOINT + "api/studentattendence1/getmonths", {
    }).toPromise().then(response => {
      this.tmonths = response;
      this.isLoading = false;
    }, err => {
      console.log(err)
      this.isLoading = false;
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

}
