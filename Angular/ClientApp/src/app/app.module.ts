import { BrowserModule, Title } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import {
  MatButtonModule, MatToolbarModule, MatSidenavModule, MatIconModule, MatListModule,
  MatCardModule, MatProgressBarModule, MatExpansionModule, MatSelectModule, MatGridListModule,
  MatSnackBarModule, MatSpinner, MatProgressSpinnerModule, MatSlideToggleModule, MatDatepickerModule,
  MatNativeDateModule, MAT_DATE_LOCALE, MatAutocompleteModule, MatTooltipModule, MatPaginatorModule,
  MatMenuModule, MatBadgeModule, MatCheckboxModule, MatStepperModule
} from '@angular/material';


import { NgModule } from '@angular/core';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app/app.component';
import { JwtHelperService, JwtModule } from '@auth0/angular-jwt';
import { AuthGuard } from './app-auth/auth-guard.service';
import { HomeComponent } from './app-home/home.component';
import { LoginComponent } from './account/login/login.component';
import { MainNavComponent } from './app-main-nav/main-nav.component';
import { LayoutModule } from '@angular/cdk/layout';
import { MatInputModule } from '@angular/material/input';
import { UsersComponent } from './users/users/users.component';
import { MatTableModule } from '@angular/material/table';
import { AddUserComponent } from './users/adduser/adduser.component';
import { SlimLoadingBarModule } from 'ng2-slim-loading-bar';
import { TokenInterceptor } from './app-auth/token.interceptor';
import { AuthService } from './app-auth/auth.service';
import { CustomSnackbarService } from './shared/snackbar.service';
import { LoaderService } from './shared/loader.service';
import { RolesComponent } from './users/roles/roles.component';
import { AddRoleComponent } from './users/addrole/addrole.component';
import { DesignationsComponent } from './masters/designations/designs/designs.component';
import { AddDesignationComponent } from './masters/designations/adddesign/adddesign.component';
import { SubjectsComponent } from './masters/subjects/subjects/subjects.component';
import { AddSubjectComponent } from './masters/subjects/addsubject/addsubject.component';
import { StaffComponent } from './masters/staff/staff/staff.component';
import { AddStaffComponent } from './masters/staff/addstaff/addstaff.component';
import { ClassesComponent } from './masters/classes/classes/classes.component';
import { AddClassComponent } from './masters/classes/addclass/addclass.component';
import { ClassTeachersComponent } from './masters/classes/classteachers/classteachers.component';
import { ClassSubjectsComponent } from './masters/classes/classsubjects/classsubjects.component';
import { FlexLayoutModule } from '@angular/flex-layout';
import { AddClassTeacherComponent } from './masters/classes/addclassteacher/addclassteacher.component';
import { MatDialogModule } from '@angular/material/dialog';
import { ConfirmDialog } from './shared/confirm/confirm-dialog';
import { AddClassSubjectComponent } from './masters/classes/addclasssubject/addclasssubject.component';
import { RegisterStudentComponent } from './students/register/register.component';
import { StudentsComponent } from './students/students/students.component';
import { SettingComponent } from './settings/setting.component';
import { StudentAdmissionComponent } from './students/admission/admission.component';
import { UCStudentComponent } from './students/ucstudent/ucstudent.component';
import { ClassFeesComponent } from './masters/classes/classfees/classfees.component';
import { AddClassFeeComponent } from './masters/classes/addclassfee/addclassfee.component';
import { StudentPaymentComponent } from './students/payment/payment.component';
import { StudentFeesComponent } from './students/studentfees/studentfees.component';
import { AddStudentFeeComponent } from './students/addstudentfee/addstudentfee.component';
import { StudentPaymentsComponent } from './students/studentpayments/studentpayments.component';
import { UCSearchStudentComponent } from './students/ucsearch/ucsearch.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { PaymentSummaryComponent } from './students/paysummary/paysummary.component';
import { UrlGuard } from './app-auth/url-guard.service';
import { UnauthorizedComponent } from './errors/unauthorized/unauthorized.component';
import { NotFoundComponent } from './errors/notfound/notfound.component';
import { MyProfileComponent } from './users/profile/myprofile.component';
import { AdminDashboardComponent } from './dashboards/admin/admindash.component';
import { AttendanceSummaryComponent } from './students/attsummary/attsummary.component';
import { StudentAttendanceComponent } from './students/attendance/attendance.component';
import { StudentAttendance1Component } from './students/attendance1/attendance1.component';
import { AttendanceSummaryChartComponent } from './students/attsummarychart/attsummchart.component';
import { ChartModule } from 'angular-highcharts';
import { PrintService } from './reports/print.service';
import { PrintLayoutComponent } from './reports/print-layout/print-layout.component';
import { PaymentReceiptComponent } from './reports/payreceipt/payreceipt.component';
import { PrintHeaderComponent } from './reports/print-header/print-header.component';
import { RptStudentByClassComponent } from './reports/studentsbyclass/studentsbyclass.component';
import { RptAttendanceByMonthComponent } from './reports/attendencebymonth/attbymonth.component';
import { AddPayHeadComponent } from './masters/payheads/addpayhead/addpayhead.component';
import { PayHeadsComponent } from './masters/payheads/payheads/payheads.component';
import { ExpensesComponent } from './saccounts/expenses/expenses/expenses.component';
import { AddExpenseComponent } from './saccounts/expenses/addexpense/addexpense.component';
import { StaffSalaryComponent } from './saccounts/staffsalary/staffsalary.component';
import { RptSalarySlipComponent } from './reports/salaryslip/salaryslip.component';
import { StudentDetailsComponent } from './students/details/details.component';
import { StatsChartComponent } from './dashboards/statschart/statschart.component';
import { BatchesComponent } from './settings/batches/batches/batches.component';
import { AddBatchComponent } from './settings/batches/addbatch/addbatch.component';
import { AlertDialog } from './shared/alertmessage/alert-dialog';
import { RoleRightsComponent } from './users/rolerights/rolerights.component';
import { TeacherDashboardComponent } from './dashboards/teacher/teacherdash.component';
import { AccountantDashboardComponent } from './dashboards/accountant/accountantdash.component';
import { ExamsComponent } from './exams/exams/exams.component';
import { AddExamComponent } from './exams/addexam/addexam.component';
import { ExaminationsComponent } from './exams/examinations/examinations.component';
import { MarksheetComponent } from './exams/marksheet/marksheet.component';
import { SubscriptionExpiredComponent } from './errors/subscriptionexpired/subscriptionexpired.component';
import { AddFirstUserComponent } from './setup/addfirstuser/addfirstuser.component';
import { CurrencySymbolPipe } from './shared/currencySymbol';
import { PreLoginComponent } from './account/pre-login/pre-login.component';
import { TempService } from './app-auth/temp.service';
import { AddFirstBatchComponent } from './setup/addfirstbatch/addfirstbatch.component';
import { LandingComponent } from './landing/landing.component';
import { AddSettingsComponent } from './setup/addsettings/addsettings.component';
import { PasswordRecoveryComponent } from './account/recovery/recovery.component';
import { NotificationsComponent } from './notifications/notifications.component';
import { GotoApplicationComponent } from './setup/gotoapp/gotoapp.component';
import { RptStudentsByAgeComponent } from './reports/studentsbyage/studentsbyage.component';
import { RptDailyExpensesComponent } from './reports/dailyexpenses/dailyexpenses.component';
import { RptCollectionsComponent } from './reports/collections/collections.component';
import { RptSalariesComponent } from './reports/salaries/salaries.component';
import { RptMarksheetsComponent } from './reports/marksheets/marksheets.component';
import { TimeTableComponent } from './masters/timetable/timetable/timetable.component';
import { TimePickerComponent } from './masters/timetable/timepicker/timepicker.component';
import { TimeSpanPickerComponent } from './masters/timetable/timespanpicker/timespanpicker.component';
import { RptTimeTableComponent } from './reports/timetable/rpttimetable.component';
import { RptTimeTableByTeacherComponent } from './reports/timetablebyteacher/timetablebyteacher.component';
import { StaffAttendanceComponent } from './staff/attendance/staffattendance.component';
import { WeeklyOffComponent } from './settings/weeklyoff/weeklyoff.component';
import { LeaveTypesComponent } from './masters/payroll/leavetypes/leavetypes.component';
import { LeavesComponent } from './masters/payroll/leaves/leaves.component';
import { ConfirmMessageDialog } from './shared/confirm-message/confirm-message';
import { AddLeavesComponent } from './masters/payroll/leaves/addleaves/addleaves.component';
import { AddHolidayComponent } from './masters/payroll/leaves/addholiday/addholiday.component';
import { LeaveApplicationComponent } from './staff/leaveapplication/leaveapplication.component';
import { LeaveApplicationsComponent } from './staff/leaveapplications/leaveapplications.component';
import { AddReasonComponent } from './staff/addreason/addreason.component';
import { RemainingLeavesComponent } from './staff/remainingleaves/remainingleaves.component';
import { UserSelectorComponent } from './users/userselector/userselector.component';
import { SendSMSMsgComponent } from './sms/sendmsg/sendmsg.component';
import { SMSSettingsComponent } from './settings/sms/smssettings.component';
import { UCSearchStaffComponent } from './staff/ucsearch/ucsearchstaff.component';
import { StaffDetailsComponent } from './staff/details/staffdetails.component';
import { UCStaffComponent } from './staff/ucstaff/ucstaff.component';
import { StaffAttendanceSummaryChartComponent } from './staff/staffattsummchart/staffattsummchart.component';
import { StaffLeavesSummaryComponent } from './staff/leavessummary/leavessummary.component';
import { Temp1Service } from './shared/temp1.service';
import { AddGuardianComponent } from './guardian/add-guardian/addguardian.component';
import { MessagesAllComponent } from './messages/messages/messagesall.component';
import { SendMessageComponent } from './messages/sendmessage/sendmessage.component';
import { MessageDetailsComponent } from './messages/viewmessage/messagedetails.component';
import { RptStudentIdCardComponent } from './reports/studentidcards/studentidcard.component';

export function getToken(): string {
  return "";
}


@NgModule({
  declarations: [
    AppComponent, HomeComponent, UnauthorizedComponent, NotFoundComponent, LoginComponent, MainNavComponent,
    UsersComponent, AddUserComponent, RolesComponent, AddRoleComponent,
    DesignationsComponent, AddDesignationComponent, SubjectsComponent, AddSubjectComponent,
    StaffComponent, AddStaffComponent, ClassesComponent, AddClassComponent, ClassTeachersComponent,
    ClassSubjectsComponent, AddClassTeacherComponent,
    ConfirmDialog, AddClassSubjectComponent, RegisterStudentComponent, StudentsComponent, SettingComponent,
    StudentAdmissionComponent, UCStudentComponent, ClassFeesComponent, AddClassFeeComponent,
    StudentPaymentComponent,
    StudentFeesComponent, AddStudentFeeComponent, StudentPaymentsComponent, UCSearchStudentComponent,
    PaymentSummaryComponent, MyProfileComponent, AdminDashboardComponent, AttendanceSummaryComponent,
    StudentAttendanceComponent, StudentAttendance1Component, AttendanceSummaryChartComponent,
    PrintLayoutComponent, PrintHeaderComponent, PaymentReceiptComponent,
    RptStudentByClassComponent, RptAttendanceByMonthComponent, AddPayHeadComponent, PayHeadsComponent,
    ExpensesComponent, AddExpenseComponent, StaffSalaryComponent, RptSalarySlipComponent,
    StudentDetailsComponent,
    StatsChartComponent, BatchesComponent, AddBatchComponent, AlertDialog, RoleRightsComponent,
    TeacherDashboardComponent, AccountantDashboardComponent, ExamsComponent, AddExamComponent,
    ExaminationsComponent, MarksheetComponent, SubscriptionExpiredComponent, AddFirstUserComponent,
    CurrencySymbolPipe, PreLoginComponent, AddFirstBatchComponent, LandingComponent, AddSettingsComponent,
    PasswordRecoveryComponent, NotificationsComponent, GotoApplicationComponent, RptStudentsByAgeComponent,
    RptDailyExpensesComponent, RptCollectionsComponent, RptSalariesComponent, RptMarksheetsComponent,
    TimeTableComponent, TimePickerComponent, TimeSpanPickerComponent, RptTimeTableComponent,
    RptTimeTableByTeacherComponent, StaffAttendanceComponent, WeeklyOffComponent, LeaveTypesComponent,
    LeavesComponent, ConfirmMessageDialog, AddLeavesComponent, AddHolidayComponent, LeaveApplicationComponent,
    LeaveApplicationsComponent, AddReasonComponent, RemainingLeavesComponent, UserSelectorComponent,
    SendSMSMsgComponent, SMSSettingsComponent, UCSearchStaffComponent, StaffDetailsComponent,
    UCStaffComponent, StaffAttendanceSummaryChartComponent, StaffLeavesSummaryComponent,
    AddGuardianComponent, MessagesAllComponent, SendMessageComponent, MessageDetailsComponent,
    RptStudentIdCardComponent

  ],
  imports: [[JwtModule.forRoot({
    config: {
      tokenGetter: getToken
    }
  })],

    BrowserModule, BrowserAnimationsModule, MatButtonModule, FormsModule, ReactiveFormsModule,
    HttpClientModule, AppRoutingModule, LayoutModule, MatToolbarModule, MatSidenavModule,
    MatIconModule, MatListModule, MatInputModule, MatCardModule, MatTableModule, MatProgressBarModule,
    MatExpansionModule, SlimLoadingBarModule, MatSelectModule, MatGridListModule, MatSnackBarModule,
    MatProgressSpinnerModule, MatSlideToggleModule, FlexLayoutModule, MatDialogModule,
    MatDatepickerModule, MatNativeDateModule, MatAutocompleteModule, MatTooltipModule, MatPaginatorModule,
    MatMenuModule, MatBadgeModule, ChartModule, MatCheckboxModule, MatStepperModule,
  ],
  entryComponents: [AddClassTeacherComponent, ConfirmDialog, AddClassSubjectComponent,
    AddClassFeeComponent, AddStudentFeeComponent, AlertDialog, AddFirstBatchComponent, AddSettingsComponent,
    TimePickerComponent, TimeSpanPickerComponent, ConfirmMessageDialog, AddLeavesComponent,
    AddHolidayComponent, AddReasonComponent, RemainingLeavesComponent, UserSelectorComponent,
    AddGuardianComponent, SMSSettingsComponent, SendMessageComponent],
  providers: [JwtHelperService, AuthGuard, AuthService, CustomSnackbarService, LoaderService, Title,
    UrlGuard, PrintService, TempService, Temp1Service,
    { provide: HTTP_INTERCEPTORS, useClass: TokenInterceptor, multi: true },
    { provide: MAT_DATE_LOCALE, useValue: "en-IN" },
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
