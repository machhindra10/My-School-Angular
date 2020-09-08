using System;
using System.Collections.Generic;

namespace MySchool.Models
{
    public partial class ClassSubjects
    {
        public int Id { get; set; }
        public int? Classid { get; set; }
        public int? Subjectid { get; set; }
        public int? Userid { get; set; }
        public DateTime? Datecreated { get; set; }
        public int? Staffid { get; set; }

        public MClasses Class { get; set; }
        public MStaff Staff { get; set; }
        public MSubjects Subject { get; set; }
    }
}
