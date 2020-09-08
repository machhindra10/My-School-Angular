import { Component, OnInit } from '@angular/core';
import { Location } from '@angular/common';
import { Title } from '@angular/platform-browser';
import { AppSettings } from '../../../app/app-settings';

@Component({
  selector: 'app-subscriptionexpired',
  templateUrl: './subscriptionexpired.component.html',
  styleUrls: ['./subscriptionexpired.component.css']
})
export class SubscriptionExpiredComponent implements OnInit {
  
  constructor(private location: Location, private titleService: Title) { }
  applink: string = AppSettings.API_ENDPOINT_MASTER;

  ngOnInit() {
    this.titleService.setTitle("Subscription Expired");
  }
  Back() {
    this.location.back();
  }
}
