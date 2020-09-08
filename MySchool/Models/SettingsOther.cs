using System;
using System.Collections.Generic;

namespace MySchool.Models
{
    public partial class SettingsOther
    {
        public int Id { get; set; }
        public string Smtpemailid { get; set; }
        public string Smtppassword { get; set; }
        public int Smtpport { get; set; }
        public string Smtphost { get; set; }
        public bool Smtpenablessl { get; set; }
        public string Smtpbccid { get; set; }
        public string Smsusername { get; set; }
        public string Smspassword { get; set; }
        public string Smskey { get; set; }
        public string Smssenderid { get; set; }
        public string Smsprofileid { get; set; }
    }
}
