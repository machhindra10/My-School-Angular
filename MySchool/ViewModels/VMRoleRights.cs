using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MySchool.ViewModels
{
    public class VMRoleRights
    {
        public decimal Id { get; set; }
        public int Menuid { get; set; }
        public string Menuname { get; set; }
        public string Displayname { get; set; }
        public string Rname { get; set; }
        public string Url { get; set; }
        public bool? Visible { get; set; }
        public int? Sort { get; set; }
        public int? Groupid { get; set; }
        public bool IsEnabled { get; set; }
        public decimal RoleRightId { get; set; }
    }
}
