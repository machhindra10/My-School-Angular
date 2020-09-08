using System;
using System.Collections.Generic;

namespace MySchool.Models
{
    public partial class Settings
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string Logo { get; set; }
        public string Appname { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Currency { get; set; }
        public string Timezoneid { get; set; }
        public string Dbbackuppath { get; set; }
        public int? Userid { get; set; }
        public string Token { get; set; }
    }
}
