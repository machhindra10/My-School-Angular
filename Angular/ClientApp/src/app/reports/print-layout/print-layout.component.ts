import { Component, OnInit } from '@angular/core';
import { PrintService } from '../print.service';

@Component({
  selector: 'app-print-layout',
  templateUrl: './print-layout.component.html',
  styleUrls: ['./print-layout.component.css']
})
export class PrintLayoutComponent implements OnInit {
  getdate = new Date();
  constructor(public printService: PrintService,) { }

  layout: string;
  ngOnInit() {
    this.layout = this.printService.layout;
  }
}
