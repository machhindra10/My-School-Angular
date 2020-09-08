import { Component, OnInit, Input, OnDestroy, AfterViewInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { PrintService } from '../print.service';
import { FadeAnimation } from '../../../app/shared/animations';
import { HttpClient } from '@angular/common/http';
import { AppSettings } from '../../../app/app-settings';
import { Title } from '@angular/platform-browser';
import { AuthService } from '../../../app/app-auth/auth.service';
import { parse } from 'url';

interface Age {
  iyear: number;
  year: string
}

@Component({
  selector: 'app-collections',
  templateUrl: './collections.component.html',
  styleUrls: ['./collections.component.css'],
  animations: [FadeAnimation]
})
export class RptCollectionsComponent implements OnInit, OnDestroy {
  currencycode = this.auth.getCurrencyCode();
  batchid: number;
  classid: number;
  month: number;
  year: number = (new Date()).getFullYear();

  batches: any = [];
  students: any = [];
  imonths: any = [];
  iyears: any = [];
  classes: any = [];
  isLoading: boolean = false;
  isLoading1: boolean = false;
  @Input() takeprint: boolean = false;
  displayedColumns: string[] = ['srno', 'prnno', 'name', 'date', 'amount'];

  constructor(private http: HttpClient, private route: ActivatedRoute, public printService: PrintService,
    private titleService: Title, private auth: AuthService) {
    
    this.takeprint = printService.takeprint;
    let params = this.route.snapshot.params['id'];
    if (params != null) {
      this.classid = parseInt(this.route.snapshot.params['id'].split(',')[0]);
      this.batchid = parseInt(this.route.snapshot.params['id'].split(',')[1]);
      this.month = parseInt(this.route.snapshot.params['id'].split(',')[2]);
      this.year = parseInt(this.route.snapshot.params['id'].split(',')[3]);      
    }
  }

  ngOnDestroy() {
    this.printService.takeprint = false;
  }

  ngOnInit() {
    this.titleService.setTitle("Collection report");

    this.getMonths();
    this.getYears();      
    this.GetClasses();
    this.GetBatches();    

    this.load();
  }

  getTotalCost() {
    return this.students.map(t => t.amount).reduce((acc, value) => acc + value, 0);
  }

  load() {
    if (this.classid != null &&
      this.month != null && this.month.toString() != NaN.toString() &&
      this.year != null && this.year.toString() != NaN.toString()  &&
      this.batchid != null && this.batchid.toString() != NaN.toString()) {
      this.isLoading1 = true;
      this.GetStudents();
    }

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
    if (this.imonths.filter(c => c.id == this.month).length > 0) {
      name = this.imonths.filter(c => c.id == this.month)[0].name;
    }
    return name;
  }

  getYearName() {
    let name = "";
    if (this.iyears.filter(c => c.id == this.year).length > 0) {
      name = this.iyears.filter(c => c.id == this.year)[0].name;
    }
    return name;
  }

  GetBatches() {
    this.isLoading = true;
    this.http.get(AppSettings.API_ENDPOINT + "api/batches", {
    }).toPromise().then(response => {
      this.batches = response;
      this.isLoading = false;
      if (this.batchid == null || this.batchid.toString() == NaN.toString()) {
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
      if (this.classid == null) {
        this.classid = 0;
      }
    }, err => {
      console.log(err)
      this.isLoading = false;
    });

  }

  getMonths() {
    this.isLoading = true;
    this.http.get(AppSettings.API_ENDPOINT + "api/settings/getmonths", {
    }).toPromise().then(response => {
      this.imonths = response;
      this.isLoading = false;
    }, err => {
      console.log(err)
      this.isLoading = false;
    });
  }

  getYears() {
    this.isLoading = true;
    this.http.get(AppSettings.API_ENDPOINT + "api/settings/getyearonly", {
    }).toPromise().then(response => {
      this.iyears = response;
      this.isLoading = false;
      if (this.year == null || this.year.toString() == NaN.toString()) {
        this.year = new Date().getFullYear();
      }
    }, err => {
      console.log(err)
      this.isLoading = false;
    });
  }

  GetStudents() {
    this.http.get(AppSettings.API_ENDPOINT + "api/studentpayments/getmonthlyreport/" + this.classid + "/" + this.month + "/" + this.year + "/" + this.batchid, {
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
    this.printService.title = "Collection Report";
    this.printService.takeprint = true;
    this.printService.layout = 'portrait';
    const invoiceIds = [this.classid.toString(), this.batchid.toString(), this.month.toString(), this.year.toString()];
    this.printService.printDocument('collectionsreport', invoiceIds);
  }

}
