import { Injectable } from '@angular/core';
import { CanActivate, Router, RouterStateSnapshot, ActivatedRouteSnapshot } from '@angular/router';
import { AuthService } from './auth.service';



@Injectable()
export class AuthGuard implements CanActivate {
  constructor(private router: Router, private auth: AuthService) {
  }
  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {

    if (this.auth.isAuthenticated() && this.auth.isMasterTokenAuthenticated()) {
      return true;
    }
    this.router.navigate(["login"], { queryParams: { returnUrl: state.url } });
    return false;
  }
}
