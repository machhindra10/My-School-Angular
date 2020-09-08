using System;
using System.Collections.Generic;

namespace MySchool.Models
{
    public partial class StaffLeaves
    {
        public long Id { get; set; }
        public int? Staffid { get; set; }
        public string Leavetype { get; set; }
        public DateTime? Datecreated { get; set; }

        public MStaff Staff { get; set; }
    }
}
