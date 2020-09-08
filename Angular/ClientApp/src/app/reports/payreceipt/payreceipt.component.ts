import { Component, OnInit, Input, OnDestroy } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { PrintService } from '../print.service';
import { HttpClient } from '@angular/common/http';
import { AppSettings } from '../../../app/app-settings';
import { FadeAnimation } from '../../../app/shared/animations';
import { Title } from '@angular/platform-browser';
import { AuthService } from '../../../app/app-auth/auth.service';



@Component({
  selector: 'app-payreceipt',
  templateUrl: './payreceipt.component.html',
  styleUrls: ['./payreceipt.component.css'],
  animations: [FadeAnimation]
})
export class PaymentReceiptComponent implements OnInit, OnDestroy {
  constructor(private http: HttpClient, route: ActivatedRoute, private printService: PrintService,
    private titleservice: Title, private auth: AuthService) {
    this.receiptid = route.snapshot.params['id'];
    this.takeprint = printService.takeprint;
  }

  currencycode = this.auth.getCurrencyCode();
  displayedColumns1: string[] = ['description', 'mode', 'chtrno', 'amount'];
  @Input() receiptid: number;
  @Input() takeprint: boolean = false;

  invoiceDetails = [];
  isLoading = false;
  sname;
  prnno;
  receiptno;
  rdate;
  address;

  ngOnInit() {
    this.titleservice.setTitle("Payment Receipt");
    this.getInvoiceDetails();
  }
  ngOnDestroy() {
    this.printService.takeprint = false;

  }
  getInvoiceDetails() {
    this.http.get(AppSettings.API_ENDPOINT + "api/studentpayments/getreceiptbyid/" + this.receiptid, {
    }).toPromise().then(response => {
      this.invoiceDetails = (<any>response);
      this.receiptno = this.invoiceDetails[0].id;
      this.rdate = this.invoiceDetails[0].datecreated;
      this.prnno = this.invoiceDetails[0].student.prnno;
      this.address = this.invoiceDetails[0].student.address;
      this.sname = this.invoiceDetails[0].student.fname + ' '
        + this.invoiceDetails[0].student.mname + ' '
        + this.invoiceDetails[0].student.lname;

      if (this.takeprint) {
        this.printService.onDataReady();
      }
    }, err => {
      console.log(err)
    });
  }

  printDocument() {
    this.printService.title = "Payment Receipt";
    this.printService.layout = "portrait";
    this.printService.takeprint = true;
    const invoiceIds = [this.receiptid.toString()];
    this.printService.printDocument('payreceipt', invoiceIds);
  }

}
