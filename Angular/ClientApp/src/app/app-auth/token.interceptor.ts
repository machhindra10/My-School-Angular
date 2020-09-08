import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,

  HttpErrorResponse,
  HttpParams
} from '@angular/common/http';

import { Observable } from 'rxjs/Observable';
import { AuthService } from './auth.service';
import { catchError } from 'rxjs/operators';
import { of } from 'rxjs';
import { error } from 'util';
import { Router } from '@angular/router';
import { CustomSnackbarService } from '../shared/snackbar.service';
import { Temp1Service } from '../shared/temp1.service';

@Injectable()
export class TokenInterceptor implements HttpInterceptor {

  constructor(private auth: AuthService, private router: Router, private snackbar: CustomSnackbarService, private temp1: Temp1Service) { }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    let token: string = this.auth.getToken();
    let database: string = this.auth.getDbNameFromMasterToken();

    //database = 'ngSchool_demo';

    if (this.temp1.ignoreHeader) {
      //console.log("ignored");

      request = request.clone({
        setHeaders: {
          //Authorization: `Bearer ${token}`,
          //"Content-Type": "application/json"
        }
      });
    }
    else if (database == "") {
      this.router.navigate(["servicelogin"]);
    }
    else {
      request = request.clone({
        setHeaders: {
          Authorization: `Bearer ${token}`,
          "Content-Type": "application/json",
          "database": database
        }
      });
    }

    //return next.handle(request);

    return next.handle(request).pipe(catchError((error, caught) => {
      this.handleAuthError(error);
      return of(error);
    }) as any);
  }

  /**
   * manage errors
   * @param err
   * @returns {any}
   */
  private handleAuthError(err: HttpErrorResponse): Observable<any> {
    //console.log('handled error ' + err.status);
    if (err.status === 404) {
      /*Record not found error*/
      this.router.navigate(["notfound"]);
      return of(err);
    }
    else if (err.status === 500) {
      /*Internal server error (database)*/
      console.log(err);
      this.snackbar.open("Error updating database!");
      return of(err);
    }
    throw err;
  }
}
