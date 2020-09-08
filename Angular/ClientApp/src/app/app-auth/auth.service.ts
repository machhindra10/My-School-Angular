import { Injectable } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';

@Injectable()
export class AuthService {
  constructor(private jwtHelper: JwtHelperService) {

  }

  public setToken(tkn) {
    localStorage.setItem("app", tkn);
  }

  public removeToken() {
    localStorage.removeItem("app");
    this.removeRRToken();
  }

  public getToken(): string {
    return localStorage.getItem('app');
  }

  public isAuthenticated(): boolean {
    const token = this.getToken();
    return !this.jwtHelper.isTokenExpired(token);
  }

  public IsUserMasterAdmin(): boolean {
    const token = this.getToken();
    var v = this.jwtHelper.decodeToken(token);
    if (v.ismasteradmin === 'True') {
      return true;
    }
    else {
      return false;
    }
  }

  public getUserId(): number {
    const token = this.getToken();
    var v = this.jwtHelper.decodeToken(token);
    return v.userid;
  }

  public getBatchId(): number {
    const token = this.getToken();
    var v = this.jwtHelper.decodeToken(token);
    return v.batchid;
  }

  public getRoleName(): string {
    const token = this.getToken();
    var v = this.jwtHelper.decodeToken(token);
    return v.rolename;
  }

  public getOrganisationToken(): string {
    const token = this.getToken();
    var v = this.jwtHelper.decodeToken(token);
    return v.orgtoken;
  }

  public getCurrencyCode(): string {
    const token = this.getToken();
    var v = this.jwtHelper.decodeToken(token);
    return v.currcode;
  }

  public getStaffId(): number {
    const token = this.getToken();
    var v = this.jwtHelper.decodeToken(token);
    return v.staffid;
  }

  //Master site token methods
  public setMasterToken(tkn) {
    localStorage.setItem("master", tkn);
  }

  public getMasterToken(): string {
    return localStorage.getItem('master');
  }

  public removeMasterToken() {
    localStorage.removeItem("master");
  }

  public isMasterTokenAuthenticated(): boolean {
    const token = this.getMasterToken();
    return !this.jwtHelper.isTokenExpired(token);
  }

  public getDbNameFromMasterToken(): string {
    const token = this.getMasterToken();
    if (token == null) {
      return "";
    }
    var v = this.jwtHelper.decodeToken(token);
    return v.db;
  }

  public getEmailFromMasterToken(): string {
    const token = this.getMasterToken();
    if (token == null) {
      return "";
    }
    var v = this.jwtHelper.decodeToken(token);
    return v.email;
  }

  public isMasterTokenAvailable(): boolean {
    const token = this.getMasterToken();
    if (token != null) {
      return true;
    } else {
      return false;
    }
  }

  public getOrgIdFromMasterToken(): string {
    const token = this.getMasterToken();
    if (token == null) {
      return "";
    }
    var v = this.jwtHelper.decodeToken(token);
    return v.orgid;
  }

  public getOrgTokenFromMasterToken(): string {
    const token = this.getMasterToken();
    if (token == null) {
      return "";
    }
    var v = this.jwtHelper.decodeToken(token);
    return v.orgtoken;
  }


  //ids Token
  public setRRToken(tkn) {
    localStorage.setItem("rr", tkn);
  }

  public getRRToken(): string {
    return localStorage.getItem('rr');
  }

  public removeRRToken() {
    localStorage.removeItem("rr");
  }

  public getIdsFromRRToken(): string {
    const token = this.getRRToken();
    if (token == null) {
      return "[]";
    }
    var v = this.jwtHelper.decodeToken(token);
    return v.ids;
  }

}
