using System;
using System.Collections.Generic;

namespace MySchool.Models
{
    public partial class MUser
    {
        public MUser()
        {
            ClassTeacher = new HashSet<ClassTeacher>();
            DailyExpenses = new HashSet<DailyExpenses>();
            MClasses = new HashSet<MClasses>();
            MStaff = new HashSet<MStaff>();
            StaffSalary = new HashSet<StaffSalary>();
        }

        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Fname { get; set; }
        public string Mname { get; set; }
        public string Lname { get; set; }
        public string Photo { get; set; }
        public string Aadharno { get; set; }
        public string Email { get; set; }
        public bool? Disabled { get; set; }
        public int RoleId { get; set; }
        public int Userid { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public bool IsMasterAdmin { get; set; }
        public bool IsAdmin { get; set; }
        public string Passverificationcode { get; set; }
        public DateTime? Lastlogin { get; set; }
        public DateTime? Currentlogin { get; set; }

        public MRoles Role { get; set; }
        public ICollection<ClassTeacher> ClassTeacher { get; set; }
        public ICollection<DailyExpenses> DailyExpenses { get; set; }
        public ICollection<MClasses> MClasses { get; set; }
        public ICollection<MStaff> MStaff { get; set; }
        public ICollection<StaffSalary> StaffSalary { get; set; }
    }
}
