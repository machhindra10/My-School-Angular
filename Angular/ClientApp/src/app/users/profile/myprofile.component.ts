import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { ActivatedRoute, Router } from '@angular/router';
import { NgForm, FormGroup } from '@angular/forms';
import { AppSettings } from '../../app-settings';
import { CustomSnackbarService } from '../../shared/snackbar.service';
import { FadeAnimation } from '../../shared/animations';
import { Title } from '@angular/platform-browser';
import { AuthService } from '../../../app/app-auth/auth.service';

@Component({
  selector: 'app-myprofile',
  templateUrl: './myprofile.component.html',
  styleUrls: ['./myprofile.component.css'],
  animations: [FadeAnimation]
})

export class MyProfileComponent implements OnInit {
  isLoading: boolean = false;
  constructor(private router: Router, private http: HttpClient, private route: ActivatedRoute,
    private snackbar: CustomSnackbarService, private titleService: Title, private auth: AuthService) { }

  userroles: any;
  user: any = {};
  user1: any = {};
  userid: number;
  breakpoint: number;


  ngOnInit() {
    this.titleService.setTitle("My Profile");
    this.breakpoint = (window.innerWidth <= 700) ? 1 : 3;
    this.GetRoles();

    this.userid = this.auth.getUserId();
    (<any>this.user).photo = '/assets/user-dummy.png';
    if (this.userid > 0) {
      this.isLoading = true;
      this.http.get(AppSettings.API_ENDPOINT + "api/users/" + this.userid, {
      }).toPromise().then(response => {
        this.user = response;
        this.isLoading = false;
      }, err => {
        this.isLoading = false;
      });
    }
  }

  GetRoles() {
    this.isLoading = true;
    this.http.get(AppSettings.API_ENDPOINT + "api/roles/getrolesenabled", {
    }).subscribe(response => {
      this.userroles = response;
    }, err => {
      console.log(err)
    });
    this.isLoading = false;
  }

  Save(form: NgForm) {
    if (form.invalid)
      return;

    this.http.put(AppSettings.API_ENDPOINT + "api/users/" + this.userid, this.user, {
    }).subscribe(response => {
      if ((<any>response).exists == true) {
        console.log((<any>response).exists);
        this.snackbar.open("User Name already exists!");
        return;
      }
      this.snackbar.open("Profile updated successfully!");
      this.router.navigate(["login"]);
    }, err => {
      console.log(err)
    });
  }

  ChangePassword(form: NgForm) {
    if (form.invalid)
      return;

    this.http.put(AppSettings.API_ENDPOINT + "api/users/updatepassword/" + this.userid, this.user1, {
    }).subscribe(response => {
      if ((<any>response).comparepassword == false) {
        this.snackbar.open("Passwords do not match!");
        return;
      } else if ((<any>response).invalidpassword == true) {
        this.snackbar.open("Old password is incorrect!");
        return;
      }
      this.snackbar.open("Password changed successfully!");
      this.user1 = {};      
      this.auth.removeToken();
    }, err => {
      console.log(err)
    });
  }

  removephoto() {
    (<any>this.user).photo = '/assets/user-dummy.png';
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
        (<any>this.user).photo = (<any>event1.target).result;
      }
    }
  }

  Back() {
    this.router.navigate(["users"]);
  }

  onResize(event) {
    this.breakpoint = (event.target.innerWidth <= 700) ? 1 : 3;
  }

}

