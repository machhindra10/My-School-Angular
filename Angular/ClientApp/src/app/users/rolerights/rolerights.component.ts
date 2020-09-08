import { Component, OnInit, ViewEncapsulation, Input } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { ActivatedRoute, Router } from '@angular/router';
import { NgForm, FormGroup } from '@angular/forms';
import { AppSettings } from '../../app-settings';
import { CustomSnackbarService } from '../../shared/snackbar.service';
import { FadeAnimation } from '../../shared/animations';
import { Title } from '@angular/platform-browser';
import { AuthService } from '../../../app/app-auth/auth.service';

@Component({
  selector: 'app-rolerights',
  templateUrl: './rolerights.component.html',
  styleUrls: ['./rolerights.component.css'],
  animations: [FadeAnimation]
})

export class RoleRightsComponent implements OnInit {
  constructor(private router: Router, private http: HttpClient, private route: ActivatedRoute,
    private snackbar: CustomSnackbarService, private titleService: Title, private auth: AuthService) { }

  isLoading: boolean = false;
  isLoading1: boolean = false;

  rolerights: any;
  displayedColumns: string[] = ['menuname','rightname', 'enabled'];
  @Input() roleid: number;
  @Input() isuserroleisadmin: boolean;
  temproleright: any;
  isusermasteradmin: boolean = false;

  ngOnInit() {
    this.isusermasteradmin = this.auth.IsUserMasterAdmin();
    this.isLoading1 = true;
    this.GetRoleRights();

  }

  GetRoleRights() {
    if (this.roleid > 0) {
      
      this.http.get(AppSettings.API_ENDPOINT + "api/rights/rightsbyroleid/" + this.roleid, {
      }).toPromise().then(response => {
        this.rolerights = response;
        this.isLoading1 = false;
      }, err => {
        console.log(err.status)
        this.isLoading1 = false;
      });
    }
  }

  UpdateRoleRight(element) {
    if (!element.isEnabled) {
      this.SaveRoleRight(element);
    } else {
      this.DeleteRoleRight(element);
    }

  }

  SaveRoleRight(element) {
    this.temproleright = { "id": 0, "roleId": this.roleid, "rightId": element.id };
    this.http.post(AppSettings.API_ENDPOINT + "api/rolerights", this.temproleright, {
    }).toPromise()
      .then(response => {
        this.GetRoleRights();
        this.snackbar.open("Updated");
      }, err => {
        console.log(err.status)
      });
  }

  DeleteRoleRight(element) {
    this.http.delete(AppSettings.API_ENDPOINT + "api/rolerights/" + element.roleRightId, {
    }).toPromise()
      .then(response => {
        this.GetRoleRights();
        this.snackbar.open("Updated");
      }, err => {
        console.log(err.status)
      });
  }

  selectedRowIndex: number = 0;

  highlight(row) {
    this.selectedRowIndex = row.id;
    console.log(row.id);
  }
  changed(row) {
    console.log(row.id);
  }
}

