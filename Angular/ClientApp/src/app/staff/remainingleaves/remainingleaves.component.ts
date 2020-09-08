import { Component, Inject, OnInit, Input } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { NgForm } from '@angular/forms';


export interface DialogData {
  staffid: number;
}

@Component({
  selector: 'remainingleaves.component',
  templateUrl: './remainingleaves.component.html',
  styleUrls: ['./remainingleaves.component.css'],
})
export class RemainingLeavesComponent implements OnInit {

  staffid = 0;

  constructor(
    public dialogRef: MatDialogRef<RemainingLeavesComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DialogData) { }

  ngOnInit() {
    if (this.data.staffid > 0) {
      this.staffid = this.data.staffid;
    }
  }

  Save(form: NgForm) {
    if (form.invalid)
      return;

    this.dialogRef.close(true);

  }

  onNoClick(): void {
    this.dialogRef.close(false);
  }
}

