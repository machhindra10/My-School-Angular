using System;
using System.Collections.Generic;

namespace MySchool.Models
{
    public partial class TExam
    {
        public TExam()
        {
            TExamMarkSheet = new HashSet<TExamMarkSheet>();
            TExamSchedule = new HashSet<TExamSchedule>();
            TExamStudentsAdmitCard = new HashSet<TExamStudentsAdmitCard>();
        }

        public int Id { get; set; }
        public int? Batchid { get; set; }
        public int? Classid { get; set; }
        public string ExamName { get; set; }
        public int? Userid { get; set; }
        public DateTime? DateCreated { get; set; }

        public Batches Batch { get; set; }
        public MClasses Class { get; set; }
        public ICollection<TExamMarkSheet> TExamMarkSheet { get; set; }
        public ICollection<TExamSchedule> TExamSchedule { get; set; }
        public ICollection<TExamStudentsAdmitCard> TExamStudentsAdmitCard { get; set; }
    }
}
