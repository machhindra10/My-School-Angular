using System;
using System.Collections.Generic;

namespace MySchool.Models
{
    public partial class TStudentFees
    {
        public int Id { get; set; }
        public int? Studentid { get; set; }
        public int? Classid { get; set; }
        public int? Batchid { get; set; }
        public string FeesType { get; set; }
        public decimal? Amount { get; set; }
        public int? Userid { get; set; }
        public DateTime? Datecreated { get; set; }

        public Batches Batch { get; set; }
        public Student Student { get; set; }
    }
}
