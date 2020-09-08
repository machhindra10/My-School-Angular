import { Component, OnInit, Input } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute, Router } from '@angular/router';
import { AppSettings } from '../../app-settings';
import { CustomSnackbarService } from '../../shared/snackbar.service';
import { FadeAnimation } from '../../shared/animations';
import { Title } from '@angular/platform-browser';
import { PrintService } from '../../../app/reports/print.service';

@Component({
  selector: 'app-ucstudent',
  templateUrl: './ucstudent.component.html',
  styleUrls: ['./ucstudent.component.css'],
  animations: [FadeAnimation]
})

export class UCStudentComponent implements OnInit {
  isLoading: boolean = false;
  isLoading1: boolean = false;
  constructor(private router: Router, private http: HttpClient, private route: ActivatedRoute,
    private snackbar: CustomSnackbarService, private titleService: Title, public printService: PrintService) { }

  student: any = {};
  @Input() studentid = 0;

  ngOnInit() {    
    this.GetStudent();    
  }
 
  GetStudent() {
    
    if (this.studentid > 0) {
      this.isLoading = true;
      this.http.get(AppSettings.API_ENDPOINT + "api/students/getdetails/" + this.studentid, {
      }).toPromise().then(response => {
        this.student = response;
        //console.log(this.student);
        this.isLoading = false;
        this.GetStudentPhoto();
      }, err => {
        console.log(err.status)
        this.isLoading = false;
      });
    }
    else {
      (<any>this.student).photo = '/assets/user-dummy.png';
    }
  }


  GetStudentPhoto() {
    (<any>this.student).photo = '/assets/user-dummy.png';
    if (this.studentid > 0) {
      this.isLoading1 = true;
      this.http.get(AppSettings.API_ENDPOINT + "api/students/getstudentphoto/" + this.studentid, {
      }).toPromise().then(response => {
        if ((<any>response).photo != null) {
          (<any>this.student).photo = (<any>response).photo;
        }
        this.isLoading1 = false;
      }, err => {
        console.log(err.status)
        this.isLoading1 = false;
      });
    }
  } 
 
}

