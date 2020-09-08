import { Injectable } from '@angular/core';
import { CanActivate, Router, RouterStateSnapshot, ActivatedRouteSnapshot } from '@angular/router';
import { AuthService } from './auth.service';
import { HttpClient } from '@angular/common/http';
import { AppSettings } from '../app-settings';
import "rxjs/add/operator/map"; 
import { Observable } from 'rxjs';



@Injectable()
export class UrlGuard implements CanActivate {
  constructor(private http: HttpClient, private router: Router, private auth: AuthService) {
  }
  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) //: Observable<boolean>
  {
    let authid = route.data['authid'];    
    var json = JSON.parse(this.auth.getIdsFromRRToken());
    let length = json.filter(o => o.Id == authid).length;    
    if (length > 0) {
      return true;
    }
    else {
      this.router.navigate(["unauthorized"]); 
      return false;
    }

    ////Use this method to do server side validation
    //return this.http.get(AppSettings.API_ENDPOINT + "api/rolerights/isuserauthorized/" + authid, {
    //}).map(response => {      
    //  if (response) {
    //    return true;
    //  }
    //  this.router.navigate(["unauthorized"]);      
    //  return false;
    //});    
  }
}
