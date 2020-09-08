import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { AppSettings } from '../../../app/app-settings';
import { HttpClient } from '@angular/common/http';
import { CustomSnackbarService } from '../../../app/shared/snackbar.service';
import { NgForm } from '@angular/forms';


export interface DialogData {
  
}

@Component({
  selector: 'addreason.component',
  templateUrl: './addreason.component.html',
  styleUrls: ['./addreason.component.css'],
})
export class AddReasonComponent implements OnInit {
  isLoading: boolean;
  reason: string;

  constructor(
    public dialogRef: MatDialogRef<AddReasonComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DialogData, private snackbar: CustomSnackbarService,
    private http: HttpClient) { }

  ngOnInit() {
    
  }
    

  Save(form: NgForm) {
    if (form.invalid)
      return; 

    this.dialogRef.close(this.reason);
   
  }

  onNoClick(): void {
    this.dialogRef.close(false);
  }
}

