import { Component, OnInit } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Router } from '@angular/router';
import { AppSettings } from '../../../app-settings';
import { CustomSnackbarService } from '../../../shared/snackbar.service';
import { LoaderService } from '../../../shared/loader.service';
import { FadeAnimation } from '../../../shared/animations';
import { Title } from '@angular/platform-browser';
import { forEach } from '@angular/router/src/utils/collection';
import { totalmem } from 'os';
import { MatDialog } from '@angular/material';
import { ConfirmDialog } from '../../../../app/shared/confirm/confirm-dialog';
import { AuthService } from '../../../../app/app-auth/auth.service';

@Component({
  selector: 'app-classes',
  templateUrl: './classes.component.html',
  styleUrls: ['./classes.component.css'],
  animations: [FadeAnimation]
})

export class ClassesComponent implements OnInit {

  isLoading: boolean = false;
  constructor(private http: HttpClient, private router: Router, private snackbar: CustomSnackbarService,
    private loaderService: LoaderService, private titleService: Title, public dialog: MatDialog,
    private auth: AuthService) { }

  currencycode = this.auth.getCurrencyCode();
  classes;
  displayedColumns: string[] = ['standard', 'classteacher', 'capacity', 'admitted', 'totalfees', 'edit', 'delete', 'enabled'];
  ngOnInit() {
    this.titleService.setTitle('Classes');
    this.GetAll();

  }


  GetAll() {
    this.isLoading = true;
    this.http.get(AppSettings.API_ENDPOINT + "api/classes/getclassesbyyear/" + 2018, {
    }).toPromise().then(response => {
      this.classes = response;
      this.isLoading = false;
    }, err => {
      console.log(err)
      this.isLoading = false;
    });

  }

  Add(id) {
    this.router.navigate(["addclass/" + id]);
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
    this.http.delete(AppSettings.API_ENDPOINT + "api/classes/" + id, {
    }).subscribe(response => {
      this.snackbar.open("Deleted");
      this.GetAll();
    }, err => {
      console.log(err.status)
    });
  }


  Update(element) {
    element.disabled = !element.disabled;
    this.http.put(AppSettings.API_ENDPOINT + "api/classes/" + element.id, element, {
    }).subscribe(response => {
      this.snackbar.open("Updated");
      //element.disabled = !element.disabled;
      return false;
    }, err => {
      console.log(err.status)

    });
  }

  getTotalFees(element) {
    let total: number = 0;
    for (let o of element) {
      total = total + o.amount;
    }
    return total.toFixed(2);
  }
}
