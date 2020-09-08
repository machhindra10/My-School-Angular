using System;
using System.Collections.Generic;

namespace MySchool.Models
{
    public partial class TLeaves
    {
        public long Id { get; set; }
        public long? Leavetypeid { get; set; }
        public decimal? Leaves { get; set; }
        public int? Year { get; set; }

        public MLeaveType Leavetype { get; set; }
    }
}
