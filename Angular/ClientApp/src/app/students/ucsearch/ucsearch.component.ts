import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute, Router } from '@angular/router';
import { AppSettings } from '../../app-settings';
import { CustomSnackbarService } from '../../shared/snackbar.service';
import { FadeAnimation } from '../../shared/animations';
import { Title } from '@angular/platform-browser';
import { FormControl } from '@angular/forms';
import { Observable } from 'rxjs';
import { startWith, map } from 'rxjs/operators';

@Component({
  selector: 'app-ucsearch',
  templateUrl: './ucsearch.component.html',
  styleUrls: ['./ucsearch.component.css'],
  animations: [FadeAnimation]
})

export class UCSearchStudentComponent implements OnInit {
  constructor(private router: Router, private http: HttpClient, private route: ActivatedRoute,
    private snackbar: CustomSnackbarService, private titleService: Title) { }

  @Output() afterSelect = new EventEmitter<number>();
  studentid = 0;
  isLoading: boolean = false;
  students: any = [];
 
  myControl = new FormControl();
  filteredOptions: Observable<any>;


  ngOnInit() {
    
    this.GetStudents();

    this.filteredOptions = this.myControl.valueChanges
      .pipe(
        startWith<string | any>(''),
        map(value => typeof value === 'string' ? value : value.fname),
        map(value => value ? this._filter(value) : this.students.slice())
      );
  }

  getSelectedOption(value) {
    this.afterSelect.emit(value.id); 
  } 

  displayFn(option?: any): string | undefined {
    return option ? option.fname + ' ' + option.mname + ' ' + option.lname : undefined;
  }

  private _filter(value: string): any {
    const filterValue = value.toLowerCase();

    return this.students.filter(option => option.fname.toLowerCase().indexOf(filterValue) === 0
      || option.mname.toLowerCase().indexOf(filterValue) === 0
      || option.lname.toLowerCase().indexOf(filterValue) === 0
      || option.prnno.toLowerCase().indexOf(filterValue) === 0);
  }

  GetStudents() {
    this.isLoading = true;
    this.http.get(AppSettings.API_ENDPOINT + "api/students/getstudentforsearch", {
    }).toPromise().then(response => {
      this.students = (<any>response);
      this.isLoading = false;
    }, err => {
      console.log(err.status)
      this.isLoading = false;
    });
  }


}

