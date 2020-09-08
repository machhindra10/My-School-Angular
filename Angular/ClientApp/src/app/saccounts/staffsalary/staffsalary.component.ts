import { Component, OnInit } from '@angular/core';
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
import { ConfirmMessageDialog } from '../../../app/shared/confirm-message/confirm-message';

@Component({
  selector: 'app-staffsalary',
  templateUrl: './staffsalary.component.html',
  styleUrls: ['./staffsalary.component.css'],
  animations: [FadeAnimation]
})

export class StaffSalaryComponent implements OnInit {

  constructor(private http: HttpClient, private router: Router, private snackbar: CustomSnackbarService,
    private loaderService: LoaderService, private titleService: Title, public dialog: MatDialog,
    public printService: PrintService, private auth: AuthService) { }

  currencycode = this.auth.getCurrencyCode();
  breakpoint: boolean;
  isLoading: boolean = false;
  isLoading1: boolean = false;
  imonth = (new Date()).getMonth();
  iyear = (new Date()).getFullYear();
  tmonths: any = [];
  tyears: any = [];
  staffsalaries: any = [];
  displayedColumns: string[] = ['staffname', 'datecreated', 'earnings', 'deductions', 'adjustments', 'netpay', 'datepaid', 'print', 'pay', 'delete'];

  currentMonth = (new Date()).getMonth();
  currentYear = (new Date()).getFullYear();

  monthNames = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun',
    'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];

  getMonthName(monthNumber) {
    return this.monthNames[monthNumber - 1];
  }

  ngOnInit() {
    this.titleService.setTitle("Staff Salary");
    this.GetMonths();
    this.GetYears();
    this.isLoading1 = true;
    this.GetAll();
  }

  GetMonths() {
    this.isLoading = true;
    this.http.get(AppSettings.API_ENDPOINT + "api/settings/getmonths", {
    }).toPromise().then(response => {
      this.tmonths = response;
      this.isLoading = false;
    }, err => {
      console.log(err)
      this.isLoading = false;
    });
  }
  GetYears() {
    this.isLoading = true;
    this.http.get(AppSettings.API_ENDPOINT + "api/settings/getyearonly", {
    }).toPromise().then(response => {
      this.tyears = response;
      this.isLoading = false;
    }, err => {
      console.log(err)
      this.isLoading = false;
    });
  }
  GetAll() {

    this.http.get(AppSettings.API_ENDPOINT + "api/staffsalaries/getsalariesbymonth/" + this.imonth + "/" + this.iyear, {
    }).toPromise().then(response => {
      this.staffsalaries = response;
      this.isLoading1 = false;
    }, err => {
      console.log(err)
      this.isLoading1 = false;
    });
  }
  load() {
    if (this.imonth != null && this.iyear != null) {
      this.isLoading1 = true;
      this.GetAll();
    }
  }
  generateSalaries() {

    if (this.iyear < (new Date()).getFullYear() || this.iyear == (new Date()).getFullYear() && this.imonth <= (new Date()).getMonth()) {

      const dialogRef = this.dialog.open(ConfirmMessageDialog, {
        width: '400px',
        data: { message: 'Please make sure you have taken Attendance for current month. Once the salaries has been generated you can not change the attendance for current month. Are you sure want to proceed?' }
      });

      dialogRef.afterClosed().subscribe(result => {
        if (result) {
          this.isLoading1 = true;
          this.http.get(AppSettings.API_ENDPOINT + "api/staffsalaries/generatesalariesbymonth/" + this.imonth + "/" + this.iyear, {
          }).toPromise().then(response => {
            this.isLoading1 = true;
            this.GetAll();
          }, err => {
            console.log(err)
            this.isLoading1 = false;
          });          
        }
      });
      
    }
    else {
      this.snackbar.open("Salary cannot be generate for current month");
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
    this.http.delete(AppSettings.API_ENDPOINT + "api/staffsalaries/" + id, {
    }).subscribe(response => {
      this.snackbar.open("Deleted");
      this.GetAll();
    }, err => {
      console.log(err.status)
    });
  }


  Pay(element) {
    this.http.put(AppSettings.API_ENDPOINT + "api/staffsalaries/salarypaid/" + element.id, element, {
    }).subscribe(response => {
      //this.snackbar.open("Updated");
      this.GetAll();
    }, err => {
      console.log(err.status)
    });
  }

  printDocument(id) {
    this.printService.title = "Salary Slip";
    this.printService.takeprint = true;
    this.printService.layout = 'portrait';
    const invoiceIds = [id.toString()];
    this.printService.printDocument('salaryslip', invoiceIds);
  }
}
