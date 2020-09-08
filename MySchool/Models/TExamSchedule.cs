using System;
using System.Collections.Generic;

namespace MySchool.Models
{
    public partial class TExamSchedule
    {
        public TExamSchedule()
        {
            TExamMarkSheet = new HashSet<TExamMarkSheet>();
        }

        public int Id { get; set; }
        public int? Batchid { get; set; }
        public int Examid { get; set; }
        public int? Classid { get; set; }
        public int? Subjectid { get; set; }
        public int? Totalmarks { get; set; }
        public int? Passingmarks { get; set; }
        public DateTime? Examdate { get; set; }
        public TimeSpan? Starttime { get; set; }
        public TimeSpan? Endtime { get; set; }
        public int? Userid { get; set; }
        public DateTime? DateCreated { get; set; }

        public Batches Batch { get; set; }
        public MClasses Class { get; set; }
        public TExam Exam { get; set; }
        public MSubjects Subject { get; set; }
        public ICollection<TExamMarkSheet> TExamMarkSheet { get; set; }
    }
}
