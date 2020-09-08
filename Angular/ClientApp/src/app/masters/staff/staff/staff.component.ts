import { Component, OnInit } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Router } from '@angular/router';
import { AppSettings } from '../../../app-settings';
import { CustomSnackbarService } from '../../../shared/snackbar.service';
import { LoaderService } from '../../../shared/loader.service';
import { FadeAnimation } from '../../../shared/animations';
import { Title } from '@angular/platform-browser';
import { MatDialog } from '@angular/material';
import { ConfirmDialog } from '../../../../app/shared/confirm/confirm-dialog';
import { UserSelectorComponent } from '../../../../app/users/userselector/userselector.component';

@Component({
  selector: 'app-staff',
  templateUrl: './staff.component.html',
  styleUrls: ['./staff.component.css'],
  animations: [FadeAnimation]
})

export class StaffComponent implements OnInit {
  breakpoint: boolean;
  isLoading: boolean = false;
  constructor(private http: HttpClient, private router: Router, private snackbar: CustomSnackbarService,
    private loaderService: LoaderService, private titleService: Title, public dialog: MatDialog) { }

  staff1;
  displayedColumns: string[] = ['staffname', 'designation', 'aadharno', 'email', 'mobile', 'edit', 'delete', 'enabled'];
  ngOnInit() {
    this.titleService.setTitle("Staff");
    this.Resize();
    this.GetAll();

  }

  details(id) {
    this.router.navigate(["staffdetails/" + id]);
  }

  Resize() {
    this.breakpoint = (window.innerWidth <= 700) ? true : false;
    if (!this.breakpoint) {
      this.displayedColumns = ['staffname', 'designation', 'aadharno', 'email', 'mobile', 'link', 'edit', 'delete', 'enabled'];
    }
    else {
      this.displayedColumns = ['staffname', 'designation', 'link', 'edit', 'delete', 'enabled'];
    }
  }

  onResize(event) {
    this.breakpoint = (event.target.innerWidth <= 700) ? true : false;
    if (!this.breakpoint) {
      this.displayedColumns = ['staffname', 'designation', 'aadharno', 'email', 'mobile', 'edit', 'delete', 'enabled'];
    }
    else {
      this.displayedColumns = ['staffname', 'designation', 'edit', 'delete', 'enabled'];
    }
  }

  GetAll() {
    this.isLoading = true;
    this.http.get(AppSettings.API_ENDPOINT + "api/staffs", {
    }).toPromise().then(response => {
      this.staff1 = response;
      this.isLoading = false;
    }, err => {
      console.log(err)
      this.isLoading = false;
    });
  }

  Add(id) {
    this.router.navigate(["addstaff/" + id]);
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
    this.http.delete(AppSettings.API_ENDPOINT + "api/staffs/" + id, {
    }).subscribe(response => {
      this.snackbar.open("Deleted");
      this.GetAll();
    }, err => {
      console.log(err.status)
    });
  }

  Link(associateuserid, staffid) {
    let ids = associateuserid + "," + staffid;
    this.router.navigate(["adduser/" + ids]);
  }
  EditLink(associateuserid, staffid) {
    let ids = associateuserid + "," + staffid;
    this.router.navigate(["adduser/" + ids]);
  }
  Update(element) {    
    this.http.get(AppSettings.API_ENDPOINT + "api/staffs/enabledisable/" + element.id, {
    }).subscribe(response => {
      this.snackbar.open("Updated");
      element.disabled = !element.disabled;
      return false;
    }, err => {
      console.log(err.status)
    });
  }

  SelectUser(staffid) {
    const dialogRef = this.dialog.open(UserSelectorComponent, {
      width: '400px',
      data: { userid: null, selectMany : false }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.http.get(AppSettings.API_ENDPOINT + "api/staffs/updateassociateuserid/" + staffid + "/" + result, {
        }).subscribe(response => {
          this.GetAll();
        }, err => {
          console.log(err)
        });
      }
    });
  }
}
