import { Component, OnInit, ViewEncapsulation, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { ActivatedRoute, Router } from '@angular/router';
import { NgForm, FormGroup } from '@angular/forms';
import { AppSettings } from '../../app-settings';
import { CustomSnackbarService } from '../../shared/snackbar.service';
import { FadeAnimation } from '../../shared/animations';
import { Title } from '@angular/platform-browser';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';

export interface DialogData {

}

@Component({
  selector: 'app-smssettings',
  templateUrl: './smssettings.component.html',
  styleUrls: ['./smssettings.component.css'],
  animations: [FadeAnimation]
})

export class SMSSettingsComponent implements OnInit {
  isLoading: boolean = false;
  constructor(
    public dialogRef: MatDialogRef<SMSSettingsComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DialogData, private snackbar: CustomSnackbarService,
    private http: HttpClient) { }


  settings: any = [];
  id: number;

  ngOnInit() {

    this.GetSMSSettings();

  }

  GetSMSSettings() {
    this.isLoading = true;
    this.http.get(AppSettings.API_ENDPOINT + "api/settingsothers/getsmssettings", {
    }).toPromise().then(response => {
      this.settings = response;
      this.id = (<any>response).id;
      this.isLoading = false;

    }, err => {
      console.log(err)
      this.isLoading = false;
    });

  }

  Save(form: NgForm) {
    if (form.invalid)
      return;

    this.http.put(AppSettings.API_ENDPOINT + "api/settingsothers/updatesmssettings/" + this.id, this.settings, {
    }).subscribe(response => {      
        this.dialogRef.close(true);      
    }, err => {
      console.log(err)
    });


  }

  onNoClick(): void {
    this.dialogRef.close(false);
  }


}

