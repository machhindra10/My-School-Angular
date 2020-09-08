import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { AppSettings } from '../../../../../app/app-settings';
import { HttpClient } from '@angular/common/http';
import { CustomSnackbarService } from '../../../../../app/shared/snackbar.service';
import { NgForm } from '@angular/forms';


export interface DialogData {
  selectMany: boolean;
  holiday: any;
  dates: Date;
  year: number;
}

@Component({
  selector: 'addholiday.component',
  templateUrl: './addholiday.component.html',
  styleUrls: ['./addholiday.component.css'],
})

export class AddHolidayComponent implements OnInit {
  isLoading: boolean;  
  holiday: any;
  dates: Date;

  minDate: Date;
  maxDate: Date;

  constructor(
    public dialogRef: MatDialogRef<AddHolidayComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DialogData, private snackbar: CustomSnackbarService,
    private http: HttpClient) { }

  ngOnInit() {

    if (this.data.holiday != null && this.data.dates != null) {
      this.holiday = this.data.holiday;
      this.dates = this.data.dates;
    }

    //if (this.data.year != null && this.data.year != 0) {
    this.minDate = new Date(new Date().getFullYear(), 0, 1);
    this.maxDate = new Date(new Date().getFullYear(), 11, 31);

    //console.log(this.minDate + "=" + this.maxDate);
    //}
  }

  

  


  Save(form: NgForm) {
    if (form.invalid)
      return;
    let data = { holiday: this.holiday, dates: this.dates };
    this.dialogRef.close(data);
  }

  onNoClick(): void {
    this.dialogRef.close(false);
  }
}

