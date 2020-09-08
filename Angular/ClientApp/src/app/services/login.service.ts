import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs/BehaviorSubject';
import { HttpClient } from '@angular/common/http';
import { AppSettings } from '../app-settings';
import { Observable } from 'rxjs';

@Injectable()
export class LoginService {
  public status: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);

  constructor(private http: HttpClient) {

  }

  checkLogin(user: any): Observable<any> {
    return this.http.post(AppSettings.API_ENDPOINT + "api/auth/login", user, {
    }).map(response => response);
  }
}
