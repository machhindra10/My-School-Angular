import { Component, OnInit, ViewEncapsulation, EventEmitter, Output } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { ActivatedRoute, Router } from '@angular/router';
import { NgForm, FormGroup } from '@angular/forms';
import { AppSettings } from '../../app-settings';
import { CustomSnackbarService } from '../../shared/snackbar.service';
import { FadeAnimation } from '../../shared/animations';
import { Title } from '@angular/platform-browser';
import { Location } from '@angular/common';
import { MatTableDataSource, MatDialog, MatDatepickerInputEvent } from '@angular/material';
import { AddClassSubjectComponent } from '../../../app/masters/classes/addclasssubject/addclasssubject.component';
import { ConfirmDialog } from '../../../app/shared/confirm/confirm-dialog';


@Component({
  selector: 'app-examinations',
  templateUrl: './examinations.component.html',
  styleUrls: ['./examinations.component.css'],
  animations: [FadeAnimation]
})

export class ExaminationsComponent implements OnInit {

  constructor(private router: Router, private http: HttpClient, private route: ActivatedRoute,
    private snackbar: CustomSnackbarService, private titleService: Title,
    private location: Location, public dialog: MatDialog) {

    let params = route.snapshot.params['id'];
    if (params != null) {
      this.classid = parseInt(route.snapshot.params['id'].split(',')[0]);
      this.fromdate = new Date(route.snapshot.params['id'].split(',')[1]);      
    }
  }

  @Output() dateChange: EventEmitter<MatDatepickerInputEvent<any>>;

  isLoading: boolean = false;
  isLoading1: boolean = false;
  subjects: any = [];
  classes: any = [];
  tExamSchedule: any = [];
  fromdate: Date = new Date();
  classid: number;

  displayedColumns: string[] = ['examname', 'examdate','class',   'delete'];

  ngOnInit() {
    this.titleService.setTitle("Examinations");

    if (this.classid == null || this.classid.toString() == NaN.toString()) {
      this.classid = 0;
    }
    
    if (this.fromdate == null || this.fromdate.toString() == "Invalid Date") {
      this.fromdate = new Date();
    }

    this.GetClasses();
    this.GetOne();
  }

  GetClasses(): any {
    this.http.get(AppSettings.API_ENDPOINT + "api/classes/getclassesenabled", {
    }).subscribe(response => {
      this.classes = response;
    }, err => {
      console.log(err.status)
    });
  }

  GetOne() {
    this.isLoading1 = true;
    let data = { date: this.fromdate, classid: this.classid };
    this.http.put(AppSettings.API_ENDPOINT + "api/examschedules/getbyclassid/", data, {
    }).subscribe(response => {
      this.tExamSchedule = response;

      this.isLoading1 = false;

    }, err => {
      console.log(err.status);
      this.isLoading1 = false;
    });
  }

  load(date: any) {   
    //this.GetOne();
    let params = [this.classid, this.fromdate.toDateString()];
    this.router.navigate(["examinations/" + params.join()]);
    this.ngOnInit();
  }

  Save(form: NgForm) {
    if (form.invalid)
      return;


  }

  Marksheet(element) {
    this.router.navigateByUrl("marksheet/" + element.id);
  }

  Delete(element, index): void {
    const dialogRef = this.dialog.open(ConfirmDialog, {
      width: '250px'
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.Delete1(element, index);
      }
    });
  }

  Delete1(element, index) {
    if (element.id == 0) {

    }
    else {
      this.http.delete(AppSettings.API_ENDPOINT + "api/examschedules/" + element.id, {
      }).subscribe(response => {

      }, err => {
        console.log(err.status)
      });
    }
  }

  Back() {
    this.location.back();
  }

}

