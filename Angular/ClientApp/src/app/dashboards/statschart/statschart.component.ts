import { Component, OnInit, Input, OnChanges, ChangeDetectionStrategy, SimpleChanges } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Router } from '@angular/router';
import { AppSettings } from '../../app-settings';
import { CustomSnackbarService } from '../../shared/snackbar.service';
import { LoaderService } from '../../shared/loader.service';
import { FadeAnimation } from '../../shared/animations';
import { Title } from '@angular/platform-browser';
import { MatDialog } from '@angular/material';
import { Chart } from 'angular-highcharts';
import { AuthService } from '../../../app/app-auth/auth.service';


@Component({
  selector: 'app-statschart',
  templateUrl: './statschart.component.html',
  styleUrls: ['./statschart.component.css'],
  animations: [FadeAnimation],

})

export class StatsChartComponent implements OnInit, OnChanges {

  isLoading: boolean = false;
  constructor(private http: HttpClient, private router: Router, private snackbar: CustomSnackbarService,
    private loaderService: LoaderService, private titleService: Title, public dialog: MatDialog,
    private auth: AuthService) { }

  @Input() studentid = 0;
  @Input() refresh = false;
  currencycode = this.auth.getCurrencyCode();
  statsdata: any[] = [];
  displayedColumns1: string[] = ['month', 'total'];
  chart: Chart;

  init() {
    let chart = new Chart({
      chart: {
        type: 'line'
      },
      title: {
        text: ''
      },
      credits: {
        enabled: false
      },
      xAxis: {
        type: 'category',

      },
      yAxis: {
        title: {
          text: 'Currency'
        }
      },
      tooltip: {
        valuePrefix: ''
      },
      plotOptions: {
        line: {
          dataLabels: {
            enabled: true,

          },
          enableMouseTracking: true
        }
      },
      series: [{
        type: 'line',
        name: 'Collections',
        data: (<any>this.statsdata).collections,
        color: '#28b779',

      }, {
        type: 'line',
        name: 'Salary',
        data: (<any>this.statsdata).salaries,
        color: '#ffb848'
      }, {
        type: 'line',
        name: 'Expenses',
        data: (<any>this.statsdata).expenses,
        color: '#F3867B'
      }]
    });
    this.chart = chart;
  }

  ngOnInit() {

    this.GetOne();
    //this.init();
    //console.log(new Date().getTimezoneOffset());
  }

  ngOnChanges(changes: SimpleChanges): void {
    this.GetOne();
  }

  GetOne() {

    this.isLoading = true;
    this.http.get(AppSettings.API_ENDPOINT + "api/dashbaord/getadmindashboardstatsdata/1", {
    }).toPromise().then(response => {
      this.statsdata = (<any>response);
      this.isLoading = false;
      this.init();
    }, err => {
      console.log(err.status);
      this.isLoading = false;
    });
  }


}
