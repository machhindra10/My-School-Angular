import { Component, OnInit, OnDestroy } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AppSettings } from '../../../app/app-settings';

@Component({
  selector: 'app-print-header',
  templateUrl: './print-header.component.html',
  styleUrls: ['./print-header.component.css']
})
export class PrintHeaderComponent implements OnInit, OnDestroy {
  constructor(private http: HttpClient,) { }
  logo1;
  appname;
  address;

  ngOnInit() {    
    this.GetLogo();
  }

  ngOnDestroy() {
    //console.log('destroyed called');
  }

  GetLogo() {
    this.http.get(AppSettings.API_ENDPOINT + "api/settings/1", {
    }).toPromise().then(response => {
      this.logo1 = (<any>response).logo;
      this.appname = (<any>response).appname;
      this.address = (<any>response).address;

    }, err => {
      console.log(err);
    });
  }
}
