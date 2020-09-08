using System;
using System.Collections.Generic;

namespace MySchool.Models
{
    public partial class MDesignation
    {
        public MDesignation()
        {
            MStaff = new HashSet<MStaff>();
        }

        public int Id { get; set; }
        public string Designname { get; set; }
        public bool? Disabled { get; set; }
        public bool Isdefault { get; set; }

        public ICollection<MStaff> MStaff { get; set; }
    }
}
