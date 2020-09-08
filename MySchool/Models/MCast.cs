using System;
using System.Collections.Generic;

namespace MySchool.Models
{
    public partial class MCast
    {
        public MCast()
        {
            Student = new HashSet<Student>();
        }

        public int Id { get; set; }
        public string Code { get; set; }
        public string Castname { get; set; }
        public bool? Disabled { get; set; }

        public ICollection<Student> Student { get; set; }
    }
}
