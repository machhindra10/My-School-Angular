import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { AppSettings } from '../../../../app/app-settings';
import { HttpClient } from '@angular/common/http';
import { NgForm } from '@angular/forms';


export interface DialogData {
  selectMany: boolean;
  filter: string;
  staffids: any;
}

@Component({
  selector: 'addclassteacher.component',
  templateUrl: './addclassteacher.component.html',
  styleUrls: ['./addclassteacher.component.css'],
})

export class AddClassTeacherComponent implements OnInit {
  isLoading: boolean;
  staff1: any;
  staffid: any = [];

  constructor(
    public dialogRef: MatDialogRef<AddClassTeacherComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DialogData,
    private http: HttpClient) { }

  ngOnInit() {

    if (this.data.staffids != null) {
      this.staffid = this.data.staffids;
    }

    this.GetStaff();
  }

  GetStaff(): any {
    if (this.data.filter == 'teachers') {
      this.isLoading = true;
      this.http.get(AppSettings.API_ENDPOINT + "api/staffs/getteachersstaffsenabled", {
      }).subscribe(response => {
        this.staff1 = response;
      }, err => {
        console.log(err)
      });
      this.isLoading = false;
    }
    else {
      this.isLoading = true;
      this.http.get(AppSettings.API_ENDPOINT + "api/staffs/getstaffsenabled", {
      }).subscribe(response => {
        this.staff1 = response;
      }, err => {
        console.log(err)
      });
      this.isLoading = false;
    }
  }

  Save(form: NgForm) {
    if (form.invalid)
      return;

    this.dialogRef.close(this.staffid);

  }

  onNoClick(): void {
    this.dialogRef.close(false);
  }
}

