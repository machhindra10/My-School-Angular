using System;
using System.Collections.Generic;

namespace MySchool.Models
{
    public partial class MRoleRights
    {
        public long Id { get; set; }
        public int? RoleId { get; set; }
        public int RightId { get; set; }

        public MRights Right { get; set; }
        public MRoles Role { get; set; }
    }
}
