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
import { load } from '@angular/core/src/render3';
import { forEach } from '@angular/router/src/utils/collection';
import { ConfirmMessageDialog } from '../../../app/shared/confirm-message/confirm-message';

@Component({
  selector: 'app-staffattendance',
  templateUrl: './staffattendance.component.html',
  styleUrls: ['./staffattendance.component.css'],
  animations: [FadeAnimation]
})

export class StaffAttendanceComponent implements OnInit {

  constructor(private http: HttpClient, private router: Router, private snackbar: CustomSnackbarService,
    private loaderService: LoaderService, private titleService: Title, public dialog: MatDialog, private auth: AuthService) { }

  isLoading: boolean = false;
  isLoading1: boolean = false;
  year = (new Date()).getFullYear();
  daysinmonth: number;

  currentMonth = (new Date()).getMonth() + 1;
  imonth = (new Date()).getMonth() + 1;
  years: any = [];
  students: any = [];
  tmonths: any = [];


  selectable: string[] = ["A", "P", "H", null];

  displayedColumns: string[] = ['name', '_1', '_2', '_3', '_4', '_5', '_6', '_7', '_8', '_9', '_10',
    '_11', '_12', '_13', '_14', '_15', '_16', '_17', '_18', '_19', '_20', '_21',
    '_22', '_23', '_24', '_25', '_26', '_27', '_28', '_29', '_30', '_31'];

  currentdate: number = 0;
  holidaysExists: boolean;
  weeklyoffsExists: boolean;

  ngOnInit() {
    this.titleService.setTitle("Staff Attendance");
    this.GetYears();
    this.GetMonths();
    this.getLeavesAndHolidayStatus();
    this.load();
  }

  generateAttendance() {
    if (this.year != null && this.imonth != null) {

      this.openConfirmDialog(this.weeklyoffsExists, this.holidaysExists);

      this.setColumnsToDisaplay(this.imonth, this.year);
      this.http.get(AppSettings.API_ENDPOINT + "api/staffattendance1/generate/" + this.imonth + "/" + this.year, {
      }).subscribe(response => {
        this.students = response;
        this.isLoading1 = false;
        this.setCurrentDay();
      }, err => {
        console.log(err)
        this.isLoading1 = false;
      });

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
      value = 'H';
    }
    else if (value == 'H') {
      value = 'P';
    }

    this.students[i][columnName] = value;

  }

  getLeavesAndHolidayStatus() {
    //this.isLoading = true;
    this.http.get(AppSettings.API_ENDPOINT + "api/holidays/getcountbyear/" + this.year, {
    }).toPromise().then(response => {

      this.weeklyoffsExists = (<any>response).weeklyoffsExists;
      this.holidaysExists = (<any>response).holidaysExists;

      if (!this.weeklyoffsExists || !this.holidaysExists) {
        this.openConfirmDialog(this.weeklyoffsExists, this.holidaysExists);
      }      
    }, err => {      
    });
  }

  openConfirmDialog(weeklyoffsExists: boolean, holidaysExists: boolean) {
    if (this.weeklyoffsExists && this.holidaysExists) {
      return;
    }
    let message = 'It seems you have missed following settings to save, please review them before generating attendance.';

    if (!weeklyoffsExists) {
      message = message + ' Weekly off are not saved!';
    }
    if (!holidaysExists) {
      message = message + ' Holidays are not saved!';
    }

    message = message + ' Are you sure to go to settings to save them?';

    const dialogRef = this.dialog.open(ConfirmMessageDialog, {
      width: '400px',
      data: { message: message }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        if (!weeklyoffsExists) {
          this.router.navigate(["weeklyoff"]);
        }
        if (!holidaysExists) {
          this.router.navigate(["leaves"]);
        }
      }
    });
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

  Save() {
    this.http.put(AppSettings.API_ENDPOINT + "api/staffattendance1/updateattendance/" + this.imonth + "/" + this.year, this.students, {
    }).subscribe(response => {
      if ((<any>response).result) {
        this.snackbar.open("Updated!");
      }
      else {
        this.snackbar.open("Attendance already upto date!");
      }
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

  setCurrentDay() {
    if (this.imonth == (new Date().getMonth() + 1)) {
      this.currentdate = new Date().getDate();
    }
    else {
      this.currentdate = 0;
    }
  }


  load() {

    if (this.year != null && this.imonth != null) {
      this.isLoading1 = true;
      this.setColumnsToDisaplay(this.imonth, this.year);
      this.GetStudents();
    }
  }

  GetStudents() {
    this.http.get(AppSettings.API_ENDPOINT + "api/staffattendance1/getbymonth/" + this.imonth + "/" + this.year, {
    }).subscribe(response => {
      this.students = response;
      this.isLoading1 = false;
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

  GetYears() {
    this.isLoading = true;
    this.http.get(AppSettings.API_ENDPOINT + "api/settings/getyearonly", {
    }).toPromise().then(response => {
      this.years = response;
      this.isLoading = false;
    }, err => {
      console.log(err)
      this.isLoading = false;
    });
  }

}
