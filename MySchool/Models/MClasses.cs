using System;
using System.Collections.Generic;

namespace MySchool.Models
{
    public partial class MClasses
    {
        public MClasses()
        {
            ClassFees = new HashSet<ClassFees>();
            ClassSubjects = new HashSet<ClassSubjects>();
            ClassTeacher = new HashSet<ClassTeacher>();
            MTimeTable = new HashSet<MTimeTable>();
            TExam = new HashSet<TExam>();
            TExamMarkSheet = new HashSet<TExamMarkSheet>();
            TExamSchedule = new HashSet<TExamSchedule>();
            TExamStudentsAdmitCard = new HashSet<TExamStudentsAdmitCard>();
            TStudentAdmission = new HashSet<TStudentAdmission>();
            TStudentAttendence = new HashSet<TStudentAttendence>();
            TStudentAttendence1 = new HashSet<TStudentAttendence1>();
        }

        public int Id { get; set; }
        public string Standard { get; set; }
        public int? Capacity { get; set; }
        public int? Userid { get; set; }
        public DateTime? Datecreated { get; set; }
        public DateTime? Datemodified { get; set; }
        public bool? Disabled { get; set; }

        public MUser User { get; set; }
        public ICollection<ClassFees> ClassFees { get; set; }
        public ICollection<ClassSubjects> ClassSubjects { get; set; }
        public ICollection<ClassTeacher> ClassTeacher { get; set; }
        public ICollection<MTimeTable> MTimeTable { get; set; }
        public ICollection<TExam> TExam { get; set; }
        public ICollection<TExamMarkSheet> TExamMarkSheet { get; set; }
        public ICollection<TExamSchedule> TExamSchedule { get; set; }
        public ICollection<TExamStudentsAdmitCard> TExamStudentsAdmitCard { get; set; }
        public ICollection<TStudentAdmission> TStudentAdmission { get; set; }
        public ICollection<TStudentAttendence> TStudentAttendence { get; set; }
        public ICollection<TStudentAttendence1> TStudentAttendence1 { get; set; }
    }
}
