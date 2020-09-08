using System;
using System.Collections.Generic;

namespace MySchool.Models
{
    public partial class ClassFees
    {
        public int Id { get; set; }
        public int? Classid { get; set; }
        public string FeesType { get; set; }
        public decimal Amount { get; set; }
        public int? Userid { get; set; }
        public DateTime? Datecreated { get; set; }

        public MClasses Class { get; set; }
    }
}
