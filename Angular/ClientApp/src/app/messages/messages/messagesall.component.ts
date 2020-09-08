import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { ActivatedRoute, Router } from '@angular/router';
import { NgForm, FormGroup } from '@angular/forms';
import { AppSettings } from '../../app-settings';
import { CustomSnackbarService } from '../../shared/snackbar.service';
import { FadeAnimation } from '../../shared/animations';
import { Title } from '@angular/platform-browser';
import { MatDialog } from '@angular/material';
import { SendMessageComponent } from '../sendmessage/sendmessage.component';
import { ConfirmDialog } from '../../../app/shared/confirm/confirm-dialog';

@Component({
  selector: 'app-messagesall',
  templateUrl: './messagesall.component.html',
  styleUrls: ['./messagesall.component.css'],
  animations: [FadeAnimation]
})

export class MessagesAllComponent implements OnInit {

  isLoading: boolean = false;

  constructor(private router: Router, private http: HttpClient, private route: ActivatedRoute,
    private snackbar: CustomSnackbarService, private titleService: Title, public dialog: MatDialog) { }

  id: number = parseInt(this.route.snapshot.paramMap.get("id"));
  breakpoint: number;

  messages: any = [];
  displayedColumns: string[] = ['message', 'date', 'totalrecepients', 'delete'];

  ngOnInit() {
    this.titleService.setTitle("Messages");
    this.breakpoint = (window.innerWidth <= 700) ? 1 : 3;

    this.GetMessages();

  }

  GetMessages() {
    this.isLoading = true;
    this.http.get(AppSettings.API_ENDPOINT + "api/messages/getallmessages", {
    }).toPromise().then(response => {
      this.messages = response;
      this.isLoading = false;

    }, err => {
      console.log(err)
      this.isLoading = false;
    });

  }

  openSettingsDialog(): void {
    const dialogRef = this.dialog.open(SendMessageComponent, {
      width: '500px',
      data: {}
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.GetMessages();
      }
    });
  }

  onResize(event) {
    this.breakpoint = (event.target.innerWidth <= 700) ? 1 : 3;
  }

  Delete(id): void {
    const dialogRef = this.dialog.open(ConfirmDialog, {
      width: '250px'
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.Delete1(id);
      }
    });
  }

  Delete1(id) {
    this.http.delete(AppSettings.API_ENDPOINT + "api/messages/" + id, {
    }).subscribe(response => {
      this.snackbar.open("Deleted");
      this.GetMessages();
    }, err => {
      console.log(err.status)
    });
  }

  details(id) {
    this.router.navigate(["messagedetails/" + id]);
  }
}

