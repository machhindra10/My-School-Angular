import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { ActivatedRoute, Router } from '@angular/router';
import { NgForm, FormGroup } from '@angular/forms';
import { AppSettings } from '../../../../app/app-settings';
import { CustomSnackbarService } from '../../../../app/shared/snackbar.service';
import { FadeAnimation } from '../../../../app/shared/animations';
import { Title } from '@angular/platform-browser';
import { MatDialog } from '@angular/material';
import { AlertDialog } from '../../../../app/shared/alertmessage/alert-dialog';

@Component({
  selector: 'app-addbatch',
  templateUrl: './addbatch.component.html',
  styleUrls: ['./addbatch.component.css'],
  animations: [FadeAnimation]
})

export class AddBatchComponent implements OnInit {
  isLoading: boolean = false;
  constructor(private router: Router, private http: HttpClient, private route: ActivatedRoute,
    private snackbar: CustomSnackbarService, private titleService: Title, public dialog: MatDialog) { }

  batch: any = {};
  batchid: number = parseInt(this.route.snapshot.paramMap.get("id"));
  
  minDate: any;

  ngOnInit() {
    this.titleService.setTitle("Add Batch");

    if (this.batchid > 0) {
      this.isLoading = true;
      this.http.get(AppSettings.API_ENDPOINT + "api/batches/" + this.batchid, {
      }).toPromise().then(response => {
        this.batch = response;
        this.isLoading = false;
      }, err => {
        this.isLoading = false;
      });
    }
    else {      
      this.isLoading = true;
      this.http.get(AppSettings.API_ENDPOINT + "api/batches/getlastbatchdate", {
      }).toPromise().then(response => {
        this.minDate = response;
        this.isLoading = false;
      }, err => {
        this.isLoading = false;
        });
      if (this.batchid == 0) {
        this.openDialog();
      }
    }
    
  }


  openDialog(): void {
    const dialogRef = this.dialog.open(AlertDialog, {
      width: '600px'
    });

    dialogRef.afterClosed().subscribe(result => {
      if (!result) {
        this.Back();
      }
    });
  }
  
  Save(form: NgForm) {
    if (form.invalid)
      return;
    
    this.http.put(AppSettings.API_ENDPOINT + "api/batches/" + this.batchid, this.batch, {
    }).subscribe(response => {
      if ((<any>response).exists == true) {
        console.log((<any>response).exists);
        this.snackbar.open("Batch Name already exists!");
        return;
      }
      this.router.navigate(["batches"]);
    }, err => {
      console.log(err)
    });
  }

  Back() {
    this.router.navigate(["batches"]);
  }

 

}

