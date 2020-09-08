import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { ActivatedRoute, Router } from '@angular/router';
import { NgForm, FormGroup } from '@angular/forms';
import { AppSettings } from '../../../app/app-settings';
import { CustomSnackbarService } from '../../../app/shared/snackbar.service';
import { FadeAnimation } from '../../../app/shared/animations';
import { Title } from '@angular/platform-browser';
import { MatDialog } from '@angular/material';
import { AlertDialog } from '../../../app/shared/alertmessage/alert-dialog';
import { ConfirmDialog } from '../../../app/shared/confirm/confirm-dialog';

@Component({
  selector: 'app-weeklyoff',
  templateUrl: './weeklyoff.component.html',
  styleUrls: ['./weeklyoff.component.css'],
  animations: [FadeAnimation]
})

export class WeeklyOffComponent implements OnInit {
  isLoading: boolean = false;
  constructor(private router: Router, private http: HttpClient, private route: ActivatedRoute,
    private snackbar: CustomSnackbarService, private titleService: Title, public dialog: MatDialog) { }

  displayedColumns: string[] = ['positions', 'weekday', 'delete'];

  weeklyoffs: any = [];
  positions: any = [];
  weekday: any;

  ngOnInit() {

    this.titleService.setTitle("Weekly Off");

    this.GetWeeklyOffs();
  }

  GetWeeklyOffs() {
    this.isLoading = true;
    this.http.get(AppSettings.API_ENDPOINT + "api/weeklyoffs", {
    }).toPromise().then(response => {
      this.weeklyoffs = response;
      this.isLoading = false;
    }, err => {
      this.isLoading = false;
      });

    this.positions = [];
    this.weekday = null;
  }

  Delete(id): void {
    const dialogRef = this.dialog.open(ConfirmDialog, {
      width: '250px'
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.DeleteUser(id);
      }
    });
  }

  DeleteUser(id) {
    this.http.delete(AppSettings.API_ENDPOINT + "api/weeklyoffs/" + id, {
    }).subscribe(response => {      
        this.snackbar.open("Deleted");
        this.GetWeeklyOffs();      
    }, err => {
      console.log(err.status);
    });
  }

  Save(form: NgForm) {
    if (form.invalid)
      return;

    let data = { posInMonth: this.positions.toString(), weekday: this.weekday };
    this.http.post(AppSettings.API_ENDPOINT + "api/weeklyoffs/", data, {
    }).subscribe(response => {
      this.GetWeeklyOffs();
    }, err => {
      console.log(err)
    });
  }

  Back() {
    this.router.navigate(["batches"]);
  }



}

