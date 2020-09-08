import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { AppSettings } from '../../../../app/app-settings';
import { HttpClient } from '@angular/common/http';
import { CustomSnackbarService } from '../../../../app/shared/snackbar.service';
import { NgForm } from '@angular/forms';


export interface DialogData {
  selectMany: boolean;
  filter: string;
  subjectid: any;
  params: any;
}

@Component({
  selector: 'addclasssubject.component',
  templateUrl: './addclasssubject.component.html',
  styleUrls: ['./addclasssubject.component.css'],
})

export class AddClassSubjectComponent implements OnInit {
  isLoading: boolean;
  subjects1: any;
  subjectids: any = [];

  constructor(
    public dialogRef: MatDialogRef<AddClassSubjectComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DialogData, private snackbar: CustomSnackbarService,
    private http: HttpClient) { }

  ngOnInit() {
    
    if (this.data.subjectid != null) {      
        this.subjectids = this.data.subjectid;      
    }
    this.GetSubjects();
  }

  getSubjectName(subjectid) {    
    let name = "";
    if (this.subjects1.filter(c => c.id == subjectid).length > 0) {
      let subject = this.subjects1.filter(c => c.id == subjectid)[0];
      name = subject.code + " - " + subject.subject;
    }
    return name;
  }

  GetSubjects(): any {
    if (this.data.filter == "onlyclasssubjects") {
      this.isLoading = true;
      this.http.get(AppSettings.API_ENDPOINT + "api/subjects/getclasssubjectsonly/" + this.data.params, {
      }).subscribe(response => {
        this.subjects1 = response;
      }, err => {
        console.log(err);
      });
      this.isLoading = false;
    }
    else {
      this.isLoading = true;
      this.http.get(AppSettings.API_ENDPOINT + "api/subjects/getsubjectsenabled", {
      }).subscribe(response => {
        this.subjects1 = response;
      }, err => {
        console.log(err);
      });
      this.isLoading = false;
    }
  }

  Save(form: NgForm) {
    if (form.invalid)
      return;

    this.dialogRef.close(this.subjectids);


  }

  onNoClick(): void {
    this.dialogRef.close(false);
  }
}

