using System;
using System.Collections.Generic;

namespace MySchool.Models
{
    public partial class TStudentAttendence
    {
        public int Id { get; set; }
        public int? Studentid { get; set; }
        public int? Classid { get; set; }
        public int? Batchid { get; set; }
        public bool? Ispresent { get; set; }
        public DateTime? Datecreated { get; set; }

        public Batches Batch { get; set; }
        public MClasses Class { get; set; }
        public Student Student { get; set; }
    }
}
