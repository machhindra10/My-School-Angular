using System;
using System.Collections.Generic;

namespace MySchool.Models
{
    public partial class MStaff
    {
        public MStaff()
        {
            ClassSubjects = new HashSet<ClassSubjects>();
            ClassTeacher = new HashSet<ClassTeacher>();
            LeaveApplication = new HashSet<LeaveApplication>();
            MStaffPayroll = new HashSet<MStaffPayroll>();
            MSubjects = new HashSet<MSubjects>();
            StaffAttendance = new HashSet<StaffAttendance>();
            StaffAttendance1 = new HashSet<StaffAttendance1>();
            StaffLeaves = new HashSet<StaffLeaves>();
            StaffSalary = new HashSet<StaffSalary>();
            StaffSalaryDetails = new HashSet<StaffSalaryDetails>();
        }

        public int Id { get; set; }
        public string Code { get; set; }
        public string Staffname { get; set; }
        public DateTime? Dob { get; set; }
        public string Photo { get; set; }
        public int? Desigid { get; set; }
        public DateTime? Doj { get; set; }
        public DateTime? Dol { get; set; }
        public string Aadharno { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Phone { get; set; }
        public bool? Disabled { get; set; }
        public int? Userid { get; set; }
        public DateTime? Datecreated { get; set; }
        public DateTime? Datemodified { get; set; }
        public int? Associateuserid { get; set; }

        public MDesignation Desig { get; set; }
        public MUser User { get; set; }
        public ICollection<ClassSubjects> ClassSubjects { get; set; }
        public ICollection<ClassTeacher> ClassTeacher { get; set; }
        public ICollection<LeaveApplication> LeaveApplication { get; set; }
        public ICollection<MStaffPayroll> MStaffPayroll { get; set; }
        public ICollection<MSubjects> MSubjects { get; set; }
        public ICollection<StaffAttendance> StaffAttendance { get; set; }
        public ICollection<StaffAttendance1> StaffAttendance1 { get; set; }
        public ICollection<StaffLeaves> StaffLeaves { get; set; }
        public ICollection<StaffSalary> StaffSalary { get; set; }
        public ICollection<StaffSalaryDetails> StaffSalaryDetails { get; set; }
    }
}
