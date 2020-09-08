import { Component, OnInit, Input, OnDestroy } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { PrintService } from '../print.service';
import { FadeAnimation } from '../../../app/shared/animations';
import { HttpClient } from '@angular/common/http';
import { AppSettings } from '../../../app/app-settings';
import { Title } from '@angular/platform-browser';

@Component({
  selector: 'app-studentidcard',
  templateUrl: './studentidcard.component.html',
  styleUrls: ['./studentidcard.component.css'],
  animations: [FadeAnimation]
})
export class RptStudentIdCardComponent implements OnInit, OnDestroy {
  batchid: number;
  classid: number;
  students = [];
  batches: any = [];
  classes: any = [];
  isLoading: boolean = false;

  logo1: any;
  appname: any;
  address: any;
  cols: string = "2";

  @Input() takeprint: boolean = false;
  displayedColumns: string[] = ['srno', 'fname', 'email', 'aadharno'];

  constructor(private http: HttpClient, route: ActivatedRoute, public printService: PrintService,
    private titleservice: Title) {
    let params = route.snapshot.params['id'];
    if (params != null) {
      this.classid = route.snapshot.params['id'].split(',')[0];
      this.batchid = route.snapshot.params['id'].split(',')[1];
      this.cols = route.snapshot.params['id'].split(',')[2];
    }
    this.takeprint = printService.takeprint;

  }
  ngOnDestroy() {
    this.printService.takeprint = false;

  }
  ngOnInit() {
    this.titleservice.setTitle("Student Identity Cards");

    this.GetClasses();
    this.GetBatches();
    this.GetLogo();

    if (this.classid != null && this.batchid != null) {
      this.getStudentsByClass();
    }
  }

  getClassName() {
    let classname = "All";
    if (this.classes.filter(c => c.id == this.classid).length > 0) {
      classname = this.classes.filter(c => c.id == this.classid)[0].standard;
    }
    return classname;
  }

  load() {
    if (this.classid != null && this.batchid != null) {
      this.getStudentsByClass();
    }
  }
  GetClasses() {
    this.isLoading = true;
    this.http.get(AppSettings.API_ENDPOINT + "api/classes/getclassesenabled", {
    }).toPromise().then(response => {
      this.classes = response;
      this.isLoading = false;
    }, err => {
      console.log(err)
      this.isLoading = false;
    });

  }

  getStudentsByClass() {
    this.http.get(AppSettings.API_ENDPOINT + "api/students/getstudentsforidentitycards/" + this.classid + "/" + this.batchid, {
    }).toPromise().then(response => {
      this.students = (<any>response);

      if (this.takeprint) {
        this.printService.onDataReady();
      }
    }, err => {
      console.log(err)
    });

  }
  GetBatches() {
    this.isLoading = true;
    this.http.get(AppSettings.API_ENDPOINT + "api/batches", {
    }).toPromise().then(response => {
      this.batches = response;
      this.isLoading = false;
      let selected = this.batches.filter(o => o.isactive == true);
      //console.log(selected);
      this.batchid = selected[0].id;
    }, err => {
      console.log(err)
      this.isLoading = false;
    });

  }

  GetLogo() {
    this.http.get(AppSettings.API_ENDPOINT + "api/settings/1", {
    }).toPromise().then(response => {
      this.logo1 = (<any>response).logo;
      this.appname = (<any>response).appname;
      this.address = (<any>response).address;

    }, err => {
      console.log(err);
    });
  }

  printDocument() {
    this.printService.title = "Students Identity Cards";
    this.printService.takeprint = true;
    this.printService.layout = 'portrait';
    const invoiceIds = [this.classid.toString(), this.batchid.toString(), this.cols.toString()];
    this.printService.printDocument('studentidcards', invoiceIds);
  }


}
