import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { Router, ActivatedRoute } from "@angular/router";

import { AppSettings } from '../app-settings';
import { AuthService } from '../app-auth/auth.service';
import { Title } from '@angular/platform-browser';
import { CustomSnackbarService } from '../shared/snackbar.service';
import { MatDialog } from '@angular/material';
import { AddSettingsComponent } from '../setup/addsettings/addsettings.component';
import { AddFirstBatchComponent } from '../setup/addfirstbatch/addfirstbatch.component';

@Component({
  selector: 'notifications',
  templateUrl: './notifications.component.html',
  styleUrls: ['./notifications.component.css']
})
export class NotificationsComponent implements OnInit {
  constructor(private router: Router, private http: HttpClient, private auth: AuthService,
    private titleService: Title, private route: ActivatedRoute, private snackbar: CustomSnackbarService,
    public dialog: MatDialog) { }

  @Output() afterInitiate = new EventEmitter<{ count: number }>();

  notifications: any = [];
  studentbirthdays: any = [];
  staffbirthdays: any = [];
  isLoading = false;
  notify_count: number;
  displayedColumns = ['name', 'dayname'];


  ngOnInit() {
    this.GetNotifications();
  }

  GetNotifications() {

    this.http.get(AppSettings.API_ENDPOINT + "api/notifications/getnotifications", {
    }).subscribe(response => {
      this.notifications = response;
      this.studentbirthdays = this.notifications.students;
      this.staffbirthdays = this.notifications.staff;
      this.notify_count = this.notifications.count;
      this.afterInitiate.emit({ count: this.notify_count });
    }, err => {
      console.log(err);

    });
  }
}
