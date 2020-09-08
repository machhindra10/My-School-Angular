import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { ActivatedRoute, Router } from '@angular/router';
import { NgForm, FormGroup } from '@angular/forms';
import { AppSettings } from '../../../app-settings';
import { CustomSnackbarService } from '../../../shared/snackbar.service';
import { FadeAnimation } from '../../../shared/animations';
import { Title } from '@angular/platform-browser';

@Component({
  selector: 'app-addclass',
  templateUrl: './addclass.component.html',
  styleUrls: ['./addclass.component.css'],
  animations: [FadeAnimation]
})

export class AddClassComponent implements OnInit {
  isLoading: boolean = false;
  constructor(private router: Router, private http: HttpClient, private route: ActivatedRoute,
    private snackbar: CustomSnackbarService, private titleService: Title) { }

  displayedColumns1: string[] = ['classteacher', 'enabled'];
  class1: any = {};
  
  id: number = parseInt(this.route.snapshot.paramMap.get("id"));
 
  ngOnInit() {
    this.titleService.setTitle('Add Class');   
    this.GetOne();    
  } 

  GetOne() {
    
    if (this.id > 0) {
      this.isLoading = true;
      this.http.get(AppSettings.API_ENDPOINT + "api/classes/" + this.id, {
      }).toPromise().then(response => {
        this.class1 = response;
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

    this.http.put(AppSettings.API_ENDPOINT + "api/classes/" + this.id, this.class1, {
    }).subscribe(response => {
      if ((<any>response).exists == true) {
        console.log((<any>response).exists);
        this.snackbar.open("Standard already exists!");
        return;
      }
      this.router.navigate(["classes"]);
    }, err => {
      console.log(err)
    });
  }

  Back() {
    this.router.navigate(["classes"]);
  }   
}

