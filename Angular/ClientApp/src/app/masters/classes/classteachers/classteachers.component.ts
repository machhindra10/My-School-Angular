import { Component, OnInit, Input } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Router } from '@angular/router';
import { AppSettings } from '../../../app-settings';
import { CustomSnackbarService } from '../../../shared/snackbar.service';
import { LoaderService } from '../../../shared/loader.service';
import { FadeAnimation } from '../../../shared/animations';
import { Title } from '@angular/platform-browser';
import { MatDialog } from '@angular/material';
import { AddClassTeacherComponent } from '../addclassteacher/addclassteacher.component';

export interface DialogData {
  classid: number;
  staffid: number;
  year: number;
}

@Component({
  selector: 'app-classteachers',
  templateUrl: './classteachers.component.html',
  styleUrls: ['./classteachers.component.css'],
  animations: [FadeAnimation]
})

export class ClassTeachersComponent implements OnInit {

  isLoading: boolean = false;
  constructor(private http: HttpClient, private router: Router, private snackbar: CustomSnackbarService,
    private loaderService: LoaderService, private titleService: Title, public dialog: MatDialog) { }

  animal: string;
  name: string;
  @Input() classid = 0;
  classteachers1;
  displayedColumns1: string[] = ['classteacher', 'from', 'to'];

  openDialog(): void {
    const dialogRef = this.dialog.open(AddClassTeacherComponent, {
      width: '400px',
      data: { selectMany: false, filter: 'teachers' }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        let data = { classid: this.classid, staffid: result };

        this.isLoading = true;
        this.http.post(AppSettings.API_ENDPOINT + "api/classteachers", data, {
        }).subscribe(response => {
          this.GetOne();
        }, err => {
          console.log(err)
        });
        this.isLoading = false;

      }
    });
  }


  ngOnInit() {
    this.titleService.setTitle('Class Teachers');
    this.GetOne();
  }


  GetOne() {
    this.isLoading = true;
    if (this.classid > 0) {
      this.http.get(AppSettings.API_ENDPOINT + "api/classteachers/getbyclassid/" + this.classid + "/" + 2018, {
      }).subscribe(response => {
        this.classteachers1 = (<any>response);
        this.isLoading = false;
      }, err => {
        console.log(err.status);
        this.isLoading = false;
      });
    }

  }
}
