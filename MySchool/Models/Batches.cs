using System;
using System.Collections.Generic;

namespace MySchool.Models
{
    public partial class Batches
    {
        public Batches()
        {
            MTimeTable = new HashSet<MTimeTable>();
            TExam = new HashSet<TExam>();
            TExamMarkSheet = new HashSet<TExamMarkSheet>();
            TExamSchedule = new HashSet<TExamSchedule>();
            TExamStudentsAdmitCard = new HashSet<TExamStudentsAdmitCard>();
            TStudentAdmission = new HashSet<TStudentAdmission>();
            TStudentAttendence = new HashSet<TStudentAttendence>();
            TStudentAttendence1 = new HashSet<TStudentAttendence1>();
            TStudentFees = new HashSet<TStudentFees>();
            TStudentPayment = new HashSet<TStudentPayment>();
        }

        public int Id { get; set; }
        public string Batch { get; set; }
        public DateTime? Startdate { get; set; }
        public DateTime? Enddate { get; set; }
        public bool? Isactive { get; set; }
        public int? Userid { get; set; }
        public DateTime? Datecreated { get; set; }

        public ICollection<MTimeTable> MTimeTable { get; set; }
        public ICollection<TExam> TExam { get; set; }
        public ICollection<TExamMarkSheet> TExamMarkSheet { get; set; }
        public ICollection<TExamSchedule> TExamSchedule { get; set; }
        public ICollection<TExamStudentsAdmitCard> TExamStudentsAdmitCard { get; set; }
        public ICollection<TStudentAdmission> TStudentAdmission { get; set; }
        public ICollection<TStudentAttendence> TStudentAttendence { get; set; }
        public ICollection<TStudentAttendence1> TStudentAttendence1 { get; set; }
        public ICollection<TStudentFees> TStudentFees { get; set; }
        public ICollection<TStudentPayment> TStudentPayment { get; set; }
    }
}
