import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { ActivatedRoute, Router } from '@angular/router';
import { NgForm, FormGroup } from '@angular/forms';
import { AppSettings } from '../../app-settings';
import { CustomSnackbarService } from '../../shared/snackbar.service';
import { FadeAnimation } from '../../shared/animations';
import { Title } from '@angular/platform-browser';
import { AuthService } from '../../../app/app-auth/auth.service';
import { ConfirmDialog } from '../../../app/shared/confirm/confirm-dialog';
import { MatDialog } from '@angular/material';

export interface leaveapplication {
  staffid: number,
  leavetypeid: number,
  datefrom: Date,
  dateto: Date,
  description: string,
  status: string
}

@Component({
  selector: 'app-leaveapplication',
  templateUrl: './leaveapplication.component.html',
  styleUrls: ['./leaveapplication.component.css'],
  animations: [FadeAnimation]
})



export class LeaveApplicationComponent implements OnInit {
  isLoading: boolean = false; isLoading1: boolean = false; isLoading2: boolean = false;
  constructor(private router: Router, private http: HttpClient, private route: ActivatedRoute,
    private snackbar: CustomSnackbarService, private titleService: Title, private auth: AuthService, public dialog: MatDialog) { }

  displayedColumns: string[] = ['leavetype', 'count'];
  leavetypes: any = [];
  leavesreamining: any = [];
  breakpoint: number;
  data: leaveapplication = {} as leaveapplication;

  applications: any;
  displayedColumns1: string[] = ['leavetype', 'description',  'datefrom', 'dateto', 'approve', 'delete'];

  minFrom = new Date(new Date().getFullYear(), 0, 1);
  minTo = new Date(new Date().getFullYear(), 0, 1);
  maxBoth = new Date(new Date().getFullYear(), 11, 31);

  //more: boolean = true;
  ngOnInit() {
    this.titleService.setTitle("Leave Application");
    this.breakpoint = (window.innerWidth <= 700) ? 1 : 3;

    this.data.staffid = this.auth.getStaffId();

    if (this.data.staffid == 0) {
      this.router.navigate(["unauthorized"]);
    }

    this.getLeaveTypes();
    this.getLeaveRemaining();
    this.GetAll();
  }

  changeDate(event) {
    this.minTo = this.data.datefrom;
  }

  Reapply(element) {
    this.data.datefrom = element.datefrom;
    this.data.dateto = element.dateto;
    this.data.description = element.description;
    this.data.leavetypeid = element.leavetypeid;
    this.data.status = "Pending";

  }

  getLeaveRemaining() {
    this.isLoading1 = true;
    this.http.get(AppSettings.API_ENDPOINT + "api/leaves/getbyyear/" + this.data.staffid + "/" + new Date().getFullYear(), {
    }).toPromise().then(response => {
      this.leavesreamining = response;
      this.isLoading1 = false;
    }, err => {
      this.isLoading1 = false;
    });
  }

  getLeaveTypes() {
    this.isLoading = true;
    this.http.get(AppSettings.API_ENDPOINT + "api/leavetypes", {
    }).toPromise().then(response => {
      this.leavetypes = response;
      this.isLoading = false;
    }, err => {
      this.isLoading = false;
    });
  }

  Save(form: NgForm) {
    if (form.invalid)
      return;

    this.data.status = "Pending";


    let tempdata = { data: this.data, leavesRemaining: this.leavesreamining }

    this.http.post(AppSettings.API_ENDPOINT + "api/leaveapplications/", tempdata, {
    }).subscribe(response => {
      if (!(<any>response).result) {
        this.snackbar.open("Sorry! you can't apply for " + (<any>response).applied + " out off " + (<any>response).available + " available " + (<any>response).leavetype);
      } else {
        this.GetAll();
        this.data = { datefrom: null, dateto: null, description: null, leavetypeid: null, staffid: this.data.staffid, status: this.data.status };
      }
      }, err => {
      console.log(err)
    });
  }

  Back() {
    //this.router.navigate(["payheads"]);
  }

  onResize(event) {
    this.breakpoint = (event.target.innerWidth <= 700) ? 1 : 3;
  }

  GetAll() {
    this.isLoading2 = true;
    this.http.get(AppSettings.API_ENDPOINT + "api/leaveapplications/getbystaffid/" + this.data.staffid, {
    }).toPromise().then(response => {
      this.applications = response;
      this.isLoading2 = false;
    }, err => {
      console.log(err)
      this.isLoading2 = false;
    });
  }

  Delete(id): void {
    const dialogRef = this.dialog.open(ConfirmDialog, {
      width: '250px'
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.Delete1(id);
      }
    });
  }

  Delete1(id) {
    this.http.delete(AppSettings.API_ENDPOINT + "api/leaveapplications/" + id, {
    }).subscribe(response => {
      this.snackbar.open("Deleted");
      this.GetAll();
    }, err => {
      console.log(err.status)
    });
  }


}

