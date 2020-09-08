import { Component, OnInit, ViewEncapsulation, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { NgForm } from '@angular/forms';
import { AppSettings } from '../../app-settings';
import { CustomSnackbarService } from '../../shared/snackbar.service';
import { FadeAnimation } from '../../shared/animations';
import { SMSSettingsComponent } from '../../../app/settings/sms/smssettings.component';

export interface DialogData {

}

@Component({
  selector: 'app-sendmessage',
  templateUrl: './sendmessage.component.html',
  styleUrls: ['./sendmessage.component.css'],
  animations: [FadeAnimation]
})

export class SendMessageComponent implements OnInit {
  isLoading: boolean = false;
  constructor(public dialogRef: MatDialogRef<SendMessageComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DialogData, private snackbar: CustomSnackbarService,
    private http: HttpClient) { }

  breakpoint: number;

  classes: any = [];
  messageData: any = {};

  ngOnInit() {
    this.breakpoint = (window.innerWidth <= 700) ? 1 : 3;

    this.GetClasses();

  }

  GetClasses() {
    this.isLoading = true;
    this.http.get(AppSettings.API_ENDPOINT + "api/classes/getclassesenabled", {
    }).toPromise().then(response => {
      this.classes = response;
      this.isLoading = false;

    }, err => {
      console.log(err)
      this.isLoading = false;
    });

  }

  Save(form: NgForm) {
    if (form.invalid)
      return;
    //console.log(this.messageData);
    this.http.post(AppSettings.API_ENDPOINT + "api/messages/sendcommonmessage", this.messageData, {
    }).subscribe(response => {
      if ((<any>response).result) {
        this.dialogRef.close(true);
      } else {
        this.snackbar.open('ERROR : sending message');
      }
    }, err => {
      console.log(err)
    });


  }

  onNoClick(): void {
    this.dialogRef.close(false);
  }

  onResize(event) {
    this.breakpoint = (event.target.innerWidth <= 700) ? 1 : 3;
  }

}

