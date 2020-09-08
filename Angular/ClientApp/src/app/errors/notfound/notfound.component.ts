import { Component, OnInit } from '@angular/core';
import { Location } from '@angular/common';
import { Title } from '@angular/platform-browser';

@Component({
  selector: 'app-notfound',
  templateUrl: './notfound.component.html',
  styleUrls: ['./notfound.component.css']
})
export class NotFoundComponent implements OnInit {
  
  constructor(private location: Location, private titleService: Title) { }

  ngOnInit() {
    this.titleService.setTitle("Not Found");
  }
  Back() {
    this.location.back();
  }
}
