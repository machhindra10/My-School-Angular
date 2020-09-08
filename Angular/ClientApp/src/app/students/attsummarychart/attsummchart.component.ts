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

@Component({
  selector: 'app-attsummchart',
  templateUrl: './attsummchart.component.html',
  styleUrls: ['./attsummchart.component.css'],
  animations: [FadeAnimation],
  
})

export class AttendanceSummaryChartComponent implements OnInit, OnChanges {  

  isLoading: boolean = false;
  constructor(private http: HttpClient, private router: Router, private snackbar: CustomSnackbarService,
    private loaderService: LoaderService, private titleService: Title, public dialog: MatDialog) { }

  @Input() studentid = 0;
  @Input() refresh = false;

  attendancesummary: any[] = [];
  displayedColumns1: string[] = ['month',  'total'];
  chart: Chart;

  init() {
    let chart = new Chart({
      chart: {
        type: 'column'
      },
      title: {
        text: ''
      },
      credits: {
        enabled: false
      },
      xAxis: {
        type: 'category',
        labels: {
          rotation: -45,
          style: {
            fontSize: '13px',
            fontFamily: 'Verdana, sans-serif'
          }
        }
      },
      yAxis: {
        min: 0,
        max: 31,        
        title: {
          text: 'Days'
        }
      },
      legend: {
        enabled: true
      },
      plotOptions: {
        column: {
          grouping: false,
          shadow: false,
          borderWidth: 0
        }
      },
      series: [{
        type: 'column',
        name: 'present',
        //color: 'green',
        colorByPoint: true,
        data: (<any>this.attendancesummary).present,
        dataLabels: {
          enabled: true,
          rotation: -90,
          color: '#FFFFFF',
          align: 'right',
          format: '{point.y}', // one decimal
          y: 10, // 10 pixels down from the top
          style: {
            fontSize: '13px',
            fontFamily: 'Verdana, sans-serif'
          }
        }
      },
        {
          type: 'column',
          name: 'leaves',          
          color:'red',
          //colorByPoint: true,
          pointPadding: 0.3,
          //pointPlacement: 0.2,          
          data: (<any>this.attendancesummary).leaves,
          dataLabels: {
            enabled: true,
            //rotation: -90,
            color: '#FFFFFF',
            //align: 'right',
            format: '{point.y}', // one decimal
            //y: 10, // 10 pixels down from the top
            style: {
              fontSize: '13px',
              fontFamily: 'Verdana, sans-serif'
            }
          }
        }]
    });
    //chart.addPoint(4);
    this.chart = chart;
    //chart.addPoint(5);
    //setTimeout(() => {
    //  chart.addPoint(7);
    //}, 2000);

    //chart.ref$.subscribe(console.log);
  }
  
  ngOnInit() {
    //this.titleService.setTitle('Student Payments Summary');
    
    this.GetOne();

  }

  ngOnChanges(changes: SimpleChanges): void {
    this.GetOne();    
  }

  GetOne() {    
    if (this.studentid > 0) {
      this.isLoading = true;
      this.http.get(AppSettings.API_ENDPOINT + "api/studentattendence1/getbystudentid/" + this.studentid, {
      }).toPromise().then(response => {
        this.attendancesummary = (<any>response);
        this.isLoading = false;
        this.init();
      }, err => {
        console.log(err.status);
        this.isLoading = false;
      });
    }
  }


}
