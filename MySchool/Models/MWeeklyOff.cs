using System;
using System.Collections.Generic;

namespace MySchool.Models
{
    public partial class MWeeklyOff
    {
        public int Id { get; set; }
        public string PosInMonth { get; set; }
        public string Weekday { get; set; }
    }
}
