import { Component, OnInit } from '@angular/core';
import { Location } from '@angular/common';
import { Title } from '@angular/platform-browser';

@Component({
  selector: 'app-unauthorized',
  templateUrl: './unauthorized.component.html',
  styleUrls: ['./unauthorized.component.css']
})
export class UnauthorizedComponent implements OnInit {
  
  constructor(private location: Location, private titleService: Title) { }

  ngOnInit() {
    this.titleService.setTitle("Unauthorized");
  }
  Back() {
    this.location.back();
  }
}
