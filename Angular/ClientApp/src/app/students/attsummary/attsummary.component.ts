import { Component, OnInit, Input, OnChanges, ChangeDetectionStrategy, SimpleChanges } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Router } from '@angular/router';
import { AppSettings } from '../../app-settings';
import { CustomSnackbarService } from '../../shared/snackbar.service';
import { LoaderService } from '../../shared/loader.service';
import { FadeAnimation } from '../../shared/animations';
import { Title } from '@angular/platform-browser';
import { MatDialog } from '@angular/material';
import { ConfirmDialog } from '../../../app/shared/confirm/confirm-dialog';


@Component({
  selector: 'app-attsummary',
  templateUrl: './attsummary.component.html',
  styleUrls: ['./attsummary.component.css'],
  animations: [FadeAnimation],
  
})

export class AttendanceSummaryComponent implements OnInit, OnChanges {  

  isLoading: boolean = false;
  constructor(private http: HttpClient, private router: Router, private snackbar: CustomSnackbarService,
    private loaderService: LoaderService, private titleService: Title, public dialog: MatDialog) { }

  @Input() studentid = 0;
  @Input() refresh = false;

  attendancesummary: any[] = [];
  displayedColumns1: string[] = ['month',  'total'];

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
      }, err => {
        console.log(err.status);
        this.isLoading = false;
      });
    }
  }


}
