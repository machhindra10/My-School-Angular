using System;
using System.Collections.Generic;

namespace MySchool.Models
{
    public partial class MessagesGuardians
    {
        public long Id { get; set; }
        public long Messageid { get; set; }
        public long Guardianid { get; set; }
        public DateTime? Delivered { get; set; }
        public DateTime? Read { get; set; }

        public StudentGuardian Guardian { get; set; }
        public Messages Message { get; set; }
    }
}
