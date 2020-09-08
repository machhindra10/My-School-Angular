import { Component, OnInit, Input } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Router } from '@angular/router';
import { AppSettings } from '../../../app-settings';
import { CustomSnackbarService } from '../../../shared/snackbar.service';
import { LoaderService } from '../../../shared/loader.service';
import { FadeAnimation } from '../../../shared/animations';
import { Title } from '@angular/platform-browser';
import { MatDialog } from '@angular/material';
import { AddClassSubjectComponent } from '../addclasssubject/addclasssubject.component';
import { ConfirmDialog } from '../../../../app/shared/confirm/confirm-dialog';
import { AddClassFeeComponent } from '../addclassfee/addclassfee.component';
import { AuthService } from '../../../../app/app-auth/auth.service';


@Component({
  selector: 'app-classfees',
  templateUrl: './classfees.component.html',
  styleUrls: ['./classfees.component.css'],
  animations: [FadeAnimation]
})

export class ClassFeesComponent implements OnInit {

  isLoading: boolean = false;
  constructor(private http: HttpClient, private router: Router, private snackbar: CustomSnackbarService,
    private loaderService: LoaderService, private titleService: Title, public dialog: MatDialog,
    private auth: AuthService) { }
  currencycode = this.auth.getCurrencyCode();
  @Input() classid = 0;

  classfees1;
  displayedColumns1: string[] = ['feestype', 'amount', 'delete'];
  ngOnInit() {
    //this.titleService.setTitle('Class Fees');
    this.GetOne();
  }

  openDialog(): void {
    const dialogRef = this.dialog.open(AddClassFeeComponent, {
      width: '400px',
      data: { classid: this.classid, feesType: "", amount: 0 }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.GetOne();
      }
    });
  }

  GetOne() {
    this.isLoading = true;
    if (this.classid > 0) {
      this.http.get(AppSettings.API_ENDPOINT + "api/classfees/getbyclassid/" + this.classid, {
      }).subscribe(response => {
        this.classfees1 = response;
        this.isLoading = false;
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
    this.http.delete(AppSettings.API_ENDPOINT + "api/classfees/" + id, {
    }).subscribe(response => {
      this.snackbar.open("Deleted");
      this.GetOne();
    }, err => {
      console.log(err.status)
    });
  }
}
