import { Component, OnInit, Input, OnDestroy } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { PrintService } from '../print.service';
import { FadeAnimation } from '../../../app/shared/animations';
import { HttpClient } from '@angular/common/http';
import { AppSettings } from '../../../app/app-settings';
import { Title } from '@angular/platform-browser';
import { AuthService } from '../../../app/app-auth/auth.service';

interface Age {
  iyear: number;
  year: string
}

@Component({
  selector: 'app-studentsbyage',
  templateUrl: './studentsbyage.component.html',
  styleUrls: ['./studentsbyage.component.css'],
  animations: [FadeAnimation]
})
export class RptStudentsByAgeComponent implements OnInit, OnDestroy {
  batchid: number;
  classid: number;
  fromYear: any;
  toYear: any;

  batches: any = [];
  students: any = [];
  fromAge: Age[] = [];
  toAge: Age[] = [];
  classes: any = [];
  isLoading: boolean = false;
  isLoading1: boolean = false;
  @Input() takeprint: boolean = false;
  displayedColumns: string[] = ['srno', 'prnno', 'name', 'age'];

  constructor(private http: HttpClient, route: ActivatedRoute, public printService: PrintService,
    private titleService: Title, private auth: AuthService) {
    let params = route.snapshot.params['id'];
    if (params != null) {
      this.classid = route.snapshot.params['id'].split(',')[0];
      this.batchid = route.snapshot.params['id'].split(',')[1];
      this.fromYear = route.snapshot.params['id'].split(',')[2];
      this.toYear = route.snapshot.params['id'].split(',')[3];
    }
    this.takeprint = printService.takeprint;
  }
  ngOnDestroy() {
    this.printService.takeprint = false;

  }
  ngOnInit() {
    this.titleService.setTitle("Students By Age");
    if (this.classid == null) {

      this.getFromAge();
      this.getToAge();

    }
    this.GetClasses();
    this.GetBatches();
    this.load();
  }

  load() {
    if (this.classid != null && this.fromYear != null && this.toYear != null && this.batchid != null) {
      this.isLoading1 = true;
      this.GetStudents();
    }

  }

  getClassName() {
    let classname = "All";
    if (this.classes.filter(c => c.id == this.classid).length > 0) {
      classname = this.classes.filter(c => c.id == this.classid)[0].standard;
    }
    return classname;
  }

  getBatchName() {
    let batchname = "";
    if (this.batches.filter(c => c.id == this.batchid).length > 0) {
      batchname = this.batches.filter(c => c.id == this.batchid)[0].batch;
    }
    return batchname;
  }

  GetBatches() {
    this.isLoading = true;
    this.http.get(AppSettings.API_ENDPOINT + "api/batches", {
    }).toPromise().then(response => {
      this.batches = response;
      this.isLoading = false;
      if (this.batchid == null) {
        let selected = this.batches.filter(o => o.isactive == true);
        this.batchid = selected[0].id;
      }
    }, err => {
      console.log(err)
      this.isLoading = false;
    });

  }

  GetClasses() {
    this.isLoading = true;
    this.http.get(AppSettings.API_ENDPOINT + "api/classes/getclassesenabled", {
    }).toPromise().then(response => {
      this.classes = response;
      this.isLoading = false;
      if (this.classid == null) {
        this.classid = 0;
      }
    }, err => {
      console.log(err)
      this.isLoading = false;
    });

  }

  getFromAge() {
    for (var i = 1; i < 31; i++) {
      let aa: Age = { iyear: i, year: i.toString() }
      this.fromAge.push(aa);
    }
  }

  getToAge() {
    this.toAge = [];
    for (var i = this.fromYear; i < 31; i++) {
      let aa: Age = { iyear: i, year: i.toString() }
      this.toAge.push(aa);
    }
    this.load();
  }

  GetStudents() {
    this.http.get(AppSettings.API_ENDPOINT + "api/students/getbyage/" + this.classid + "/" + this.batchid + "/" + this.fromYear + "/" + this.toYear, {
    }).toPromise().then(response => {
      this.students = response;
      this.isLoading1 = false;
      if (this.takeprint) {
        this.printService.onDataReady();
      }

    }, err => {
      console.log(err)
      this.isLoading1 = false;
    });
  }

  printDocument() {
    this.printService.title = "Students By Age";
    this.printService.takeprint = true;
    this.printService.layout = 'portrait';
    const invoiceIds = [this.classid.toString(), this.batchid.toString(), this.fromYear.toString(), this.toYear.toString()];
    this.printService.printDocument('studentbyage', invoiceIds);
  }

}
