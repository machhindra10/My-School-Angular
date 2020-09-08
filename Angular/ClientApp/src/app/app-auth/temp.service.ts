import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class TempService {
  
  email: string;

  constructor() { }

  setEmail(email) {
    this.email = email;
  }
  getEmail(): string {
    return this.email;
  }
}
