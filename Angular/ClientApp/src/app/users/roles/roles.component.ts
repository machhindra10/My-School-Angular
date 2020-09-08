import { Component, OnInit } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Router } from '@angular/router';
import { AppSettings } from '../../app-settings';
import { CustomSnackbarService } from '../../shared/snackbar.service';
import { LoaderService } from '../../shared/loader.service';
import { FadeAnimation } from '../../shared/animations';
import { Title } from '@angular/platform-browser';
import { ConfirmDialog } from '../../../app/shared/confirm/confirm-dialog';
import { MatDialog } from '@angular/material';

@Component({
  selector: 'app-roles',
  templateUrl: './roles.component.html',
  styleUrls: ['./roles.component.css'],
  animations: [FadeAnimation] 
})

export class RolesComponent implements OnInit {
  breakpoint: boolean;
  isLoading: boolean = false;
  constructor(private http: HttpClient, private router: Router, private snackbar: CustomSnackbarService,
    private loaderService: LoaderService, private titleService: Title, public dialog: MatDialog) { }

  userroles;
  displayedColumns: string[] = ['rolename', 'edit', 'delete', 'enabled'];
  ngOnInit() {
    this.titleService.setTitle("Roles");    
    this.GetAll();    
  }
  

  GetAll() {
    this.isLoading = true;
    this.http.get(AppSettings.API_ENDPOINT + "api/roles", {
    }).toPromise().then(response => {
      this.userroles = response;
      this.isLoading = false;
    }, err => {
      console.log(err)
      this.isLoading = false;
      });
  }

  Add(roleid) {
    this.router.navigate(["addrole/" + roleid]);
  }

  Delete(roleid): void {
    const dialogRef = this.dialog.open(ConfirmDialog, {
      width: '250px'
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.Delete1(roleid);
      }
    });
  }

  Delete1(roleid) {    
      this.http.delete(AppSettings.API_ENDPOINT + "api/roles/" + roleid, {
      }).subscribe(response => {
        this.snackbar.open("Deleted");
        this.GetAll();
      }, err => {
        console.log(err.status)
      });   
  }

  Update(element) {
    element.disabled = !element.disabled;
    this.http.put(AppSettings.API_ENDPOINT + "api/roles/" + element.id, element, {
    }).subscribe(response => {
      this.snackbar.open("Updated");
      //element.disabled = !element.disabled;
      return false;
    }, err => {
      console.log(err.status)
    });
  }
 
}
