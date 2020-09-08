import { Component, OnInit } from '@angular/core';
import { LoaderService } from '../shared/loader.service';
import { Title } from '@angular/platform-browser';



@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
    showLoader: boolean;

  constructor(private loaderService: LoaderService, private titleService: Title) {
  }
  ngOnInit() {
    this.titleService.setTitle("Home");
    this.loaderService.status.subscribe((val: boolean) => {
      this.showLoader = val;
    });
    //console.log("home");
  }
}
