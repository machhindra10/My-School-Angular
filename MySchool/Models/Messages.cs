using System;
using System.Collections.Generic;

namespace MySchool.Models
{
    public partial class Messages
    {
        public Messages()
        {
            MessagesGuardians = new HashSet<MessagesGuardians>();
            MessagesStudents = new HashSet<MessagesStudents>();
        }

        public long Id { get; set; }
        public string Message { get; set; }
        public string Type { get; set; }
        public DateTime? Datecreated { get; set; }
        public int? Userid { get; set; }

        public ICollection<MessagesGuardians> MessagesGuardians { get; set; }
        public ICollection<MessagesStudents> MessagesStudents { get; set; }
    }
}
