using System;
using System.Collections.Generic;

namespace MySchool.Models
{
    public partial class ClassTeacher
    {
        public int Id { get; set; }
        public int? Classid { get; set; }
        public int? Staffid { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public bool? Isactive { get; set; }
        public int? Userid { get; set; }

        public MClasses Class { get; set; }
        public MStaff Staff { get; set; }
        public MUser User { get; set; }
    }
}
