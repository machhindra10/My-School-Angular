import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { ActivatedRoute, Router } from '@angular/router';
import { NgForm, FormGroup } from '@angular/forms';
import { AppSettings } from '../../app-settings';
import { CustomSnackbarService } from '../../shared/snackbar.service';
import { FadeAnimation } from '../../shared/animations';
import { Title } from '@angular/platform-browser';
import { MatDialog } from '@angular/material';
import { SMSSettingsComponent } from '../../../app/settings/sms/smssettings.component';

@Component({
  selector: 'app-sendmsg',
  templateUrl: './sendmsg.component.html',
  styleUrls: ['./sendmsg.component.css'],
  animations: [FadeAnimation]
})

export class SendSMSMsgComponent implements OnInit {
  isLoading: boolean = false;
  constructor(private router: Router, private http: HttpClient, private route: ActivatedRoute,
    private snackbar: CustomSnackbarService, private titleService: Title, public dialog: MatDialog) { }

  id: number = parseInt(this.route.snapshot.paramMap.get("id"));
  breakpoint: number;

  classes: any = [];
  messageData: any = {};

  ngOnInit() {
    this.titleService.setTitle("Send Message");
    this.breakpoint = (window.innerWidth <= 700) ? 1 : 3;

    this.GetClasses();

  }

  GetClasses() {
    this.isLoading = true;
    this.http.get(AppSettings.API_ENDPOINT + "api/classes/getclassesenabled", {
    }).toPromise().then(response => {
      this.classes = response;
      this.isLoading = false;

    }, err => {
      console.log(err)
      this.isLoading = false;
    });

  }

  Save(form: NgForm) {
    if (form.invalid)
      return;

    this.http.post(AppSettings.API_ENDPOINT + "api/smss/sendcommonsms", this.messageData, {
    }).subscribe(response => {
      //console.log((<any>response).Status);
      if ((<any>response).Status == "OK") {
        this.snackbar.open((<any>response).Response.Message);
        this.messageData.message = null;
        this.messageData.classids = null;
        this.messageData.towhom = null;
      } else if ((<any>response).Status == "ERROR") {
        this.snackbar.open('ERROR: ' + (<any>response).Response.Message);
      }
    }, err => {
      console.log(err)
    });


  }

  openSettingsDialog(): void {
    const dialogRef = this.dialog.open(SMSSettingsComponent, {
      width: '400px',
      data: {}
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        //this.GetOne();
      }
    });
  }

  onResize(event) {
    this.breakpoint = (event.target.innerWidth <= 700) ? 1 : 3;
  }

}

