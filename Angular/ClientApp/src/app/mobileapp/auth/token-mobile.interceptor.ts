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
import { AuthService } from '../../app-auth/auth.service';
import { catchError } from 'rxjs/operators';
import { of } from 'rxjs';
import { Router } from '@angular/router';
import { AuthMobileService } from './auth-mobile.service';


@Injectable()
export class TokenMobileInterceptor implements HttpInterceptor {

  constructor(private auth: AuthMobileService, private router: Router) { }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    let token: string = this.auth.getMobileToken();
    let database: string = this.auth.getDbNameFromMasterToken();

    //token = null;

    request = request.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`,
        "Content-Type": "application/json",
        "database": database
      }
    });

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
    console.log('handled error ' + err.status);
    if (err.status === 404) {
      /*Record not found error*/
      return of(err);
    }
    else if (err.status === 500) {
      /*Internal server error (database)*/
      return of(err);
    }
    throw err;
  }
}
