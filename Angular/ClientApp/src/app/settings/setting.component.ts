import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { ActivatedRoute, Router } from '@angular/router';
import { NgForm, FormGroup } from '@angular/forms';
import { AppSettings } from '../app-settings';
import { CustomSnackbarService } from '../shared/snackbar.service';
import { FadeAnimation } from '../shared/animations';
import { Title } from '@angular/platform-browser';
import { AuthService } from '../app-auth/auth.service';

@Component({
  selector: 'app-setting',
  templateUrl: './setting.component.html',
  styleUrls: ['./setting.component.css'],
  animations: [FadeAnimation]
})

export class SettingComponent implements OnInit {
  isLoading: boolean = false;
  constructor(private router: Router, private http: HttpClient, private route: ActivatedRoute,
    private snackbar: CustomSnackbarService, private titleService: Title, private auth: AuthService) { }

  settings: any = {};
  timezones: any = [];
  countries: any = [];
  currencies: any = [];
  userid: number = parseInt(this.route.snapshot.paramMap.get("id"));
  breakpoint: number;
  

  ngOnInit() {
    this.titleService.setTitle("Settings");
    this.breakpoint = (window.innerWidth <= 700) ? 1 : 3;
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
    this.http.get(AppSettings.API_ENDPOINT_MASTER + "api/countries", {
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
    this.http.get(AppSettings.API_ENDPOINT_MASTER + "api/currencies", {
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

  //GetYears() {
  //  this.isLoading = true;
  //  this.http.get(AppSettings.API_ENDPOINT + "api/settings/getyear", {
  //  }).subscribe(response => {
  //    this.years1 = response;
  //  }, err => {
  //    console.log(err)
  //  });
  //  this.isLoading = false;
  //}

  Save(form: NgForm) {
    if (form.invalid)
      return;
    if (this.settings.id > 0) {
      this.http.put(AppSettings.API_ENDPOINT + "api/settings/" + this.settings.id, this.settings, {
      }).subscribe(response => {        
        this.snackbar.open("Settings updated!");
        this.router.navigate(["login"]);
      }, err => {
        console.log(err);
        this.snackbar.open("error updating settings!");
      });
    } else {
      this.settings.token = this.auth.getOrgTokenFromMasterToken();
      this.http.post(AppSettings.API_ENDPOINT + "api/settings/", this.settings, {
      }).subscribe(response => {        
        this.snackbar.open("Settings updated!");
        this.router.navigate(["login"]);
      }, err => {
        console.log(err);
        this.snackbar.open("error updating settings!");
      });
    }
  }
  
  onResize(event) {
    this.breakpoint = (event.target.innerWidth <= 700) ? 1 : 3;
  }

}

