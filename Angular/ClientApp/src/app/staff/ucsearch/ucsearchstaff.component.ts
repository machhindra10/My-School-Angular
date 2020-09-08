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
  selector: 'app-ucsearchstaff',
  templateUrl: './ucsearchstaff.component.html',
  styleUrls: ['./ucsearchstaff.component.css'],
  animations: [FadeAnimation]
})

export class UCSearchStaffComponent implements OnInit {
  constructor(private router: Router, private http: HttpClient, private route: ActivatedRoute,
    private snackbar: CustomSnackbarService, private titleService: Title) { }

  @Output() afterSelect = new EventEmitter<number>();
  staffid = 0;
  isLoading: boolean = false;
  staffs: any = [];
 
  myControl = new FormControl();
  filteredOptions: Observable<any>;


  ngOnInit() {
    
    this.GetStaff();

    this.filteredOptions = this.myControl.valueChanges
      .pipe(
        startWith<string | any>(''),
        map(value => typeof value === 'string' ? value : value.staffname),
      map(value => value ? this._filter(value) : this.staffs.slice())
      );
  }

  getSelectedOption(value) {
    this.afterSelect.emit(value.id); 
  } 

  displayFn(option?: any): string | undefined {
    return option ? option.staffname : undefined;
  }

  private _filter(value: string): any {
    const filterValue = value.toLowerCase();

    return this.staffs.filter(option => option.staffname.toLowerCase().indexOf(filterValue) === 0);
  }

  GetStaff() {
    this.isLoading = true;
    this.http.get(AppSettings.API_ENDPOINT + "api/staffs/getstaffforsearch", {
    }).toPromise().then(response => {
      this.staffs = (<any>response);
      this.isLoading = false;
    }, err => {
      console.log(err.status)
      this.isLoading = false;
    });
  }


}

