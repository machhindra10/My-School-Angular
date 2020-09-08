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
import { PrintService } from '../../../app/reports/print.service';
import { AuthService } from '../../../app/app-auth/auth.service';


@Component({
  selector: 'app-studentpayments',
  templateUrl: './studentpayments.component.html',
  styleUrls: ['./studentpayments.component.css'],
  animations: [FadeAnimation],

})

export class StudentPaymentsComponent implements OnInit, OnChanges {

  isLoading: boolean = false;
  constructor(private http: HttpClient, private router: Router, private snackbar: CustomSnackbarService,
    private loaderService: LoaderService, private titleService: Title, public dialog: MatDialog,
    private printService: PrintService, private auth: AuthService) { }

  currencycode = this.auth.getCurrencyCode();
  @Input() studentid = 0;
  @Input() refresh = false;
  @Input() allowdelete: boolean = false;

  studentpayments1: any[] = [];
  displayedColumns1: string[] = ['description', 'mode', 'chtrno', 'amount', 'print'];

  ngOnInit() {
    //this.titleService.setTitle('Student Payments');
    if (this.allowdelete) {
      this.displayedColumns1 = ['description', 'mode', 'chtrno', 'amount', 'print', 'delete'];
    }
    this.GetOne();
  }

  ngOnChanges(changes: SimpleChanges): void {
    this.GetOne();
  }

  GetOne() {
    if (this.studentid > 0) {
      this.isLoading = true;
      this.http.get(AppSettings.API_ENDPOINT + "api/studentpayments/getbystudentid/" + this.studentid, {
      }).toPromise().then(response => {
        this.studentpayments1 = (<any>response);
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
    this.http.delete(AppSettings.API_ENDPOINT + "api/studentpayments/" + id, {
    }).toPromise().then(response => {
      this.snackbar.open("Deleted");
      this.GetOne();
    }, err => {
      console.log(err.status)
    });
  }

  getTotalCost() {
    return this.studentpayments1.map(t => t.amount).reduce((acc, value) => acc + value, 0);
  }

  print(receiptid) {
    this.printService.title = "Payment Receipt";
    this.printService.layout = "portrait";
    this.printService.takeprint = true;
    const invoiceIds = [receiptid.toString()];
    this.printService.printDocument('payreceipt', invoiceIds);
  }
}
