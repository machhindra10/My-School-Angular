import { Component, OnInit } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Router } from '@angular/router';
import { AppSettings } from '../../../app-settings';
import { CustomSnackbarService } from '../../../shared/snackbar.service';
import { LoaderService } from '../../../shared/loader.service';
import { FadeAnimation } from '../../../shared/animations';
import { Title } from '@angular/platform-browser';
import { MatDialog } from '@angular/material';
import { ConfirmDialog } from '../../../../app/shared/confirm/confirm-dialog';

@Component({
  selector: 'app-subjects',
  templateUrl: './subjects.component.html',
  styleUrls: ['./subjects.component.css'],
  animations: [FadeAnimation]
})

export class SubjectsComponent implements OnInit {
  breakpoint: boolean;
  isLoading: boolean = false;
  constructor(private http: HttpClient, private router: Router, private snackbar: CustomSnackbarService,
    private loaderService: LoaderService, private titleService: Title, public dialog: MatDialog) { }

  subjects1;
  displayedColumns: string[] = ['code','subject', 'edit', 'delete', 'enabled'];
  ngOnInit() {
    this.titleService.setTitle("Subjects");
    this.GetAll();
  }


  GetAll() {
    this.isLoading = true;
    this.http.get(AppSettings.API_ENDPOINT + "api/subjects", {
    }).toPromise().then(response => {
      this.subjects1 = response;
      this.isLoading = false;
    }, err => {
      console.log(err)
      this.isLoading = false;
    });    
  }

  Add(id) {
    this.router.navigate(["addsubject/" + id]);
  }

  Delete(id): void {
    const dialogRef = this.dialog.open(ConfirmDialog, {
      width: '250px'
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.Delete1(id);
      }
    });
  }

  Delete1(id) {    
      this.http.delete(AppSettings.API_ENDPOINT + "api/subjects/" + id, {
      }).subscribe(response => {
        this.snackbar.open("Deleted");
        this.GetAll();
      }, err => {
        console.log(err.status)
      });    
  }


  Update(element) {
    element.disabled = !element.disabled;
    this.http.put(AppSettings.API_ENDPOINT + "api/subjects/" + element.id, element, {
    }).subscribe(response => {
      this.snackbar.open("Updated");
      //element.disabled = !element.disabled;
      return false;
    }, err => {
      console.log(err.status)
    });
  }
}
