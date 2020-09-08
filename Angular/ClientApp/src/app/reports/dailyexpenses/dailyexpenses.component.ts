import { Component, OnInit, Input, OnDestroy, Output, EventEmitter } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { PrintService } from '../print.service';
import { FadeAnimation } from '../../../app/shared/animations';
import { HttpClient } from '@angular/common/http';
import { AppSettings } from '../../../app/app-settings';
import { Title } from '@angular/platform-browser';
import { load } from '@angular/core/src/render3';
import { MatDatepickerInputEvent } from '@angular/material';
import { AuthService } from '../../../app/app-auth/auth.service';

@Component({
  selector: 'app-dailyexpenses',
  templateUrl: './dailyexpenses.component.html',
  styleUrls: ['./dailyexpenses.component.css'],
  animations: [FadeAnimation]
})
export class RptDailyExpensesComponent implements OnInit, OnDestroy {
  fromdate: Date;
  todate: Date;
  expenses = [];

  isLoading: boolean = false;

  @Input() takeprint: boolean = false;
  displayedColumns: string[] = ['srno', 'details', 'receiptno', 'datecreated', 'amount'];

  constructor(private http: HttpClient, route: ActivatedRoute, public printService: PrintService,
    private titleservice: Title, private auth: AuthService) {
    let params = route.snapshot.params['id'];
    if (params != null) {
      this.fromdate = new Date(route.snapshot.params['id'].split(',')[0]);
      this.todate = new Date(route.snapshot.params['id'].split(',')[1]);

      console.log(this.fromdate.toString());
      console.log(this.todate.toString());
    }
    this.takeprint = printService.takeprint;
  }

  currencycode = this.auth.getCurrencyCode();
  @Output() dateChange: EventEmitter<MatDatepickerInputEvent<any>>;

  ngOnDestroy() {
    this.printService.takeprint = false;

  }
  ngOnInit() {
    this.titleservice.setTitle("Daily Expense Report");

    if (this.fromdate == null || this.fromdate.toString() == "Invalid Date" &&
      this.todate == null || this.todate.toString() == "Invalid Date") {
      this.fromdate = new Date(new Date().getFullYear(), new Date().getMonth(), 1);
      this.todate = new Date(new Date().getFullYear(), new Date().getMonth() + 1, 0);
    }
    this.load();
  }

  load() {
    if (this.fromdate == null || this.fromdate.toString() == "Invalid Date" &&
      this.todate == null || this.todate.toString() == "Invalid Date") {
      return;
    }
    this.getReport();
  }

  getTotalCost() {
    return this.expenses.map(t => t.amount).reduce((acc, value) => acc + value, 0);
  }

  getReport() {
    let params = { fromdate: this.fromdate, todate: this.todate };

    this.http.post(AppSettings.API_ENDPOINT + "api/dailyexpenses/getreport/", params, {
    }).toPromise().then(response => {
      this.expenses = (<any>response);
      if (this.takeprint) {
        this.printService.onDataReady();
      }
    }, err => {
      console.log(err)
    });

  }

  printDocument() {
    this.printService.title = "Daily Expense Report";
    this.printService.takeprint = true;
    this.printService.layout = 'portrait';
    const invoiceIds = [this.fromdate.toDateString(), this.todate.toDateString()];
    this.printService.printDocument('dailyexpensereport', invoiceIds);
  }


}
