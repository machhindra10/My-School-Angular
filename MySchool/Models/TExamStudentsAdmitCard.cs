using System;
using System.Collections.Generic;

namespace MySchool.Models
{
    public partial class TExamStudentsAdmitCard
    {
        public int Id { get; set; }
        public int? Batchid { get; set; }
        public int? Examid { get; set; }
        public int? Classid { get; set; }
        public int? Studentid { get; set; }
        public int? Userid { get; set; }
        public DateTime? DateCreated { get; set; }
        public bool? Disabled { get; set; }

        public Batches Batch { get; set; }
        public MClasses Class { get; set; }
        public TExam Exam { get; set; }
        public Student Student { get; set; }
    }
}
