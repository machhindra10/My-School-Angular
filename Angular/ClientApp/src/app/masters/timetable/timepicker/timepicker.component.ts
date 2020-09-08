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
  params: any;
}

@Component({
  selector: 'timepicker.component',
  templateUrl: './timepicker.component.html',
  styleUrls: ['./timepicker.component.css'],
})

export class TimePickerComponent implements OnInit {
  isLoading: boolean;
  subjects1: any;
  subjectids: any = [];

  constructor(
    public dialogRef: MatDialogRef<TimePickerComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DialogData, private snackbar: CustomSnackbarService,
    private http: HttpClient) { }

  ngOnInit() {

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

