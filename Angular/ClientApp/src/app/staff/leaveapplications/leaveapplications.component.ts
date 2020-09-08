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
import { AuthService } from '../../../app/app-auth/auth.service';
import { AddReasonComponent } from '../addreason/addreason.component';
import { RemainingLeavesComponent } from '../remainingleaves/remainingleaves.component';

@Component({
  selector: 'app-leaveapplications',
  templateUrl: './leaveapplications.component.html',
  styleUrls: ['./leaveapplications.component.css'],
  animations: [FadeAnimation]
})

export class LeaveApplicationsComponent implements OnInit {
  breakpoint: boolean;
  isLoading: boolean = false;

  constructor(private http: HttpClient, private router: Router, private snackbar: CustomSnackbarService,
    private loaderService: LoaderService, private titleService: Title, public dialog: MatDialog, private auth: AuthService) { }

  
  applications: any;
  displayedColumns: string[] = ['staffname', 'leavetype', 'description','datefrom', 'dateto', 'approve', 'info'];

  ngOnInit() {
    this.titleService.setTitle("Leave Applications");    
      this.GetAll();   
  }

  info(element) {
    const dialogRef = this.dialog.open(RemainingLeavesComponent, {
      width: '400px',
      data: { staffid: element.staffid }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) { }
    });
  }

  Reject(element) {

    const dialogRef = this.dialog.open(AddReasonComponent, {
      width: '400px',
      data: {  }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {

        this.isLoading = true;
        this.http.get(AppSettings.API_ENDPOINT + "api/leaveapplications/changestatustorejected/" + element.id + "/" + result, {
        }).toPromise().then(response => {
          this.GetAll();
          this.isLoading = false;
        }, err => {
          console.log(err)
          this.isLoading = false;
        });
      }
    });
   
  }

  Approve(element) {
    this.isLoading = true;
    this.http.get(AppSettings.API_ENDPOINT + "api/leaveapplications/changestatustoapproved/" + element.id, {
    }).toPromise().then(response => {
      this.GetAll();
      this.isLoading = false;
    }, err => {
      console.log(err)
      this.isLoading = false;
    });
  }

  GetAll() {
    this.isLoading = true;
    this.http.get(AppSettings.API_ENDPOINT + "api/leaveapplications/getall/", {
    }).toPromise().then(response => {
      this.applications = response;
      this.isLoading = false;
    }, err => {
      console.log(err)
      this.isLoading = false;
    });
  }

  Add(id) {
    //this.router.navigate(["addpayhead/" + id]);
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


  Update(element) {    
    this.http.put(AppSettings.API_ENDPOINT + "api/leaveapplications/" + element.id, element, {
    }).subscribe(response => {
      this.snackbar.open("Updated");
      return false;
    }, err => {
      console.log(err.status)
    });
  }
}
