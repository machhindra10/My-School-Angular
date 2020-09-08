using System;
using System.Collections.Generic;

namespace MySchool.Models
{
    public partial class TExamMarkSheet
    {
        public int Id { get; set; }
        public int? ExmschId { get; set; }
        public int? Batchid { get; set; }
        public int? Examid { get; set; }
        public int? Classid { get; set; }
        public int? Studentid { get; set; }
        public int? Subjectid { get; set; }
        public int? Obtained { get; set; }
        public int? Practical { get; set; }
        public int? Totalmarks { get; set; }
        public string Grade { get; set; }
        public string Remarks { get; set; }
        public int? Userid { get; set; }
        public DateTime? DateCreated { get; set; }

        public Batches Batch { get; set; }
        public MClasses Class { get; set; }
        public TExam Exam { get; set; }
        public TExamSchedule Exmsch { get; set; }
        public Student Student { get; set; }
        public MSubjects Subject { get; set; }
    }
}
