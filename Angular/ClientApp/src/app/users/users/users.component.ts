import { Component, OnInit } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Router } from '@angular/router';
import { AppSettings } from '../../app-settings';
import { element } from 'protractor';
import { CustomSnackbarService } from '../../shared/snackbar.service';
import { LoaderService } from '../../shared/loader.service';
import { FadeAnimation } from '../../shared/animations';
import { Title } from '@angular/platform-browser';
import { MatDialog } from '@angular/material';
import { ConfirmDialog } from '../../../app/shared/confirm/confirm-dialog';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.css'],
  animations: [FadeAnimation]
})

export class UsersComponent implements OnInit {
  isLoading: boolean = false;
  constructor(private http: HttpClient, private router: Router, private snackbar: CustomSnackbarService,
    private loaderService: LoaderService, private titleService: Title, public dialog: MatDialog) { }

  users: any = [];
  displayedColumns: string[] = ['fname', 'rolename','email', 'aadharno', 'edit', 'delete', 'enabled'];
  ngOnInit() {
    this.titleService.setTitle("Users");
    this.GetUsers();    
  }


  GetUsers() {
    this.isLoading = true;
    this.http.get(AppSettings.API_ENDPOINT + "api/users", {
    }).toPromise().then(response => {
      this.users = response;
      this.isLoading = false;
    }, err => {
      console.log(err);
      this.isLoading = false;
    });
  }

  Add(userid) {
    this.router.navigate(["adduser/" + userid]);
  }

  Delete(userid): void {
    const dialogRef = this.dialog.open(ConfirmDialog, {
      width: '250px'
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.DeleteUser(userid);
      }
    });
  }

  DeleteUser(userid) {
    this.http.delete(AppSettings.API_ENDPOINT + "api/users/" + userid, {
    }).toPromise().then(response => {
      this.UpdateAssociateUserIdOfStaff(response);
      this.snackbar.open("Deleted");
      this.GetUsers();
    }, err => {
      console.log(err.status)
    });
  }

  UpdateAssociateUserIdOfStaff(res) {
    let userid = (<any>res).id;
    this.http.get(AppSettings.API_ENDPOINT + "api/staffs/getbyassociateuserid/" + userid, {
    }).toPromise().then(response => {
      let staff = response;
      if (staff != null) {
        this.UpdateAssociateUserId(staff);
      }
    }, err => {
      console.log(err.status)
    });
  }

  UpdateAssociateUserId(staff) {
    (<any>staff).associateuserid = 0;
    this.http.put(AppSettings.API_ENDPOINT + "api/staffs/updateassociateuserid/" + (<any>staff).id, staff, {
    }).subscribe(response => {
    }, err => {
      console.log(err)
    });
  }

  Update(element) {    
    this.http.get(AppSettings.API_ENDPOINT + "api/users/disableuser/" + element.id, {
    }).subscribe(response => {
      this.snackbar.open("Updated");
      element.disabled = !element.disabled;
      return false;
    }, err => {
      console.log(err.status)
    });
  }

}
