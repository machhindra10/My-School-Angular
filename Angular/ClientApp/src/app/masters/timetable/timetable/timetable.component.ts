import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Router, ActivatedRoute } from '@angular/router';
import { AppSettings } from '../../../app-settings';
import { element } from 'protractor';
import { CustomSnackbarService } from '../../../shared/snackbar.service';
import { LoaderService } from '../../../shared/loader.service';
import { FadeAnimation } from '../../../shared/animations';
import { Title } from '@angular/platform-browser';
import { MatDialog, MatTableDataSource, MatPaginator } from '@angular/material';
import { ConfirmDialog } from '../../../../app/shared/confirm/confirm-dialog';
import { AuthService } from '../../../../app/app-auth/auth.service';
import { NgForm } from '@angular/forms';
import { AddClassSubjectComponent } from '../../classes/addclasssubject/addclasssubject.component';
import { TimePickerComponent } from '../timepicker/timepicker.component';
import { TimeSpanPickerComponent } from '../timespanpicker/timespanpicker.component';
import { PrintService } from '../../../../app/reports/print.service';

@Component({
  selector: 'app-timetable',
  templateUrl: './timetable.component.html',
  styleUrls: ['./timetable.component.css'],
  animations: [FadeAnimation]
})

export class TimeTableComponent implements OnInit {

  constructor(private http: HttpClient, private router: Router, private snackbar: CustomSnackbarService,
    private loaderService: LoaderService, private titleService: Title, public dialog: MatDialog, private auth: AuthService,
    private route: ActivatedRoute, public printService: PrintService) { }

  isLoading: boolean = false;
  isLoading1: boolean = false;
  slot: any = 30;

  slots: any = [];
  classid = parseInt(this.route.snapshot.params['id']);
  batchid = this.auth.getBatchId();
  classes: any = [];
  timetabledata: any = [];
  subjects: any = [];
  displayedColumns: string[] = ['fromtime', '_sunday', '_monday', '_tuesday', '_wednesday', '_thursday', '_friday', '_saturday', 'delete'];
  dataSource = new MatTableDataSource(this.timetabledata);

  ngOnInit() {
    this.titleService.setTitle("Time Table");
    this.GetClasses();
    this.fillSlots();
    if (this.classid != null) {
      this.load();
    }
  }

  fillSlots() {
    for (var i = 15; i < 121; i = i + 15) {
      this.slots.push(i);
    }
  }


  getDaysInMonth(month, year) {
    return new Date(year, month, 0).getDate();
  }

  getSubjectName(subjectid) {
    return this.subjects.filter(o => o.id == subjectid)[0].subject;
  }

  Add(): void {
    const dialogRef = this.dialog.open(TimeSpanPickerComponent, {
      width: '400px',
      data: { selectMany: false, filter: "", slot: this.slot }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {

        let data = { id: 0, classid: this.classid, fromtime: result.fromtime, totime: result.totime };



        this.isLoading = true;
        this.http.post(AppSettings.API_ENDPOINT + "api/timetables", data, {
        }).subscribe(response => {
          if ((<any>response).exists) {
            this.snackbar.open("Time slot already exists!");
          }
          else {
            this.GetStudents();
          }
        }, err => {
          console.log(err)
        });
        this.isLoading = false;
      }
    });
  }

  AddBreak(): void {
    const dialogRef = this.dialog.open(TimeSpanPickerComponent, {
      width: '400px',
      data: { selectMany: false, filter: "", slot: this.slot }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {

        let data = {
          id: 0, classid: this.classid, fromtime: result.fromtime, totime: result.totime,
          sunday: 0, monday: 0, tuesday: 0, wednesday: 0, thursday: 0, friday: 0, saturday: 0
        };

        this.isLoading = true;
        this.http.post(AppSettings.API_ENDPOINT + "api/timetables", data, {
        }).subscribe(response => {
          if ((<any>response).exists) {
            this.snackbar.open("Time slot already exists!");
          }
          else {
            this.GetStudents();
          }
        }, err => {
          console.log(err)
        });
        this.isLoading = false;
      }
    });
  }

  openSubjectsDialog(fromtime, totime, timetable, index): void {
    const dialogRef = this.dialog.open(AddClassSubjectComponent, {
      width: '400px',
      data: { selectMany: false, filter: "onlyclasssubjects", subjectid: (timetable.subject != null ? timetable.subjectid : 0), params: this.classid }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {

        this.isLoading = true;
        this.http.get(AppSettings.API_ENDPOINT + "api/timetables/assignsubject/" + timetable.id + "/" + result + "/" + index, {
        }).subscribe(response => {
          if ((<any>response).exists) {
            this.snackbar.open("Teacher of this subject is already assigned for this time slot");
          }
          else if ((<any>response).result) {
            this.GetStudents();
          }
        }, err => {
          console.log(err)
        });
        this.isLoading = false;
      }
    });
  }

  toTime(timeString) {
    var timeTokens = timeString.split(':');
    return new Date(1970, 0, 1, timeTokens[0], timeTokens[1], timeTokens[2]);
  }

  load() {

    if (this.classid != null) {
      this.isLoading1 = true;

      this.GetStudents();
    }
  }

  GetStudents() {
    this.http.get(AppSettings.API_ENDPOINT + "api/timetables/getbyclassid/" + this.classid + "/" + this.batchid, {
    }).toPromise().then(response => {
      this.timetabledata = response;
      this.dataSource = new MatTableDataSource(this.timetabledata);
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

  getSubjects() {
    this.isLoading = true;
    this.http.get(AppSettings.API_ENDPOINT + "api/subjects/getsubjectsenabled", {
    }).subscribe(response => {
      this.subjects = response;
    }, err => {
      console.log(err);
    });
    this.isLoading = false;
  }

  Delete(id): void {
    const dialogRef = this.dialog.open(ConfirmDialog, {
      width: '250px'
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.Delete1(id);
      }
    });
  }

  Delete1(id) {
    this.http.delete(AppSettings.API_ENDPOINT + "api/timetables/" + id, {
    }).subscribe(response => {
      this.snackbar.open("Deleted");
      this.GetStudents();
    }, err => {
      console.log(err.status)
    });
  }

  printDocument() {
    this.printService.title = "Time Table Report";
    this.printService.takeprint = true;
    this.printService.layout = 'portrait';
    const invoiceIds = [this.classid.toString(), this.batchid.toString()];
    this.printService.printDocument('timetablereport', invoiceIds);
  }

}
