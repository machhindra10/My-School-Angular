using System;
using System.Collections.Generic;

namespace MySchool.Models
{
    public partial class MPayrollTemplete
    {
        public int Id { get; set; }
        public string Payname { get; set; }
        public decimal? Amount { get; set; }
        public string Type { get; set; }
    }
}
