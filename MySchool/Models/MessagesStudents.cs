using System;
using System.Collections.Generic;

namespace MySchool.Models
{
    public partial class MessagesStudents
    {
        public long Id { get; set; }
        public long Messageid { get; set; }
        public int Studentid { get; set; }
        public DateTime? Delivered { get; set; }
        public DateTime? Read { get; set; }

        public Messages Message { get; set; }
        public Student Student { get; set; }
    }
}
