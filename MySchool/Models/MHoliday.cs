using System;
using System.Collections.Generic;

namespace MySchool.Models
{
    public partial class MHoliday
    {
        public int Id { get; set; }
        public string Holiday { get; set; }
        public DateTime? Dates { get; set; }
    }
}
