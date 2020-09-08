using System;
using System.Collections.Generic;

namespace MySchool.Models
{
    public partial class StudentGuardian
    {
        public StudentGuardian()
        {
            MessagesGuardians = new HashSet<MessagesGuardians>();
            Student = new HashSet<Student>();
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public long Mobile { get; set; }
        public string Password { get; set; }
        public string Address { get; set; }
        public bool Disabled { get; set; }
        public int? Userid { get; set; }
        public DateTime? Datecreated { get; set; }
        public string Passverificationcode { get; set; }

        public ICollection<MessagesGuardians> MessagesGuardians { get; set; }
        public ICollection<Student> Student { get; set; }
    }
}
