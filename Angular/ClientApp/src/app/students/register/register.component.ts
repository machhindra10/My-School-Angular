import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute, Router } from '@angular/router';
import { NgForm } from '@angular/forms';
import { AppSettings } from '../../app-settings';
import { CustomSnackbarService } from '../../shared/snackbar.service';
import { FadeAnimation } from '../../shared/animations';
import { Title } from '@angular/platform-browser';
import { Location } from '@angular/common';
import { MatDialog } from '@angular/material';
import { AddGuardianComponent } from '../../../app/guardian/add-guardian/addguardian.component';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css'],
  animations: [FadeAnimation]
})

export class RegisterStudentComponent implements OnInit {
  isLoading: boolean = false;
  isLoading1: boolean = false; isLoading2: boolean = false;
  constructor(private router: Router, private http: HttpClient, private route: ActivatedRoute,
    private snackbar: CustomSnackbarService, private titleService: Title, private location: Location,
    public dialog: MatDialog) { }

  selectedFile: any;
  student: any = {};
  casts: any;
  guardians: any;
  genders: any;
  studentid: number = parseInt(this.route.snapshot.paramMap.get("id"));
  breakpoint: number;
  canEdit: boolean = false;

  ngOnInit() {
    this.titleService.setTitle("Student Registration");
    this.breakpoint = (window.innerWidth <= 700) ? 1 : 3;

    this.GetGenders();
    this.GetCasts();
    this.GetGuardians();
    this.GetStudent();

  }

  removephoto() {
    (<any>this.student).photo = '/assets/user-dummy.png';
  }

  GetCasts() {
    this.isLoading = true;
    this.http.get(AppSettings.API_ENDPOINT + "api/casts", {
    }).toPromise().then(response => {
      this.casts = response;
      this.isLoading = false;
    }, err => {
      console.log(err.status)
      this.isLoading = false;
    });
  }

  GetGuardians() {
    this.isLoading2 = true;
    this.http.get(AppSettings.API_ENDPOINT + "api/studentguardians", {
    }).toPromise().then(response => {
      this.guardians = response;
      this.guardianSelectionChange();

      this.isLoading2 = false;
    }, err => {
      console.log(err.status)
      this.isLoading2 = false;
    });
  }

  GetGenders() {
    this.isLoading = true;
    this.http.get(AppSettings.API_ENDPOINT + "api/students/getgender", {
    }).toPromise().then(response => {
      this.genders = response;
      this.isLoading = false;
    }, err => {
      console.log(err.status)
      this.isLoading = false;
    });
  }

  async GetStudent() {

    if (this.studentid > 0) {
      this.isLoading = true;
      this.http.get(AppSettings.API_ENDPOINT + "api/students/" + this.studentid, {
      }).toPromise().then(response => {
        this.student = response;
        this.isLoading = false;
        this.GetStudentPhoto();
        this.guardianSelectionChange();
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

  onFileChanged(event) {
    if (event.target.files && event.target.files[0]) {

      let filesize = event.target.files[0].size;
      if (filesize > 20000) {
        this.snackbar.open('File must be less than 20KB');
        console.log(filesize);
        return;
      }
      var reader = new FileReader();
      reader.readAsDataURL(event.target.files[0]);
      reader.onload = (event1) => {
        (<any>this.student).photo = (<any>event1.target).result;
      }
    }
  }

  Save(form: NgForm) {
    if (form.invalid)
      return;

    this.http.put(AppSettings.API_ENDPOINT + "api/students/" + this.studentid, this.student, {
    }).subscribe(response => {
      if ((<any>response).exists == true) {
        console.log((<any>response).exists);
        this.snackbar.open("Student Name already exists!");
        return;
      }
      else {
        //console.log(response.id);
        if (this.studentid > 0) {
          this.location.back();
        }
        this.router.navigate(["admission/" + (<any>response).id]);
      }

    }, err => {
      console.log(err)
    });
  }

  Back() {
    this.location.back();
  }

  onResize(event) {
    this.breakpoint = (event.target.innerWidth <= 700) ? 1 : 3;
  }

  addGuardian(): void {
    const dialogRef = this.dialog.open(AddGuardianComponent, {
      width: '400px',
      data: { guardianid: 0 }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result != null && result.result) {
        this.GetGuardians();
        this.student.guardianid = result.id;
      }
    });
  }

  editGuardian(): void {
    const dialogRef = this.dialog.open(AddGuardianComponent, {
      width: '400px',
      data: { guardianid: this.student.guardianid }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result != null && result.result) {
        this.GetGuardians();
        this.student.guardianid = result.id;
      }
    });
  }

  guardianSelectionChange() {
    if (this.student.guardianid != null) {
      this.canEdit = true;
    }
    else {
      this.canEdit = false;
    }
  }
}

