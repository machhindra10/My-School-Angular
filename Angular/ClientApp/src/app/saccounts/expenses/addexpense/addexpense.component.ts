import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { ActivatedRoute, Router } from '@angular/router';
import { NgForm, FormGroup } from '@angular/forms';
import { AppSettings } from '../../../app-settings';
import { CustomSnackbarService } from '../../../shared/snackbar.service';
import { FadeAnimation } from '../../../shared/animations';
import { Title } from '@angular/platform-browser';

@Component({
  selector: 'app-addexpense',
  templateUrl: './addexpense.component.html',
  styleUrls: ['./addexpense.component.css'],
  animations: [FadeAnimation]
})

export class AddExpenseComponent implements OnInit {
  isLoading: boolean = false;
  constructor(private router: Router, private http: HttpClient, private route: ActivatedRoute,
    private snackbar: CustomSnackbarService, private titleService: Title) { }

  expense: any = {};
  id: number = parseInt(this.route.snapshot.paramMap.get("id"));
  breakpoint: number;


  ngOnInit() {
    this.titleService.setTitle("Add Expense");
    this.breakpoint = (window.innerWidth <= 700) ? 1 : 3;    

    this.GetOne();
    
  }  

  GetOne() {
    
    if (this.id > 0) {
      this.isLoading = true;
      this.http.get(AppSettings.API_ENDPOINT + "api/dailyexpenses/" + this.id, {
      }).toPromise().then(response => {
        this.expense = response;
        this.isLoading = false
      }, err => {
        console.log(err.status);
        this.isLoading = false
      });
    } 
  }

  Save(form: NgForm) {
    if (form.invalid)
      return;
    
    this.http.post(AppSettings.API_ENDPOINT + "api/dailyexpenses", this.expense, {
    }).subscribe(response => {
      //if ((<any>response).exists == true) {
      //  console.log((<any>response).exists);
      //  this.snackbar.open("Expense already exists!");
      //  return;
      //}
      this.router.navigate(["dailyexpenses"]);
    }, err => {
      console.log(err)
    });
  }

  Back() {
    this.router.navigate(["dailyexpenses"]);
  }

  onResize(event) {
    this.breakpoint = (event.target.innerWidth <= 700) ? 1 : 3;
  }

}

