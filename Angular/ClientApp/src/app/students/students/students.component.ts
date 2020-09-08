import { Component, OnInit, ViewChild } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Router } from '@angular/router';
import { AppSettings } from '../../app-settings';
import { element } from 'protractor';
import { CustomSnackbarService } from '../../shared/snackbar.service';
import { LoaderService } from '../../shared/loader.service';
import { FadeAnimation } from '../../shared/animations';
import { Title } from '@angular/platform-browser';
import { MatDialog, MatTableDataSource, MatPaginator } from '@angular/material';
import { ConfirmDialog } from '../../../app/shared/confirm/confirm-dialog';

@Component({
  selector: 'app-students',
  templateUrl: './students.component.html',
  styleUrls: ['./students.component.css'],
  animations: [FadeAnimation]
})

export class StudentsComponent implements OnInit {
  breakpoint: boolean;
  isLoading: boolean = false;
  constructor(private http: HttpClient, private router: Router, private snackbar: CustomSnackbarService,
    private loaderService: LoaderService, private titleService: Title, public dialog: MatDialog) { }

  students: any = [];
  displayedColumns: string[] = ['fname', 'email', 'aadharno', 'edit', 'delete', 'enabled'];
  dataSource: any;

  @ViewChild(MatPaginator) paginator: MatPaginator;



  ngOnInit() {
    this.titleService.setTitle("students");

    this.GetUsers();

    this.breakpoint = (window.innerWidth <= 700) ? true : false;
    if (!this.breakpoint) {
      this.displayedColumns = ['fname', 'email', 'aadharno', 'edit', 'delete', 'enabled'];
    }
    else {
      this.displayedColumns = ['fname', 'edit', 'delete', 'enabled'];
    }
  }

  details(id) {
    this.router.navigate(["studentdetails/" + id]);
  }

  GetUsers() {
    this.isLoading = true;
    this.http.get(AppSettings.API_ENDPOINT + "api/students", {
    }).toPromise().then(response => {
      this.students = response;
      this.dataSource = new MatTableDataSource<any>(this.students);
      this.dataSource.paginator = this.paginator;
      this.isLoading = false;
    }, err => {
      console.log(err)
      this.isLoading = false;
    });

  }

  Add(userid) {
    this.router.navigate(["register/" + userid]);
  }

  Delete(userid): void {
    const dialogRef = this.dialog.open(ConfirmDialog, {
      width: '250px'
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.DeleteUser(userid);
      }
    });
  }

  DeleteUser(userid) {
    this.http.delete(AppSettings.API_ENDPOINT + "api/students/" + userid, {
    }).subscribe(response => {
      this.snackbar.open("Deleted");
      this.GetUsers();
    }, err => {
      console.log(err.status)
    });
  }

  Update(element) {
    this.http.get(AppSettings.API_ENDPOINT + "api/students/enabledisable/" + element.id, {
    }).subscribe(response => {
      this.snackbar.open("Updated");
      element.disabled = !element.disabled;
      return false;
    }, err => {
      console.log(err.status)
    });
  }

  onResize(event) {
    this.breakpoint = (event.target.innerWidth <= 700) ? true : false;
    if (!this.breakpoint) {
      this.displayedColumns = ['fname', 'email', 'aadharno', 'edit', 'delete', 'enabled'];
    }
    else {
      this.displayedColumns = ['fname', 'edit', 'delete', 'enabled'];
    }
  }
}
