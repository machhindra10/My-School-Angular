using System;
using System.Collections.Generic;

namespace MySchool.Models
{
    public partial class StaffSalary
    {
        public StaffSalary()
        {
            StaffSalaryDetails = new HashSet<StaffSalaryDetails>();
        }

        public int Id { get; set; }
        public int? Staffid { get; set; }
        public int? Month { get; set; }
        public int? Year { get; set; }
        public decimal? Earnings { get; set; }
        public decimal? Deductions { get; set; }
        public decimal? Adjustments { get; set; }
        public decimal? Netpay { get; set; }
        public bool? Ispaid { get; set; }
        public DateTime? Datepaid { get; set; }
        public int? Userid { get; set; }
        public DateTime? Datecreated { get; set; }

        public MStaff Staff { get; set; }
        public MUser User { get; set; }
        public ICollection<StaffSalaryDetails> StaffSalaryDetails { get; set; }
    }
}
