using System;
using System.Collections.Generic;

namespace MySchool.Models
{
    public partial class StaffSalaryDetails
    {
        public int Id { get; set; }
        public int? SsId { get; set; }
        public int? Staffid { get; set; }
        public string Head { get; set; }
        public decimal? Amount { get; set; }
        public string Type { get; set; }

        public StaffSalary Ss { get; set; }
        public MStaff Staff { get; set; }
    }
}
