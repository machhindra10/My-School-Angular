import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { AppSettings } from '../../../app/app-settings';
import { HttpClient } from '@angular/common/http';
import { CustomSnackbarService } from '../../../app/shared/snackbar.service';
import { NgForm } from '@angular/forms';


export interface DialogData {
  selectMany: boolean;
  filter: string;
  userid: any;
  params: any;
}

@Component({
  selector: 'userselector.component',
  templateUrl: './userselector.component.html',
  styleUrls: ['./userselector.component.css'],
})

export class UserSelectorComponent implements OnInit {
  isLoading: boolean;
  users: any;
  userids: any = [];

  constructor(
    public dialogRef: MatDialogRef<UserSelectorComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DialogData, private snackbar: CustomSnackbarService,
    private http: HttpClient) { }

  ngOnInit() {
    
    if (this.data.userid != null) {      
      this.userids = this.data.userid;      
    }
    this.getUsers();
  }

  getUserName(userid) {    
    let name = "";
    if (this.users.filter(c => c.id == userid).length > 0) {
      let user = this.users.filter(c => c.id == userid)[0];
      name = user.fname + " " + user.mname + " " + user.lname;
    }
    return name;
  }

  getUsers(): any {
    
      this.isLoading = true;
      this.http.get(AppSettings.API_ENDPOINT + "api/users/getforselector", {
      }).subscribe(response => {
        this.users = response;
      }, err => {
        console.log(err);
      });
      this.isLoading = false;
    }
  

  Save(form: NgForm) {
    if (form.invalid)
      return;

    this.dialogRef.close(this.userids);


  }

  onNoClick(): void {
    this.dialogRef.close(false);
  }
}

