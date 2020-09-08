using System;
using System.Collections.Generic;

namespace MySchool.Models
{
    public partial class Menu
    {
        public Menu()
        {
            MRights = new HashSet<MRights>();
        }

        public int Id { get; set; }
        public string Menu1 { get; set; }
        public string Url { get; set; }
        public bool? Visible { get; set; }
        public int? Sort { get; set; }
        public bool? Isdefault { get; set; }
        public string Icon { get; set; }

        public ICollection<MRights> MRights { get; set; }
    }
}
