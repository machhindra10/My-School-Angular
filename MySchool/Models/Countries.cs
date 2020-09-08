using System;
using System.Collections.Generic;

namespace MySchool.Models
{
    public partial class Countries
    {
        public int Id { get; set; }
        public string Sortname { get; set; }
        public string Name { get; set; }
        public int? Phonecode { get; set; }
    }
}
