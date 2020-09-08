import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Router } from '@angular/router';
import { AppSettings } from '../../app-settings';
import { CustomSnackbarService } from '../../shared/snackbar.service';
import { LoaderService } from '../../shared/loader.service';
import { FadeAnimation } from '../../shared/animations';
import { Title } from '@angular/platform-browser';
import { MatDialog } from '@angular/material';
import { ConfirmDialog } from '../../../app/shared/confirm/confirm-dialog';
import { AddStudentFeeComponent } from '../addstudentfee/addstudentfee.component';
import { forEach } from '@angular/router/src/utils/collection';
import { AuthService } from '../../../app/app-auth/auth.service';


@Component({
  selector: 'app-studentfees',
  templateUrl: './studentfees.component.html',
  styleUrls: ['./studentfees.component.css'],
  animations: [FadeAnimation]
})

export class StudentFeesComponent implements OnInit {

  isLoading: boolean = false;
  constructor(private http: HttpClient, private router: Router, private snackbar: CustomSnackbarService,
    private loaderService: LoaderService, private titleService: Title, public dialog: MatDialog,
    private auth: AuthService) { }

  @Output() afterUpdate = new EventEmitter<boolean>();
  currencycode = this.auth.getCurrencyCode();
  @Input() studentid = 0;
  @Input() allowdelete: boolean = false;

  studentfees1: any[] = [];

  displayedColumns1: string[] = ['feestype', 'amount', 'delete'];
  ngOnInit() {
    //this.titleService.setTitle('Class Fees');
    if (this.allowdelete) {
      this.displayedColumns1 = ['feestype', 'amount', 'delete'];
    }
    else {
      this.displayedColumns1 = ['feestype', 'amount'];
    }
    this.GetOne();
  }

  openDialog(): void {
    const dialogRef = this.dialog.open(AddStudentFeeComponent, {
      width: '400px',
      data: { studentid: this.studentid, feesType: "", amount: 0 }
    });

    dialogRef.afterClosed().toPromise().then(result => {
      if (result) {
        this.GetOne();
      }
    });
  }

  GetOne() {
    this.isLoading = true;
    if (this.studentid > 0) {
      this.http.get(AppSettings.API_ENDPOINT + "api/studentfees/getbystudentid/" + this.studentid, {
      }).toPromise().then(response => {
        this.studentfees1 = (<any>response);
        this.isLoading = false;
        this.afterUpdate.emit(true); 
      }, err => {
        console.log(err.status);
        this.isLoading = false;
      });
    }
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
    this.http.delete(AppSettings.API_ENDPOINT + "api/studentfees/" + id, {
    }).subscribe(response => {
      this.snackbar.open("Deleted");
      this.GetOne();
    }, err => {
      console.log(err.status)
    });
  }

  getTotalCost() {
    return this.studentfees1.map(t => t.amount).reduce((acc, value) => acc + value, 0);
  }

  UpdateFromClassFeesTemplete() {
    this.http.get(AppSettings.API_ENDPOINT + "api/studentfees/updatefromclasstemplete/" + this.studentid, {
    }).toPromise().then(response => {
      if ((<any>response).result) {
        this.GetOne();
        this.snackbar.open("Updated!");
      }
      else {
        this.snackbar.open("Fees already updated! nothing to update");
      }
      this.isLoading = false;
    }, err => {
      console.log(err.status);
      this.isLoading = false;
    });
  }
}
