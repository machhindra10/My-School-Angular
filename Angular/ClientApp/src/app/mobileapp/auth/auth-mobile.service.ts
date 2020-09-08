import { Injectable } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';

@Injectable()
export class AuthMobileService {
  constructor(private jwtHelper: JwtHelperService) {

  }

  public setMobileToken(tkn) {
    localStorage.setItem("mobileapp", tkn);
  }

  public removeMobileToken() {
    localStorage.removeItem("mobileapp");
  }

  public getMobileToken(): string {
    return localStorage.getItem('mobileapp');
  }

  public isAuthenticated(): boolean {
    const token = this.getMobileToken();
    return !this.jwtHelper.isTokenExpired(token);
  }

  public getGuardianId(): string {
    const token = this.getMobileToken();
    var v = this.jwtHelper.decodeToken(token);
    return v.guardianid;
  }

  public getGuardianMobile(): number {
    const token = this.getMobileToken();
    var v = this.jwtHelper.decodeToken(token);
    return v.mobile;
  }

  public getGuardianName(): number {
    const token = this.getMobileToken();
    var v = this.jwtHelper.decodeToken(token);
    return v.guardianname;
  }

  public getBatchId(): number {
    const token = this.getMobileToken();
    var v = this.jwtHelper.decodeToken(token);
    return v.batchid;
  }

  public getOrganisationToken(): string {
    const token = this.getMobileToken();
    var v = this.jwtHelper.decodeToken(token);
    return v.orgtoken;
  }

  public getCurrencyCode(): string {
    const token = this.getMobileToken();
    var v = this.jwtHelper.decodeToken(token);
    return v.currcode;
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

}
