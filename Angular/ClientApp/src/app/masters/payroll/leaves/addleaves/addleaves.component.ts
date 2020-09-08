import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { AppSettings } from '../../../../../app/app-settings';
import { HttpClient } from '@angular/common/http';
import { CustomSnackbarService } from '../../../../../app/shared/snackbar.service';
import { NgForm } from '@angular/forms';


export interface DialogData {
  selectMany: boolean;
  leavetypeid: any;
  leaves: number;
  params: any;
}

@Component({
  selector: 'addleaves.component',
  templateUrl: './addleaves.component.html',
  styleUrls: ['./addleaves.component.css'],
})

export class AddLeavesComponent implements OnInit {
  isLoading: boolean;
  leavetypes: any;
  leavetypeids: any = [];
  leaves: number;

  constructor(
    public dialogRef: MatDialogRef<AddLeavesComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DialogData, private snackbar: CustomSnackbarService,
    private http: HttpClient) { }

  ngOnInit() {

    if (this.data.leavetypeid != null && this.data.leaves != null) {
      this.leavetypeids = this.data.leavetypeid;
      this.leaves = this.data.leaves;
    }
    this.getLeaveTypes();
  }

  getLeaveTypeName(id) {
    let name = "";
    if (this.leavetypes.filter(c => c.id == id).length > 0) {
      let subject = this.leavetypes.filter(c => c.id == id)[0];
      name = subject.leavetype;
    }
    return name;
  }

  getLeaveTypes() {
    this.isLoading = true;
    this.http.get(AppSettings.API_ENDPOINT + "api/leavetypes", {
    }).toPromise().then(response => {
      this.leavetypes = response;
      this.isLoading = false;
    }, err => {
      this.isLoading = false;
    });
  }  


  Save(form: NgForm) {
    if (form.invalid)
      return;
    let data = { leavetypeids: this.leavetypeids, leaves: this.leaves };
    this.dialogRef.close(data);
  }

  onNoClick(): void {
    this.dialogRef.close(false);
  }
}

