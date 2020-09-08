using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MySchool.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Batches",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    batch = table.Column<string>(maxLength: 50, nullable: true),
                    startdate = table.Column<DateTime>(type: "date", nullable: true),
                    enddate = table.Column<DateTime>(type: "date", nullable: true),
                    isactive = table.Column<bool>(nullable: true),
                    userid = table.Column<int>(nullable: true),
                    datecreated = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Batches", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "mCast",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    code = table.Column<string>(maxLength: 5, nullable: true),
                    castname = table.Column<string>(maxLength: 50, nullable: true),
                    disabled = table.Column<bool>(nullable: true, defaultValueSql: "((0))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mCast", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "mDesignation",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    designname = table.Column<string>(maxLength: 50, nullable: true, defaultValueSql: "('')"),
                    disabled = table.Column<bool>(nullable: true, defaultValueSql: "((0))"),
                    isdefault = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mDesignation", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Menu",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    menu = table.Column<string>(maxLength: 50, nullable: true, defaultValueSql: "('')"),
                    url = table.Column<string>(nullable: true, defaultValueSql: "('')"),
                    visible = table.Column<bool>(nullable: true, defaultValueSql: "((0))"),
                    sort = table.Column<int>(nullable: true),
                    isdefault = table.Column<bool>(nullable: true, defaultValueSql: "((0))"),
                    icon = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Menu", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "mFees",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    fees_type = table.Column<string>(maxLength: 50, nullable: true, defaultValueSql: "('')"),
                    amount = table.Column<decimal>(nullable: true, defaultValueSql: "((0))"),
                    disabled = table.Column<bool>(nullable: true, defaultValueSql: "((0))"),
                    userid = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mFees", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "mGender",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    gname = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mGender", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "mHoliday",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    holiday = table.Column<string>(maxLength: 50, nullable: true),
                    dates = table.Column<DateTime>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mHoliday", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "mLeaveType",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    code = table.Column<string>(maxLength: 2, nullable: true),
                    leavetype = table.Column<string>(maxLength: 50, nullable: true),
                    iscarryforward = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mLeaveType", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "mPayHead",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    head = table.Column<string>(maxLength: 100, nullable: true),
                    amount = table.Column<decimal>(nullable: true),
                    type = table.Column<string>(maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mPayHead", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "mPaymentModes",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    modename = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mPaymentModes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "mRoles",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    rolename = table.Column<string>(maxLength: 50, nullable: true),
                    userid = table.Column<int>(nullable: true),
                    date_created = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    date_modified = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    isadmin = table.Column<bool>(nullable: true, defaultValueSql: "((0))"),
                    isdefault = table.Column<bool>(nullable: true),
                    disabled = table.Column<bool>(nullable: true, defaultValueSql: "((0))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mRoles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "mWeeklyOff",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    posInMonth = table.Column<string>(maxLength: 10, nullable: true),
                    weekday = table.Column<string>(maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mWeeklyOff", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Settings",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    key = table.Column<string>(nullable: true),
                    logo = table.Column<string>(nullable: true),
                    appname = table.Column<string>(maxLength: 50, nullable: true),
                    address = table.Column<string>(maxLength: 500, nullable: true),
                    city = table.Column<string>(maxLength: 50, nullable: true),
                    state = table.Column<string>(maxLength: 50, nullable: true),
                    country = table.Column<string>(maxLength: 50, nullable: true),
                    currency = table.Column<string>(maxLength: 3, nullable: true),
                    timezoneid = table.Column<string>(maxLength: 150, nullable: true),
                    dbbackuppath = table.Column<string>(maxLength: 500, nullable: true),
                    userid = table.Column<int>(nullable: true),
                    token = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settings", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "SettingsOther",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    smtpemailid = table.Column<string>(maxLength: 50, nullable: true),
                    smtppassword = table.Column<string>(nullable: true),
                    smtpport = table.Column<int>(nullable: false),
                    smtphost = table.Column<string>(maxLength: 50, nullable: true),
                    smsusername = table.Column<string>(maxLength: 50, nullable: true),
                    smspassword = table.Column<string>(nullable: true),
                    smskey = table.Column<string>(nullable: true),
                    smssenderid = table.Column<string>(maxLength: 10, nullable: true),
                    smsprofileid = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SettingsOther", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Student",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    prnno = table.Column<string>(maxLength: 15, nullable: true),
                    fname = table.Column<string>(maxLength: 50, nullable: true),
                    mname = table.Column<string>(maxLength: 50, nullable: true),
                    lname = table.Column<string>(maxLength: 50, nullable: true),
                    gender = table.Column<string>(maxLength: 20, nullable: true),
                    photo = table.Column<string>(nullable: true),
                    dob = table.Column<DateTime>(type: "date", nullable: true),
                    aadharno = table.Column<string>(maxLength: 15, nullable: true),
                    address = table.Column<string>(maxLength: 500, nullable: true),
                    email = table.Column<string>(maxLength: 50, nullable: true),
                    mobile = table.Column<string>(maxLength: 15, nullable: true),
                    phone = table.Column<string>(maxLength: 15, nullable: true),
                    castid = table.Column<int>(nullable: true),
                    guardian_name = table.Column<string>(maxLength: 200, nullable: true),
                    guardian_relation = table.Column<string>(maxLength: 50, nullable: true),
                    guardian_addresss = table.Column<string>(maxLength: 500, nullable: true),
                    guardian_mobile = table.Column<string>(maxLength: 15, nullable: true),
                    disabled = table.Column<bool>(nullable: true),
                    userid = table.Column<int>(nullable: true),
                    datecreated = table.Column<DateTime>(type: "datetime", nullable: true),
                    datemodified = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Student", x => x.id);
                    table.ForeignKey(
                        name: "FK_Student_mCast",
                        column: x => x.castid,
                        principalTable: "mCast",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "mRights",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    menuid = table.Column<int>(nullable: false, defaultValueSql: "((1))"),
                    displayname = table.Column<string>(maxLength: 50, nullable: true, defaultValueSql: "('')"),
                    rname = table.Column<string>(maxLength: 50, nullable: true),
                    url = table.Column<string>(nullable: true),
                    visible = table.Column<bool>(nullable: true, defaultValueSql: "((0))"),
                    sort = table.Column<int>(nullable: true),
                    groupid = table.Column<int>(nullable: true, defaultValueSql: "((0))"),
                    authid = table.Column<string>(maxLength: 50, nullable: true),
                    parentid = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mRights", x => x.id);
                    table.ForeignKey(
                        name: "FK_mRights_Menu",
                        column: x => x.menuid,
                        principalTable: "Menu",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tLeaves",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    leavetypeid = table.Column<long>(nullable: true),
                    leaves = table.Column<decimal>(nullable: true),
                    year = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tLeaves", x => x.id);
                    table.ForeignKey(
                        name: "FK_tLeaves_mLeaveType",
                        column: x => x.leavetypeid,
                        principalTable: "mLeaveType",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "mUser",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    user_name = table.Column<string>(maxLength: 15, nullable: true),
                    password = table.Column<string>(maxLength: 500, nullable: false),
                    fname = table.Column<string>(maxLength: 50, nullable: false),
                    mname = table.Column<string>(maxLength: 50, nullable: true),
                    lname = table.Column<string>(maxLength: 50, nullable: true),
                    photo = table.Column<string>(nullable: true),
                    aadharno = table.Column<string>(maxLength: 15, nullable: true),
                    email = table.Column<string>(maxLength: 50, nullable: false),
                    disabled = table.Column<bool>(nullable: true),
                    role_id = table.Column<int>(nullable: false),
                    userid = table.Column<int>(nullable: false),
                    date_created = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    date_modified = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    is_master_admin = table.Column<bool>(nullable: false),
                    is_admin = table.Column<bool>(nullable: false),
                    passverificationcode = table.Column<string>(nullable: true),
                    lastlogin = table.Column<DateTime>(type: "datetime", nullable: true),
                    currentlogin = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mUser", x => x.id);
                    table.ForeignKey(
                        name: "FK_mUser_mRoles",
                        column: x => x.role_id,
                        principalTable: "mRoles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tStudentFees",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    studentid = table.Column<int>(nullable: true),
                    classid = table.Column<int>(nullable: true),
                    batchid = table.Column<int>(nullable: true),
                    fees_type = table.Column<string>(maxLength: 50, nullable: true),
                    amount = table.Column<decimal>(nullable: true),
                    userid = table.Column<int>(nullable: true),
                    datecreated = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tStudentFees", x => x.id);
                    table.ForeignKey(
                        name: "FK_tStudentFees_Batches",
                        column: x => x.batchid,
                        principalTable: "Batches",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tStudentFees_Student",
                        column: x => x.studentid,
                        principalTable: "Student",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tStudentPayment",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    studentid = table.Column<int>(nullable: true),
                    batchid = table.Column<int>(nullable: true),
                    description = table.Column<string>(maxLength: 100, nullable: true),
                    mode = table.Column<string>(maxLength: 15, nullable: true),
                    chtrno = table.Column<string>(maxLength: 50, nullable: true),
                    amount = table.Column<decimal>(nullable: true, defaultValueSql: "((0))"),
                    userid = table.Column<int>(nullable: true),
                    datecreated = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tStudentPayment", x => x.id);
                    table.ForeignKey(
                        name: "FK_tStudentPayment_Batches",
                        column: x => x.batchid,
                        principalTable: "Batches",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tStudentPayment_Student",
                        column: x => x.studentid,
                        principalTable: "Student",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "mRoleRights",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    role_id = table.Column<int>(nullable: true),
                    right_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mRoleRights", x => x.id);
                    table.ForeignKey(
                        name: "FK_mRoleRights_mRights",
                        column: x => x.right_id,
                        principalTable: "mRights",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_mRoleRights_mRoles",
                        column: x => x.role_id,
                        principalTable: "mRoles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DailyExpenses",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    details = table.Column<string>(maxLength: 500, nullable: true),
                    datecreated = table.Column<DateTime>(type: "datetime", nullable: true),
                    amount = table.Column<decimal>(nullable: true),
                    receiptno = table.Column<string>(maxLength: 500, nullable: true),
                    userid = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyExpenses", x => x.id);
                    table.ForeignKey(
                        name: "FK_DailyExpenses_mUser",
                        column: x => x.userid,
                        principalTable: "mUser",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "mClasses",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    standard = table.Column<string>(maxLength: 50, nullable: true, defaultValueSql: "('')"),
                    capacity = table.Column<int>(nullable: true),
                    userid = table.Column<int>(nullable: true),
                    datecreated = table.Column<DateTime>(type: "datetime", nullable: true),
                    datemodified = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    disabled = table.Column<bool>(nullable: true, defaultValueSql: "((0))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mClasses", x => x.id);
                    table.ForeignKey(
                        name: "FK_mStanderd_mUser",
                        column: x => x.userid,
                        principalTable: "mUser",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "mStaff",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    code = table.Column<string>(maxLength: 10, nullable: true),
                    staffname = table.Column<string>(maxLength: 500, nullable: true, defaultValueSql: "('')"),
                    dob = table.Column<DateTime>(type: "date", nullable: true),
                    photo = table.Column<string>(nullable: true),
                    desigid = table.Column<int>(nullable: true),
                    doj = table.Column<DateTime>(type: "datetime", nullable: true),
                    dol = table.Column<DateTime>(type: "datetime", nullable: true),
                    aadharno = table.Column<string>(maxLength: 15, nullable: true),
                    address = table.Column<string>(nullable: true),
                    email = table.Column<string>(maxLength: 50, nullable: true),
                    mobile = table.Column<string>(maxLength: 13, nullable: true),
                    phone = table.Column<string>(maxLength: 15, nullable: true),
                    disabled = table.Column<bool>(nullable: true, defaultValueSql: "((0))"),
                    userid = table.Column<int>(nullable: true),
                    datecreated = table.Column<DateTime>(type: "datetime", nullable: true),
                    datemodified = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    associateuserid = table.Column<int>(nullable: true, defaultValueSql: "((0))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mStaff", x => x.id);
                    table.ForeignKey(
                        name: "FK_Staff_mDesignation",
                        column: x => x.desigid,
                        principalTable: "mDesignation",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Staff_mUser",
                        column: x => x.userid,
                        principalTable: "mUser",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ClassFees",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    classid = table.Column<int>(nullable: true),
                    fees_type = table.Column<string>(maxLength: 50, nullable: true),
                    amount = table.Column<decimal>(nullable: false),
                    userid = table.Column<int>(nullable: true),
                    datecreated = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassFees", x => x.id);
                    table.ForeignKey(
                        name: "FK_ClassFees_mStanderd",
                        column: x => x.classid,
                        principalTable: "mClasses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "mTimeTable",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    classid = table.Column<int>(nullable: true),
                    batchid = table.Column<int>(nullable: true),
                    fromtime = table.Column<TimeSpan>(nullable: true),
                    totime = table.Column<TimeSpan>(nullable: true),
                    sunday = table.Column<int>(nullable: true),
                    monday = table.Column<int>(nullable: true),
                    tuesday = table.Column<int>(nullable: true),
                    wednesday = table.Column<int>(nullable: true),
                    thursday = table.Column<int>(nullable: true),
                    friday = table.Column<int>(nullable: true),
                    saturday = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mTimeTable", x => x.id);
                    table.ForeignKey(
                        name: "FK_TimeTable_Batches",
                        column: x => x.batchid,
                        principalTable: "Batches",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TimeTable_mClasses",
                        column: x => x.classid,
                        principalTable: "mClasses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tExam",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    batchid = table.Column<int>(nullable: true),
                    classid = table.Column<int>(nullable: true),
                    exam_name = table.Column<string>(maxLength: 500, nullable: true),
                    userid = table.Column<int>(nullable: true),
                    date_created = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tExam", x => x.id);
                    table.ForeignKey(
                        name: "FK_tExam_Batches",
                        column: x => x.batchid,
                        principalTable: "Batches",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tExam_mClasses",
                        column: x => x.classid,
                        principalTable: "mClasses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tStudentAdmission",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    rollno = table.Column<int>(nullable: true),
                    studentid = table.Column<int>(nullable: true),
                    classid = table.Column<int>(nullable: true),
                    batchid = table.Column<int>(nullable: true),
                    userid = table.Column<int>(nullable: true),
                    datecreated = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    cancelled = table.Column<bool>(nullable: true, defaultValueSql: "((0))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tStudentAdmission", x => x.id);
                    table.ForeignKey(
                        name: "FK_tStudentAdmission_Batches",
                        column: x => x.batchid,
                        principalTable: "Batches",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tAdmission_mStanderd",
                        column: x => x.classid,
                        principalTable: "mClasses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tAdmission_Student",
                        column: x => x.studentid,
                        principalTable: "Student",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tStudentAttendence",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    studentid = table.Column<int>(nullable: true),
                    classid = table.Column<int>(nullable: true),
                    batchid = table.Column<int>(nullable: true),
                    ispresent = table.Column<bool>(nullable: true, defaultValueSql: "((0))"),
                    datecreated = table.Column<DateTime>(type: "date", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tStudentAttendence", x => x.id);
                    table.ForeignKey(
                        name: "FK_tStudentAttendence_Batches",
                        column: x => x.batchid,
                        principalTable: "Batches",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tStudentPresenty_mClasses",
                        column: x => x.classid,
                        principalTable: "mClasses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tStudentPresenty_Student",
                        column: x => x.studentid,
                        principalTable: "Student",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tStudentAttendence1",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    studentid = table.Column<int>(nullable: true),
                    classid = table.Column<int>(nullable: true),
                    batchid = table.Column<int>(nullable: true),
                    year = table.Column<int>(nullable: true),
                    month = table.Column<int>(nullable: true),
                    _1 = table.Column<string>(maxLength: 2, nullable: true),
                    _2 = table.Column<string>(maxLength: 2, nullable: true),
                    _3 = table.Column<string>(maxLength: 2, nullable: true),
                    _4 = table.Column<string>(maxLength: 2, nullable: true),
                    _5 = table.Column<string>(maxLength: 2, nullable: true),
                    _6 = table.Column<string>(maxLength: 2, nullable: true),
                    _7 = table.Column<string>(maxLength: 2, nullable: true),
                    _8 = table.Column<string>(maxLength: 2, nullable: true),
                    _9 = table.Column<string>(maxLength: 2, nullable: true),
                    _10 = table.Column<string>(maxLength: 2, nullable: true),
                    _11 = table.Column<string>(maxLength: 2, nullable: true),
                    _12 = table.Column<string>(maxLength: 2, nullable: true),
                    _13 = table.Column<string>(maxLength: 2, nullable: true),
                    _14 = table.Column<string>(maxLength: 2, nullable: true),
                    _15 = table.Column<string>(maxLength: 2, nullable: true),
                    _16 = table.Column<string>(maxLength: 2, nullable: true),
                    _17 = table.Column<string>(maxLength: 2, nullable: true),
                    _18 = table.Column<string>(maxLength: 2, nullable: true),
                    _19 = table.Column<string>(maxLength: 2, nullable: true),
                    _20 = table.Column<string>(maxLength: 2, nullable: true),
                    _21 = table.Column<string>(maxLength: 2, nullable: true),
                    _22 = table.Column<string>(maxLength: 2, nullable: true),
                    _23 = table.Column<string>(maxLength: 2, nullable: true),
                    _24 = table.Column<string>(maxLength: 2, nullable: true),
                    _25 = table.Column<string>(maxLength: 2, nullable: true),
                    _26 = table.Column<string>(maxLength: 2, nullable: true),
                    _27 = table.Column<string>(maxLength: 2, nullable: true),
                    _28 = table.Column<string>(maxLength: 2, nullable: true),
                    _29 = table.Column<string>(maxLength: 2, nullable: true),
                    _30 = table.Column<string>(maxLength: 2, nullable: true),
                    _31 = table.Column<string>(maxLength: 2, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tStudentAttendence1", x => x.id);
                    table.ForeignKey(
                        name: "FK_tStudentAttendence1_Batches",
                        column: x => x.batchid,
                        principalTable: "Batches",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tStudentAttendence1_mClasses",
                        column: x => x.classid,
                        principalTable: "mClasses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tStudentAttendence1_Student",
                        column: x => x.studentid,
                        principalTable: "Student",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ClassTeacher",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    classid = table.Column<int>(nullable: true),
                    staffid = table.Column<int>(nullable: true),
                    from = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    to = table.Column<DateTime>(type: "datetime", nullable: true),
                    isactive = table.Column<bool>(nullable: true, defaultValueSql: "((0))"),
                    userid = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassTeacher", x => x.id);
                    table.ForeignKey(
                        name: "FK_ClassTeacher_mStanderd",
                        column: x => x.classid,
                        principalTable: "mClasses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClassTeacher_Staff",
                        column: x => x.staffid,
                        principalTable: "mStaff",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClassTeacher_mUser",
                        column: x => x.userid,
                        principalTable: "mUser",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LeaveApplication",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    staffid = table.Column<int>(nullable: false),
                    leavetypeid = table.Column<long>(nullable: false),
                    datefrom = table.Column<DateTime>(type: "date", nullable: false),
                    dateto = table.Column<DateTime>(type: "date", nullable: false),
                    description = table.Column<string>(maxLength: 500, nullable: true),
                    status = table.Column<string>(maxLength: 10, nullable: true),
                    reason = table.Column<string>(maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaveApplication", x => x.id);
                    table.ForeignKey(
                        name: "FK_LeaveApplication_mLeaveType",
                        column: x => x.leavetypeid,
                        principalTable: "mLeaveType",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LeaveApplication_mStaff",
                        column: x => x.staffid,
                        principalTable: "mStaff",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "mStaffPayroll",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    staffid = table.Column<int>(nullable: true),
                    head = table.Column<string>(maxLength: 100, nullable: true),
                    amount = table.Column<decimal>(nullable: true),
                    type = table.Column<string>(maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mStaffPayroll", x => x.id);
                    table.ForeignKey(
                        name: "FK_mStaffPayroll_mStaff",
                        column: x => x.staffid,
                        principalTable: "mStaff",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "mSubjects",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    code = table.Column<string>(maxLength: 6, nullable: true),
                    subject = table.Column<string>(maxLength: 50, nullable: true),
                    staffid = table.Column<int>(nullable: true),
                    userid = table.Column<int>(nullable: true),
                    datecreated = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    disabled = table.Column<bool>(nullable: true, defaultValueSql: "((0))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mSubjects", x => x.id);
                    table.ForeignKey(
                        name: "FK_mSubjects_mStaff",
                        column: x => x.staffid,
                        principalTable: "mStaff",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StaffAttendance",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    staffid = table.Column<int>(nullable: true),
                    ispresent = table.Column<string>(maxLength: 2, nullable: true),
                    datecreated = table.Column<DateTime>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaffAttendance", x => x.id);
                    table.ForeignKey(
                        name: "FK_StaffAttendance_mStaff",
                        column: x => x.staffid,
                        principalTable: "mStaff",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StaffAttendance1",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    staffid = table.Column<int>(nullable: false),
                    year = table.Column<int>(nullable: false),
                    month = table.Column<int>(nullable: false),
                    _1 = table.Column<string>(maxLength: 2, nullable: true),
                    _2 = table.Column<string>(maxLength: 2, nullable: true),
                    _3 = table.Column<string>(maxLength: 2, nullable: true),
                    _4 = table.Column<string>(maxLength: 2, nullable: true),
                    _5 = table.Column<string>(maxLength: 2, nullable: true),
                    _6 = table.Column<string>(maxLength: 2, nullable: true),
                    _7 = table.Column<string>(maxLength: 2, nullable: true),
                    _8 = table.Column<string>(maxLength: 2, nullable: true),
                    _9 = table.Column<string>(maxLength: 2, nullable: true),
                    _10 = table.Column<string>(maxLength: 2, nullable: true),
                    _11 = table.Column<string>(maxLength: 2, nullable: true),
                    _12 = table.Column<string>(maxLength: 2, nullable: true),
                    _13 = table.Column<string>(maxLength: 2, nullable: true),
                    _14 = table.Column<string>(maxLength: 2, nullable: true),
                    _15 = table.Column<string>(maxLength: 2, nullable: true),
                    _16 = table.Column<string>(maxLength: 2, nullable: true),
                    _17 = table.Column<string>(maxLength: 2, nullable: true),
                    _18 = table.Column<string>(maxLength: 2, nullable: true),
                    _19 = table.Column<string>(maxLength: 2, nullable: true),
                    _20 = table.Column<string>(maxLength: 2, nullable: true),
                    _21 = table.Column<string>(maxLength: 2, nullable: true),
                    _22 = table.Column<string>(maxLength: 2, nullable: true),
                    _23 = table.Column<string>(maxLength: 2, nullable: true),
                    _24 = table.Column<string>(maxLength: 2, nullable: true),
                    _25 = table.Column<string>(maxLength: 2, nullable: true),
                    _26 = table.Column<string>(maxLength: 2, nullable: true),
                    _27 = table.Column<string>(maxLength: 2, nullable: true),
                    _28 = table.Column<string>(maxLength: 2, nullable: true),
                    _29 = table.Column<string>(maxLength: 2, nullable: true),
                    _30 = table.Column<string>(maxLength: 2, nullable: true),
                    _31 = table.Column<string>(maxLength: 2, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaffAttendance1", x => x.id);
                    table.ForeignKey(
                        name: "FK_StaffAttendance1_mStaff",
                        column: x => x.staffid,
                        principalTable: "mStaff",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StaffLeaves",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    staffid = table.Column<int>(nullable: true),
                    leavetype = table.Column<string>(maxLength: 2, nullable: true),
                    datecreated = table.Column<DateTime>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaffLeaves", x => x.id);
                    table.ForeignKey(
                        name: "FK_StaffLeaves_mStaff",
                        column: x => x.staffid,
                        principalTable: "mStaff",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StaffSalary",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    staffid = table.Column<int>(nullable: true),
                    month = table.Column<int>(nullable: true),
                    year = table.Column<int>(nullable: true),
                    earnings = table.Column<decimal>(nullable: true, defaultValueSql: "((0))"),
                    deductions = table.Column<decimal>(nullable: true, defaultValueSql: "((0))"),
                    adjustments = table.Column<decimal>(nullable: true, defaultValueSql: "((0))"),
                    netpay = table.Column<decimal>(nullable: true),
                    ispaid = table.Column<bool>(nullable: true, defaultValueSql: "((0))"),
                    datepaid = table.Column<DateTime>(type: "datetime", nullable: true),
                    userid = table.Column<int>(nullable: true),
                    datecreated = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaffSalary", x => x.id);
                    table.ForeignKey(
                        name: "FK_StaffSalary_mStaff",
                        column: x => x.staffid,
                        principalTable: "mStaff",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StaffSalary_mUser",
                        column: x => x.userid,
                        principalTable: "mUser",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tExamStudentsAdmitCard",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    batchid = table.Column<int>(nullable: true),
                    examid = table.Column<int>(nullable: true),
                    classid = table.Column<int>(nullable: true),
                    studentid = table.Column<int>(nullable: true),
                    userid = table.Column<int>(nullable: true),
                    date_created = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    disabled = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tExamStudentsAdmitCard", x => x.id);
                    table.ForeignKey(
                        name: "FK_tExamStudentsAdmitCard_Batches",
                        column: x => x.batchid,
                        principalTable: "Batches",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tExamStudentsAdmitCard_mClasses",
                        column: x => x.classid,
                        principalTable: "mClasses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tExamStudentsAdmitCard_tExam",
                        column: x => x.examid,
                        principalTable: "tExam",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tExamStudentsAdmitCard_Student",
                        column: x => x.studentid,
                        principalTable: "Student",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ClassSubjects",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    classid = table.Column<int>(nullable: true),
                    subjectid = table.Column<int>(nullable: true),
                    userid = table.Column<int>(nullable: true),
                    datecreated = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    staffid = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassSubjects", x => x.id);
                    table.ForeignKey(
                        name: "FK_tClassSubjects_mClasses",
                        column: x => x.classid,
                        principalTable: "mClasses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClassSubjects_mStaff_Staffid",
                        column: x => x.staffid,
                        principalTable: "mStaff",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tClassSubjects_mSubjects",
                        column: x => x.subjectid,
                        principalTable: "mSubjects",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tExamSchedule",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    batchid = table.Column<int>(nullable: true),
                    examid = table.Column<int>(nullable: false),
                    classid = table.Column<int>(nullable: true),
                    subjectid = table.Column<int>(nullable: true),
                    totalmarks = table.Column<int>(nullable: true),
                    passingmarks = table.Column<int>(nullable: true),
                    examdate = table.Column<DateTime>(type: "date", nullable: true),
                    starttime = table.Column<TimeSpan>(nullable: true),
                    endtime = table.Column<TimeSpan>(nullable: true),
                    userid = table.Column<int>(nullable: true),
                    date_created = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tExamSchedule", x => x.id);
                    table.ForeignKey(
                        name: "FK_tExamSchedule_Batches",
                        column: x => x.batchid,
                        principalTable: "Batches",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tExamSchedule_mClasses1",
                        column: x => x.classid,
                        principalTable: "mClasses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tExamSchedule_mExams1",
                        column: x => x.examid,
                        principalTable: "tExam",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tExamSchedule_mSubjects",
                        column: x => x.subjectid,
                        principalTable: "mSubjects",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StaffSalaryDetails",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ss_id = table.Column<int>(nullable: true),
                    staffid = table.Column<int>(nullable: true),
                    head = table.Column<string>(maxLength: 100, nullable: true),
                    amount = table.Column<decimal>(nullable: true),
                    type = table.Column<string>(maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaffSalaryDetails", x => x.id);
                    table.ForeignKey(
                        name: "FK_StaffSalaryDetails_StaffSalary",
                        column: x => x.ss_id,
                        principalTable: "StaffSalary",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StaffSalaryDetails_mStaff",
                        column: x => x.staffid,
                        principalTable: "mStaff",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tExamMarkSheet",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    exmsch_id = table.Column<int>(nullable: true),
                    batchid = table.Column<int>(nullable: true),
                    examid = table.Column<int>(nullable: true),
                    classid = table.Column<int>(nullable: true),
                    studentid = table.Column<int>(nullable: true),
                    subjectid = table.Column<int>(nullable: true),
                    obtained = table.Column<int>(nullable: true),
                    practical = table.Column<int>(nullable: true),
                    totalmarks = table.Column<int>(nullable: true),
                    grade = table.Column<string>(maxLength: 2, nullable: true),
                    remarks = table.Column<string>(maxLength: 10, nullable: true),
                    userid = table.Column<int>(nullable: true),
                    date_created = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tExamMarkSheet", x => x.id);
                    table.ForeignKey(
                        name: "FK_tExamMarkSheet_Batches1",
                        column: x => x.batchid,
                        principalTable: "Batches",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tExamMarkSheet_mClasses",
                        column: x => x.classid,
                        principalTable: "mClasses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tExamMarkSheet_mExams",
                        column: x => x.examid,
                        principalTable: "tExam",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tExamMarkSheet_tExamSchedule",
                        column: x => x.exmsch_id,
                        principalTable: "tExamSchedule",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tExamMarkSheet_Student",
                        column: x => x.studentid,
                        principalTable: "Student",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tExamMarkSheet_mSubjects",
                        column: x => x.subjectid,
                        principalTable: "mSubjects",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClassFees_classid",
                table: "ClassFees",
                column: "classid");

            migrationBuilder.CreateIndex(
                name: "IX_ClassSubjects_classid",
                table: "ClassSubjects",
                column: "classid");

            migrationBuilder.CreateIndex(
                name: "IX_ClassSubjects_Staffid",
                table: "ClassSubjects",
                column: "staffid");

            migrationBuilder.CreateIndex(
                name: "IX_ClassSubjects_subjectid",
                table: "ClassSubjects",
                column: "subjectid");

            migrationBuilder.CreateIndex(
                name: "IX_ClassTeacher_classid",
                table: "ClassTeacher",
                column: "classid");

            migrationBuilder.CreateIndex(
                name: "IX_ClassTeacher_staffid",
                table: "ClassTeacher",
                column: "staffid");

            migrationBuilder.CreateIndex(
                name: "IX_ClassTeacher_userid",
                table: "ClassTeacher",
                column: "userid");

            migrationBuilder.CreateIndex(
                name: "IX_DailyExpenses_userid",
                table: "DailyExpenses",
                column: "userid");

            migrationBuilder.CreateIndex(
                name: "IX_LeaveApplication_leavetypeid",
                table: "LeaveApplication",
                column: "leavetypeid");

            migrationBuilder.CreateIndex(
                name: "IX_LeaveApplication_staffid",
                table: "LeaveApplication",
                column: "staffid");

            migrationBuilder.CreateIndex(
                name: "IX_mClasses_userid",
                table: "mClasses",
                column: "userid");

            migrationBuilder.CreateIndex(
                name: "IX_mRights_menuid",
                table: "mRights",
                column: "menuid");

            migrationBuilder.CreateIndex(
                name: "IX_mRoleRights_right_id",
                table: "mRoleRights",
                column: "right_id");

            migrationBuilder.CreateIndex(
                name: "IX_mRoleRights_role_id",
                table: "mRoleRights",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "IX_mStaff_desigid",
                table: "mStaff",
                column: "desigid");

            migrationBuilder.CreateIndex(
                name: "IX_mStaff_userid",
                table: "mStaff",
                column: "userid");

            migrationBuilder.CreateIndex(
                name: "IX_mStaffPayroll_staffid",
                table: "mStaffPayroll",
                column: "staffid");

            migrationBuilder.CreateIndex(
                name: "IX_mSubjects_staffid",
                table: "mSubjects",
                column: "staffid");

            migrationBuilder.CreateIndex(
                name: "IX_mTimeTable_batchid",
                table: "mTimeTable",
                column: "batchid");

            migrationBuilder.CreateIndex(
                name: "IX_mTimeTable_classid",
                table: "mTimeTable",
                column: "classid");

            migrationBuilder.CreateIndex(
                name: "IX_mUser_role_id",
                table: "mUser",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "IX_StaffAttendance_staffid",
                table: "StaffAttendance",
                column: "staffid");

            migrationBuilder.CreateIndex(
                name: "IX_StaffAttendance1_staffid",
                table: "StaffAttendance1",
                column: "staffid");

            migrationBuilder.CreateIndex(
                name: "IX_StaffLeaves_staffid",
                table: "StaffLeaves",
                column: "staffid");

            migrationBuilder.CreateIndex(
                name: "IX_StaffSalary_staffid",
                table: "StaffSalary",
                column: "staffid");

            migrationBuilder.CreateIndex(
                name: "IX_StaffSalary_userid",
                table: "StaffSalary",
                column: "userid");

            migrationBuilder.CreateIndex(
                name: "IX_StaffSalaryDetails_ss_id",
                table: "StaffSalaryDetails",
                column: "ss_id");

            migrationBuilder.CreateIndex(
                name: "IX_StaffSalaryDetails_staffid",
                table: "StaffSalaryDetails",
                column: "staffid");

            migrationBuilder.CreateIndex(
                name: "IX_Student_castid",
                table: "Student",
                column: "castid");

            migrationBuilder.CreateIndex(
                name: "IX_tExam_batchid",
                table: "tExam",
                column: "batchid");

            migrationBuilder.CreateIndex(
                name: "IX_tExam_classid",
                table: "tExam",
                column: "classid");

            migrationBuilder.CreateIndex(
                name: "IX_tExamMarkSheet_batchid",
                table: "tExamMarkSheet",
                column: "batchid");

            migrationBuilder.CreateIndex(
                name: "IX_tExamMarkSheet_classid",
                table: "tExamMarkSheet",
                column: "classid");

            migrationBuilder.CreateIndex(
                name: "IX_tExamMarkSheet_examid",
                table: "tExamMarkSheet",
                column: "examid");

            migrationBuilder.CreateIndex(
                name: "IX_tExamMarkSheet_exmsch_id",
                table: "tExamMarkSheet",
                column: "exmsch_id");

            migrationBuilder.CreateIndex(
                name: "IX_tExamMarkSheet_studentid",
                table: "tExamMarkSheet",
                column: "studentid");

            migrationBuilder.CreateIndex(
                name: "IX_tExamMarkSheet_subjectid",
                table: "tExamMarkSheet",
                column: "subjectid");

            migrationBuilder.CreateIndex(
                name: "IX_tExamSchedule_batchid",
                table: "tExamSchedule",
                column: "batchid");

            migrationBuilder.CreateIndex(
                name: "IX_tExamSchedule_classid",
                table: "tExamSchedule",
                column: "classid");

            migrationBuilder.CreateIndex(
                name: "IX_tExamSchedule_examid",
                table: "tExamSchedule",
                column: "examid");

            migrationBuilder.CreateIndex(
                name: "IX_tExamSchedule_subjectid",
                table: "tExamSchedule",
                column: "subjectid");

            migrationBuilder.CreateIndex(
                name: "IX_tExamStudentsAdmitCard_batchid",
                table: "tExamStudentsAdmitCard",
                column: "batchid");

            migrationBuilder.CreateIndex(
                name: "IX_tExamStudentsAdmitCard_classid",
                table: "tExamStudentsAdmitCard",
                column: "classid");

            migrationBuilder.CreateIndex(
                name: "IX_tExamStudentsAdmitCard_examid",
                table: "tExamStudentsAdmitCard",
                column: "examid");

            migrationBuilder.CreateIndex(
                name: "IX_tExamStudentsAdmitCard_studentid",
                table: "tExamStudentsAdmitCard",
                column: "studentid");

            migrationBuilder.CreateIndex(
                name: "IX_tLeaves_leavetypeid",
                table: "tLeaves",
                column: "leavetypeid");

            migrationBuilder.CreateIndex(
                name: "IX_tStudentAdmission_batchid",
                table: "tStudentAdmission",
                column: "batchid");

            migrationBuilder.CreateIndex(
                name: "IX_tStudentAdmission_classid",
                table: "tStudentAdmission",
                column: "classid");

            migrationBuilder.CreateIndex(
                name: "IX_tStudentAdmission_studentid",
                table: "tStudentAdmission",
                column: "studentid");

            migrationBuilder.CreateIndex(
                name: "IX_tStudentAttendence_batchid",
                table: "tStudentAttendence",
                column: "batchid");

            migrationBuilder.CreateIndex(
                name: "IX_tStudentAttendence_classid",
                table: "tStudentAttendence",
                column: "classid");

            migrationBuilder.CreateIndex(
                name: "IX_tStudentAttendence_studentid",
                table: "tStudentAttendence",
                column: "studentid");

            migrationBuilder.CreateIndex(
                name: "IX_tStudentAttendence1_batchid",
                table: "tStudentAttendence1",
                column: "batchid");

            migrationBuilder.CreateIndex(
                name: "IX_tStudentAttendence1_classid",
                table: "tStudentAttendence1",
                column: "classid");

            migrationBuilder.CreateIndex(
                name: "IX_tStudentAttendence1_studentid",
                table: "tStudentAttendence1",
                column: "studentid");

            migrationBuilder.CreateIndex(
                name: "IX_tStudentFees_batchid",
                table: "tStudentFees",
                column: "batchid");

            migrationBuilder.CreateIndex(
                name: "IX_tStudentFees_studentid",
                table: "tStudentFees",
                column: "studentid");

            migrationBuilder.CreateIndex(
                name: "IX_tStudentPayment_batchid",
                table: "tStudentPayment",
                column: "batchid");

            migrationBuilder.CreateIndex(
                name: "IX_tStudentPayment_studentid",
                table: "tStudentPayment",
                column: "studentid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClassFees");

            migrationBuilder.DropTable(
                name: "ClassSubjects");

            migrationBuilder.DropTable(
                name: "ClassTeacher");

            migrationBuilder.DropTable(
                name: "DailyExpenses");

            migrationBuilder.DropTable(
                name: "LeaveApplication");

            migrationBuilder.DropTable(
                name: "mFees");

            migrationBuilder.DropTable(
                name: "mGender");

            migrationBuilder.DropTable(
                name: "mHoliday");

            migrationBuilder.DropTable(
                name: "mPayHead");

            migrationBuilder.DropTable(
                name: "mPaymentModes");

            migrationBuilder.DropTable(
                name: "mRoleRights");

            migrationBuilder.DropTable(
                name: "mStaffPayroll");

            migrationBuilder.DropTable(
                name: "mTimeTable");

            migrationBuilder.DropTable(
                name: "mWeeklyOff");

            migrationBuilder.DropTable(
                name: "Settings");

            migrationBuilder.DropTable(
                name: "SettingsOther");

            migrationBuilder.DropTable(
                name: "StaffAttendance");

            migrationBuilder.DropTable(
                name: "StaffAttendance1");

            migrationBuilder.DropTable(
                name: "StaffLeaves");

            migrationBuilder.DropTable(
                name: "StaffSalaryDetails");

            migrationBuilder.DropTable(
                name: "tExamMarkSheet");

            migrationBuilder.DropTable(
                name: "tExamStudentsAdmitCard");

            migrationBuilder.DropTable(
                name: "tLeaves");

            migrationBuilder.DropTable(
                name: "tStudentAdmission");

            migrationBuilder.DropTable(
                name: "tStudentAttendence");

            migrationBuilder.DropTable(
                name: "tStudentAttendence1");

            migrationBuilder.DropTable(
                name: "tStudentFees");

            migrationBuilder.DropTable(
                name: "tStudentPayment");

            migrationBuilder.DropTable(
                name: "mRights");

            migrationBuilder.DropTable(
                name: "StaffSalary");

            migrationBuilder.DropTable(
                name: "tExamSchedule");

            migrationBuilder.DropTable(
                name: "mLeaveType");

            migrationBuilder.DropTable(
                name: "Student");

            migrationBuilder.DropTable(
                name: "Menu");

            migrationBuilder.DropTable(
                name: "tExam");

            migrationBuilder.DropTable(
                name: "mSubjects");

            migrationBuilder.DropTable(
                name: "mCast");

            migrationBuilder.DropTable(
                name: "Batches");

            migrationBuilder.DropTable(
                name: "mClasses");

            migrationBuilder.DropTable(
                name: "mStaff");

            migrationBuilder.DropTable(
                name: "mDesignation");

            migrationBuilder.DropTable(
                name: "mUser");

            migrationBuilder.DropTable(
                name: "mRoles");
        }
    }
}
