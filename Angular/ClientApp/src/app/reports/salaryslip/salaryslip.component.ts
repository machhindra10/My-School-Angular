import { Component, OnInit, ViewEncapsulation, OnDestroy } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { ActivatedRoute, Router } from '@angular/router';
import { NgForm, FormGroup } from '@angular/forms';
import { AppSettings } from '../../app-settings';
import { FadeAnimation } from '../../shared/animations';
import { PrintService } from '../print.service';
import { Title } from '@angular/platform-browser';
import { MatDialog } from '@angular/material';
import { AuthService } from '../../../app/app-auth/auth.service';

@Component({
  selector: 'app-salaryslip',
  templateUrl: './salaryslip.component.html',
  styleUrls: ['./salaryslip.component.css'],
  animations: [FadeAnimation]
})

export class RptSalarySlipComponent implements OnInit, OnDestroy {
  constructor(private http: HttpClient, private route: ActivatedRoute, private printService: PrintService,
    private titleService: Title, private auth: AuthService) { }

  currencycode = this.auth.getCurrencyCode();
  isLoading: boolean = false;
  isLoading1: boolean = false;
  staff1: any = {};
  salary: any = {};
  salaryDetails: any = [];
  salaryDetailsE: any = [];
  salaryDetailsD: any = [];
  displayedColumns: string[] = ['head', 'amount'];
  salaryslipid = this.route.snapshot.params['id'];
  takeprint = this.printService.takeprint;

  totalDays: number;
  daysPayable: number;

  monthNames = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun',
    'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];

  getMonthName(monthNumber) {
    return this.monthNames[monthNumber - 1];
  }

  ngOnInit() {
    this.titleService.setTitle("Staff Salary Details");
    this.GetStaffSalary();
  }
  ngOnDestroy() {
    this.printService.takeprint = false;
  }


  GetStaffSalary() {
    if (this.salaryslipid > 0) {
      this.isLoading = true;
      this.http.get(AppSettings.API_ENDPOINT + "api/staffsalaries/getsalariesbyidreport/" + this.salaryslipid, {
      }).toPromise().then(response => {

        this.totalDays = (<any>response).totalDays;
        this.daysPayable = (<any>response).daysPayable;

        this.salary = (<any>response).staffSalary;
        this.salaryDetails = this.salary.staffSalaryDetails;
        this.staff1 = this.salary.staff;
        this.Saperate();
        if (this.takeprint) {
          this.printService.onDataReady();
        }
      }, err => {
        console.log(err.status);
        this.isLoading = false;
      });
    }
  }

  Saperate() {
    this.salaryDetailsE = this.salaryDetails.filter(o => o.type == 'Earnings');
    this.salaryDetailsD = this.salaryDetails.filter(o => o.type == 'Deductions');
  }

  printDocument() {
    this.printService.title = "Salary Slip";
    this.printService.takeprint = true;
    this.printService.layout = 'portrait';
    const invoiceIds = [this.salaryslipid.toString()];
    this.printService.printDocument('salaryslip', invoiceIds);
  }
}

