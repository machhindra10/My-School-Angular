using System;
using System.Collections.Generic;

namespace MySchool.Models
{
    public partial class LeaveApplication
    {
        public int Id { get; set; }
        public int Staffid { get; set; }
        public long Leavetypeid { get; set; }
        public DateTime Datefrom { get; set; }
        public DateTime Dateto { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string Reason { get; set; }

        public MLeaveType Leavetype { get; set; }
        public MStaff Staff { get; set; }
    }
}
