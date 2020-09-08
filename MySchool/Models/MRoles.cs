using System;
using System.Collections.Generic;

namespace MySchool.Models
{
    public partial class MRoles
    {
        public MRoles()
        {
            MRoleRights = new HashSet<MRoleRights>();
            MUser = new HashSet<MUser>();
        }

        public int Id { get; set; }
        public string Rolename { get; set; }
        public int? Userid { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public bool? Isadmin { get; set; }
        public bool? Isdefault { get; set; }
        public bool? Disabled { get; set; }

        public ICollection<MRoleRights> MRoleRights { get; set; }
        public ICollection<MUser> MUser { get; set; }
    }
}
