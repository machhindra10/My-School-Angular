import { Component, OnInit, Input, OnDestroy } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { PrintService } from '../print.service';
import { FadeAnimation } from '../../../app/shared/animations';
import { HttpClient } from '@angular/common/http';
import { AppSettings } from '../../../app/app-settings';
import { Title } from '@angular/platform-browser';
import { AuthService } from '../../../app/app-auth/auth.service';

interface Age {
  iyear: number;
  year: string
}

@Component({
  selector: 'app-salaries',
  templateUrl: './salaries.component.html',
  styleUrls: ['./salaries.component.css'],
  animations: [FadeAnimation]
})
export class RptSalariesComponent implements OnInit, OnDestroy {
  currencycode = this.auth.getCurrencyCode();
  
  month: number;
  year: number = (new Date()).getFullYear();

 
  students: any = [];
  imonths: any = [];
  iyears: any = [];
  
  isLoading: boolean = false;
  isLoading1: boolean = false;
  @Input() takeprint: boolean = false;
  displayedColumns: string[] = ['srno', 'name', 'period', 'date', 'amount'];
  monthNames = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun',
    'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];

  constructor(private http: HttpClient, route: ActivatedRoute, public printService: PrintService,
    private titleService: Title, private auth: AuthService) {
    let params = route.snapshot.params['id'];
    if (params != null) {
      this.month = parseInt(route.snapshot.params['id'].split(',')[0]);
      this.year = parseInt(route.snapshot.params['id'].split(',')[1]);
    }
    this.takeprint = printService.takeprint;
  }

  ngOnDestroy() {
    this.printService.takeprint = false;
  }

  ngOnInit() {
    this.titleService.setTitle("Salaries Report");
    this.getMonths();
    this.getYears();   
    this.load();
  }

  getMonthNameShort(monthNumber) {
    return this.monthNames[monthNumber - 1];
  }

  getTotalCost() {
    return this.students.map(t => t.netpay).reduce((acc, value) => acc + value, 0);
  }

  load() {
    if (this.month != null && this.month.toString() != NaN.toString() &&
      this.year != null && this.year.toString() != NaN.toString()) {
      this.isLoading1 = true;
      this.GetStudents();
    }

  } 

  

  getMonthName() {
    let name = "";
    if (this.imonths.filter(c => c.id == this.month).length > 0) {
      name = this.imonths.filter(c => c.id == this.month)[0].name;
    }
    return name;
  }

  getYearName() {
    let name = "";
    if (this.iyears.filter(c => c.id == this.year).length > 0) {
      name = this.iyears.filter(c => c.id == this.year)[0].name;
    }
    return name;
  }
   

  getMonths() {
    this.isLoading = true;
    this.http.get(AppSettings.API_ENDPOINT + "api/settings/getmonths", {
    }).toPromise().then(response => {
      this.imonths = response;
      this.isLoading = false;
    }, err => {
      console.log(err)
      this.isLoading = false;
    });
  }

  getYears() {
    this.isLoading = true;
    this.http.get(AppSettings.API_ENDPOINT + "api/settings/getyearonly", {
    }).toPromise().then(response => {
      this.iyears = response;
      if (this.year == null || this.year.toString() == NaN.toString()) {
        this.year = (new Date()).getFullYear();
      }
      this.isLoading = false;
    }, err => {
      console.log(err)
      this.isLoading = false;
    });
  }

  GetStudents() {
    this.http.get(AppSettings.API_ENDPOINT + "api/staffsalaries/getreportbymonth/" +  this.month + "/" + this.year, {
    }).toPromise().then(response => {
      this.students = response;
      this.isLoading1 = false;
      if (this.takeprint) {
        this.printService.onDataReady();
      }

    }, err => {
      console.log(err)
      this.isLoading1 = false;
    });
  }

  printDocument() {
    this.printService.title = "Salaries Report";
    this.printService.takeprint = true;
    this.printService.layout = 'portrait';
    const invoiceIds = [this.month.toString(), this.year.toString()];
    this.printService.printDocument('salariesreport', invoiceIds);
  }

}
