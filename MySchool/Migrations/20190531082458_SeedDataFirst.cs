using Microsoft.EntityFrameworkCore.Migrations;

namespace MySchool.Migrations
{
    public partial class SeedDataFirst : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //Casts
            migrationBuilder.Sql("SET IDENTITY_INSERT [mCast] ON");
            migrationBuilder.Sql("INSERT [mCast] ([id], [code], [castname], [disabled]) VALUES (1, N'UR', N'Un Reserved', 0)");
            migrationBuilder.Sql("INSERT [mCast] ([id], [code], [castname], [disabled]) VALUES (2, N'SC', N'SC', 0)");
            migrationBuilder.Sql("INSERT [mCast] ([id], [code], [castname], [disabled]) VALUES (3, N'ST', N'ST', 0)");
            migrationBuilder.Sql("INSERT [mCast] ([id], [code], [castname], [disabled]) VALUES (4, N'OBC', N'Other Backward Classes', 0)");
            migrationBuilder.Sql("INSERT [mCast] ([id], [code], [castname], [disabled]) VALUES (5, N'SBC', N'Special Backward Class', 0)");
            migrationBuilder.Sql("INSERT [mCast] ([id], [code], [castname], [disabled]) VALUES (6, N'VJ', N'Vimukta Jati', 0)");
            migrationBuilder.Sql("INSERT [mCast] ([id], [code], [castname], [disabled]) VALUES (7, N'NT-B', N'Nomadic Tribes - B', 0)");
            migrationBuilder.Sql("INSERT [mCast] ([id], [code], [castname], [disabled]) VALUES (8, N'NT-C', N'Nomadic Tribes - C', 0)");
            migrationBuilder.Sql("INSERT [mCast] ([id], [code], [castname], [disabled]) VALUES (9, N'NT-D', N'Nomadic Tribes - D', 0)");
            migrationBuilder.Sql("SET IDENTITY_INSERT [mCast] OFF");

            //Designations
            migrationBuilder.Sql("SET IDENTITY_INSERT [mDesignation] ON ");
            migrationBuilder.Sql("INSERT [mDesignation] ([id], [designname], [disabled], [isdefault]) VALUES (1, N'Administrator', 0, 1)");
            migrationBuilder.Sql("INSERT [mDesignation] ([id], [designname], [disabled], [isdefault]) VALUES (2, N'Teacher', 0, 1)");
            migrationBuilder.Sql("INSERT [mDesignation] ([id], [designname], [disabled], [isdefault]) VALUES (3, N'Accountant', 0, 1)");
            migrationBuilder.Sql("SET IDENTITY_INSERT [mDesignation] OFF ");

            //Gender
            migrationBuilder.Sql("SET IDENTITY_INSERT [mGender] ON");
            migrationBuilder.Sql("INSERT [mGender] ([id], [gname]) VALUES (1, N'Male')");
            migrationBuilder.Sql("INSERT [mGender] ([id], [gname]) VALUES (2, N'Female')");
            migrationBuilder.Sql("INSERT [mGender] ([id], [gname]) VALUES (3, N'Unknown')");
            migrationBuilder.Sql("SET IDENTITY_INSERT [mGender] OFF");

            //Payment Modes
            migrationBuilder.Sql("SET IDENTITY_INSERT [mPaymentModes] ON");
            migrationBuilder.Sql("INSERT [mPaymentModes] ([id], [modename]) VALUES (1, N'Cash')");
            migrationBuilder.Sql("INSERT [mPaymentModes] ([id], [modename]) VALUES (2, N'Cheque')");
            migrationBuilder.Sql("INSERT [mPaymentModes] ([id], [modename]) VALUES (3, N'Online')");
            migrationBuilder.Sql("SET IDENTITY_INSERT [mPaymentModes] OFF");

            //Settings Others
            migrationBuilder.Sql("SET IDENTITY_INSERT [SettingsOther] ON ");
            migrationBuilder.Sql("INSERT [SettingsOther] ([id], [smtpemailid], [smtppassword], [smtpport], [smtphost], [smsusername], [smspassword], [smskey], [smssenderid], [smsprofileid]) VALUES (1, N'myschool.care57@gmail.com', N'8SYKdU6nfqBFBoTcliyt8Q==', 587, N'smtp.gmail.com', N'', N'', N'010VGsbGBu0Wr8Fg1FOE', N'NEURON', N'100240')");
            migrationBuilder.Sql("SET IDENTITY_INSERT [SettingsOther] OFF");

            //Roles
            migrationBuilder.Sql("SET IDENTITY_INSERT [mRoles] ON");
            migrationBuilder.Sql("INSERT [mRoles] ([id], [rolename], [userid],  [isadmin], [isdefault], [disabled]) VALUES (1, N'Administrator', 1,  1, 1, 0)");
            migrationBuilder.Sql("INSERT [mRoles] ([id], [rolename], [userid],  [isadmin], [isdefault], [disabled]) VALUES (2, N'Accountant', 1,  0, 1, 0)");
            migrationBuilder.Sql("INSERT [mRoles] ([id], [rolename], [userid],  [isadmin], [isdefault], [disabled]) VALUES (3, N'Teacher', 1,  0, 1, 0)");
            migrationBuilder.Sql("SET IDENTITY_INSERT [mRoles] OFF");

            //Menu
            migrationBuilder.Sql("SET IDENTITY_INSERT[Menu] ON");
            migrationBuilder.Sql("INSERT [Menu] ([Id], [menu], [url], [visible], [sort], [isdefault], [icon]) VALUES (1, N'Students', N'/Students', 0, 3, 0, N'group')");
            migrationBuilder.Sql("INSERT [Menu] ([Id], [menu], [url], [visible], [sort], [isdefault], [icon]) VALUES (2, N'Admin', N'/users', 0, 8, 0, N'supervised_user_circle')");
            migrationBuilder.Sql("INSERT [Menu] ([Id], [menu], [url], [visible], [sort], [isdefault], [icon]) VALUES (3, N'Home', N'/Home', 0, 1, 1, NULL)");
            migrationBuilder.Sql("INSERT [Menu] ([Id], [menu], [url], [visible], [sort], [isdefault], [icon]) VALUES (4, N'Dashboards', N'/customers', 0, 2, 0, N'home')");
            migrationBuilder.Sql("INSERT [Menu] ([Id], [menu], [url], [visible], [sort], [isdefault], [icon]) VALUES (6, N'Staff', N'/Staff', 0, 4, 0, N'group')");
            migrationBuilder.Sql("INSERT [Menu] ([Id], [menu], [url], [visible], [sort], [isdefault], [icon]) VALUES (7, N'Settings', N'/Settings', 0, 9, 0, N'settings')");
            migrationBuilder.Sql("INSERT [Menu] ([Id], [menu], [url], [visible], [sort], [isdefault], [icon]) VALUES (8, N'Reports', N'/Reports', 0, 10, 0, N'library_books')");
            migrationBuilder.Sql("INSERT [Menu] ([Id], [menu], [url], [visible], [sort], [isdefault], [icon]) VALUES (9, N'Masters', N'/Masters', 0, 7, 0, N'chrome_reader_mode')");
            migrationBuilder.Sql("INSERT [Menu] ([Id], [menu], [url], [visible], [sort], [isdefault], [icon]) VALUES (10, N'Examination', N'/Exams', 0, 5, 0, N'assignment')");
            migrationBuilder.Sql("INSERT [Menu] ([Id], [menu], [url], [visible], [sort], [isdefault], [icon]) VALUES (11, N'Accounts', N'/accounts', 0, 6, 0, N'money')");
            migrationBuilder.Sql("SET IDENTITY_INSERT[Menu] OFF ");

            //Rights
            migrationBuilder.Sql("SET IDENTITY_INSERT [mRights] ON ");
            migrationBuilder.Sql("INSERT [mRights] ([id], [menuid], [displayname], [rname], [url], [visible], [sort], [groupid], [authid], [parentid]) VALUES (1, 2, N'Add/Modify User', N'Add User', N'/adduser', 0, 2, 1, N'1', NULL)");
            migrationBuilder.Sql("INSERT [mRights] ([id], [menuid], [displayname], [rname], [url], [visible], [sort], [groupid], [authid], [parentid]) VALUES (2, 2, N'View Users', N'Users', N'/users', 1, 1, 1, N'2', NULL)");
            migrationBuilder.Sql("INSERT [mRights] ([id], [menuid], [displayname], [rname], [url], [visible], [sort], [groupid], [authid], [parentid]) VALUES (3, 1, N'Add/Modify Student', N'Registration', N'/register/0', 1, 5, 12, N'3', NULL)");
            migrationBuilder.Sql("INSERT [mRights] ([id], [menuid], [displayname], [rname], [url], [visible], [sort], [groupid], [authid], [parentid]) VALUES (4, 9, N'Add/Modify Staff', N'Add Staff', N'/addstaff', 0, 26, 3, N'4', NULL)");
            migrationBuilder.Sql("INSERT [mRights] ([id], [menuid], [displayname], [rname], [url], [visible], [sort], [groupid], [authid], [parentid]) VALUES (5, 2, N'View Roles', N'Roles', N'/roles', 1, 3, 4, N'5', NULL)");
            migrationBuilder.Sql("INSERT [mRights] ([id], [menuid], [displayname], [rname], [url], [visible], [sort], [groupid], [authid], [parentid]) VALUES (6, 2, N'Add/Modify Roles', N'Add Role', N'/addrole', 0, 4, 4, N'6', NULL)");
            migrationBuilder.Sql("INSERT [mRights] ([id], [menuid], [displayname], [rname], [url], [visible], [sort], [groupid], [authid], [parentid]) VALUES (7, 7, N'Settings', N'School Information', N'/settings', 1, 10, 6, N'7', NULL)");
            migrationBuilder.Sql("INSERT [mRights] ([id], [menuid], [displayname], [rname], [url], [visible], [sort], [groupid], [authid], [parentid]) VALUES (8, 8, N'Students By Class', N'Students By Class', N'/studentbyclass', 1, 11, 7, N'8', NULL)");
            migrationBuilder.Sql("INSERT [mRights] ([id], [menuid], [displayname], [rname], [url], [visible], [sort], [groupid], [authid], [parentid]) VALUES (9, 1, N'View Students', N'Students', N'/students', 1, 32, 2, N'9', NULL)");
            migrationBuilder.Sql("INSERT [mRights] ([id], [menuid], [displayname], [rname], [url], [visible], [sort], [groupid], [authid], [parentid]) VALUES (10, 6, N'View Staff / Employees', N'Staff / Employees', N'/staff', 1, 39, 3, N'10', NULL)");
            migrationBuilder.Sql("INSERT [mRights] ([id], [menuid], [displayname], [rname], [url], [visible], [sort], [groupid], [authid], [parentid]) VALUES (11, 9, N'Add Designation', N'Add Designation', N'/adddesignation', 0, 28, 8, N'11', NULL)");
            migrationBuilder.Sql("INSERT [mRights] ([id], [menuid], [displayname], [rname], [url], [visible], [sort], [groupid], [authid], [parentid]) VALUES (12, 9, N'Designations', N'Designations', N'/designations', 1, 29, 8, N'12', NULL)");
            migrationBuilder.Sql("INSERT [mRights] ([id], [menuid], [displayname], [rname], [url], [visible], [sort], [groupid], [authid], [parentid]) VALUES (13, 9, N'Add Class', N'Add Class', N'/addclass', 0, 15, 9, N'13', NULL)");
            migrationBuilder.Sql("INSERT [mRights] ([id], [menuid], [displayname], [rname], [url], [visible], [sort], [groupid], [authid], [parentid]) VALUES (14, 9, N'Classes', N'Classes', N'/classes', 1, 14, 9, N'14', NULL)");
            migrationBuilder.Sql("INSERT [mRights] ([id], [menuid], [displayname], [rname], [url], [visible], [sort], [groupid], [authid], [parentid]) VALUES (15, 1, N'Admission', N'Admission', N'/admission/0', 1, 16, 20, N'15', NULL)");
            migrationBuilder.Sql("INSERT [mRights] ([id], [menuid], [displayname], [rname], [url], [visible], [sort], [groupid], [authid], [parentid]) VALUES (16, 1, N'Attendance', N'Attendance', N'/attendance', 1, 19, 11, N'17', NULL)");
            migrationBuilder.Sql("INSERT [mRights] ([id], [menuid], [displayname], [rname], [url], [visible], [sort], [groupid], [authid], [parentid]) VALUES (17, 9, N'Add Subject', N'Add Subject', N'/addsubject', 0, 25, 13, N'20', NULL)");
            migrationBuilder.Sql("INSERT [mRights] ([id], [menuid], [displayname], [rname], [url], [visible], [sort], [groupid], [authid], [parentid]) VALUES (18, 8, N'Attendance By Month', N'Attendance By Month', N'/attendancebymonth', 1, 12, 14, N'21', NULL)");
            migrationBuilder.Sql("INSERT [mRights] ([id], [menuid], [displayname], [rname], [url], [visible], [sort], [groupid], [authid], [parentid]) VALUES (19, 8, N'Students By Age', N'Students By Age', N'/studentbyage', 1, 10, 15, N'22', NULL)");
            migrationBuilder.Sql("INSERT [mRights] ([id], [menuid], [displayname], [rname], [url], [visible], [sort], [groupid], [authid], [parentid]) VALUES (20, 10, N'Exam Schedules', N'Schedules', N'/exams', 1, 23, 16, N'23', NULL)");
            migrationBuilder.Sql("INSERT [mRights] ([id], [menuid], [displayname], [rname], [url], [visible], [sort], [groupid], [authid], [parentid]) VALUES (21, 10, N'Add Exam', N'Add Exam', N'/addexam', 0, 24, 16, N'24', NULL)");
            migrationBuilder.Sql("INSERT [mRights] ([id], [menuid], [displayname], [rname], [url], [visible], [sort], [groupid], [authid], [parentid]) VALUES (22, 10, N'Examinations', N'Examinations', N'/examinations/0', 1, 25, 16, N'25', NULL)");
            migrationBuilder.Sql("INSERT [mRights] ([id], [menuid], [displayname], [rname], [url], [visible], [sort], [groupid], [authid], [parentid]) VALUES (23, 10, N'Marksheet', N'Marksheet', N'/marksheet', 0, 26, 16, N'26', NULL)");
            migrationBuilder.Sql("INSERT [mRights] ([id], [menuid], [displayname], [rname], [url], [visible], [sort], [groupid], [authid], [parentid]) VALUES (24, 11, N'Collect Fees', N'Collect Fees', N'/payment/0', 1, 18, 19, N'28', NULL)");
            migrationBuilder.Sql("INSERT [mRights] ([id], [menuid], [displayname], [rname], [url], [visible], [sort], [groupid], [authid], [parentid]) VALUES (25, 1, N'Search Students', N'Search', N'/studentdetails/0', 1, 1, 18, N'29', NULL)");
            migrationBuilder.Sql("INSERT [mRights] ([id], [menuid], [displayname], [rname], [url], [visible], [sort], [groupid], [authid], [parentid]) VALUES (26, 9, N'Add Payroll Head', N'Add Payroll Head', N'/addpayhead', 0, 1, 21, N'31', 52)");
            migrationBuilder.Sql("INSERT [mRights] ([id], [menuid], [displayname], [rname], [url], [visible], [sort], [groupid], [authid], [parentid]) VALUES (27, 9, N'Payroll Heads', N'Payroll Heads', N'/payheads', 1, 33, 21, N'32', 52)");
            migrationBuilder.Sql("INSERT [mRights] ([id], [menuid], [displayname], [rname], [url], [visible], [sort], [groupid], [authid], [parentid]) VALUES (28, 11, N'Daily Expenses', N'Daily Expenses', N'/dailyexpenses', 1, 34, 22, N'33', NULL)");
            migrationBuilder.Sql("INSERT [mRights] ([id], [menuid], [displayname], [rname], [url], [visible], [sort], [groupid], [authid], [parentid]) VALUES (29, 11, N'Add Dailiy Expense', N'Add Daily Expense', N'/adddailyexpense', 0, 35, 22, N'34', NULL)");
            migrationBuilder.Sql("INSERT [mRights] ([id], [menuid], [displayname], [rname], [url], [visible], [sort], [groupid], [authid], [parentid]) VALUES (30, 11, N'Staff Salary', N'Staff Salary', N'/staffsalary', 1, 36, 23, N'35', NULL)");
            migrationBuilder.Sql("INSERT [mRights] ([id], [menuid], [displayname], [rname], [url], [visible], [sort], [groupid], [authid], [parentid]) VALUES (31, 11, N'Salary Slip', N'Salary Slip', N'/salaryslip/171', 0, 37, 23, N'36', NULL)");
            migrationBuilder.Sql("INSERT [mRights] ([id], [menuid], [displayname], [rname], [url], [visible], [sort], [groupid], [authid], [parentid]) VALUES (32, 4, N'Admin Dashboard', N'Dashboard', N'/admindashboard', 1, 38, 24, N'37', NULL)");
            migrationBuilder.Sql("INSERT [mRights] ([id], [menuid], [displayname], [rname], [url], [visible], [sort], [groupid], [authid], [parentid]) VALUES (33, 7, N'Batch / Year', N'Batch / Year', N'/batches', 1, 11, 1, N'38', NULL)");
            migrationBuilder.Sql("INSERT [mRights] ([id], [menuid], [displayname], [rname], [url], [visible], [sort], [groupid], [authid], [parentid]) VALUES (34, 7, N'Add Batch', N'Add Batch', N'/addbatch', 0, 12, 1, N'39', NULL)");
            migrationBuilder.Sql("INSERT [mRights] ([id], [menuid], [displayname], [rname], [url], [visible], [sort], [groupid], [authid], [parentid]) VALUES (35, 9, N'Subjects', N'Subjects', N'/subjects', 1, 2, 13, N'40', NULL)");
            migrationBuilder.Sql("INSERT [mRights] ([id], [menuid], [displayname], [rname], [url], [visible], [sort], [groupid], [authid], [parentid]) VALUES (36, 4, N'Teachers Dashboard', N'Dashboard', N'/teacherdashboard', 1, 3, 3, N'41', NULL)");
            migrationBuilder.Sql("INSERT [mRights] ([id], [menuid], [displayname], [rname], [url], [visible], [sort], [groupid], [authid], [parentid]) VALUES (37, 4, N'Accountant Dashboard', N'Dashboard', N'/accountantdashboard', 1, 3, 3, N'42', NULL)");
            migrationBuilder.Sql("INSERT [mRights] ([id], [menuid], [displayname], [rname], [url], [visible], [sort], [groupid], [authid], [parentid]) VALUES (38, 8, N'Expenses Report', N'Expenses', N'/dailyexpensereport/0', 1, 13, 15, N'43', NULL)");
            migrationBuilder.Sql("INSERT [mRights] ([id], [menuid], [displayname], [rname], [url], [visible], [sort], [groupid], [authid], [parentid]) VALUES (39, 8, N'Collection Report', N'Fees Collections', N'/collectionsreport/0', 1, 14, 15, N'44', NULL)");
            migrationBuilder.Sql("INSERT [mRights] ([id], [menuid], [displayname], [rname], [url], [visible], [sort], [groupid], [authid], [parentid]) VALUES (40, 8, N'Salaries Report', N'Salaries By Month', N'/salariesreport/0', 1, 15, 15, N'45', NULL)");
            migrationBuilder.Sql("INSERT [mRights] ([id], [menuid], [displayname], [rname], [url], [visible], [sort], [groupid], [authid], [parentid]) VALUES (41, 8, N'Mark Sheets', N'Mark Sheets', N'/marksheets', 1, 16, 15, N'46', NULL)");
            migrationBuilder.Sql("INSERT [mRights] ([id], [menuid], [displayname], [rname], [url], [visible], [sort], [groupid], [authid], [parentid]) VALUES (42, 9, N'Time Table', N'Time Table', N'/timetable/0', 1, 36, 1, N'47', NULL)");
            migrationBuilder.Sql("INSERT [mRights] ([id], [menuid], [displayname], [rname], [url], [visible], [sort], [groupid], [authid], [parentid]) VALUES (43, 8, N'Teacher''s Time Table', N'Timetable By Teacher', N'/timetablebyteacher/0', 1, 17, 15, N'48', NULL)");
            migrationBuilder.Sql("INSERT [mRights] ([id], [menuid], [displayname], [rname], [url], [visible], [sort], [groupid], [authid], [parentid]) VALUES (44, 6, N'Staff''s Attendance', N'Staff''s Attendance', N'/staffattendance', 1, 38, 25, N'49', NULL)");
            migrationBuilder.Sql("INSERT [mRights] ([id], [menuid], [displayname], [rname], [url], [visible], [sort], [groupid], [authid], [parentid]) VALUES (45, 7, N'Weekly Off', N'Weekly Off', N'/weeklyoff', 1, 12, 12, N'50', 52)");
            migrationBuilder.Sql("INSERT [mRights] ([id], [menuid], [displayname], [rname], [url], [visible], [sort], [groupid], [authid], [parentid]) VALUES (46, 9, N'Leave Types', N'Leave Types', N'/leavetypes', 1, 34, 12, N'51', 52)");
            migrationBuilder.Sql("INSERT [mRights] ([id], [menuid], [displayname], [rname], [url], [visible], [sort], [groupid], [authid], [parentid]) VALUES (47, 9, N'Leaves', N'Leaves', N'/leaves', 1, 35, 12, N'52', 52)");
            migrationBuilder.Sql("INSERT [mRights] ([id], [menuid], [displayname], [rname], [url], [visible], [sort], [groupid], [authid], [parentid]) VALUES (48, 6, N'Leave Application', N'Leave Application', N'/leaveapplication', 1, 2, 1, N'53', NULL)");
            migrationBuilder.Sql("INSERT [mRights] ([id], [menuid], [displayname], [rname], [url], [visible], [sort], [groupid], [authid], [parentid]) VALUES (49, 6, N'Leave Applications', N'Leave Applications', N'/leaveapplications', 1, 3, 1, N'54', NULL)");
            migrationBuilder.Sql("INSERT [mRights] ([id], [menuid], [displayname], [rname], [url], [visible], [sort], [groupid], [authid], [parentid]) VALUES (50, 7, N'Send SMS Message', N'Send SMS', N'/sendsms', 1, 39, 1, N'55', NULL)");
            migrationBuilder.Sql("INSERT [mRights] ([id], [menuid], [displayname], [rname], [url], [visible], [sort], [groupid], [authid], [parentid]) VALUES (51, 6, N'Search Staff/Employees', N'Search', N'/staffdetails/0', 1, 1, 1, N'56', NULL)");
            migrationBuilder.Sql("INSERT [mRights] ([id], [menuid], [displayname], [rname], [url], [visible], [sort], [groupid], [authid], [parentid]) VALUES (52, 9, N'Payroll', N'Payroll', NULL, 1, 1, 1, NULL, NULL)");

            migrationBuilder.Sql("SET IDENTITY_INSERT [mRights] OFF");

           


            //Role Rights
            migrationBuilder.Sql("SET IDENTITY_INSERT [mRoleRights] ON ");
            migrationBuilder.Sql("INSERT [mRoleRights] ([id], [role_id], [right_id]) VALUES (1, 1, 5)");
            migrationBuilder.Sql("INSERT [mRoleRights] ([id], [role_id], [right_id]) VALUES (2, 1, 6)");
            migrationBuilder.Sql("INSERT [mRoleRights] ([id], [role_id], [right_id]) VALUES (3, 1, 32)");
            migrationBuilder.Sql("INSERT [mRoleRights] ([id], [role_id], [right_id]) VALUES (4, 1, 25)");
            migrationBuilder.Sql("INSERT [mRoleRights] ([id], [role_id], [right_id]) VALUES (5, 1, 3)");
            migrationBuilder.Sql("INSERT [mRoleRights] ([id], [role_id], [right_id]) VALUES (6, 1, 15)");
            migrationBuilder.Sql("INSERT [mRoleRights] ([id], [role_id], [right_id]) VALUES (7, 1, 16)");
            migrationBuilder.Sql("INSERT [mRoleRights] ([id], [role_id], [right_id]) VALUES (8, 1, 20)");
            migrationBuilder.Sql("INSERT [mRoleRights] ([id], [role_id], [right_id]) VALUES (9, 1, 21)");
            migrationBuilder.Sql("INSERT [mRoleRights] ([id], [role_id], [right_id]) VALUES (10, 1, 22)");
            migrationBuilder.Sql("INSERT [mRoleRights] ([id], [role_id], [right_id]) VALUES (11, 1, 23)");
            migrationBuilder.Sql("INSERT [mRoleRights] ([id], [role_id], [right_id]) VALUES (12, 1, 24)");
            migrationBuilder.Sql("INSERT [mRoleRights] ([id], [role_id], [right_id]) VALUES (13, 1, 28)");
            migrationBuilder.Sql("INSERT [mRoleRights] ([id], [role_id], [right_id]) VALUES (14, 1, 29)");
            migrationBuilder.Sql("INSERT [mRoleRights] ([id], [role_id], [right_id]) VALUES (15, 1, 30)");
            migrationBuilder.Sql("INSERT [mRoleRights] ([id], [role_id], [right_id]) VALUES (16, 1, 31)");
            migrationBuilder.Sql("INSERT [mRoleRights] ([id], [role_id], [right_id]) VALUES (17, 1, 26)");
            migrationBuilder.Sql("INSERT [mRoleRights] ([id], [role_id], [right_id]) VALUES (18, 1, 27)");
            migrationBuilder.Sql("INSERT [mRoleRights] ([id], [role_id], [right_id]) VALUES (19, 1, 35)");
            migrationBuilder.Sql("INSERT [mRoleRights] ([id], [role_id], [right_id]) VALUES (20, 1, 17)");
            migrationBuilder.Sql("INSERT [mRoleRights] ([id], [role_id], [right_id]) VALUES (21, 1, 4)");
            migrationBuilder.Sql("INSERT [mRoleRights] ([id], [role_id], [right_id]) VALUES (22, 1, 9)");
            migrationBuilder.Sql("INSERT [mRoleRights] ([id], [role_id], [right_id]) VALUES (23, 1, 10)");
            migrationBuilder.Sql("INSERT [mRoleRights] ([id], [role_id], [right_id]) VALUES (24, 1, 11)");
            migrationBuilder.Sql("INSERT [mRoleRights] ([id], [role_id], [right_id]) VALUES (25, 1, 12)");
            migrationBuilder.Sql("INSERT [mRoleRights] ([id], [role_id], [right_id]) VALUES (26, 1, 13)");
            migrationBuilder.Sql("INSERT [mRoleRights] ([id], [role_id], [right_id]) VALUES (27, 1, 14)");
            migrationBuilder.Sql("INSERT [mRoleRights] ([id], [role_id], [right_id]) VALUES (28, 1, 1)");
            migrationBuilder.Sql("INSERT [mRoleRights] ([id], [role_id], [right_id]) VALUES (29, 1, 2)");
            migrationBuilder.Sql("INSERT [mRoleRights] ([id], [role_id], [right_id]) VALUES (30, 1, 33)");
            migrationBuilder.Sql("INSERT [mRoleRights] ([id], [role_id], [right_id]) VALUES (31, 1, 34)");
            migrationBuilder.Sql("INSERT [mRoleRights] ([id], [role_id], [right_id]) VALUES (32, 1, 7)");
            migrationBuilder.Sql("INSERT [mRoleRights] ([id], [role_id], [right_id]) VALUES (33, 1, 8)");
            migrationBuilder.Sql("INSERT [mRoleRights] ([id], [role_id], [right_id]) VALUES (34, 1, 18)");
            migrationBuilder.Sql("INSERT [mRoleRights] ([id], [role_id], [right_id]) VALUES (35, 2, 37)");
            migrationBuilder.Sql("INSERT [mRoleRights] ([id], [role_id], [right_id]) VALUES (36, 2, 25)");
            migrationBuilder.Sql("INSERT [mRoleRights] ([id], [role_id], [right_id]) VALUES (37, 2, 3)");
            migrationBuilder.Sql("INSERT [mRoleRights] ([id], [role_id], [right_id]) VALUES (38, 2, 15)");
            migrationBuilder.Sql("INSERT [mRoleRights] ([id], [role_id], [right_id]) VALUES (39, 2, 24)");
            migrationBuilder.Sql("INSERT [mRoleRights] ([id], [role_id], [right_id]) VALUES (40, 2, 28)");
            migrationBuilder.Sql("INSERT [mRoleRights] ([id], [role_id], [right_id]) VALUES (41, 2, 29)");
            migrationBuilder.Sql("INSERT [mRoleRights] ([id], [role_id], [right_id]) VALUES (42, 2, 30)");
            migrationBuilder.Sql("INSERT [mRoleRights] ([id], [role_id], [right_id]) VALUES (43, 2, 31)");
            migrationBuilder.Sql("INSERT [mRoleRights] ([id], [role_id], [right_id]) VALUES (45, 3, 25)");
            migrationBuilder.Sql("INSERT [mRoleRights] ([id], [role_id], [right_id]) VALUES (46, 3, 16)");
            migrationBuilder.Sql("INSERT [mRoleRights] ([id], [role_id], [right_id]) VALUES (47, 3, 22)");
            migrationBuilder.Sql("INSERT [mRoleRights] ([id], [role_id], [right_id]) VALUES (48, 3, 23)");
            migrationBuilder.Sql("INSERT [mRoleRights] ([id], [role_id], [right_id]) VALUES (49, 3, 36)");
            migrationBuilder.Sql("INSERT [mRoleRights] ([id], [role_id], [right_id]) VALUES (50, 1, 38)");
            migrationBuilder.Sql("INSERT [mRoleRights] ([id], [role_id], [right_id]) VALUES (51, 1, 39)");
            migrationBuilder.Sql("INSERT [mRoleRights] ([id], [role_id], [right_id]) VALUES (52, 1, 40)");
            migrationBuilder.Sql("INSERT [mRoleRights] ([id], [role_id], [right_id]) VALUES (53, 1, 19)");
            migrationBuilder.Sql("INSERT [mRoleRights] ([id], [role_id], [right_id]) VALUES (54, 1, 41)");
            migrationBuilder.Sql("INSERT [mRoleRights] ([id], [role_id], [right_id]) VALUES (55, 1, 42)");
            migrationBuilder.Sql("INSERT [mRoleRights] ([id], [role_id], [right_id]) VALUES (56, 1, 43)");
            migrationBuilder.Sql("INSERT [mRoleRights] ([id], [role_id], [right_id]) VALUES (57, 1, 44)");
            migrationBuilder.Sql("INSERT [mRoleRights] ([id], [role_id], [right_id]) VALUES (58, 1, 45)");
            migrationBuilder.Sql("INSERT [mRoleRights] ([id], [role_id], [right_id]) VALUES (59, 1, 46)");
            migrationBuilder.Sql("INSERT [mRoleRights] ([id], [role_id], [right_id]) VALUES (60, 1, 47)");
            migrationBuilder.Sql("INSERT [mRoleRights] ([id], [role_id], [right_id]) VALUES (61, 1, 48)");
            migrationBuilder.Sql("INSERT [mRoleRights] ([id], [role_id], [right_id]) VALUES (62, 1, 49)");
            migrationBuilder.Sql("INSERT [mRoleRights] ([id], [role_id], [right_id]) VALUES (63, 1, 50)");
            migrationBuilder.Sql("INSERT [mRoleRights] ([id], [role_id], [right_id]) VALUES (64, 1, 51)");
            migrationBuilder.Sql("INSERT [mRoleRights] ([id], [role_id], [right_id]) VALUES (65, 2, 48)");
            migrationBuilder.Sql("INSERT [mRoleRights] ([id], [role_id], [right_id]) VALUES (66, 3, 48)");

            migrationBuilder.Sql("SET IDENTITY_INSERT [mRoleRights] OFF");            

            //migrationBuilder.Sql("");
            //migrationBuilder.Sql("");
            //migrationBuilder.Sql("");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
