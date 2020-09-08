using System;
using System.Collections.Generic;

namespace MySchool.Models
{
    public partial class MRights
    {
        public MRights()
        {
            MRoleRights = new HashSet<MRoleRights>();
        }

        public int Id { get; set; }
        public int Menuid { get; set; }
        public string Displayname { get; set; }
        public string Rname { get; set; }
        public string Url { get; set; }
        public bool? Visible { get; set; }
        public int? Sort { get; set; }
        public int? Groupid { get; set; }
        public string Authid { get; set; }
        public int? Parentid { get; set; }

        public Menu Menu { get; set; }
        public ICollection<MRoleRights> MRoleRights { get; set; }
    }
}
