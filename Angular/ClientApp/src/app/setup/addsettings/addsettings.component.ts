import { Component, OnInit, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { ActivatedRoute, Router } from '@angular/router';
import { NgForm, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AppSettings } from '../../../app/app-settings';
import { CustomSnackbarService } from '../../../app/shared/snackbar.service';
import { FadeAnimation } from '../../../app/shared/animations';
import { Title } from '@angular/platform-browser';
import { AuthService } from '../../../app/app-auth/auth.service';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';


@Component({
  selector: 'app-addsettings',
  templateUrl: './addsettings.component.html',
  styleUrls: ['./addsettings.component.css'],
  animations: [FadeAnimation]
})

export class AddSettingsComponent implements OnInit {

  constructor(private router: Router, private http: HttpClient, private route: ActivatedRoute,
    private snackbar: CustomSnackbarService, private titleService: Title,
    private auth: AuthService, public dialogRef: MatDialogRef<AddSettingsComponent>) {
    dialogRef.disableClose = true;
  }

  isLoading: boolean = false;
  settings: any = {};
  timezones: any = [];
  countries: any = [];
  currencies: any = [];

  ngOnInit() {
    this.titleService.setTitle("Settings");
    this.settings.id = 0;
    this.settings.logo = "";
    this.GetAllSettings();
    this.GetTimeZones();
    this.GetCountries();
    this.GetCurrencies();
  }

  GetAllSettings() {

    this.isLoading = true;
    this.http.get(AppSettings.API_ENDPOINT + "api/settings/getdefault", {
    }).subscribe(response => {
      if ((<any>response).result) {
        this.settings = (<any>response).settings;
        console.log("apeared");
      }
      this.isLoading = false;
    }, err => {
      console.log(err.status)
      this.isLoading = false;
    });

  }

  GetTimeZones() {

    this.isLoading = true;
    this.http.get(AppSettings.API_ENDPOINT + "api/settings/gettimezones", {
    }).subscribe(response => {
      this.timezones = response;
      this.isLoading = false;
    }, err => {
      console.log(err.status)
      this.isLoading = false;
    });

  }

  GetCountries() {

    this.isLoading = true;
    this.http.get(AppSettings.API_ENDPOINT + "api/countries", {
    }).subscribe(response => {
      this.countries = response;
      this.isLoading = false;
    }, err => {
      console.log(err.status)
      this.isLoading = false;
    });

  }

  GetCurrencies() {

    this.isLoading = true;
    this.http.get(AppSettings.API_ENDPOINT + "api/currencies", {
    }).subscribe(response => {
      this.currencies = response;
      this.isLoading = false;
    }, err => {
      console.log(err.status)
      this.isLoading = false;
    });

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
        (<any>this.settings).logo = (<any>event1.target).result;
      }
    }
  } 

  Save(form: NgForm) {
    if (form.invalid)
      return;
    if (this.settings.id > 0) {
      this.http.put(AppSettings.API_ENDPOINT + "api/settings/" + this.settings.id, this.settings, {
      }).subscribe(response => {
        //this.snackbar.open("Settings updated!");

      }, err => {
        console.log(err);
        this.snackbar.open("error updating settings!");
      });
    } else {
      this.settings.token = this.auth.getOrgTokenFromMasterToken();
      this.settings.userid = this.auth.getUserId();
      this.http.post(AppSettings.API_ENDPOINT + "api/settings/", this.settings, {
      }).subscribe(response => {
        //this.snackbar.open("Settings updated!");
        this.dialogRef.close(true);
      }, err => {
        console.log(err);
        this.snackbar.open("error updating settings!");
      });
    }    
  }
  
}

