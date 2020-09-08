import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { ActivatedRoute, Router } from '@angular/router';
import { NgForm, FormGroup } from '@angular/forms';
import { AppSettings } from '../../app-settings';
import { CustomSnackbarService } from '../../shared/snackbar.service';
import { FadeAnimation } from '../../shared/animations';
import { Title } from '@angular/platform-browser';
import { Location } from '@angular/common';
import { AuthService } from '../../../app/app-auth/auth.service';

@Component({
  selector: 'app-adduser',
  templateUrl: './adduser.component.html',
  styleUrls: ['./adduser.component.css'],
  animations: [FadeAnimation]
})

export class AddUserComponent implements OnInit {
  isLoading: boolean = false;
  constructor(private router: Router, private http: HttpClient, private route: ActivatedRoute,
    private snackbar: CustomSnackbarService, private titleService: Title, private location: Location,
    private auth: AuthService) {

    let params = this.route.snapshot.paramMap.get("id");
    if (params != null) {
      this.userid = parseInt(params.split(',')[0]);
      if (params.split(',')[1] != null) {
        this.staffid = parseInt(params.split(',')[1]);
      }
    }
  }

  staffid: number = 0;
  userroles: any;
  user: any = {};
  staff: any = {};
  userid: number;
  breakpoint: number;
  orgid: string = this.auth.getOrgIdFromMasterToken();

  ngOnInit() {
    this.titleService.setTitle("Add User");
    this.breakpoint = (window.innerWidth <= 700) ? 1 : 3;
    this.GetRoles();

    if (this.userid > 0) {
      this.isLoading = true;
      this.http.get(AppSettings.API_ENDPOINT + "api/users/" + this.userid, {
      }).toPromise().then(response => {
        this.user = response;
        this.isLoading = false;
      }, err => {
        this.isLoading = false;
      });
    } else {
      (<any>this.user).id = this.userid;
      (<any>this.user).password = 'school@123';

      if (this.staffid > 0) {
        this.GetStaffDetails();
      }
    }
  }

  GetStaffDetails() {
    this.isLoading = true;
    this.http.get(AppSettings.API_ENDPOINT + "api/staffs/" + this.staffid, {
    }).toPromise().then(response => {
      this.staff = response;
      (<any>this.user).email = (<any>this.staff).email;
      (<any>this.user).aadharno = (<any>this.staff).aadharno;
      (<any>this.user).photo = (<any>this.staff).photo;
      let staffname = (<any>this.staff).staffname.split(' ');
      if (staffname[0] != null) { (<any>this.user).fname = staffname[0]; }
      if (staffname[1] != null) { (<any>this.user).mname = staffname[1]; }
      if (staffname[2] != null) { (<any>this.user).lname = staffname[2]; }
      this.isLoading = false;

    }, err => {
      this.isLoading = false;
    });
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
    if (this.userid == 0) {
      this.user.photo = "/assets/user-dummy.png";
    }
    this.http.put(AppSettings.API_ENDPOINT + "api/users/" + this.userid, this.user, {
    }).toPromise().then(response => {
      if ((<any>response).exists == true) {
        this.snackbar.open("Username / Email already exists!");
        return;
      }
      if (this.userid == 0 && this.staffid > 0) {
        console.log(this.userid + "-" + this.staffid);
        this.UpdateAssociateUserId((<any>response).id);
      }
      if (this.userid == 0) {
        this.saveUsertoMaster();
      }
      this.router.navigate(["users"]);
    }, err => {
      console.log(err)
    });
  }

  saveUsertoMaster() {
    let masteruser = { orgid: this.orgid, email: this.user.email };
    this.http.post(AppSettings.API_ENDPOINT_MASTER + "api/users/", masteruser, {
    }).toPromise().then(response => {
      if ((<any>response).result) {        
      }
    }, err => {
      console.log(err)
    });
  }

  UpdateAssociateUserId(ass_id) {
    //(<any>this.staff).associateuserid = (<any>res).id;
    this.http.get(AppSettings.API_ENDPOINT + "api/staffs/updateassociateuserid/" + this.staffid + "/" + ass_id, {
    }).subscribe(response => {
    }, err => {
      console.log(err)
    });
  }

  blockUnblock() {
    this.http.get(AppSettings.API_ENDPOINT + "api/users/disableuser/" + (<any>this.user).id, {
    }).subscribe(response => {
      this.snackbar.open("Updated");
      (<any>this.user).disabled = !(<any>this.user).disabled;
      return false;
    }, err => {
      console.log(err.status)
    });
  }

  Back() {
    //this.router.navigate(["users"]);
    this.location.back();
  }

  onResize(event) {
    this.breakpoint = (event.target.innerWidth <= 700) ? 1 : 3;
  }

}

