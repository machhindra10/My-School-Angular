using System;
using System.Collections.Generic;

namespace MySchool.Models
{
    public partial class MPayHead
    {
        public int Id { get; set; }
        public string Head { get; set; }
        public decimal? Amount { get; set; }
        public string Type { get; set; }
    }
}
