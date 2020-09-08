import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { AppSettings } from '../../../app/app-settings';
import { HttpClient } from '@angular/common/http';
import { CustomSnackbarService } from '../../../app/shared/snackbar.service';
import { NgForm } from '@angular/forms';


export interface DialogData {
  studentid: number;
  feesType: string;
  amount: number;
}

@Component({
  selector: 'addstudentfee.component',
  templateUrl: './addstudentfee.component.html',
  styleUrls: ['./addstudentfee.component.css'],
})
export class AddStudentFeeComponent implements OnInit {
    isLoading: boolean;    

  constructor(
    public dialogRef: MatDialogRef<AddStudentFeeComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DialogData, private snackbar: CustomSnackbarService,
    private http: HttpClient) { }

  ngOnInit() {
    
  }
    

  Save(form: NgForm) {
    if (form.invalid)
      return;  
    this.isLoading = true;
    this.http.post(AppSettings.API_ENDPOINT + "api/studentfees", this.data, {
    }).subscribe(response => {
      this.dialogRef.close(true);
    }, err => {
      console.log(err);
      this.snackbar.open("Error adding fees!");
    });
    this.isLoading = false;
  }

  onNoClick(): void {
    this.dialogRef.close(false);
  }
}

