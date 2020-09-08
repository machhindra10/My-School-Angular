using System;
using System.Collections.Generic;

namespace MySchool.Models
{
    public partial class MTimeTable
    {
        public int Id { get; set; }
        public int? Classid { get; set; }
        public int? Batchid { get; set; }
        public TimeSpan? Fromtime { get; set; }
        public TimeSpan? Totime { get; set; }
        public int? Sunday { get; set; }
        public int? Monday { get; set; }
        public int? Tuesday { get; set; }
        public int? Wednesday { get; set; }
        public int? Thursday { get; set; }
        public int? Friday { get; set; }
        public int? Saturday { get; set; }

        public Batches Batch { get; set; }
        public MClasses Class { get; set; }
    }
}
