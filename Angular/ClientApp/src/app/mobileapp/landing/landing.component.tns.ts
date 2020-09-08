import { Component, OnInit } from '@angular/core';
import { RouterExtensions } from 'nativescript-angular/router';
import { AlertMobileService } from '../shared/alert-mobile.service';
import { AuthMobileService } from '../auth/auth-mobile.service';

@Component({
  selector: 'app-landing',
  templateUrl: './landing.component.html',
  styleUrls: ['./landing.component.css']
})
export class LandingComponent implements OnInit {

  processing: boolean = false;

  constructor(private routerExtensions: RouterExtensions, private auth: AuthMobileService, private alertService: AlertMobileService) { }

  ngOnInit() {
    this.initial();
  }

  initial() {
    this.processing = true;
    //this.auth.removeMasterToken();

    let database = this.auth.getDbNameFromMasterToken();
    //this.alertService.alert(database);
    if (database == "" || database == null) {
      this.routerExtensions.navigate(["/parentlogin"], { clearHistory: true });
    }
    else
      this.routerExtensions.navigate(["/listmessages"], { clearHistory: true });
  }
}


