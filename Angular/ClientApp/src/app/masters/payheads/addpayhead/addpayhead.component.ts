import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { ActivatedRoute, Router } from '@angular/router';
import { NgForm, FormGroup } from '@angular/forms';
import { AppSettings } from '../../../app-settings';
import { CustomSnackbarService } from '../../../shared/snackbar.service';
import { FadeAnimation } from '../../../shared/animations';
import { Title } from '@angular/platform-browser';

@Component({
  selector: 'app-addpayhead',
  templateUrl: './addpayhead.component.html',
  styleUrls: ['./addpayhead.component.css'],
  animations: [FadeAnimation]
})

export class AddPayHeadComponent implements OnInit {
  isLoading: boolean = false;
  constructor(private router: Router, private http: HttpClient, private route: ActivatedRoute,
    private snackbar: CustomSnackbarService, private titleService: Title) { }

  payrollhead: any = {};
  id: number = parseInt(this.route.snapshot.paramMap.get("id"));
  breakpoint: number;


  ngOnInit() {
    this.titleService.setTitle("Add Payroll Head");
    this.breakpoint = (window.innerWidth <= 700) ? 1 : 3;    

    this.GetOne();
    
  }  

  GetOne() {
    
    if (this.id > 0) {
      this.isLoading = true;
      this.http.get(AppSettings.API_ENDPOINT + "api/payheads/" + this.id, {
      }).toPromise().then(response => {
        this.payrollhead = response;
        this.isLoading = false
        this.payrollhead.amount = 0;
      }, err => {
        console.log(err.status);
        this.isLoading = false
      });
    } 
  }

  Save(form: NgForm) {
    if (form.invalid)
      return;

    this.http.put(AppSettings.API_ENDPOINT + "api/payheads/" + this.id, this.payrollhead, {
    }).subscribe(response => {
      if ((<any>response).exists == true) {
        console.log((<any>response).exists);
        this.snackbar.open("Pay head already exists!");
        return;
      }
      this.router.navigate(["payheads"]);
    }, err => {
      console.log(err)
    });
  }

  Back() {
    this.router.navigate(["payheads"]);
  }

  onResize(event) {
    this.breakpoint = (event.target.innerWidth <= 700) ? 1 : 3;
  }

}

