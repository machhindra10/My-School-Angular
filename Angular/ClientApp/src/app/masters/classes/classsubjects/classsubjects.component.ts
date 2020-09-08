import { Component, OnInit, Input } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Router } from '@angular/router';
import { AppSettings } from '../../../app-settings';
import { CustomSnackbarService } from '../../../shared/snackbar.service';
import { LoaderService } from '../../../shared/loader.service';
import { FadeAnimation } from '../../../shared/animations';
import { Title } from '@angular/platform-browser';
import { MatDialog } from '@angular/material';
import { AddClassSubjectComponent } from '../addclasssubject/addclasssubject.component';
import { ConfirmDialog } from '../../../../app/shared/confirm/confirm-dialog';
import { AddClassTeacherComponent } from '../addclassteacher/addclassteacher.component';


@Component({
  selector: 'app-classsubjects',
  templateUrl: './classsubjects.component.html',
  styleUrls: ['./classsubjects.component.css'],
  animations: [FadeAnimation]
})

export class ClassSubjectsComponent implements OnInit {
  
  isLoading: boolean = false;
  constructor(private http: HttpClient, private router: Router, private snackbar: CustomSnackbarService,
    private loaderService: LoaderService, private titleService: Title, public dialog: MatDialog) { }

  @Input() classid = 0;

  classsubjects1;
  displayedColumns1: string[] = ['classsubject','staffname', 'delete'];
  ngOnInit() {
    //this.titleService.setTitle('Class Subjects');
    this.GetOne();
  }

  openDialog(): void {
    const dialogRef = this.dialog.open(AddClassSubjectComponent, {
      width: '400px',
      data: { selectMany: true, filter: null, subjectid: null, params: null }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {      

        this.isLoading = true;
        this.http.get(AppSettings.API_ENDPOINT + "api/classsubjects/addmultiple/" + this.classid  + "/" + result, {
        }).subscribe(response => {
          this.GetOne();
        }, err => {
          console.log(err)
        });
        this.isLoading = false;        
      }
    });
  }

  openTeachersDialog(id, staffid): void {
    const dialogRef = this.dialog.open(AddClassTeacherComponent, {
      width: '400px',
      data: { selectMany: false, filter: 'teachers', staffids: staffid }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (staffid == result) {
        return;
      }
      if (result) {        
        this.isLoading = true;
        this.http.get(AppSettings.API_ENDPOINT + "api/classsubjects/assignteacher/" + id + "/" + result, {
        }).subscribe(response => {
          if (response) {
            this.GetOne();
          }
        }, err => {
          console.log(err)
        });
        this.isLoading = false;

      }
    });
  }

  GetOne() {
    this.isLoading = true;
    if (this.classid > 0) {
      this.http.get(AppSettings.API_ENDPOINT + "api/classsubjects/getbyclassid/" + this.classid, {
      }).subscribe(response => {
        this.classsubjects1 = response;
        this.isLoading = false;
      }, err => {
        console.log(err.status);
        this.isLoading = false;
      });
    }
    
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
    this.http.delete(AppSettings.API_ENDPOINT + "api/classsubjects/" + id, {
    }).subscribe(response => {
      this.snackbar.open("Deleted");
      this.GetOne();
    }, err => {
      console.log(err.status)
    });
  }

 
}
