import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { ActivatedRoute, Router } from '@angular/router';
import { NgForm, FormGroup } from '@angular/forms';
import { AppSettings } from '../../../app-settings';
import { CustomSnackbarService } from '../../../shared/snackbar.service';
import { FadeAnimation } from '../../../shared/animations';
import { Title } from '@angular/platform-browser';
import { MatDialog } from '@angular/material';
import { ConfirmDialog } from '../../../../app/shared/confirm/confirm-dialog';
import { AuthService } from '../../../../app/app-auth/auth.service';
import { Location } from '@angular/common';


@Component({
  selector: 'app-addstaff',
  templateUrl: './addstaff.component.html',
  styleUrls: ['./addstaff.component.css'],
  animations: [FadeAnimation]
})

export class AddStaffComponent implements OnInit {
  constructor(private router: Router, private http: HttpClient, private route: ActivatedRoute,
    private snackbar: CustomSnackbarService, private titleService: Title, public dialog: MatDialog,
    private auth: AuthService, private location: Location) { }

  currencycode = this.auth.getCurrencyCode();
  edit: boolean = false;
  isLoading: boolean = false;
  isLoading1: boolean = false;
  designations: any = [];
  staff1: any = {};
  staffpayroll: any = [];
  staffpayrollE: any = [];
  staffpayrollD: any = [];
  id: number = parseInt(this.route.snapshot.paramMap.get("id"));
  breakpoint: number;
  displayedColumns: string[] = ['head', 'amount', 'delete'];


  ngOnInit() {
    this.titleService.setTitle("Add Staff");

    this.staff1.photo = '../assets/user-dummy.png';
    this.breakpoint = (window.innerWidth <= 700) ? 1 : 3;
    this.GetDesignations();
    this.GetOne();
    this.GetStaffPayroll();
  }
  GetDesignations(): any {
    this.http.get(AppSettings.API_ENDPOINT + "api/designations/getdesignationsenabled", {
    }).subscribe(response => {
      this.designations = response;
    }, err => {
      console.log(err.status)
    });
  }

  GetOne() {

    if (this.id > 0) {
      this.isLoading = true;
      this.http.get(AppSettings.API_ENDPOINT + "api/staffs/" + this.id, {
      }).toPromise().then(response => {
        this.staff1 = response;
        this.isLoading = false;
      }, err => {
        console.log(err.status);
        this.isLoading = false;
      });
    }

  }

  GetStaffPayroll() {
    if (this.id > 0) {
      this.isLoading1 = true;
      this.http.get(AppSettings.API_ENDPOINT + "api/staffpayrolls/getbystaffid/" + this.id, {
      }).toPromise().then(response => {
        this.staffpayroll = response;
        this.isLoading1 = false;
        this.Saperate();
      }, err => {
        console.log(err.status);
        this.isLoading1 = false;
      });
    }
  }

  Saperate() {
    this.staffpayrollE = this.staffpayroll.filter(o => o.type == 'Earnings');
    this.staffpayrollD = this.staffpayroll.filter(o => o.type == 'Deductions');
  }

  Save(form: NgForm) {
    if (form.invalid)
      return;

    this.http.put(AppSettings.API_ENDPOINT + "api/staffs/" + this.id, this.staff1, {
    }).subscribe(response => {
      if ((<any>response).exists == true) {
        this.snackbar.open("Staff Name already exists!");
        return;
      }
      else if ((<any>response).codeexists == true) {
        this.snackbar.open("Employee Code already exists!");
        return;
      }
      else if ((<any>response).emailexists == true) {
        this.snackbar.open("Email already exists!");
        return;
      }
      //this.router.navigate(["staff"]);
      this.location.back();
    }, err => {
      console.log(err)
    });
  }

  EditPayroll() {
    this.edit = true;
  }

  SavePayroll() {

    this.http.post(AppSettings.API_ENDPOINT + "api/staffpayrolls/updatepayroll/", this.staffpayroll, {
    }).subscribe(response => {
      this.edit = false;
      this.snackbar.open("Payroll updated!");
    }, err => {
      this.snackbar.open("Error! please enter valid amount!");
    });
  }

  GeneratePayroll(id) {
    this.http.get(AppSettings.API_ENDPOINT + "api/staffpayrolls/setstaffpayroll/" + id, {
    }).toPromise().then(response => {
      this.GetStaffPayroll();
    }, err => {
      console.log(err.status);
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
    this.http.delete(AppSettings.API_ENDPOINT + "api/staffpayrolls/" + id, {
    }).subscribe(response => {
      this.snackbar.open("Deleted");
      this.GetStaffPayroll();
    }, err => {
      console.log(err.status)
    });
  }

  removephoto() {
    (<any>this.staff1).photo = '/assets/user-dummy.png';
  }

  onFileChanged(event) {
    if (event.target.files && event.target.files[0]) {

      let filesize = event.target.files[0].size;
      if (filesize > 20000) {
        this.snackbar.open('File must be less than 20KB');
        console.log(filesize);
        return;
      }
      var reader = new FileReader();
      reader.readAsDataURL(event.target.files[0]);
      reader.onload = (event1) => {
        (<any>this.staff1).photo = (<any>event1.target).result;
      }
    }
  }

  Back() {
    this.location.back();
  }

  onResize(event) {
    this.breakpoint = (event.target.innerWidth <= 700) ? 1 : 3;
  }

}

