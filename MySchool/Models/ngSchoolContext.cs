using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using MySchool.Models;

namespace MySchool.Models
{
    public partial class ngSchoolContext : DbContext
    {
        public ngSchoolContext()
        {
        }

        public ngSchoolContext(DbContextOptions<ngSchoolContext> options)
            : base(options)
        {
            //Database.Migrate();
        }

        public virtual DbSet<Batches> Batches { get; set; }
        public virtual DbSet<ClassFees> ClassFees { get; set; }
        public virtual DbSet<ClassSubjects> ClassSubjects { get; set; }
        public virtual DbSet<ClassTeacher> ClassTeacher { get; set; }
        public virtual DbSet<DailyExpenses> DailyExpenses { get; set; }
        public virtual DbSet<LeaveApplication> LeaveApplication { get; set; }
        public virtual DbSet<MCast> MCast { get; set; }
        public virtual DbSet<MClasses> MClasses { get; set; }
        public virtual DbSet<MDesignation> MDesignation { get; set; }
        public virtual DbSet<Menu> Menu { get; set; }
        public virtual DbSet<Messages> Messages { get; set; }
        public virtual DbSet<MessagesGuardians> MessagesGuardians { get; set; }
        public virtual DbSet<MessagesStudents> MessagesStudents { get; set; }
        public virtual DbSet<MFees> MFees { get; set; }
        public virtual DbSet<MGender> MGender { get; set; }
        public virtual DbSet<MHoliday> MHoliday { get; set; }
        public virtual DbSet<MLeaveType> MLeaveType { get; set; }
        public virtual DbSet<MPayHead> MPayHead { get; set; }
        public virtual DbSet<MPaymentModes> MPaymentModes { get; set; }
        public virtual DbSet<MRights> MRights { get; set; }
        public virtual DbSet<MRoleRights> MRoleRights { get; set; }
        public virtual DbSet<MRoles> MRoles { get; set; }
        public virtual DbSet<MStaff> MStaff { get; set; }
        public virtual DbSet<MStaffPayroll> MStaffPayroll { get; set; }
        public virtual DbSet<MSubjects> MSubjects { get; set; }
        public virtual DbSet<MTimeTable> MTimeTable { get; set; }
        public virtual DbSet<MUser> MUser { get; set; }
        public virtual DbSet<MWeeklyOff> MWeeklyOff { get; set; }
        public virtual DbSet<Settings> Settings { get; set; }
        public virtual DbSet<SettingsOther> SettingsOther { get; set; }
        public virtual DbSet<StaffAttendance> StaffAttendance { get; set; }
        public virtual DbSet<StaffAttendance1> StaffAttendance1 { get; set; }
        public virtual DbSet<StaffLeaves> StaffLeaves { get; set; }
        public virtual DbSet<StaffSalary> StaffSalary { get; set; }
        public virtual DbSet<StaffSalaryDetails> StaffSalaryDetails { get; set; }
        public virtual DbSet<Student> Student { get; set; }
        public virtual DbSet<StudentGuardian> StudentGuardian { get; set; }
        public virtual DbSet<TExam> TExam { get; set; }
        public virtual DbSet<TExamMarkSheet> TExamMarkSheet { get; set; }
        public virtual DbSet<TExamSchedule> TExamSchedule { get; set; }
        public virtual DbSet<TExamStudentsAdmitCard> TExamStudentsAdmitCard { get; set; }
        public virtual DbSet<TLeaves> TLeaves { get; set; }
        public virtual DbSet<TStudentAdmission> TStudentAdmission { get; set; }
        public virtual DbSet<TStudentAttendence> TStudentAttendence { get; set; }
        public virtual DbSet<TStudentAttendence1> TStudentAttendence1 { get; set; }
        public virtual DbSet<TStudentFees> TStudentFees { get; set; }
        public virtual DbSet<TStudentPayment> TStudentPayment { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=MACHHINDRA-PC\\HP;Database=ngSchool;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Batches>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Batch)
                    .HasColumnName("batch")
                    .HasMaxLength(50);

                entity.Property(e => e.Datecreated)
                    .HasColumnName("datecreated")
                    .HasColumnType("datetime");

                entity.Property(e => e.Enddate)
                    .HasColumnName("enddate")
                    .HasColumnType("date");

                entity.Property(e => e.Isactive).HasColumnName("isactive");

                entity.Property(e => e.Startdate)
                    .HasColumnName("startdate")
                    .HasColumnType("date");

                entity.Property(e => e.Userid).HasColumnName("userid");
            });

            modelBuilder.Entity<ClassFees>(entity =>
            {
                entity.HasIndex(e => e.Classid);

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Amount).HasColumnName("amount");

                entity.Property(e => e.Classid).HasColumnName("classid");

                entity.Property(e => e.Datecreated)
                    .HasColumnName("datecreated")
                    .HasColumnType("datetime");

                entity.Property(e => e.FeesType)
                    .HasColumnName("fees_type")
                    .HasMaxLength(50);

                entity.Property(e => e.Userid).HasColumnName("userid");

                entity.HasOne(d => d.Class)
                    .WithMany(p => p.ClassFees)
                    .HasForeignKey(d => d.Classid)
                    .HasConstraintName("FK_ClassFees_mStanderd");
            });

            modelBuilder.Entity<ClassSubjects>(entity =>
            {
                entity.HasIndex(e => e.Classid);

                entity.HasIndex(e => e.Staffid)
                    .HasName("IX_ClassSubjects_Staffid");

                entity.HasIndex(e => e.Subjectid);

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Classid).HasColumnName("classid");

                entity.Property(e => e.Datecreated)
                    .HasColumnName("datecreated")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Staffid).HasColumnName("staffid");

                entity.Property(e => e.Subjectid).HasColumnName("subjectid");

                entity.Property(e => e.Userid).HasColumnName("userid");

                entity.HasOne(d => d.Class)
                    .WithMany(p => p.ClassSubjects)
                    .HasForeignKey(d => d.Classid)
                    .HasConstraintName("FK_tClassSubjects_mClasses");

                entity.HasOne(d => d.Staff)
                    .WithMany(p => p.ClassSubjects)
                    .HasForeignKey(d => d.Staffid)
                    .HasConstraintName("FK_ClassSubjects_mStaff_Staffid");

                entity.HasOne(d => d.Subject)
                    .WithMany(p => p.ClassSubjects)
                    .HasForeignKey(d => d.Subjectid)
                    .HasConstraintName("FK_tClassSubjects_mSubjects");
            });

            modelBuilder.Entity<ClassTeacher>(entity =>
            {
                entity.HasIndex(e => e.Classid);

                entity.HasIndex(e => e.Staffid);

                entity.HasIndex(e => e.Userid);

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Classid).HasColumnName("classid");

                entity.Property(e => e.From)
                    .HasColumnName("from")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Isactive)
                    .HasColumnName("isactive")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Staffid).HasColumnName("staffid");

                entity.Property(e => e.To)
                    .HasColumnName("to")
                    .HasColumnType("datetime");

                entity.Property(e => e.Userid).HasColumnName("userid");

                entity.HasOne(d => d.Class)
                    .WithMany(p => p.ClassTeacher)
                    .HasForeignKey(d => d.Classid)
                    .HasConstraintName("FK_ClassTeacher_mStanderd");

                entity.HasOne(d => d.Staff)
                    .WithMany(p => p.ClassTeacher)
                    .HasForeignKey(d => d.Staffid)
                    .HasConstraintName("FK_ClassTeacher_Staff");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ClassTeacher)
                    .HasForeignKey(d => d.Userid)
                    .HasConstraintName("FK_ClassTeacher_mUser");
            });

            modelBuilder.Entity<DailyExpenses>(entity =>
            {
                entity.HasIndex(e => e.Userid);

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Amount).HasColumnName("amount");

                entity.Property(e => e.Datecreated)
                    .HasColumnName("datecreated")
                    .HasColumnType("datetime");

                entity.Property(e => e.Details)
                    .HasColumnName("details")
                    .HasMaxLength(500);

                entity.Property(e => e.Receiptno)
                    .HasColumnName("receiptno")
                    .HasMaxLength(500);

                entity.Property(e => e.Userid).HasColumnName("userid");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.DailyExpenses)
                    .HasForeignKey(d => d.Userid)
                    .HasConstraintName("FK_DailyExpenses_mUser");
            });

            modelBuilder.Entity<LeaveApplication>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Datefrom)
                    .HasColumnName("datefrom")
                    .HasColumnType("date");

                entity.Property(e => e.Dateto)
                    .HasColumnName("dateto")
                    .HasColumnType("date");

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasMaxLength(500);

                entity.Property(e => e.Leavetypeid).HasColumnName("leavetypeid");

                entity.Property(e => e.Reason)
                    .HasColumnName("reason")
                    .HasMaxLength(500);

                entity.Property(e => e.Staffid).HasColumnName("staffid");

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasMaxLength(10);

                entity.HasOne(d => d.Leavetype)
                    .WithMany(p => p.LeaveApplication)
                    .HasForeignKey(d => d.Leavetypeid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LeaveApplication_mLeaveType");

                entity.HasOne(d => d.Staff)
                    .WithMany(p => p.LeaveApplication)
                    .HasForeignKey(d => d.Staffid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LeaveApplication_mStaff");
            });

            modelBuilder.Entity<MCast>(entity =>
            {
                entity.ToTable("mCast");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Castname)
                    .HasColumnName("castname")
                    .HasMaxLength(50);

                entity.Property(e => e.Code)
                    .HasColumnName("code")
                    .HasMaxLength(5);

                entity.Property(e => e.Disabled)
                    .HasColumnName("disabled")
                    .HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<MClasses>(entity =>
            {
                entity.ToTable("mClasses");

                entity.HasIndex(e => e.Userid);

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Capacity).HasColumnName("capacity");

                entity.Property(e => e.Datecreated)
                    .HasColumnName("datecreated")
                    .HasColumnType("datetime");

                entity.Property(e => e.Datemodified)
                    .HasColumnName("datemodified")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Disabled)
                    .HasColumnName("disabled")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Standard)
                    .HasColumnName("standard")
                    .HasMaxLength(50)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Userid).HasColumnName("userid");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.MClasses)
                    .HasForeignKey(d => d.Userid)
                    .HasConstraintName("FK_mStanderd_mUser");
            });

            modelBuilder.Entity<MDesignation>(entity =>
            {
                entity.ToTable("mDesignation");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Designname)
                    .HasColumnName("designname")
                    .HasMaxLength(50)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Disabled)
                    .HasColumnName("disabled")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Isdefault).HasColumnName("isdefault");
            });

            modelBuilder.Entity<Menu>(entity =>
            {
                entity.Property(e => e.Icon)
                    .HasColumnName("icon")
                    .HasMaxLength(50);

                entity.Property(e => e.Isdefault)
                    .HasColumnName("isdefault")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Menu1)
                    .HasColumnName("menu")
                    .HasMaxLength(50)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Sort).HasColumnName("sort");

                entity.Property(e => e.Url)
                    .HasColumnName("url")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Visible)
                    .HasColumnName("visible")
                    .HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<Messages>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Datecreated)
                    .HasColumnName("datecreated")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Message)
                    .IsRequired()
                    .HasColumnName("message");

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .HasMaxLength(3);

                entity.Property(e => e.Userid).HasColumnName("userid");
            });

            modelBuilder.Entity<MessagesGuardians>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Delivered)
                    .HasColumnName("delivered")
                    .HasColumnType("datetime");

                entity.Property(e => e.Guardianid).HasColumnName("guardianid");

                entity.Property(e => e.Messageid).HasColumnName("messageid");

                entity.Property(e => e.Read)
                    .HasColumnName("read")
                    .HasColumnType("datetime");

                entity.HasOne(d => d.Guardian)
                    .WithMany(p => p.MessagesGuardians)
                    .HasForeignKey(d => d.Guardianid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MessagesGuardians_StudentGuardian");

                entity.HasOne(d => d.Message)
                    .WithMany(p => p.MessagesGuardians)
                    .HasForeignKey(d => d.Messageid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MessageRecepients_Messages");
            });

            modelBuilder.Entity<MessagesStudents>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Delivered)
                    .HasColumnName("delivered")
                    .HasColumnType("datetime");

                entity.Property(e => e.Messageid).HasColumnName("messageid");

                entity.Property(e => e.Read)
                    .HasColumnName("read")
                    .HasColumnType("datetime");

                entity.Property(e => e.Studentid).HasColumnName("studentid");

                entity.HasOne(d => d.Message)
                    .WithMany(p => p.MessagesStudents)
                    .HasForeignKey(d => d.Messageid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MessagesStudents_Messages");

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.MessagesStudents)
                    .HasForeignKey(d => d.Studentid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MessagesStudents_Student");
            });

            modelBuilder.Entity<MFees>(entity =>
            {
                entity.ToTable("mFees");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Amount)
                    .HasColumnName("amount")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Disabled)
                    .HasColumnName("disabled")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.FeesType)
                    .HasColumnName("fees_type")
                    .HasMaxLength(50)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Userid).HasColumnName("userid");
            });

            modelBuilder.Entity<MGender>(entity =>
            {
                entity.ToTable("mGender");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Gname)
                    .HasColumnName("gname")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<MHoliday>(entity =>
            {
                entity.ToTable("mHoliday");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Dates)
                    .HasColumnName("dates")
                    .HasColumnType("date");

                entity.Property(e => e.Holiday)
                    .HasColumnName("holiday")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<MLeaveType>(entity =>
            {
                entity.ToTable("mLeaveType");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Code)
                    .HasColumnName("code")
                    .HasMaxLength(2);

                entity.Property(e => e.Iscarryforward).HasColumnName("iscarryforward");

                entity.Property(e => e.Leavetype)
                    .HasColumnName("leavetype")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<MPayHead>(entity =>
            {
                entity.ToTable("mPayHead");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Amount).HasColumnName("amount");

                entity.Property(e => e.Head)
                    .HasColumnName("head")
                    .HasMaxLength(100);

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .HasMaxLength(10);
            });

            modelBuilder.Entity<MPaymentModes>(entity =>
            {
                entity.ToTable("mPaymentModes");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Modename)
                    .HasColumnName("modename")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<MRights>(entity =>
            {
                entity.ToTable("mRights");

                entity.HasIndex(e => e.Menuid);

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Authid)
                    .HasColumnName("authid")
                    .HasMaxLength(50);

                entity.Property(e => e.Displayname)
                    .HasColumnName("displayname")
                    .HasMaxLength(50)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Groupid)
                    .HasColumnName("groupid")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Menuid)
                    .HasColumnName("menuid")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Parentid).HasColumnName("parentid");

                entity.Property(e => e.Rname)
                    .HasColumnName("rname")
                    .HasMaxLength(50);

                entity.Property(e => e.Sort).HasColumnName("sort");

                entity.Property(e => e.Url).HasColumnName("url");

                entity.Property(e => e.Visible)
                    .HasColumnName("visible")
                    .HasDefaultValueSql("((0))");

                entity.HasOne(d => d.Menu)
                    .WithMany(p => p.MRights)
                    .HasForeignKey(d => d.Menuid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_mRights_Menu");
            });

            modelBuilder.Entity<MRoleRights>(entity =>
            {
                entity.ToTable("mRoleRights");

                entity.HasIndex(e => e.RightId);

                entity.HasIndex(e => e.RoleId);

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.RightId).HasColumnName("right_id");

                entity.Property(e => e.RoleId).HasColumnName("role_id");

                entity.HasOne(d => d.Right)
                    .WithMany(p => p.MRoleRights)
                    .HasForeignKey(d => d.RightId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_mRoleRights_mRights");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.MRoleRights)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK_mRoleRights_mRoles");
            });

            modelBuilder.Entity<MRoles>(entity =>
            {
                entity.ToTable("mRoles");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.DateCreated)
                    .HasColumnName("date_created")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DateModified)
                    .HasColumnName("date_modified")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Disabled)
                    .HasColumnName("disabled")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Isadmin)
                    .HasColumnName("isadmin")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Isdefault).HasColumnName("isdefault");

                entity.Property(e => e.Rolename)
                    .HasColumnName("rolename")
                    .HasMaxLength(50);

                entity.Property(e => e.Userid).HasColumnName("userid");
            });

            modelBuilder.Entity<MStaff>(entity =>
            {
                entity.ToTable("mStaff");

                entity.HasIndex(e => e.Desigid);

                entity.HasIndex(e => e.Userid);

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Aadharno)
                    .HasColumnName("aadharno")
                    .HasMaxLength(15);

                entity.Property(e => e.Address).HasColumnName("address");

                entity.Property(e => e.Associateuserid)
                    .HasColumnName("associateuserid")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Code)
                    .HasColumnName("code")
                    .HasMaxLength(10);

                entity.Property(e => e.Datecreated)
                    .HasColumnName("datecreated")
                    .HasColumnType("datetime");

                entity.Property(e => e.Datemodified)
                    .HasColumnName("datemodified")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Desigid).HasColumnName("desigid");

                entity.Property(e => e.Disabled)
                    .HasColumnName("disabled")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Dob)
                    .HasColumnName("dob")
                    .HasColumnType("date");

                entity.Property(e => e.Doj)
                    .HasColumnName("doj")
                    .HasColumnType("datetime");

                entity.Property(e => e.Dol)
                    .HasColumnName("dol")
                    .HasColumnType("datetime");

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasMaxLength(50);

                entity.Property(e => e.Mobile)
                    .HasColumnName("mobile")
                    .HasMaxLength(13);

                entity.Property(e => e.Phone)
                    .HasColumnName("phone")
                    .HasMaxLength(15);

                entity.Property(e => e.Photo).HasColumnName("photo");

                entity.Property(e => e.Staffname)
                    .HasColumnName("staffname")
                    .HasMaxLength(500)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Userid).HasColumnName("userid");

                entity.HasOne(d => d.Desig)
                    .WithMany(p => p.MStaff)
                    .HasForeignKey(d => d.Desigid)
                    .HasConstraintName("FK_Staff_mDesignation");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.MStaff)
                    .HasForeignKey(d => d.Userid)
                    .HasConstraintName("FK_Staff_mUser");
            });

            modelBuilder.Entity<MStaffPayroll>(entity =>
            {
                entity.ToTable("mStaffPayroll");

                entity.HasIndex(e => e.Staffid);

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Amount).HasColumnName("amount");

                entity.Property(e => e.Head)
                    .HasColumnName("head")
                    .HasMaxLength(100);

                entity.Property(e => e.Staffid).HasColumnName("staffid");

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .HasMaxLength(10);

                entity.HasOne(d => d.Staff)
                    .WithMany(p => p.MStaffPayroll)
                    .HasForeignKey(d => d.Staffid)
                    .HasConstraintName("FK_mStaffPayroll_mStaff");
            });

            modelBuilder.Entity<MSubjects>(entity =>
            {
                entity.ToTable("mSubjects");

                entity.HasIndex(e => e.Staffid);

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Code)
                    .HasColumnName("code")
                    .HasMaxLength(6);

                entity.Property(e => e.Datecreated)
                    .HasColumnName("datecreated")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Disabled)
                    .HasColumnName("disabled")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Staffid).HasColumnName("staffid");

                entity.Property(e => e.Subject)
                    .HasColumnName("subject")
                    .HasMaxLength(50);

                entity.Property(e => e.Userid).HasColumnName("userid");

                entity.HasOne(d => d.Staff)
                    .WithMany(p => p.MSubjects)
                    .HasForeignKey(d => d.Staffid)
                    .HasConstraintName("FK_mSubjects_mStaff");
            });

            modelBuilder.Entity<MTimeTable>(entity =>
            {
                entity.ToTable("mTimeTable");

                entity.HasIndex(e => e.Batchid);

                entity.HasIndex(e => e.Classid);

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Batchid).HasColumnName("batchid");

                entity.Property(e => e.Classid).HasColumnName("classid");

                entity.Property(e => e.Friday).HasColumnName("friday");

                entity.Property(e => e.Fromtime).HasColumnName("fromtime");

                entity.Property(e => e.Monday).HasColumnName("monday");

                entity.Property(e => e.Saturday).HasColumnName("saturday");

                entity.Property(e => e.Sunday).HasColumnName("sunday");

                entity.Property(e => e.Thursday).HasColumnName("thursday");

                entity.Property(e => e.Totime).HasColumnName("totime");

                entity.Property(e => e.Tuesday).HasColumnName("tuesday");

                entity.Property(e => e.Wednesday).HasColumnName("wednesday");

                entity.HasOne(d => d.Batch)
                    .WithMany(p => p.MTimeTable)
                    .HasForeignKey(d => d.Batchid)
                    .HasConstraintName("FK_TimeTable_Batches");

                entity.HasOne(d => d.Class)
                    .WithMany(p => p.MTimeTable)
                    .HasForeignKey(d => d.Classid)
                    .HasConstraintName("FK_TimeTable_mClasses");
            });

            modelBuilder.Entity<MUser>(entity =>
            {
                entity.ToTable("mUser");

                entity.HasIndex(e => e.RoleId);

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Aadharno)
                    .HasColumnName("aadharno")
                    .HasMaxLength(15);

                entity.Property(e => e.Currentlogin)
                    .HasColumnName("currentlogin")
                    .HasColumnType("datetime");

                entity.Property(e => e.DateCreated)
                    .HasColumnName("date_created")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DateModified)
                    .HasColumnName("date_modified")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Disabled).HasColumnName("disabled");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email")
                    .HasMaxLength(50);

                entity.Property(e => e.Fname)
                    .IsRequired()
                    .HasColumnName("fname")
                    .HasMaxLength(50);

                entity.Property(e => e.IsAdmin).HasColumnName("is_admin");

                entity.Property(e => e.IsMasterAdmin).HasColumnName("is_master_admin");

                entity.Property(e => e.Lastlogin)
                    .HasColumnName("lastlogin")
                    .HasColumnType("datetime");

                entity.Property(e => e.Lname)
                    .HasColumnName("lname")
                    .HasMaxLength(50);

                entity.Property(e => e.Mname)
                    .HasColumnName("mname")
                    .HasMaxLength(50);

                entity.Property(e => e.Passverificationcode).HasColumnName("passverificationcode");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("password")
                    .HasMaxLength(500);

                entity.Property(e => e.Photo).HasColumnName("photo");

                entity.Property(e => e.RoleId).HasColumnName("role_id");

                entity.Property(e => e.UserName)
                    .HasColumnName("user_name")
                    .HasMaxLength(15);

                entity.Property(e => e.Userid).HasColumnName("userid");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.MUser)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_mUser_mRoles");
            });

            modelBuilder.Entity<MWeeklyOff>(entity =>
            {
                entity.ToTable("mWeeklyOff");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.PosInMonth)
                    .HasColumnName("posInMonth")
                    .HasMaxLength(10);

                entity.Property(e => e.Weekday)
                    .HasColumnName("weekday")
                    .HasMaxLength(10);
            });

            modelBuilder.Entity<Settings>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Address)
                    .HasColumnName("address")
                    .HasMaxLength(500);

                entity.Property(e => e.Appname)
                    .HasColumnName("appname")
                    .HasMaxLength(50);

                entity.Property(e => e.City)
                    .HasColumnName("city")
                    .HasMaxLength(50);

                entity.Property(e => e.Country)
                    .HasColumnName("country")
                    .HasMaxLength(50);

                entity.Property(e => e.Currency)
                    .HasColumnName("currency")
                    .HasMaxLength(3);

                entity.Property(e => e.Dbbackuppath)
                    .HasColumnName("dbbackuppath")
                    .HasMaxLength(500);

                entity.Property(e => e.Key).HasColumnName("key");

                entity.Property(e => e.Logo).HasColumnName("logo");

                entity.Property(e => e.State)
                    .HasColumnName("state")
                    .HasMaxLength(50);

                entity.Property(e => e.Timezoneid)
                    .HasColumnName("timezoneid")
                    .HasMaxLength(150);

                entity.Property(e => e.Token).HasColumnName("token");

                entity.Property(e => e.Userid).HasColumnName("userid");
            });

            modelBuilder.Entity<SettingsOther>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Smskey).HasColumnName("smskey");

                entity.Property(e => e.Smspassword).HasColumnName("smspassword");

                entity.Property(e => e.Smsprofileid)
                    .HasColumnName("smsprofileid")
                    .HasMaxLength(50);

                entity.Property(e => e.Smssenderid)
                    .HasColumnName("smssenderid")
                    .HasMaxLength(10);

                entity.Property(e => e.Smsusername)
                    .HasColumnName("smsusername")
                    .HasMaxLength(50);

                entity.Property(e => e.Smtpbccid)
                    .HasColumnName("smtpbccid")
                    .HasMaxLength(50);

                entity.Property(e => e.Smtpemailid)
                    .HasColumnName("smtpemailid")
                    .HasMaxLength(50);

                entity.Property(e => e.Smtpenablessl).HasColumnName("smtpenablessl");

                entity.Property(e => e.Smtphost)
                    .IsRequired()
                    .HasColumnName("smtphost")
                    .HasMaxLength(50);

                entity.Property(e => e.Smtppassword).HasColumnName("smtppassword");

                entity.Property(e => e.Smtpport).HasColumnName("smtpport");
            });

            modelBuilder.Entity<StaffAttendance>(entity =>
            {
                entity.HasIndex(e => e.Staffid);

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Datecreated)
                    .HasColumnName("datecreated")
                    .HasColumnType("date");

                entity.Property(e => e.Ispresent)
                    .HasColumnName("ispresent")
                    .HasMaxLength(2);

                entity.Property(e => e.Staffid).HasColumnName("staffid");

                entity.HasOne(d => d.Staff)
                    .WithMany(p => p.StaffAttendance)
                    .HasForeignKey(d => d.Staffid)
                    .HasConstraintName("FK_StaffAttendance_mStaff");
            });

            modelBuilder.Entity<StaffAttendance1>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Month).HasColumnName("month");

                entity.Property(e => e.Staffid).HasColumnName("staffid");

                entity.Property(e => e.Year).HasColumnName("year");

                entity.Property(e => e._1).HasMaxLength(2);

                entity.Property(e => e._10).HasMaxLength(2);

                entity.Property(e => e._11).HasMaxLength(2);

                entity.Property(e => e._12).HasMaxLength(2);

                entity.Property(e => e._13).HasMaxLength(2);

                entity.Property(e => e._14).HasMaxLength(2);

                entity.Property(e => e._15).HasMaxLength(2);

                entity.Property(e => e._16).HasMaxLength(2);

                entity.Property(e => e._17).HasMaxLength(2);

                entity.Property(e => e._18).HasMaxLength(2);

                entity.Property(e => e._19).HasMaxLength(2);

                entity.Property(e => e._2).HasMaxLength(2);

                entity.Property(e => e._20).HasMaxLength(2);

                entity.Property(e => e._21).HasMaxLength(2);

                entity.Property(e => e._22).HasMaxLength(2);

                entity.Property(e => e._23).HasMaxLength(2);

                entity.Property(e => e._24).HasMaxLength(2);

                entity.Property(e => e._25).HasMaxLength(2);

                entity.Property(e => e._26).HasMaxLength(2);

                entity.Property(e => e._27).HasMaxLength(2);

                entity.Property(e => e._28).HasMaxLength(2);

                entity.Property(e => e._29).HasMaxLength(2);

                entity.Property(e => e._3).HasMaxLength(2);

                entity.Property(e => e._30).HasMaxLength(2);

                entity.Property(e => e._31).HasMaxLength(2);

                entity.Property(e => e._4).HasMaxLength(2);

                entity.Property(e => e._5).HasMaxLength(2);

                entity.Property(e => e._6).HasMaxLength(2);

                entity.Property(e => e._7).HasMaxLength(2);

                entity.Property(e => e._8).HasMaxLength(2);

                entity.Property(e => e._9).HasMaxLength(2);

                entity.HasOne(d => d.Staff)
                    .WithMany(p => p.StaffAttendance1)
                    .HasForeignKey(d => d.Staffid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StaffAttendance1_mStaff");
            });

            modelBuilder.Entity<StaffLeaves>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Datecreated)
                    .HasColumnName("datecreated")
                    .HasColumnType("date");

                entity.Property(e => e.Leavetype)
                    .HasColumnName("leavetype")
                    .HasMaxLength(2);

                entity.Property(e => e.Staffid).HasColumnName("staffid");

                entity.HasOne(d => d.Staff)
                    .WithMany(p => p.StaffLeaves)
                    .HasForeignKey(d => d.Staffid)
                    .HasConstraintName("FK_StaffLeaves_mStaff");
            });

            modelBuilder.Entity<StaffSalary>(entity =>
            {
                entity.HasIndex(e => e.Staffid);

                entity.HasIndex(e => e.Userid);

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Adjustments)
                    .HasColumnName("adjustments")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Datecreated)
                    .HasColumnName("datecreated")
                    .HasColumnType("datetime");

                entity.Property(e => e.Datepaid)
                    .HasColumnName("datepaid")
                    .HasColumnType("datetime");

                entity.Property(e => e.Deductions)
                    .HasColumnName("deductions")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Earnings)
                    .HasColumnName("earnings")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Ispaid)
                    .HasColumnName("ispaid")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Month).HasColumnName("month");

                entity.Property(e => e.Netpay).HasColumnName("netpay");

                entity.Property(e => e.Staffid).HasColumnName("staffid");

                entity.Property(e => e.Userid).HasColumnName("userid");

                entity.Property(e => e.Year).HasColumnName("year");

                entity.HasOne(d => d.Staff)
                    .WithMany(p => p.StaffSalary)
                    .HasForeignKey(d => d.Staffid)
                    .HasConstraintName("FK_StaffSalary_mStaff");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.StaffSalary)
                    .HasForeignKey(d => d.Userid)
                    .HasConstraintName("FK_StaffSalary_mUser");
            });

            modelBuilder.Entity<StaffSalaryDetails>(entity =>
            {
                entity.HasIndex(e => e.SsId);

                entity.HasIndex(e => e.Staffid);

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Amount).HasColumnName("amount");

                entity.Property(e => e.Head)
                    .HasColumnName("head")
                    .HasMaxLength(100);

                entity.Property(e => e.SsId).HasColumnName("ss_id");

                entity.Property(e => e.Staffid).HasColumnName("staffid");

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .HasMaxLength(10);

                entity.HasOne(d => d.Ss)
                    .WithMany(p => p.StaffSalaryDetails)
                    .HasForeignKey(d => d.SsId)
                    .HasConstraintName("FK_StaffSalaryDetails_StaffSalary");

                entity.HasOne(d => d.Staff)
                    .WithMany(p => p.StaffSalaryDetails)
                    .HasForeignKey(d => d.Staffid)
                    .HasConstraintName("FK_StaffSalaryDetails_mStaff");
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasIndex(e => e.Castid);

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Aadharno)
                    .HasColumnName("aadharno")
                    .HasMaxLength(15);

                entity.Property(e => e.Address)
                    .HasColumnName("address")
                    .HasMaxLength(500);

                entity.Property(e => e.Castid).HasColumnName("castid");

                entity.Property(e => e.Datecreated)
                    .HasColumnName("datecreated")
                    .HasColumnType("datetime");

                entity.Property(e => e.Datemodified)
                    .HasColumnName("datemodified")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Disabled).HasColumnName("disabled");

                entity.Property(e => e.Dob)
                    .HasColumnName("dob")
                    .HasColumnType("date");

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasMaxLength(50);

                entity.Property(e => e.Fname)
                    .HasColumnName("fname")
                    .HasMaxLength(50);

                entity.Property(e => e.Gender)
                    .HasColumnName("gender")
                    .HasMaxLength(20);

                entity.Property(e => e.GuardianRelation)
                    .HasColumnName("guardian_relation")
                    .HasMaxLength(50);

                entity.Property(e => e.Guardianid).HasColumnName("guardianid");

                entity.Property(e => e.Lname)
                    .HasColumnName("lname")
                    .HasMaxLength(50);

                entity.Property(e => e.Mname)
                    .HasColumnName("mname")
                    .HasMaxLength(50);

                entity.Property(e => e.Mobile)
                    .HasColumnName("mobile")
                    .HasMaxLength(15);

                entity.Property(e => e.Phone)
                    .HasColumnName("phone")
                    .HasMaxLength(15);

                entity.Property(e => e.Photo).HasColumnName("photo");

                entity.Property(e => e.Prnno)
                    .HasColumnName("prnno")
                    .HasMaxLength(15);

                entity.Property(e => e.Userid).HasColumnName("userid");

                entity.HasOne(d => d.Cast)
                    .WithMany(p => p.Student)
                    .HasForeignKey(d => d.Castid)
                    .HasConstraintName("FK_Student_mCast");

                entity.HasOne(d => d.Guardian)
                    .WithMany(p => p.Student)
                    .HasForeignKey(d => d.Guardianid)
                    .HasConstraintName("FK_Student_StudentGuardian");
            });

            modelBuilder.Entity<StudentGuardian>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Address)
                    .HasColumnName("address")
                    .HasMaxLength(500);

                entity.Property(e => e.Datecreated)
                    .HasColumnName("datecreated")
                    .HasColumnType("datetime");

                entity.Property(e => e.Disabled).HasColumnName("disabled");

                entity.Property(e => e.Mobile).HasColumnName("mobile");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(500);

                entity.Property(e => e.Passverificationcode)
                    .HasColumnName("passverificationcode")
                    .HasMaxLength(8);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("password");

                entity.Property(e => e.Userid).HasColumnName("userid");
            });

            modelBuilder.Entity<TExam>(entity =>
            {
                entity.ToTable("tExam");

                entity.HasIndex(e => e.Batchid);

                entity.HasIndex(e => e.Classid);

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Batchid).HasColumnName("batchid");

                entity.Property(e => e.Classid).HasColumnName("classid");

                entity.Property(e => e.DateCreated)
                    .HasColumnName("date_created")
                    .HasColumnType("datetime");

                entity.Property(e => e.ExamName)
                    .HasColumnName("exam_name")
                    .HasMaxLength(500);

                entity.Property(e => e.Userid).HasColumnName("userid");

                entity.HasOne(d => d.Batch)
                    .WithMany(p => p.TExam)
                    .HasForeignKey(d => d.Batchid)
                    .HasConstraintName("FK_tExam_Batches");

                entity.HasOne(d => d.Class)
                    .WithMany(p => p.TExam)
                    .HasForeignKey(d => d.Classid)
                    .HasConstraintName("FK_tExam_mClasses");
            });

            modelBuilder.Entity<TExamMarkSheet>(entity =>
            {
                entity.ToTable("tExamMarkSheet");

                entity.HasIndex(e => e.Batchid);

                entity.HasIndex(e => e.Classid);

                entity.HasIndex(e => e.Examid);

                entity.HasIndex(e => e.ExmschId);

                entity.HasIndex(e => e.Studentid);

                entity.HasIndex(e => e.Subjectid);

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Batchid).HasColumnName("batchid");

                entity.Property(e => e.Classid).HasColumnName("classid");

                entity.Property(e => e.DateCreated)
                    .HasColumnName("date_created")
                    .HasColumnType("datetime");

                entity.Property(e => e.Examid).HasColumnName("examid");

                entity.Property(e => e.ExmschId).HasColumnName("exmsch_id");

                entity.Property(e => e.Grade)
                    .HasColumnName("grade")
                    .HasMaxLength(2);

                entity.Property(e => e.Obtained).HasColumnName("obtained");

                entity.Property(e => e.Practical).HasColumnName("practical");

                entity.Property(e => e.Remarks)
                    .HasColumnName("remarks")
                    .HasMaxLength(10);

                entity.Property(e => e.Studentid).HasColumnName("studentid");

                entity.Property(e => e.Subjectid).HasColumnName("subjectid");

                entity.Property(e => e.Totalmarks).HasColumnName("totalmarks");

                entity.Property(e => e.Userid).HasColumnName("userid");

                entity.HasOne(d => d.Batch)
                    .WithMany(p => p.TExamMarkSheet)
                    .HasForeignKey(d => d.Batchid)
                    .HasConstraintName("FK_tExamMarkSheet_Batches1");

                entity.HasOne(d => d.Class)
                    .WithMany(p => p.TExamMarkSheet)
                    .HasForeignKey(d => d.Classid)
                    .HasConstraintName("FK_tExamMarkSheet_mClasses");

                entity.HasOne(d => d.Exam)
                    .WithMany(p => p.TExamMarkSheet)
                    .HasForeignKey(d => d.Examid)
                    .HasConstraintName("FK_tExamMarkSheet_mExams");

                entity.HasOne(d => d.Exmsch)
                    .WithMany(p => p.TExamMarkSheet)
                    .HasForeignKey(d => d.ExmschId)
                    .HasConstraintName("FK_tExamMarkSheet_tExamSchedule");

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.TExamMarkSheet)
                    .HasForeignKey(d => d.Studentid)
                    .HasConstraintName("FK_tExamMarkSheet_Student");

                entity.HasOne(d => d.Subject)
                    .WithMany(p => p.TExamMarkSheet)
                    .HasForeignKey(d => d.Subjectid)
                    .HasConstraintName("FK_tExamMarkSheet_mSubjects");
            });

            modelBuilder.Entity<TExamSchedule>(entity =>
            {
                entity.ToTable("tExamSchedule");

                entity.HasIndex(e => e.Batchid);

                entity.HasIndex(e => e.Classid);

                entity.HasIndex(e => e.Examid);

                entity.HasIndex(e => e.Subjectid);

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Batchid).HasColumnName("batchid");

                entity.Property(e => e.Classid).HasColumnName("classid");

                entity.Property(e => e.DateCreated)
                    .HasColumnName("date_created")
                    .HasColumnType("datetime");

                entity.Property(e => e.Endtime).HasColumnName("endtime");

                entity.Property(e => e.Examdate)
                    .HasColumnName("examdate")
                    .HasColumnType("date");

                entity.Property(e => e.Examid).HasColumnName("examid");

                entity.Property(e => e.Passingmarks).HasColumnName("passingmarks");

                entity.Property(e => e.Starttime).HasColumnName("starttime");

                entity.Property(e => e.Subjectid).HasColumnName("subjectid");

                entity.Property(e => e.Totalmarks).HasColumnName("totalmarks");

                entity.Property(e => e.Userid).HasColumnName("userid");

                entity.HasOne(d => d.Batch)
                    .WithMany(p => p.TExamSchedule)
                    .HasForeignKey(d => d.Batchid)
                    .HasConstraintName("FK_tExamSchedule_Batches");

                entity.HasOne(d => d.Class)
                    .WithMany(p => p.TExamSchedule)
                    .HasForeignKey(d => d.Classid)
                    .HasConstraintName("FK_tExamSchedule_mClasses1");

                entity.HasOne(d => d.Exam)
                    .WithMany(p => p.TExamSchedule)
                    .HasForeignKey(d => d.Examid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tExamSchedule_mExams1");

                entity.HasOne(d => d.Subject)
                    .WithMany(p => p.TExamSchedule)
                    .HasForeignKey(d => d.Subjectid)
                    .HasConstraintName("FK_tExamSchedule_mSubjects");
            });

            modelBuilder.Entity<TExamStudentsAdmitCard>(entity =>
            {
                entity.ToTable("tExamStudentsAdmitCard");

                entity.HasIndex(e => e.Batchid);

                entity.HasIndex(e => e.Classid);

                entity.HasIndex(e => e.Examid);

                entity.HasIndex(e => e.Studentid);

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Batchid).HasColumnName("batchid");

                entity.Property(e => e.Classid).HasColumnName("classid");

                entity.Property(e => e.DateCreated)
                    .HasColumnName("date_created")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Disabled).HasColumnName("disabled");

                entity.Property(e => e.Examid).HasColumnName("examid");

                entity.Property(e => e.Studentid).HasColumnName("studentid");

                entity.Property(e => e.Userid).HasColumnName("userid");

                entity.HasOne(d => d.Batch)
                    .WithMany(p => p.TExamStudentsAdmitCard)
                    .HasForeignKey(d => d.Batchid)
                    .HasConstraintName("FK_tExamStudentsAdmitCard_Batches");

                entity.HasOne(d => d.Class)
                    .WithMany(p => p.TExamStudentsAdmitCard)
                    .HasForeignKey(d => d.Classid)
                    .HasConstraintName("FK_tExamStudentsAdmitCard_mClasses");

                entity.HasOne(d => d.Exam)
                    .WithMany(p => p.TExamStudentsAdmitCard)
                    .HasForeignKey(d => d.Examid)
                    .HasConstraintName("FK_tExamStudentsAdmitCard_tExam");

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.TExamStudentsAdmitCard)
                    .HasForeignKey(d => d.Studentid)
                    .HasConstraintName("FK_tExamStudentsAdmitCard_Student");
            });

            modelBuilder.Entity<TLeaves>(entity =>
            {
                entity.ToTable("tLeaves");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Leaves).HasColumnName("leaves");

                entity.Property(e => e.Leavetypeid).HasColumnName("leavetypeid");

                entity.Property(e => e.Year).HasColumnName("year");

                entity.HasOne(d => d.Leavetype)
                    .WithMany(p => p.TLeaves)
                    .HasForeignKey(d => d.Leavetypeid)
                    .HasConstraintName("FK_tLeaves_mLeaveType");
            });

            modelBuilder.Entity<TStudentAdmission>(entity =>
            {
                entity.ToTable("tStudentAdmission");

                entity.HasIndex(e => e.Batchid);

                entity.HasIndex(e => e.Classid);

                entity.HasIndex(e => e.Studentid);

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Batchid).HasColumnName("batchid");

                entity.Property(e => e.Cancelled)
                    .HasColumnName("cancelled")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Classid).HasColumnName("classid");

                entity.Property(e => e.Datecreated)
                    .HasColumnName("datecreated")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Rollno).HasColumnName("rollno");

                entity.Property(e => e.Studentid).HasColumnName("studentid");

                entity.Property(e => e.Userid).HasColumnName("userid");

                entity.HasOne(d => d.Batch)
                    .WithMany(p => p.TStudentAdmission)
                    .HasForeignKey(d => d.Batchid)
                    .HasConstraintName("FK_tStudentAdmission_Batches");

                entity.HasOne(d => d.Class)
                    .WithMany(p => p.TStudentAdmission)
                    .HasForeignKey(d => d.Classid)
                    .HasConstraintName("FK_tAdmission_mStanderd");

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.TStudentAdmission)
                    .HasForeignKey(d => d.Studentid)
                    .HasConstraintName("FK_tAdmission_Student");
            });

            modelBuilder.Entity<TStudentAttendence>(entity =>
            {
                entity.ToTable("tStudentAttendence");

                entity.HasIndex(e => e.Batchid);

                entity.HasIndex(e => e.Classid);

                entity.HasIndex(e => e.Studentid);

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Batchid).HasColumnName("batchid");

                entity.Property(e => e.Classid).HasColumnName("classid");

                entity.Property(e => e.Datecreated)
                    .HasColumnName("datecreated")
                    .HasColumnType("date")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Ispresent)
                    .HasColumnName("ispresent")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Studentid).HasColumnName("studentid");

                entity.HasOne(d => d.Batch)
                    .WithMany(p => p.TStudentAttendence)
                    .HasForeignKey(d => d.Batchid)
                    .HasConstraintName("FK_tStudentAttendence_Batches");

                entity.HasOne(d => d.Class)
                    .WithMany(p => p.TStudentAttendence)
                    .HasForeignKey(d => d.Classid)
                    .HasConstraintName("FK_tStudentPresenty_mClasses");

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.TStudentAttendence)
                    .HasForeignKey(d => d.Studentid)
                    .HasConstraintName("FK_tStudentPresenty_Student");
            });

            modelBuilder.Entity<TStudentAttendence1>(entity =>
            {
                entity.ToTable("tStudentAttendence1");

                entity.HasIndex(e => e.Batchid);

                entity.HasIndex(e => e.Classid);

                entity.HasIndex(e => e.Studentid);

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Batchid).HasColumnName("batchid");

                entity.Property(e => e.Classid).HasColumnName("classid");

                entity.Property(e => e.Month).HasColumnName("month");

                entity.Property(e => e.Studentid).HasColumnName("studentid");

                entity.Property(e => e.Year).HasColumnName("year");

                entity.Property(e => e._1).HasMaxLength(2);

                entity.Property(e => e._10).HasMaxLength(2);

                entity.Property(e => e._11).HasMaxLength(2);

                entity.Property(e => e._12).HasMaxLength(2);

                entity.Property(e => e._13).HasMaxLength(2);

                entity.Property(e => e._14).HasMaxLength(2);

                entity.Property(e => e._15).HasMaxLength(2);

                entity.Property(e => e._16).HasMaxLength(2);

                entity.Property(e => e._17).HasMaxLength(2);

                entity.Property(e => e._18).HasMaxLength(2);

                entity.Property(e => e._19).HasMaxLength(2);

                entity.Property(e => e._2).HasMaxLength(2);

                entity.Property(e => e._20).HasMaxLength(2);

                entity.Property(e => e._21).HasMaxLength(2);

                entity.Property(e => e._22).HasMaxLength(2);

                entity.Property(e => e._23).HasMaxLength(2);

                entity.Property(e => e._24).HasMaxLength(2);

                entity.Property(e => e._25).HasMaxLength(2);

                entity.Property(e => e._26).HasMaxLength(2);

                entity.Property(e => e._27).HasMaxLength(2);

                entity.Property(e => e._28).HasMaxLength(2);

                entity.Property(e => e._29).HasMaxLength(2);

                entity.Property(e => e._3).HasMaxLength(2);

                entity.Property(e => e._30).HasMaxLength(2);

                entity.Property(e => e._31).HasMaxLength(2);

                entity.Property(e => e._4).HasMaxLength(2);

                entity.Property(e => e._5).HasMaxLength(2);

                entity.Property(e => e._6).HasMaxLength(2);

                entity.Property(e => e._7).HasMaxLength(2);

                entity.Property(e => e._8).HasMaxLength(2);

                entity.Property(e => e._9).HasMaxLength(2);

                entity.HasOne(d => d.Batch)
                    .WithMany(p => p.TStudentAttendence1)
                    .HasForeignKey(d => d.Batchid)
                    .HasConstraintName("FK_tStudentAttendence1_Batches");

                entity.HasOne(d => d.Class)
                    .WithMany(p => p.TStudentAttendence1)
                    .HasForeignKey(d => d.Classid)
                    .HasConstraintName("FK_tStudentAttendence1_mClasses");

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.TStudentAttendence1)
                    .HasForeignKey(d => d.Studentid)
                    .HasConstraintName("FK_tStudentAttendence1_Student");
            });

            modelBuilder.Entity<TStudentFees>(entity =>
            {
                entity.ToTable("tStudentFees");

                entity.HasIndex(e => e.Batchid);

                entity.HasIndex(e => e.Studentid);

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Amount).HasColumnName("amount");

                entity.Property(e => e.Batchid).HasColumnName("batchid");

                entity.Property(e => e.Classid).HasColumnName("classid");

                entity.Property(e => e.Datecreated)
                    .HasColumnName("datecreated")
                    .HasColumnType("datetime");

                entity.Property(e => e.FeesType)
                    .HasColumnName("fees_type")
                    .HasMaxLength(50);

                entity.Property(e => e.Studentid).HasColumnName("studentid");

                entity.Property(e => e.Userid).HasColumnName("userid");

                entity.HasOne(d => d.Batch)
                    .WithMany(p => p.TStudentFees)
                    .HasForeignKey(d => d.Batchid)
                    .HasConstraintName("FK_tStudentFees_Batches");

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.TStudentFees)
                    .HasForeignKey(d => d.Studentid)
                    .HasConstraintName("FK_tStudentFees_Student");
            });

            modelBuilder.Entity<TStudentPayment>(entity =>
            {
                entity.ToTable("tStudentPayment");

                entity.HasIndex(e => e.Batchid);

                entity.HasIndex(e => e.Studentid);

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Amount)
                    .HasColumnName("amount")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Batchid).HasColumnName("batchid");

                entity.Property(e => e.Chtrno)
                    .HasColumnName("chtrno")
                    .HasMaxLength(50);

                entity.Property(e => e.Datecreated)
                    .HasColumnName("datecreated")
                    .HasColumnType("datetime");

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasMaxLength(100);

                entity.Property(e => e.Mode)
                    .HasColumnName("mode")
                    .HasMaxLength(15);

                entity.Property(e => e.Studentid).HasColumnName("studentid");

                entity.Property(e => e.Userid).HasColumnName("userid");

                entity.HasOne(d => d.Batch)
                    .WithMany(p => p.TStudentPayment)
                    .HasForeignKey(d => d.Batchid)
                    .HasConstraintName("FK_tStudentPayment_Batches");

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.TStudentPayment)
                    .HasForeignKey(d => d.Studentid)
                    .HasConstraintName("FK_tStudentPayment_Student");
            });
        }

        public DbSet<MySchool.Models.Sample> Sample { get; set; }
    }
}
