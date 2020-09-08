using System;
using System.Collections.Generic;

namespace MySchool.Models
{
    public partial class MLeaveType
    {
        public MLeaveType()
        {
            LeaveApplication = new HashSet<LeaveApplication>();
            TLeaves = new HashSet<TLeaves>();
        }

        public long Id { get; set; }
        public string Code { get; set; }
        public string Leavetype { get; set; }
        public bool? Iscarryforward { get; set; }

        public ICollection<LeaveApplication> LeaveApplication { get; set; }
        public ICollection<TLeaves> TLeaves { get; set; }
    }
}
