import { Component } from '@angular/core';
import { MatDialogRef } from '@angular/material';

@Component({
  selector: 'alert-dialog',
  templateUrl: './alert-dialog.html'
})
export class AlertDialog {
  constructor(public dialogRef: MatDialogRef<AlertDialog>) {
    dialogRef.disableClose = true;
  }

}

