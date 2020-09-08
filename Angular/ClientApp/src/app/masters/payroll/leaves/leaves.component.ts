import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { ActivatedRoute, Router } from '@angular/router';
import { NgForm, FormGroup } from '@angular/forms';
import { AppSettings } from '../../../../app/app-settings';
import { CustomSnackbarService } from '../../../../app/shared/snackbar.service';
import { FadeAnimation } from '../../../../app/shared/animations';
import { Title } from '@angular/platform-browser';
import { MatDialog } from '@angular/material';
import { AlertDialog } from '../../../../app/shared/alertmessage/alert-dialog';
import { ConfirmDialog } from '../../../../app/shared/confirm/confirm-dialog';
import factory from 'highcharts/modules/stock';
import { AddLeavesComponent } from './addleaves/addleaves.component';
import { AddHolidayComponent } from './addholiday/addholiday.component';

@Component({
  selector: 'app-leaves',
  templateUrl: './leaves.component.html',
  styleUrls: ['./leaves.component.css'],
  animations: [FadeAnimation]
})

export class LeavesComponent implements OnInit {
  isLoading: boolean = false;
  isLoading1: boolean = false;
  isLoading2: boolean = false;

  constructor(private router: Router, private http: HttpClient, private route: ActivatedRoute,
    private snackbar: CustomSnackbarService, private titleService: Title, public dialog: MatDialog) { }

  displayedColumns: string[] = ['leavetype', 'leaves', 'dummy', 'delete'];
  displayedColumns1: string[] = ['holiday', 'dates', 'dummy', 'delete'];


  listleaves: any = [];
  leavetypes: any = [];
  years: any = [];

  year: number = (new Date()).getFullYear();
  currentYear: number = (new Date()).getFullYear();

  leavetypeid: number;
  leaves: number;

  addnew: boolean = false;

  listholidays: any = [];
  totalLeaves: number = 0;

  ngOnInit() {

    this.titleService.setTitle("Leaves");
    this.GetYears();
    
    this.load();
  }  

  load() {
    this.getLeaves();
    this.getHolidays();
  }

  getTotal() {
    this.totalLeaves = this.getTotalHolidays() + this.getTotalLeaves();
  }

  getTotalLeaves() {
    return this.listleaves.map(t => t.leaves).reduce((acc, value) => acc + value, 0);
  }

  getTotalHolidays() {
    return this.listholidays.length;
  }

  getLeaves() {
    this.isLoading1 = true;
    this.http.get(AppSettings.API_ENDPOINT + "api/leaves/getbyyear/" + this.year, {
    }).toPromise().then(response => {
      this.listleaves = response;
      this.isLoading1 = false;
      this.getTotal();
    }, err => {
      this.isLoading1 = false;
    });    

    if (this.year == this.currentYear) {
      this.displayedColumns = ['leavetype', 'leaves', 'dummy', 'delete'];
    } else {
      this.displayedColumns = ['leavetype', 'leaves', 'dummy'];
    }
  }

  getHolidays() {
    this.isLoading2 = true;
    this.http.get(AppSettings.API_ENDPOINT + "api/holidays/getbyyear/" + this.year, {
    }).toPromise().then(response => {
      this.listholidays = response;
      this.isLoading2 = false;
      this.getTotal();
    }, err => {
      this.isLoading2 = false;
    });    

    if (this.year == this.currentYear) {
      this.displayedColumns1 = ['holiday', 'dates', 'dummy', 'delete'];
    } else {
      this.displayedColumns1 = ['holiday', 'dates', 'dummy'];
    }
  }

  AddNew(): void {
    const dialogRef = this.dialog.open(AddLeavesComponent, {
      width: '400px',
      data: { selectMany: false, leavetypeid: null, params: null }
    });

    dialogRef.afterClosed().subscribe(result => {      
      if (result.leavetypeids != null && result.leaves != null) {
        
        if (this.year > 0) {
          let data = { leavetypeid: result.leavetypeids, leaves: result.leaves, year: this.year };
          this.http.post(AppSettings.API_ENDPOINT + "api/leaves/", data, {
          }).subscribe(response => {
            //this.snackbar.open("Leaves Added successfully!")
            this.getLeaves();

          }, err => {
            console.log(err)
          });
        }
      }
    });
  }

  AddNewHoliday(): void {
    const dialogRef = this.dialog.open(AddHolidayComponent, {
      width: '400px',
      data: { selectMany: false, params: null }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result.holiday != null && result.dates != null) {

        if (this.year > 0) {
          let data = { holiday: result.holiday, dates: result.dates, year: this.year };
          this.http.post(AppSettings.API_ENDPOINT + "api/holidays/", data, {
          }).subscribe(response => {
            this.getHolidays();
          }, err => {
            console.log(err)
          });
        }
      }
    });
  }

  Delete(id): void {
    const dialogRef = this.dialog.open(ConfirmDialog, {
      width: '250px'
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.DeleteUser(id);
      }
    });
  }

  DeleteUser(id) {
    this.http.delete(AppSettings.API_ENDPOINT + "api/leaves/" + id, {
    }).subscribe(response => {
      this.snackbar.open("Deleted");
      this.getLeaves();
    }, err => {
      console.log(err.status);
    });
  }

  DeleteHoliday(id): void {
    const dialogRef = this.dialog.open(ConfirmDialog, {
      width: '250px'
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.DeleteHoliday1(id);
      }
    });
  }

  DeleteHoliday1(id) {
    this.http.delete(AppSettings.API_ENDPOINT + "api/holidays/" + id, {
    }).subscribe(response => {
      this.snackbar.open("Deleted");
      this.getHolidays();
    }, err => {
      console.log(err.status);
    });
  }

  GetYears() {
    this.isLoading = true;
    this.http.get(AppSettings.API_ENDPOINT + "api/settings/getyearonly", {
    }).toPromise().then(response => {
      this.years = response;
      this.isLoading = false;
    }, err => {
      console.log(err)
      this.isLoading = false;
    });
  }

  Back() {
    //this.router.navigate(["batches"]);
  }



}

