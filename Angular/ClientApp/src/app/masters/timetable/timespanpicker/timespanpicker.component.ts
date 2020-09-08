import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { AppSettings } from '../../../../app/app-settings';
import { HttpClient } from '@angular/common/http';
import { CustomSnackbarService } from '../../../../app/shared/snackbar.service';
import { NgForm } from '@angular/forms';


export interface DialogData {
  selectMany: boolean;
  filter: string;
  fromtime: any;
  totime: any;
  slot: any;
  params: any;
}

@Component({
  selector: 'timespanpicker.component',
  templateUrl: './timespanpicker.component.html',
  styleUrls: ['./timespanpicker.component.css'],
})

export class TimeSpanPickerComponent implements OnInit {
  isLoading: boolean;
  subjects1: any;
  subjectids: any = [];
  timespans: any = [];
  timespansTo: any = [];

  constructor(
    public dialogRef: MatDialogRef<TimeSpanPickerComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DialogData, private snackbar: CustomSnackbarService,
    private http: HttpClient) { }

  ngOnInit() {
    this.getTimeSpans();
  }

  getTimeSpans() {
    this.isLoading = true;
    this.http.get(AppSettings.API_ENDPOINT + "api/timetables/gettimespans/" + this.data.slot, {
    }).subscribe(response => {
      this.timespans = response;
    }, err => {
      console.log(err)
    });
    this.isLoading = false;
  }

  FillTo() {
    this.data.totime = null;
    this.timespansTo = this.timespans.filter(o => o > this.data.fromtime);


    this.timespansTo = this.timespansTo.splice(0, 1);
    this.data.totime = this.timespansTo[0];
  }

  toTime(timeString) {
    var timeTokens = timeString.split(':');
    return new Date(1970, 0, 1, timeTokens[0], timeTokens[1], timeTokens[2]);
  }

  Save(form: NgForm) {
    if (form.invalid)
      return;

    this.dialogRef.close({ fromtime: this.data.fromtime, totime: this.data.totime });


  }

  onNoClick(): void {
    this.dialogRef.close(false);
  }
}

