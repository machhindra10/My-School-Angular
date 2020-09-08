import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { AppSettings } from '../../app-settings';
import { HttpClient } from '@angular/common/http';
import { CustomSnackbarService } from '../../shared/snackbar.service';
import { NgForm } from '@angular/forms';


export interface DialogData {
  guardianid: number;
}

@Component({
  selector: 'addguardian.component',
  templateUrl: './addguardian.component.html',
  styleUrls: ['./addguardian.component.css'],
})
export class AddGuardianComponent implements OnInit {
  isLoading: boolean;
  guardian: any = {};

  constructor(
    public dialogRef: MatDialogRef<AddGuardianComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DialogData, private snackbar: CustomSnackbarService,
    private http: HttpClient) { }

  ngOnInit() {
    if (this.data.guardianid > 0) {
      this.load();
    }
  }

  load() {
    if (this.data.guardianid == 0)
      return;

    this.isLoading = true;
    this.http.get(AppSettings.API_ENDPOINT + "api/studentguardians/" + this.data.guardianid, {
    }).subscribe(response => {
      this.guardian = response;
    }, err => {
      console.log(err);
    });
    this.isLoading = false;
  }

  Save(form: NgForm) {
    if (form.invalid)
      return;

    if (this.data.guardianid == 0) {
      this.isLoading = true;
      this.http.post(AppSettings.API_ENDPOINT + "api/studentguardians/", this.guardian, {
      }).subscribe(response => {
        if ((<any>response).result) {
          this.dialogRef.close({ result: true, id: (<any>response).id });
        } else {
          if ((<any>response).nameExists) {
            this.snackbar.open("Name already exists!");
          }
          else if ((<any>response).mobileExists) {
            this.snackbar.open("Mobile already exists!");
          }
        }
      }, err => {
        console.log(err);
      });
      this.isLoading = false;
    } else {
      this.isLoading = true;
      this.http.put(AppSettings.API_ENDPOINT + "api/studentguardians/" + this.data.guardianid, this.guardian, {
      }).subscribe(response => {
        if ((<any>response).result) {
          this.dialogRef.close({ result: true, id: (<any>response).id });
        } else {
          if ((<any>response).nameExists) {
            this.snackbar.open("Name already exists!");
          }
          else if ((<any>response).mobileExists) {
            this.snackbar.open("Mobile already exists!");
          }
        }
      }, err => {
        console.log(err);
      });
      this.isLoading = false;
    }
  }

  onNoClick(): void {
    this.dialogRef.close({ result: false, id: 0 });
  }
}

