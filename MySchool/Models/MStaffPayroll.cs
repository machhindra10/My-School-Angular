using System;
using System.Collections.Generic;

namespace MySchool.Models
{
    public partial class MStaffPayroll
    {
        public int Id { get; set; }
        public int? Staffid { get; set; }
        public string Head { get; set; }
        public decimal? Amount { get; set; }
        public string Type { get; set; }

        public MStaff Staff { get; set; }
    }
}
