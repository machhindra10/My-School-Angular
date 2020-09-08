import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { HomeComponent } from './app-home/home.component';
import { LoginComponent } from './account/login/login.component';
import { AuthGuard } from './app-auth/auth-guard.service';
import { UsersComponent } from './users/users/users.component'
import { AddUserComponent } from './users/adduser/adduser.component';
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
import { RegisterStudentComponent } from './students/register/register.component';
import { StudentsComponent } from './students/students/students.component';
import { SettingComponent } from './settings/setting.component';
import { StudentAdmissionComponent } from './students/admission/admission.component';
import { StudentPaymentComponent } from './students/payment/payment.component';
import { UrlGuard } from './app-auth/url-guard.service';
import { UnauthorizedComponent } from './errors/unauthorized/unauthorized.component';
import { NotFoundComponent } from './errors/notfound/notfound.component';
import { MyProfileComponent } from './users/profile/myprofile.component';
import { AdminDashboardComponent } from './dashboards/admin/admindash.component';
import { StudentAttendanceComponent } from './students/attendance/attendance.component';
import { StudentAttendance1Component } from './students/attendance1/attendance1.component';
import { PrintLayoutComponent } from './reports/print-layout/print-layout.component';
import { PaymentReceiptComponent } from './reports/payreceipt/payreceipt.component';
import { RptStudentByClassComponent } from './reports/studentsbyclass/studentsbyclass.component';
import { RptAttendanceByMonthComponent } from './reports/attendencebymonth/attbymonth.component';
import { AddPayHeadComponent } from './masters/payheads/addpayhead/addpayhead.component';
import { PayHeadsComponent } from './masters/payheads/payheads/payheads.component';
import { ExpensesComponent } from './saccounts/expenses/expenses/expenses.component';
import { AddExpenseComponent } from './saccounts/expenses/addexpense/addexpense.component';
import { StaffSalaryComponent } from './saccounts/staffsalary/staffsalary.component';
import { RptSalarySlipComponent } from './reports/salaryslip/salaryslip.component';
import { StudentDetailsComponent } from './students/details/details.component';
import { BatchesComponent } from './settings/batches/batches/batches.component';
import { AddBatchComponent } from './settings/batches/addbatch/addbatch.component';
import { TeacherDashboardComponent } from './dashboards/teacher/teacherdash.component';
import { AccountantDashboardComponent } from './dashboards/accountant/accountantdash.component';
import { ExamsComponent } from './exams/exams/exams.component';
import { AddExamComponent } from './exams/addexam/addexam.component';
import { ExaminationsComponent } from './exams/examinations/examinations.component';
import { MarksheetComponent } from './exams/marksheet/marksheet.component';
import { SubscriptionExpiredComponent } from './errors/subscriptionexpired/subscriptionexpired.component';
import { AddFirstUserComponent } from './setup/addfirstuser/addfirstuser.component';
import { PreLoginComponent } from './account/pre-login/pre-login.component';
import { LandingComponent } from './landing/landing.component';
import { PasswordRecoveryComponent } from './account/recovery/recovery.component';
import { GotoApplicationComponent } from './setup/gotoapp/gotoapp.component';
import { RptStudentsByAgeComponent } from './reports/studentsbyage/studentsbyage.component';
import { RptDailyExpensesComponent } from './reports/dailyexpenses/dailyexpenses.component';
import { RptCollectionsComponent } from './reports/collections/collections.component';
import { RptSalariesComponent } from './reports/salaries/salaries.component';
import { RptMarksheetsComponent } from './reports/marksheets/marksheets.component';
import { TimeTableComponent } from './masters/timetable/timetable/timetable.component';
import { RptTimeTableComponent } from './reports/timetable/rpttimetable.component';
import { RptTimeTableByTeacherComponent } from './reports/timetablebyteacher/timetablebyteacher.component';
import { StaffAttendanceComponent } from './staff/attendance/staffattendance.component';
import { WeeklyOffComponent } from './settings/weeklyoff/weeklyoff.component';
import { LeaveTypesComponent } from './masters/payroll/leavetypes/leavetypes.component';
import { LeavesComponent } from './masters/payroll/leaves/leaves.component';
import { LeaveApplicationComponent } from './staff/leaveapplication/leaveapplication.component';
import { LeaveApplicationsComponent } from './staff/leaveapplications/leaveapplications.component';
import { SendSMSMsgComponent } from './sms/sendmsg/sendmsg.component';
import { StaffDetailsComponent } from './staff/details/staffdetails.component';
import { MessagesAllComponent } from './messages/messages/messagesall.component';
import { MessageDetailsComponent } from './messages/viewmessage/messagedetails.component';
import { RptStudentIdCardComponent } from './reports/studentidcards/studentidcard.component';


const routes: Routes = [
  { path: 'gotoapp/:token', component: GotoApplicationComponent, },
  { path: 'createuser/:token', component: AddFirstUserComponent, },
  { path: 'authenticating', component: LandingComponent, canActivate: [AuthGuard], data: { authid: "0" }, },
  { path: 'passwordrecovery/:id', component: PasswordRecoveryComponent, },
  { path: 'subscriptionexpired', component: SubscriptionExpiredComponent, },
  { path: 'servicelogin', component: PreLoginComponent, },
  { path: 'login', component: LoginComponent },
  {
    path: 'reports', outlet: 'print', component: PrintLayoutComponent,
    children: [
      { path: 'payreceipt/:id', component: PaymentReceiptComponent },
      { path: 'studentbyclass/:id', component: RptStudentByClassComponent },
      { path: 'attendancebymonth/:id', component: RptAttendanceByMonthComponent },
      { path: 'salaryslip/:id', component: RptSalarySlipComponent },
      { path: 'studentbyage/:id', component: RptStudentsByAgeComponent },
      { path: 'dailyexpensereport/:id', component: RptDailyExpensesComponent },
      { path: 'collectionsreport/:id', component: RptCollectionsComponent },
      { path: 'salariesreport/:id', component: RptSalariesComponent },
      { path: 'marksheets/:id', component: RptMarksheetsComponent },
      { path: 'timetablereport/:id', component: RptTimeTableComponent },
      { path: 'timetablebyteacher/:id', component: RptTimeTableByTeacherComponent },
      { path: 'studentidcards/:id', component: RptStudentIdCardComponent },
    ]
  },
  {
    path: '', component: HomeComponent, canActivate: [AuthGuard],
    children: [
      { path: 'notfound', component: NotFoundComponent, },
      { path: 'unauthorized', component: UnauthorizedComponent, },
      { path: 'users', component: UsersComponent, canActivate: [AuthGuard, UrlGuard], data: { authid: "2" }, },
      { path: 'adduser/:id', component: AddUserComponent, canActivate: [AuthGuard, UrlGuard], data: { authid: "1" }, },
      { path: 'roles', component: RolesComponent, canActivate: [AuthGuard, UrlGuard], data: { authid: "5" }, },
      { path: 'addrole/:id', component: AddRoleComponent, canActivate: [AuthGuard, UrlGuard], data: { authid: "6" }, },
      { path: 'designations', component: DesignationsComponent, canActivate: [AuthGuard, UrlGuard], data: { authid: "12" }, },
      { path: 'adddesignation/:id', component: AddDesignationComponent, canActivate: [AuthGuard, UrlGuard], data: { authid: "11" }, },
      { path: 'subjects', component: SubjectsComponent, canActivate: [AuthGuard, UrlGuard], data: { authid: "35" }, },
      { path: 'addsubject/:id', component: AddSubjectComponent, canActivate: [AuthGuard, UrlGuard], data: { authid: "17" }, },
      { path: 'staff', component: StaffComponent, canActivate: [AuthGuard, UrlGuard], data: { authid: "10" }, },
      { path: 'addstaff/:id', component: AddStaffComponent, canActivate: [AuthGuard, UrlGuard], data: { authid: "4" }, },
      { path: 'classes', component: ClassesComponent, canActivate: [AuthGuard, UrlGuard], data: { authid: "14" }, },
      { path: 'addclass/:id', component: AddClassComponent, canActivate: [AuthGuard, UrlGuard], data: { authid: "13" }, },
      { path: 'register/:id', component: RegisterStudentComponent, canActivate: [AuthGuard, UrlGuard], data: { authid: "3" }, },
      { path: 'students', component: StudentsComponent, canActivate: [AuthGuard, UrlGuard], data: { authid: "9" }, },
      { path: 'settings', component: SettingComponent, canActivate: [AuthGuard, UrlGuard], data: { authid: "7" }, },
      { path: 'admission/:id', component: StudentAdmissionComponent, canActivate: [AuthGuard, UrlGuard], data: { authid: "15" }, },
      { path: 'payment/:id', component: StudentPaymentComponent, canActivate: [AuthGuard, UrlGuard], data: { authid: "24" }, },
      { path: 'myprofile', component: MyProfileComponent, canActivate: [AuthGuard], data: { authid: "" }, },
      { path: 'studentdetails/:id', component: StudentDetailsComponent, canActivate: [AuthGuard, UrlGuard], data: { authid: "25" }, },
      { path: 'admindashboard', component: AdminDashboardComponent, canActivate: [AuthGuard, UrlGuard], data: { authid: "32" }, },
      { path: 'teacherdashboard', component: TeacherDashboardComponent, canActivate: [AuthGuard, UrlGuard], data: { authid: "36" }, },
      { path: 'accountantdashboard', component: AccountantDashboardComponent, canActivate: [AuthGuard, UrlGuard], data: { authid: "37" }, },
      { path: 'attendance1', component: StudentAttendanceComponent, canActivate: [AuthGuard, UrlGuard], data: { authid: "" }, },
      { path: 'attendance', component: StudentAttendance1Component, canActivate: [AuthGuard, UrlGuard], data: { authid: "16" }, },
      { path: 'payheads', component: PayHeadsComponent, canActivate: [AuthGuard, UrlGuard], data: { authid: "27" }, },
      { path: 'addpayhead/:id', component: AddPayHeadComponent, canActivate: [AuthGuard, UrlGuard], data: { authid: "26" }, },
      { path: 'dailyexpenses', component: ExpensesComponent, canActivate: [AuthGuard, UrlGuard], data: { authid: "28" }, },
      { path: 'adddailyexpense/:id', component: AddExpenseComponent, canActivate: [AuthGuard, UrlGuard], data: { authid: "29" }, },
      { path: 'staffsalary', component: StaffSalaryComponent, canActivate: [AuthGuard, UrlGuard], data: { authid: "30" }, },
      { path: 'batches', component: BatchesComponent, canActivate: [AuthGuard, UrlGuard], data: { authid: "33" }, },
      { path: 'addbatch/:id', component: AddBatchComponent, canActivate: [AuthGuard, UrlGuard], data: { authid: "34" }, },
      { path: 'exams', component: ExamsComponent, canActivate: [AuthGuard, UrlGuard], data: { authid: "20" }, },
      { path: 'addexam/:id', component: AddExamComponent, canActivate: [AuthGuard, UrlGuard], data: { authid: "21" }, },
      { path: 'examinations/:id', component: ExaminationsComponent, canActivate: [AuthGuard, UrlGuard], data: { authid: "22" }, },
      { path: 'marksheet/:id', component: MarksheetComponent, canActivate: [AuthGuard, UrlGuard], data: { authid: "23" }, },
      { path: 'timetable/:id', component: TimeTableComponent, canActivate: [AuthGuard, UrlGuard], data: { authid: "42" }, },
      { path: 'staffattendance', component: StaffAttendanceComponent, canActivate: [AuthGuard, UrlGuard], data: { authid: "44" }, },
      { path: 'weeklyoff', component: WeeklyOffComponent, canActivate: [AuthGuard, UrlGuard], data: { authid: "45" }, },
      { path: 'leavetypes', component: LeaveTypesComponent, canActivate: [AuthGuard, UrlGuard], data: { authid: "46" }, },
      { path: 'leaves', component: LeavesComponent, canActivate: [AuthGuard, UrlGuard], data: { authid: "47" }, },
      { path: 'leaveapplication', component: LeaveApplicationComponent, canActivate: [AuthGuard, UrlGuard], data: { authid: "48" }, },
      { path: 'leaveapplications', component: LeaveApplicationsComponent, canActivate: [AuthGuard, UrlGuard], data: { authid: "49" }, },
      { path: 'sendsms', component: SendSMSMsgComponent, canActivate: [AuthGuard, UrlGuard], data: { authid: "50" }, },
      { path: 'staffdetails/:id', component: StaffDetailsComponent, canActivate: [AuthGuard, UrlGuard], data: { authid: "51" }, },
      { path: 'messages', component: MessagesAllComponent, canActivate: [AuthGuard, UrlGuard], data: { authid: "53" }, },
      { path: 'messagedetails/:id', component: MessageDetailsComponent, canActivate: [AuthGuard, UrlGuard], data: { authid: "54" }, },


      { path: 'payreceipt/:id', component: PaymentReceiptComponent },
      { path: 'studentbyclass', component: RptStudentByClassComponent },
      { path: 'attendancebymonth', component: RptAttendanceByMonthComponent },
      { path: 'salaryslip/:id', component: RptSalarySlipComponent },
      { path: 'studentbyage', component: RptStudentsByAgeComponent },
      { path: 'dailyexpensereport/:id', component: RptDailyExpensesComponent },
      { path: 'collectionsreport/:id', component: RptCollectionsComponent },
      { path: 'salariesreport/:id', component: RptSalariesComponent },
      { path: 'marksheets', component: RptMarksheetsComponent },
      { path: 'timetablereport/:id', component: RptTimeTableComponent },
      { path: 'timetablebyteacher/:id', component: RptTimeTableByTeacherComponent },
      { path: 'studentidcards', component: RptStudentIdCardComponent },
    ]
  },


];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
