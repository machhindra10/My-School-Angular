import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute, Router } from '@angular/router';
import { NgForm } from '@angular/forms';
import { AppSettings } from '../../app-settings';
import { CustomSnackbarService } from '../../shared/snackbar.service';
import { FadeAnimation } from '../../shared/animations';
import { Title } from '@angular/platform-browser';
import { PrintService } from '../../../app/reports/print.service';
import { Location } from '@angular/common';

@Component({
  selector: 'app-staffdetails',
  templateUrl: './staffdetails.component.html',
  styleUrls: ['./staffdetails.component.css'],
  animations: [FadeAnimation]
})

export class StaffDetailsComponent implements OnInit {
  isLoading: boolean = false;

  constructor(private router: Router, private http: HttpClient, private route: ActivatedRoute,
    private snackbar: CustomSnackbarService, private titleService: Title, public printService: PrintService,
    private location: Location) { }

  reloadchild: boolean = false;
  staffid: number = parseInt(this.route.snapshot.paramMap.get("id"));

  ngOnInit() {
    this.titleService.setTitle("Staff Details");

    if (this.staffid > 0) {
      
    }
  }

  edit() {
    if (this.staffid > 0) {
      this.router.navigate(["addstaff/" + this.staffid]);
    }
  }

  onAfterSelect(id: number) {
    this.staffid = id;
    if (this.staffid > 0) {
      this.router.navigate(["staffdetails/" + this.staffid]);
    }
  }

  onAfterFeesUpdate(result: boolean) {
    if (result) {
      this.reloadchild = !this.reloadchild;
    }
  }

  selectAnotherStudent() {
    this.staffid = 0;
    this.router.navigate(["staffdetails/0"]);
  }

  Save(form: NgForm) {
    if (form.invalid)
      return;

    
  }

   
  Back() {
    
    this.location.back();
    this.staffid = 0;
  }

}

