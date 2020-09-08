using System;
using System.Collections.Generic;

namespace MySchool.Models
{
    public partial class Student
    {
        public Student()
        {
            MessagesStudents = new HashSet<MessagesStudents>();
            TExamMarkSheet = new HashSet<TExamMarkSheet>();
            TExamStudentsAdmitCard = new HashSet<TExamStudentsAdmitCard>();
            TStudentAdmission = new HashSet<TStudentAdmission>();
            TStudentAttendence = new HashSet<TStudentAttendence>();
            TStudentAttendence1 = new HashSet<TStudentAttendence1>();
            TStudentFees = new HashSet<TStudentFees>();
            TStudentPayment = new HashSet<TStudentPayment>();
        }

        public int Id { get; set; }
        public string Prnno { get; set; }
        public string Fname { get; set; }
        public string Mname { get; set; }
        public string Lname { get; set; }
        public string Gender { get; set; }
        public string Photo { get; set; }
        public DateTime? Dob { get; set; }
        public string Aadharno { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Phone { get; set; }
        public int? Castid { get; set; }
        public long? Guardianid { get; set; }
        public string GuardianRelation { get; set; }
        public bool? Disabled { get; set; }
        public int? Userid { get; set; }
        public DateTime? Datecreated { get; set; }
        public DateTime? Datemodified { get; set; }

        public MCast Cast { get; set; }
        public StudentGuardian Guardian { get; set; }
        public ICollection<MessagesStudents> MessagesStudents { get; set; }
        public ICollection<TExamMarkSheet> TExamMarkSheet { get; set; }
        public ICollection<TExamStudentsAdmitCard> TExamStudentsAdmitCard { get; set; }
        public ICollection<TStudentAdmission> TStudentAdmission { get; set; }
        public ICollection<TStudentAttendence> TStudentAttendence { get; set; }
        public ICollection<TStudentAttendence1> TStudentAttendence1 { get; set; }
        public ICollection<TStudentFees> TStudentFees { get; set; }
        public ICollection<TStudentPayment> TStudentPayment { get; set; }
    }
}
