import { Injectable } from '@angular/core';
import {Router} from '@angular/router';

@Injectable({
  providedIn: 'root'
})

export class PrintService {
  isPrinting = false;
  takeprint: boolean = false;
  layout: string = 'portrait';
  title: string = '';
  showHeader: boolean = true;
  showFooter: boolean = true;
  constructor(private router: Router) { }

  printDocument(documentName: string, documentData: any) {
    this.isPrinting = true;
    this.router.navigate(['/',
      { outlets: {
        'print': ['reports', documentName, documentData.join()]
      }}]);
  }

  onDataReady() {
    setTimeout(() => {
      window.print();
      this.isPrinting = false;
      this.title = "";      
      this.router.navigate([{ outlets: { print: null }}]);
    });
  }
}
