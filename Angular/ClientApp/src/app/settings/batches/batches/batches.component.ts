import { Component, OnInit } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Router } from '@angular/router';
import { AppSettings } from '../../../app-settings';
import { element } from 'protractor';
import { CustomSnackbarService } from '../../../shared/snackbar.service';
import { LoaderService } from '../../../shared/loader.service';
import { FadeAnimation } from '../../../shared/animations';
import { Title } from '@angular/platform-browser';
import { MatDialog } from '@angular/material';
import { ConfirmDialog } from '../../../../app/shared/confirm/confirm-dialog';

@Component({
  selector: 'app-batches',
  templateUrl: './batches.component.html',
  styleUrls: ['./batches.component.css'],
  animations: [FadeAnimation]
})

export class BatchesComponent implements OnInit {
  isLoading: boolean = false;
  constructor(private http: HttpClient, private router: Router, private snackbar: CustomSnackbarService,
    private loaderService: LoaderService, private titleService: Title, public dialog: MatDialog) { }

  batches: any = [];
  displayedColumns: string[] = ['batch', 'startdate', 'enddate', 'isactive', 'edit'];
  ngOnInit() {
    this.titleService.setTitle("Batches");
    this.GetBatches();
  }


  GetBatches() {
    this.isLoading = true;
    this.http.get(AppSettings.API_ENDPOINT + "api/batches", {
    }).subscribe(response => {
      this.batches = response;
      this.isLoading = false;
    }, err => {
      console.log(err);
      this.isLoading = false;
    });
  }

  Add(userid) {
    this.router.navigate(["addbatch/" + userid]);
  }

  Delete(userid): void {
    const dialogRef = this.dialog.open(ConfirmDialog, {
      width: '250px'
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.DeleteUser(userid);
      }
    });
  }

  DeleteUser(userid) {
    this.http.delete(AppSettings.API_ENDPOINT + "api/batches/" + userid, {
    }).subscribe(response => {
      if ((<any>response).status == 'notdelete') {
        this.snackbar.open("Cant delete, at least one batch is must!");
      }else if ((<any>response).status == 'activenotdelete') {
        this.snackbar.open("Cant delete active batch!");
      }else {
        this.snackbar.open("Deleted");
        this.GetBatches();
      }
    }, err => {
      console.log(err.status)
    });
  }

  Update(element) {    
    element.isactive = true;
    this.http.put(AppSettings.API_ENDPOINT + "api/batches/setactive/" + element.id, element, {
    }).subscribe(response => {
      this.snackbar.open("Updated! Please logout and login again!");
      this.GetBatches();
      return false;
    }, err => {
      console.log(err.status)
    });
  }

}
