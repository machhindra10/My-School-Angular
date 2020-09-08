import { Component, OnInit, Input, OnDestroy } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { PrintService } from '../print.service';
import { FadeAnimation } from '../../../app/shared/animations';
import { HttpClient } from '@angular/common/http';
import { AppSettings } from '../../../app/app-settings';
import { Title } from '@angular/platform-browser';
import { AuthService } from '../../../app/app-auth/auth.service';

@Component({
  selector: 'app-attbymonth',
  templateUrl: './attbymonth.component.html',
  styleUrls: ['./attbymonth.component.css'],
  animations: [FadeAnimation]
})
export class RptAttendanceByMonthComponent implements OnInit, OnDestroy {
  batchid: number;
  classid: number;
  imonth = (new Date()).getMonth() + 1;
  year = (new Date()).getFullYear();
  daysinmonth: number;

  batches: any = [];
  students: any = [];
  tmonths: any = [];
  classes: any = [];
  isLoading: boolean = false;
  isLoading1: boolean = false;
  @Input() takeprint: boolean = false;
  displayedColumns: string[] = ['name', '_1', '_2', '_3', '_4', '_5', '_6', '_7', '_8', '_9', '_10',
    '_11', '_12', '_13', '_14', '_15', '_16', '_17', '_18', '_19', '_20', '_21',
    '_22', '_23', '_24', '_25', '_26', '_27', '_28', '_29', '_30', '_31'];

  constructor(private http: HttpClient, route: ActivatedRoute, public printService: PrintService,
    private titleService: Title, private auth: AuthService) {
    let params = route.snapshot.params['id'];
    if (params != null) {
      this.classid = route.snapshot.params['id'].split(',')[0];
      this.imonth = route.snapshot.params['id'].split(',')[1];
      this.batchid = route.snapshot.params['id'].split(',')[2];
    }
    this.takeprint = printService.takeprint;
  }
  ngOnDestroy() {
    this.printService.takeprint = false;

  }
  ngOnInit() {
    this.titleService.setTitle("Attendance By Month");
    
      this.GetClasses();
      this.GetMonths();
      this.GetBatches();
    
    this.load();
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

  getMonthName() {
    let name = "";
    if (this.tmonths.filter(c => c.id == this.imonth).length > 0) {
      name = this.tmonths.filter(c => c.id == this.imonth)[0].dyr;
    }
    return name;
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


  load() {
    if (this.classid != null && this.imonth != null && this.batchid != null) {
      this.isLoading1 = true;
      this.setColumnsToDisaplay(this.imonth, this.year);
      this.GetStudents();
    }

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

  GetStudents() {
    this.http.get(AppSettings.API_ENDPOINT + "api/studentattendence1/getstudents/" + this.classid + "/" + this.imonth + "/" + this.batchid, {
    }).toPromise().then(response => {
      this.students = response;
      this.isLoading1 = false;
      if (this.takeprint) {
        this.printService.onDataReady();
      }

    }, err => {
      console.log(err)
      this.isLoading1 = false;
    });
  }

  printDocument() {
    this.printService.title = "Attendance";
    this.printService.takeprint = true;
    this.printService.layout = 'landscape';
    const invoiceIds = [this.classid.toString(), this.imonth.toString(), this.batchid.toString()];
    this.printService.printDocument('attendancebymonth', invoiceIds);
  }


}
