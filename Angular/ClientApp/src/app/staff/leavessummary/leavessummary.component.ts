import { Component, Inject, OnInit, Input } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { AppSettings } from '../../../app/app-settings';
import { HttpClient } from '@angular/common/http';
import { CustomSnackbarService } from '../../../app/shared/snackbar.service';
import { FadeAnimation } from '../../../app/shared/animations';

@Component({
  selector: 'app-staff-leavessummary',
  templateUrl: './leavessummary.component.html',
  styleUrls: ['./leavessummary.component.css'],
  animations: [FadeAnimation],
})
export class StaffLeavesSummaryComponent implements OnInit {

  constructor(private snackbar: CustomSnackbarService, private http: HttpClient) { }

  isLoading: boolean; isLoading1: boolean = false;
  @Input() staffid = 0;
  @Input() refresh: boolean;
  @Input() showTitle: boolean = true;
  leavesreamining: any = [];
  displayedColumns: string[] = ['leavetype', 'count'];

  ngOnInit() {
    if (this.staffid > 0) {
      this.getLeaveRemaining();
    }
  }

  getLeaveRemaining() {
    this.isLoading1 = true;
    this.http.get(AppSettings.API_ENDPOINT + "api/leaves/getbyyear/" + this.staffid + "/" + new Date().getFullYear(), {
    }).toPromise().then(response => {
      this.leavesreamining = response;
      this.isLoading1 = false;
    }, err => {
      this.isLoading1 = false;
    });
  }


}

