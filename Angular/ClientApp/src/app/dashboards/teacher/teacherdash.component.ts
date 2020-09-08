import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute, Router } from '@angular/router';
import { NgForm } from '@angular/forms';
import { AppSettings } from '../../app-settings';
import { CustomSnackbarService } from '../../shared/snackbar.service';
import { FadeAnimation } from '../../shared/animations';
import { Title } from '@angular/platform-browser';
import { AuthService } from '../../../app/app-auth/auth.service';
import { retry } from 'rxjs/operators';
import { forEach } from '@angular/router/src/utils/collection';

@Component({
  selector: 'app-teacherdash',
  templateUrl: './teacherdash.component.html',
  styleUrls: ['./teacherdash.component.css'],
  animations: [FadeAnimation]
})

export class TeacherDashboardComponent implements OnInit {
  isLoading: boolean = false;
  isLoading1: boolean = false;
  constructor(private router: Router, private http: HttpClient, private route: ActivatedRoute,
    private snackbar: CustomSnackbarService, private titleService: Title, private auth: AuthService) { }

  staffid = this.auth.getStaffId();
  currencycode = this.auth.getCurrencyCode();
  studentid: number = 0;
  dataCount: any;
  timetabledata: any = [];
  subjects: any = [];
  //displayedColumns: string[] = ['fromtime', '_sunday', '_monday', '_tuesday', '_wednesday', '_thursday', '_friday', '_saturday'];
  displayedColumns: string[] = ['fromtime'];


  ngOnInit() {
    this.titleService.setTitle("Teacher's Dashboard");
    //this.getSubjects();
    //this.loadDashboardData();
    this.LoadTimeTable();
  }

  toTime(timeString) {
    var timeTokens = timeString.split(':');
    return new Date(1970, 0, 1, timeTokens[0], timeTokens[1], timeTokens[2]);
  }

  //getSubjectName(subjectid) {
  //  if (this.subjects.filter(o => o.id == subjectid).length > 0) {
  //    return this.subjects.filter(o => o.id == subjectid)[0].subject;
  //  } else {
  //    return '';
  //  }
  //}

  LoadTimeTable() {
    
    if (this.staffid == 0) {
      //console.log("not a teacher");
      return;
    }
    this.isLoading1 = true;
    this.http.get(AppSettings.API_ENDPOINT + "api/timetables/getteacherstimetable/" + this.staffid, {
    }).toPromise().then(response => {
      this.timetabledata = response;
      this.isLoading1 = false;
      this.setColumnsToDisplay();
    }, err => {
      this.isLoading1 = false;
      console.log(err.status);
    });
  }

  setColumnsToDisplay() {
    
    for (var i = 0; i < this.timetabledata.length; i++) {
      if (this.timetabledata[i].sunday != null) {
        this.displayedColumns.push('_sunday');
        break;
      }
    }
    for (var i = 0; i < this.timetabledata.length; i++) {
      if (this.timetabledata[i].monday != null) {
        this.displayedColumns.push('_monday');
        break;
      }
    }
    for (var i = 0; i < this.timetabledata.length; i++) {
      if (this.timetabledata[i].tuesday != null) {
        this.displayedColumns.push('_tuesday');
        break;
      }
    }
    for (var i = 0; i < this.timetabledata.length; i++) {
      if (this.timetabledata[i].wednesday != null) {
        this.displayedColumns.push('_wednesday');
        break;
      }
    }
    for (var i = 0; i < this.timetabledata.length; i++) {
      if (this.timetabledata[i].thursday != null) {
        this.displayedColumns.push('_thursday');
        break;
      }
    }
    for (var i = 0; i < this.timetabledata.length; i++) {
      if (this.timetabledata[i].friday != null) {
        this.displayedColumns.push('_friday');
        break;
      }
    }
    for (var i = 0; i < this.timetabledata.length; i++) {
      if (this.timetabledata[i].saturday != null) {
        this.displayedColumns.push('_saturday');
        break;
      }
    }    
  }

  loadDashboardData() {
    this.isLoading = true;
    this.http.get(AppSettings.API_ENDPOINT + "api/dashbaord/getadmindashboarddata/2", {
    }).toPromise().then(response => {
      this.dataCount = response;
      this.isLoading = false;
    }, err => {
      this.isLoading = false;
      console.log(err.status);
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

  onAfterSelect(id: number) {
    this.studentid = id;
  }

  selectAnotherStudent() {
    this.studentid = 0;
  }

  Back() {
    this.router.navigate(["students"]);
  }

}

