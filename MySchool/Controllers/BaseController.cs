using Microsoft.AspNetCore.Mvc;
using MySchool.Models;
using System;
using System.Linq;

namespace MySchool.Controllers
{
    public abstract class BaseController : ControllerBase
    {
        protected int GetUserId()
        {
            return int.Parse(this.User.Claims.First(i => i.Type == "userid").Value);
        }
        protected int GetUserRoleId()
        {
            return int.Parse(this.User.Claims.First(i => i.Type == "userroleid").Value);
        }
        protected int GetBatchId()
        {
            return int.Parse(this.User.Claims.First(i => i.Type == "batchid").Value);
        }
        protected bool GetIsUserMasterAdmin()
        {
            return bool.Parse(this.User.Claims.First(i => i.Type == "ismasteradmin").Value);
        }
        protected string GetTimeZone()
        {
            return Convert.ToString(this.User.Claims.First(i => i.Type == "tz").Value);
        }

        protected DateTime GetTimeZoneDate(DateTime? datetime)
        {
            //ngSchoolContext _context = new ngSchoolContext();
            string timezoneId = this.GetTimeZone();// _context.Settings.Where(e => e.Id == 1).Select(e => e.Timezoneid).FirstOrDefault();
            if (string.IsNullOrEmpty(timezoneId))
            {
                timezoneId = "India Stanadard Time";
            }
            TimeZoneInfo infotime = TimeZoneInfo.FindSystemTimeZoneById(timezoneId);
            DateTime thisDate = TimeZoneInfo.ConvertTime(Convert.ToDateTime(datetime), infotime);
            //_context.Dispose();
            return thisDate;
            //return Convert.ToDateTime(datetime);
        }
    }
}
