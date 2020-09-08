import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

export interface DialogData {  
  message: string; 
}

@Component({
  selector: 'confirm-message',
  templateUrl: './confirm-message.html'
})
export class ConfirmMessageDialog {
  constructor(public dialogRef: MatDialogRef<ConfirmMessageDialog>,
    @Inject(MAT_DIALOG_DATA) public data: DialogData) {
    dialogRef.disableClose = true;
  }

}

