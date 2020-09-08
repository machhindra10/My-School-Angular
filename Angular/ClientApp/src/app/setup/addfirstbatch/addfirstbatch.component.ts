import { Component, OnInit, ViewEncapsulation, ViewChild } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { ActivatedRoute, Router } from '@angular/router';
import { NgForm, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AppSettings } from '../../../app/app-settings';
import { CustomSnackbarService } from '../../../app/shared/snackbar.service';
import { FadeAnimation } from '../../../app/shared/animations';
import { Title } from '@angular/platform-browser';
import { AuthService } from '../../../app/app-auth/auth.service';
import { MatDialogRef } from '@angular/material';

@Component({
  selector: 'app-addfirstbatch',
  templateUrl: './addfirstbatch.component.html',
  styleUrls: ['./addfirstbatch.component.css'],
  animations: [FadeAnimation]
})

export class AddFirstBatchComponent implements OnInit {

  constructor(private router: Router, private http: HttpClient, private route: ActivatedRoute,
    private snackbar: CustomSnackbarService, private titleService: Title, 
    private auth: AuthService, public dialogRef: MatDialogRef<AddFirstBatchComponent>) {
    dialogRef.disableClose = true;
  }

  isLoading: boolean = false;
  batchid: number = 0;
  batch: any = {};  
  minDate: any;

  ngOnInit() {
    this.titleService.setTitle("Create Batch");  
  }

  SaveBatch(form: NgForm) {
    if (form.invalid)
      return;

    this.isLoading = true;
    this.batch.userid = this.auth.getUserId();
    this.batch.isactive = true;
    this.http.post(AppSettings.API_ENDPOINT + "api/batches/addfirstbatch/", this.batch, {
    }).subscribe(response => {
      if ((<any>response).result) {
        //this.router.navigate(["authenticating"]);
        this.dialogRef.close(true);
      }
    }, err => {
      console.log(err)
      this.snackbar.open("error updating database!");
    });
  }
}

