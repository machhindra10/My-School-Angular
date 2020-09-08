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
import { ConfirmDialog } from '../../../../app/shared/confirm/confirm-dialog';

@Component({
  selector: 'app-leavetypes',
  templateUrl: './leavetypes.component.html',
  styleUrls: ['./leavetypes.component.css'],
  animations: [FadeAnimation]
})

export class LeaveTypesComponent implements OnInit {
  isLoading: boolean = false;
  constructor(private router: Router, private http: HttpClient, private route: ActivatedRoute,
    private snackbar: CustomSnackbarService, private titleService: Title, public dialog: MatDialog) { }

  displayedColumns: string[] = ['code', 'leavetype', 'iscarryforward', 'dummy', 'edit', 'delete'];

  leavetypes: any = [];

  id: number = 0;
  code: any;
  leavetype: any;

  iscarryforward: boolean;

  ngOnInit() {

    this.titleService.setTitle("Leave Types");

    this.getLeaveTypes();
  }

  getLeaveTypes() {
    this.isLoading = true;
    this.http.get(AppSettings.API_ENDPOINT + "api/leavetypes", {
    }).toPromise().then(response => {
      this.leavetypes = response;
      this.isLoading = false;
    }, err => {
      this.isLoading = false;
    });

    this.id = 0;
    this.code = null;
    this.leavetype = null;
    this.iscarryforward = false;
  }

  Update(element) {
    this.id = parseInt(element.id);
    this.code = element.code;
    this.leavetype = element.leavetype;
    this.iscarryforward = element.iscarryforward;
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
    this.http.delete(AppSettings.API_ENDPOINT + "api/leavetypes/" + id, {
    }).subscribe(response => {
      this.snackbar.open("Deleted");
      this.getLeaveTypes();
    }, err => {
      console.log(err.status);
    });
  }

  Save(form: NgForm) {
    if (form.invalid)
      return;

    console.log(this.id);
    if (this.id == 0) {
      let data = { id: this.id, code: this.code, leavetype: this.leavetype, iscarryforward: this.iscarryforward };
      this.http.post(AppSettings.API_ENDPOINT + "api/leavetypes/", data, {
      }).subscribe(response => {
        if ((<any>response).nameexists) {
          this.snackbar.open("Leave type Name already exists!")
        }
        else if ((<any>response).codeexists) {
          this.snackbar.open("Leave type Code already exists!")
        }
        else {
          this.snackbar.open("Leave type Added successfully!")
          this.getLeaveTypes();
        }

      }, err => {
        console.log(err)
      });
    } else {
      let data = { id: this.id, code: this.code, leavetype: this.leavetype, iscarryforward: this.iscarryforward };
      this.http.put(AppSettings.API_ENDPOINT + "api/leavetypes/" + this.id, data, {
      }).subscribe(response => {
        if ((<any>response).nameexists) {
          this.snackbar.open("Leave type Name already exists!")
        }
        else if ((<any>response).codeexists) {
          this.snackbar.open("Leave type Code already exists!")
        }
        else {
          this.snackbar.open("Leave type Updated successfully!")
          this.getLeaveTypes();
        }
      }, err => {
        console.log(err)
      });
    }
  }

  Back() {
    //this.router.navigate(["batches"]);
  }



}

