import { Injectable } from '@angular/core';
import { alert, prompt } from "tns-core-modules/ui/dialogs";

@Injectable()
export class AlertMobileService {
  constructor() {
  }

  alert(message: string) {
    return alert({
      title: "My School App",
      okButtonText: "OK",
      message: message
    });
  }
}
