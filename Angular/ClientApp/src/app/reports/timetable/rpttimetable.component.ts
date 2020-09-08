import { Component, OnInit, Input, OnDestroy } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { PrintService } from '../print.service';
import { FadeAnimation } from '../../../app/shared/animations';
import { HttpClient } from '@angular/common/http';
import { AppSettings } from '../../../app/app-settings';
import { Title } from '@angular/platform-browser';

@Component({
  selector: 'app-rpttimetable',
  templateUrl: './rpttimetable.component.html',
  styleUrls: ['./rpttimetable.component.css'],
  animations: [FadeAnimation]
})
export class RptTimeTableComponent implements OnInit, OnDestroy {
  batchid: number;
  classid: number;
  students = [];
  batches: any = [];
  classes: any = [];
  timetabledata: any = [];
  isLoading: boolean = false;

  @Input() takeprint: boolean = false;
  displayedColumns: string[] = ['fromtime'];

  constructor(private http: HttpClient, route: ActivatedRoute, public printService: PrintService,
    private titleservice: Title) {
    let params = route.snapshot.params['id'];
    if (params != null) {
      this.classid = parseInt(route.snapshot.params['id'].split(',')[0]);
      this.batchid = route.snapshot.params['id'].split(',')[1];
    }
    this.takeprint = printService.takeprint;

  }

  ngOnDestroy() {
    this.printService.takeprint = false;

  }

  ngOnInit() {
    this.titleservice.setTitle("Time Table");
    
      this.GetClasses();
      this.GetBatches();
    
    if (this.classid != null && this.batchid != null) {
      this.load();
    }
  }

  setColumnsToDisplay() {

    this.displayedColumns = ['fromtime'];

    for (var i = 0; i < this.timetabledata.length; i++) {
      if (this.timetabledata[i].sunday.subject != null) {        
        this.displayedColumns.push('_sunday');
        break;
      }
    }
    for (var i = 0; i < this.timetabledata.length; i++) {
      if (this.timetabledata[i].monday.subject != null) {
        this.displayedColumns.push('_monday');
        break;
      }
    }
    for (var i = 0; i < this.timetabledata.length; i++) {
      if (this.timetabledata[i].tuesday.subject != null) {
        this.displayedColumns.push('_tuesday');
        break;
      }
    }
    for (var i = 0; i < this.timetabledata.length; i++) {
      if (this.timetabledata[i].wednesday.subject != null) {
        this.displayedColumns.push('_wednesday');
        break;
      }
    }
    for (var i = 0; i < this.timetabledata.length; i++) {
      if (this.timetabledata[i].thursday.subject != null) {
        this.displayedColumns.push('_thursday');
        break;
      }
    }
    for (var i = 0; i < this.timetabledata.length; i++) {
      if (this.timetabledata[i].friday.subject != null) {
        this.displayedColumns.push('_friday');
        break;
      }
    }
    for (var i = 0; i < this.timetabledata.length; i++) {
      if (this.timetabledata[i].saturday.subject != null) {
        this.displayedColumns.push('_saturday');
        break;
      }
    }
  }

  toTime(timeString) {
    var timeTokens = timeString.split(':');
    return new Date(1970, 0, 1, timeTokens[0], timeTokens[1], timeTokens[2]);
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

  load() {
    if (this.classid != null && this.batchid != null) {
      this.GetStudents();
    }
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

  GetStudents() {
    this.http.get(AppSettings.API_ENDPOINT + "api/timetables/getbyclassidreport/" + this.classid + "/" + this.batchid, {
    }).toPromise().then(response => {
      this.timetabledata = response;
      this.setColumnsToDisplay();
      if (this.takeprint) {
        this.printService.onDataReady();
      }
    }, err => {
      console.log(err)
      
    });
  }

  GetBatches() {
    this.isLoading = true;
    this.http.get(AppSettings.API_ENDPOINT + "api/batches", {
    }).toPromise().then(response => {
      this.batches = response;
      this.isLoading = false;
      let selected = this.batches.filter(o => o.isactive == true);
      //console.log(selected);
      this.batchid = selected[0].id;
    }, err => {
      console.log(err)
      this.isLoading = false;
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
