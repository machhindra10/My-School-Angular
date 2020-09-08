import { Component, OnInit, Input } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute, Router } from '@angular/router';
import { AppSettings } from '../../app-settings';
import { CustomSnackbarService } from '../../shared/snackbar.service';
import { FadeAnimation } from '../../shared/animations';
import { Title } from '@angular/platform-browser';
import { PrintService } from '../../../app/reports/print.service';

@Component({
  selector: 'app-ucstaff',
  templateUrl: './ucstaff.component.html',
  styleUrls: ['./ucstaff.component.css'],
  animations: [FadeAnimation]
})

export class UCStaffComponent implements OnInit {
  isLoading: boolean = false;
  isLoading1: boolean = false;
  constructor(private router: Router, private http: HttpClient, private route: ActivatedRoute,
    private snackbar: CustomSnackbarService, private titleService: Title, public printService: PrintService) { }

  staff: any = {};
  @Input() staffid = 0;

  ngOnInit() {    
    this.GetStaff();    
  }
 
  GetStaff() {
    
    if (this.staffid > 0) {
      this.isLoading = true;
      this.http.get(AppSettings.API_ENDPOINT + "api/staffs/getdetails/" + this.staffid, {
      }).toPromise().then(response => {
        this.staff = response;
        this.isLoading = false;
        this.GetStaffPhoto();
      }, err => {
        console.log(err.status)
        this.isLoading = false;
      });
    }
    else {
      (<any>this.staff).photo = '/assets/user-dummy.png';
    }
  }


  GetStaffPhoto() {
    (<any>this.staff).photo = '/assets/user-dummy.png';
    if (this.staffid > 0) {
      this.isLoading1 = true;
      this.http.get(AppSettings.API_ENDPOINT + "api/staffs/getstaffphoto/" + this.staffid, {
      }).toPromise().then(response => {
        if ((<any>response).photo != null) {
          (<any>this.staff).photo = (<any>response).photo;
        }
        this.isLoading1 = false;
      }, err => {
        console.log(err.status)
        this.isLoading1 = false;
      });
    }
  } 
 
}

