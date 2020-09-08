import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { ActivatedRoute, Router } from '@angular/router';
import { NgForm, FormGroup } from '@angular/forms';
import { AppSettings } from '../../../app-settings';
import { CustomSnackbarService } from '../../../shared/snackbar.service';
import { FadeAnimation } from '../../../shared/animations';
import { Title } from '@angular/platform-browser';

@Component({
  selector: 'app-addsubject',
  templateUrl: './addsubject.component.html',
  styleUrls: ['./addsubject.component.css'],
  animations: [FadeAnimation]
})

export class AddSubjectComponent implements OnInit {
  isLoading: boolean = false;
  staffs: any;
  constructor(private router: Router, private http: HttpClient, private route: ActivatedRoute,
    private snackbar: CustomSnackbarService, private titleService: Title) { }

  subject1: any = {};
  id: number = parseInt(this.route.snapshot.paramMap.get("id"));
  breakpoint: number;


  ngOnInit() {
    this.titleService.setTitle("Add Subject");
    this.breakpoint = (window.innerWidth <= 700) ? 1 : 3;
    this.GetStaffs();
    this.GetOne();

  }
  GetStaffs(): any {
    this.http.get(AppSettings.API_ENDPOINT + "api/staffs/getstaffsenabled", {
    }).subscribe(response => {
      this.staffs = response;
    }, err => {
      console.log(err.status)
    });
  }

  GetOne() {    
    if (this.id > 0) {
      this.isLoading = true;
      this.http.get(AppSettings.API_ENDPOINT + "api/subjects/" + this.id, {
      }).subscribe(response => {
        this.subject1 = response;
        this.isLoading = false;
      }, err => {
        console.log(err.status);
        this.isLoading = false;
      });
    }
    
  }

  Save(form: NgForm) {
    if (form.invalid)
      return;

    this.http.put(AppSettings.API_ENDPOINT + "api/subjects/" + this.id, this.subject1, {
    }).subscribe(response => {
      if ((<any>response).exists == true) {
        console.log((<any>response).exists);
        this.snackbar.open("Subject already exists!");
        return;
      }
      this.router.navigate(["subjects"]);
    }, err => {
      console.log(err)
    });
  }

  Back() {
    this.router.navigate(["subjects"]);
  }

  onResize(event) {
    this.breakpoint = (event.target.innerWidth <= 700) ? 1 : 3;
  }

}

