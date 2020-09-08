using System;
using System.Collections.Generic;

namespace MySchool.Models
{
    public partial class StaffAttendance
    {
        public long Id { get; set; }
        public int? Staffid { get; set; }
        public string Ispresent { get; set; }
        public DateTime? Datecreated { get; set; }

        public MStaff Staff { get; set; }
    }
}
