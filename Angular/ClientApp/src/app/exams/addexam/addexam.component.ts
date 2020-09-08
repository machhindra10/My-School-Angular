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
import { AddClassSubjectComponent } from '../../../app/masters/classes/addclasssubject/addclasssubject.component';
import { ConfirmDialog } from '../../../app/shared/confirm/confirm-dialog';
import { forEach } from '@angular/router/src/utils/collection';

export interface examSchedule {
  id: number;
  batchid: number;
  examid: number;
  classid: number;
  subjectid: number;
  subjectName: string;
  examdate: any;
  starttime: any;
  endtime: any;
  userid: number;
  dateCreated: any;
  totalmarks: number;
  passingmarks: number;
}
@Component({
  selector: 'app-addexam',
  templateUrl: './addexam.component.html',
  styleUrls: ['./addexam.component.css'],
  animations: [FadeAnimation]
})

export class AddExamComponent implements OnInit {

  constructor(private router: Router, private http: HttpClient, private route: ActivatedRoute,
    private snackbar: CustomSnackbarService, private titleService: Title,
    private location: Location, public dialog: MatDialog) { }

  isLoading: boolean = false;
  isLoading1: boolean = false;
  subjects: any = [];
  subjects1: any = [];
  classes: any = [];
  exam: any = {};
  tExamSchedule: examSchedule[] = [];

  id: number = parseInt(this.route.snapshot.paramMap.get("id"));
  displayedColumns: string[] = ['subject', 'examdate', 'starttime', 'endtime', 'delete'];
  dataSource = new MatTableDataSource(this.tExamSchedule);

  ngOnInit() {
    this.titleService.setTitle("Add Exam");
    this.GetClasses();
    this.GetSubjects();
    this.GetOne();
  }

  openDialog(): void {
    const dialogRef = this.dialog.open(AddClassSubjectComponent, {
      width: '400px',
      data: { selectMany : true }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (!result) { return; }

      for (var i = 0; i < result.length; i++) {

        let subjectname = this.subjects1.filter(s => s.id == result[i])[0].subject;

        let es: examSchedule = {
          id: 0,
          batchid: 0,
          examid: this.exam.examid,
          classid: this.exam.classid,
          subjectName: subjectname,
          subjectid: (<any>result)[i],
          examdate: new Date(),
          starttime: null,
          endtime: null,
          userid: 0,
          dateCreated: null,
          totalmarks: null,
          passingmarks: null
        };
        this.tExamSchedule.push(es);
      }

      this.dataSource = new MatTableDataSource(this.tExamSchedule);

    });
  }

  GetClasses(): any {
    this.http.get(AppSettings.API_ENDPOINT + "api/classes/getclassesenabled", {
    }).subscribe(response => {
      this.classes = response;
    }, err => {
      console.log(err.status)
    });
  }

  GetSubjects(): any {
    this.http.get(AppSettings.API_ENDPOINT + "api/subjects/getsubjectsenabled", {
    }).subscribe(response => {
      this.subjects1 = response;
    }, err => {
      console.log(err.status)
    });
  }

  GetClassSubjects(): any {
    this.isLoading1 = true;
    this.http.get(AppSettings.API_ENDPOINT + "api/classsubjects/getbyclassid/" + this.exam.classid, {
    }).toPromise().then(response => {
      this.subjects = response;
      this.tExamSchedule = [];
      for (let sub of this.subjects) {
        let es: examSchedule = {
          id: 0,
          batchid: 0,
          examid: this.exam.examid,
          classid: this.exam.classid,
          subjectName: (<any>sub).subject.subject,
          subjectid: (<any>sub).subjectid,
          examdate: new Date(),
          starttime: null,
          endtime: null,
          userid: 0,
          dateCreated: null,
          totalmarks: null,
          passingmarks: null
        };
        this.tExamSchedule.push(es);
      }

      this.dataSource = new MatTableDataSource(this.tExamSchedule);
      this.isLoading1 = false;
    }, err => {
      console.log(err.status);
      this.isLoading1 = false;
    });
  }

  GetOne() {
    if (this.id > 0) {
      this.isLoading = true;
      this.isLoading1 = true;
      this.http.get(AppSettings.API_ENDPOINT + "api/exams/" + this.id, {
      }).toPromise().then(response => {
        this.exam = response;
        this.tExamSchedule = [];
        for (let examSch of this.exam.tExamSchedule) {
          let es: examSchedule = {
            id: examSch.id,
            batchid: examSch.batchid,
            examid: examSch.examid,
            classid: examSch.classid,
            subjectName: examSch.subject.subject,
            subjectid: examSch.subjectid,
            examdate: examSch.examdate,
            starttime: examSch.starttime,
            endtime: examSch.endtime,
            userid: examSch.userid,
            dateCreated: examSch.dateCreated,
            totalmarks: examSch.totalmarks,
            passingmarks: examSch.passingmarks
          };
          this.tExamSchedule.push(es);
        }
        this.dataSource = new MatTableDataSource(this.tExamSchedule);
        this.isLoading = false;
        this.isLoading1 = false;
        //console.log(this.dataSource);
      }, err => {
        console.log(err.status);
        this.isLoading = false;
        this.isLoading1 = false;
      });
    }
  }

  load() {
    //console.log(this.exam.classid);
    if (this.exam.classid != null) {
      this.GetClassSubjects();
    }
  }

  Save(form: NgForm) {
    if (form.invalid)
      return;

    this.exam.tExamSchedule = this.dataSource.data;
    //console.log(this.exam.tExamSchedule);

    if (this.id == 0) {

      this.http.post(AppSettings.API_ENDPOINT + "api/exams/", this.exam, {
      }).subscribe(response => {
        if ((<any>response).exists == true) {
          console.log((<any>response).exists);
          this.snackbar.open("Exam already exists for selected class!");
          return;
        }
        this.router.navigate(["exams"]);
      }, err => {
        console.log(err)
      });
    } else if (this.id > 0) {
      this.http.put(AppSettings.API_ENDPOINT + "api/exams/" + this.id, this.exam, {
      }).toPromise().then(response => {
        if ((<any>response).exists == true) {
          console.log((<any>response).exists);
          this.snackbar.open("Exam already exists for selected class!");
          return;
        }
        this.router.navigate(["exams"]);
      }, err => {
        console.log(err)
      });
    }
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
      this.tExamSchedule.splice(index, 1);
      this.dataSource = new MatTableDataSource(this.tExamSchedule);

    }
    else {
      this.http.delete(AppSettings.API_ENDPOINT + "api/examschedules/" + element.id, {
      }).subscribe(response => {
        if ((<any>response).result) {
          this.tExamSchedule.splice(index, 1);
          this.dataSource = new MatTableDataSource(this.tExamSchedule);
        } else {
          this.snackbar.open("Cant delete! Marksheet is already generated for this subject!");
          return;
        }

      }, err => {
        console.log(err.status)
      });
    }
  }

  Back() {
    this.location.back();
  }

}

