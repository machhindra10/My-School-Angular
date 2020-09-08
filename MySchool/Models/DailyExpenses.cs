using System;
using System.Collections.Generic;

namespace MySchool.Models
{
    public partial class DailyExpenses
    {
        public int Id { get; set; }
        public string Details { get; set; }
        public DateTime? Datecreated { get; set; }
        public decimal? Amount { get; set; }
        public string Receiptno { get; set; }
        public int? Userid { get; set; }

        public MUser User { get; set; }
    }
}
