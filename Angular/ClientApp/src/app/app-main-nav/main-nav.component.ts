import { Component, OnInit, AfterViewInit, Output, EventEmitter, ViewChild } from '@angular/core';
import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { CanActivate, Router, RouterStateSnapshot } from '@angular/router';

import { AppSettings } from '../app-settings';
import { AuthService } from '../app-auth/auth.service';
import { LoaderService } from '../shared/loader.service';
import { FadeAnimation } from '../shared/animations';


@Component({
  selector: 'app-main-nav',
  templateUrl: './main-nav.component.html',
  styleUrls: ['./main-nav.component.css'],
  animations: [FadeAnimation]
})


export class MainNavComponent implements OnInit, AfterViewInit {

  isHandset$: Observable<boolean> = this.breakpointObserver.observe(Breakpoints.Handset)
    .pipe(
      map(result => result.matches)
    );
  showLoader: boolean;
  constructor(private breakpointObserver: BreakpointObserver, private http: HttpClient,
    private router: Router, private auth: AuthService, private loaderService: LoaderService) { }

  @ViewChild('drawer') sideNavdrawer;  

  isLoading: boolean = false;
  menus: any;
  logo1;
  appname: string = "";
  appnameTemp: string = "";
  userphoto;
  notify_count: number;

  ngOnInit() {
    this.loaderService.status.subscribe((val: boolean) => {
      this.showLoader = val;
    });
    this.GetMenus();
    this.GetLogo();
  }

  getNotificationCount(response) {    
    this.notify_count = response.count;
  }

  ngAfterViewInit() {    
    this.GetUserPhoto();    
  }

  public toggleSideNav() {
    if (this.sideNavdrawer.mode == "over") {
      this.sideNavdrawer.toggle();
    }
  }

  myProfile() {
    this.router.navigate(["myprofile"]);
  }

  GetBatchName() {
    let batchid = this.auth.getBatchId();
    this.http.get(AppSettings.API_ENDPOINT + "api/batches/" + batchid, {
    }).subscribe(response => {
      this.appnameTemp = this.appnameTemp + " (" + (<any>response).batch + ")";
      this.appname = (window.innerWidth <= 700) ? this.appnameTemp.substring(0, 10).toString() + " ..." : this.appnameTemp;

    }, err => {
      console.log(err);

    });
  }

 

  GetMenus() {
    this.isLoading = true;
    this.http.get(AppSettings.API_ENDPOINT + "api/menu", {
    }).subscribe(response => {
      this.menus = response;
      this.isLoading = false;
    }, err => {
      console.log(err);
      this.isLoading = false;
    });
  }

  GetLogo() {
    this.http.get(AppSettings.API_ENDPOINT + "api/settings/1", {
    }).toPromise().then(response => {
      this.logo1 = (<any>response).logo;
      this.appnameTemp = (<any>response).appname;
      this.GetBatchName();
    }, err => {
      console.log(err);
    });
  }

  GetUserPhoto() {
    let userid = this.auth.getUserId();
    this.http.get(AppSettings.API_ENDPOINT + "api/users/getuserphoto/" + userid, {
    }).toPromise().then(response => {
      this.userphoto = (<any>response).userphoto;
    }, err => {
      console.log(err);
    });
  }


  isUserAuthenticated() {
    if (this.auth.isAuthenticated()) {
      return true;
    }
    else {
      this.router.navigate(["login"]);

      return false;
    }
  }

  logOut() {
    this.auth.removeToken();
    this.router.navigate(["login"]);
  }

  onResize(event) {
    this.appname = (event.target.innerWidth <= 700) ? this.appnameTemp.substring(0, 10).toString() + " ..." : this.appnameTemp;
  }
}
