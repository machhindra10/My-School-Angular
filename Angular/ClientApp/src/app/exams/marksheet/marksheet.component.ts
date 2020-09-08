import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { ActivatedRoute, Router } from '@angular/router';
import { NgForm, FormGroup } from '@angular/forms';
import { AppSettings } from '../../app-settings';
import { CustomSnackbarService } from '../../shared/snackbar.service';
import { FadeAnimation } from '../../shared/animations';
import { Title } from '@angular/platform-browser';
import { Location } from '@angular/common';
import { MatTableDataSource, MatDialog } from '@angular/material';
import { ConfirmDialog } from '../../../app/shared/confirm/confirm-dialog';

export interface imarksheet {
  id: number;
  batchid: number;
  examid: number;
  classid: number;
  subjectid: number;
  studentid: number;  
  obtained: number;
  practical: number;
  totalmarks: number;
  grade: string;
  userid: number;
  dateCreated: any;
  studentname: string;
}
@Component({
  selector: 'app-marksheet',
  templateUrl: './marksheet.component.html',
  styleUrls: ['./marksheet.component.css'],
  animations: [FadeAnimation]
})

export class MarksheetComponent implements OnInit {

  constructor(private router: Router, private http: HttpClient, private route: ActivatedRoute,
    private snackbar: CustomSnackbarService, private titleService: Title,
    private location: Location, public dialog: MatDialog) { }

  scheduleid = parseInt(this.route.snapshot.paramMap.get("id"));
  isLoading: boolean = false;
  isLoading1: boolean = false;

  examschedule: any = {};
  classname;
  examname;
  subject;
  examdate1: Date;
  sexamdate1: string;

  marksheet: any = [];

  displayedColumns: string[] = ['sname', 'obtained', 'practical','total'];
  dataSource = new MatTableDataSource();

  ngOnInit() {
    this.titleService.setTitle("Marksheet");

    this.GetOne();
  }

  getTotal(index) {
    this.marksheet = this.dataSource.data;
    this.marksheet[index].totalmarks = this.marksheet[index].obtained + this.marksheet[index].practical;

    this.dataSource = new MatTableDataSource(this.marksheet);
  }

  GetOne() {
    if (this.scheduleid > 0) {
      this.isLoading = true;
      this.http.get(AppSettings.API_ENDPOINT + "api/examschedules/getbyid/" + this.scheduleid, {
      }).toPromise().then(response => {
        this.examschedule = response;
        this.classname = this.examschedule.class.standard;
        this.examname = this.examschedule.exam.examName;
        this.subject = this.examschedule.subject.subject;
        this.isLoading = false;

        this.dataSource = new MatTableDataSource(this.examschedule.tExamMarkSheet);
      }, err => {
        console.log(err.status);
        this.isLoading = false;
      });
    }
  }  

  Save(form: NgForm) {
    if (form.invalid)
      return;

    this.http.put(AppSettings.API_ENDPOINT + "api/examschedules/" + this.scheduleid, this.examschedule, {
    }).subscribe(response => {    
    }, err => {
      console.log(err)
    });


    let marksheet = this.dataSource.data;    
    this.http.post(AppSettings.API_ENDPOINT + "api/exammarksheets/insertmany/", marksheet, {
    }).subscribe(response => {
      this.snackbar.open("Marksheet updated!");
      this.Back();
    }, err => {
      console.log(err)
    });
    
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
      //this.tExamSchedule.splice(index, 1);
      //this.dataSource = new MatTableDataSource(this.tExamSchedule);

    }
    else {
      this.http.delete(AppSettings.API_ENDPOINT + "api/examschedules/" + element.id, {
      }).subscribe(response => {
        //this.tExamSchedule.splice(index, 1);
        //this.dataSource = new MatTableDataSource(this.tExamSchedule);

      }, err => {
        console.log(err.status)
      });
    }
  }

  Back() {
    this.location.back();
  }

}

