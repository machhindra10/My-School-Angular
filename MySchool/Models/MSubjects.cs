using System;
using System.Collections.Generic;

namespace MySchool.Models
{
    public partial class MSubjects
    {
        public MSubjects()
        {
            ClassSubjects = new HashSet<ClassSubjects>();
            TExamMarkSheet = new HashSet<TExamMarkSheet>();
            TExamSchedule = new HashSet<TExamSchedule>();
        }

        public int Id { get; set; }
        public string Code { get; set; }
        public string Subject { get; set; }
        public int? Staffid { get; set; }
        public int? Userid { get; set; }
        public DateTime? Datecreated { get; set; }
        public bool? Disabled { get; set; }

        public MStaff Staff { get; set; }
        public ICollection<ClassSubjects> ClassSubjects { get; set; }
        public ICollection<TExamMarkSheet> TExamMarkSheet { get; set; }
        public ICollection<TExamSchedule> TExamSchedule { get; set; }
    }
}
