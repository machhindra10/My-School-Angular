using System;
using System.Collections.Generic;

namespace MySchool.Models
{
    public partial class MFees
    {
        public int Id { get; set; }
        public string FeesType { get; set; }
        public decimal? Amount { get; set; }
        public bool? Disabled { get; set; }
        public int? Userid { get; set; }
    }
}
