import { Component, OnInit, Input, OnDestroy } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { PrintService } from '../print.service';
import { FadeAnimation } from '../../../app/shared/animations';
import { HttpClient } from '@angular/common/http';
import { AppSettings } from '../../../app/app-settings';
import { Title } from '@angular/platform-browser';
import { AuthService } from '../../../app/app-auth/auth.service';

interface Age {
  iyear: number;
  year: string
}

@Component({
  selector: 'app-marksheets',
  templateUrl: './marksheets.component.html',
  styleUrls: ['./marksheets.component.css'],
  animations: [FadeAnimation]
})
export class RptMarksheetsComponent implements OnInit, OnDestroy {

  batchid: number;
  classid: number;
  examid: any;
  studentids: any = [];

  batches: any = [];
  students: any = [];
  exams: any = [];
  classes: any = [];
  studentMarksheets: any = [];


  isLoading: boolean = false;
  isLoading1: boolean = false;
  @Input() takeprint: boolean = false;
  displayedColumns: string[] = ['srno', 'code', 'subject', 'total', 'obtained', 'practical', 'totalobtained'];

  constructor(private http: HttpClient, route: ActivatedRoute, public printService: PrintService,
    private titleService: Title, private auth: AuthService) {
    let params = route.snapshot.params['id'];
    if (params != null) {
      this.batchid = route.snapshot.params['id'].split(',')[0];
      this.classid = route.snapshot.params['id'].split(',')[1];
      this.examid = route.snapshot.params['id'].split(',')[2];

      let paramsA: any = route.snapshot.params['id'].split(',');
      paramsA.splice(0, 3); //it removes first three items from array hence remaining items are studentIds
      this.studentids = paramsA;
    }
    this.takeprint = printService.takeprint;
  }

  ngOnDestroy() {
    this.printService.takeprint = false;

  }

  ngOnInit() {
    this.titleService.setTitle("Student Marksheets");

    this.GetClasses();
    this.GetBatches();
    this.generateMarksheet();
  }

  generateMarksheet() {

    if (this.classid != null && this.examid != null && this.batchid != null && this.studentids != null) {
      this.isLoading1 = true;
      this.GetStudentsMarksheet();
    }
  }

  getStudentName(studentid) {
    let name = "";
    if (this.students.filter(c => c.id == studentid).length > 0) {
      name = this.students.filter(c => c.id == studentid)[0].name;
    }
    return name;
  }

  getClassName() {
    let classname = "All";
    if (this.classes.filter(c => c.id == this.classid).length > 0) {
      classname = this.classes.filter(c => c.id == this.classid)[0].standard;
    }
    return classname;
  }

  getBatchName() {
    let batchname = "";
    if (this.batches.filter(c => c.id == this.batchid).length > 0) {
      batchname = this.batches.filter(c => c.id == this.batchid)[0].batch;
    }
    return batchname;
  }

  GetBatches() {
    this.isLoading = true;
    this.http.get(AppSettings.API_ENDPOINT + "api/batches", {
    }).toPromise().then(response => {
      this.batches = response;
      this.isLoading = false;
      if (this.batchid == null) {
        let selected = this.batches.filter(o => o.isactive == true);
        this.batchid = selected[0].id;
      }
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

  getOthers() {

    this.studentids = [];
    this.examid = null;
    this.studentMarksheets = [];

    this.getExams();
    this.getStudents();
  }

  reset() {
    this.studentMarksheets = [];
  }

  getExams() {
    //this.isLoading = true;
    this.http.get(AppSettings.API_ENDPOINT + "api/exams/getbyclassid/" + this.classid, {
    }).toPromise().then(response => {
      this.exams = response;
      this.isLoading = false;
    }, err => {
      console.log(err)
      this.isLoading = false;
    });
  }

  getStudents() {
    //this.isLoading = true;
    this.http.get(AppSettings.API_ENDPOINT + "api/students/getbyclassid/" + this.classid + "/" + this.batchid, {
    }).toPromise().then(response => {
      this.students = response;
      this.isLoading = false;
    }, err => {
      console.log(err)
      this.isLoading = false;
    });
  }

  GetStudentsMarksheet() {
    this.http.get(AppSettings.API_ENDPOINT + "api/exammarksheets/getmarksheetreport/" + this.batchid + "/" + this.classid + "/" + this.examid + "/" + this.studentids, {
    }).toPromise().then(response => {
      this.studentMarksheets = response;
      this.isLoading1 = false;
      if (this.takeprint) {
        this.printService.onDataReady();
      }

    }, err => {
      console.log(err)
      this.isLoading1 = false;
    });
  }

  getTotalMarks(element) {
    return element.map(t => t.exmsch.totalmarks).reduce((acc, value) => acc + value, 0);
  }

  getTotalTheory(element) {
    return element.map(t => t.obtained).reduce((acc, value) => acc + value, 0);
  }
  getTotalPractical(element) {
    return element.map(t => t.practical).reduce((acc, value) => acc + value, 0);
  }
  getTotalObtained(element) {
    return element.map(t => t.totalmarks).reduce((acc, value) => acc + value, 0);
  }

  printDocument() {
    this.printService.title = "Student Marksheet";
    this.printService.takeprint = true;
    this.printService.layout = 'portrait';
    const invoiceIds = [this.batchid.toString(), this.classid.toString(), this.examid.toString(), this.studentids];
    this.printService.printDocument('marksheets', invoiceIds);
  }

}
