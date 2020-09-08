using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySchool.Models;

namespace MySchool.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : BaseController, IDisposable
    {
        private readonly ngSchoolContext _context;

        public NotificationsController(ngSchoolContext context)
        {
            _context = context;
        }

        // GET api/values/5
        [HttpGet(), Route("getnotifications")]
        public IActionResult GetNotificationsAll()
        {
            Notification notifications = new Notification
            {
                Students = GetStudentBirthdays(),
                Staff = GetStaffBirthdays()
            };

            notifications.Count = notifications.Students.Count() + notifications.Staff.Count();

            return Ok(notifications);
        }

        private List<Birthday> GetStudentBirthdays()
        {
            List<Birthday> birthdays = new List<Birthday>();

            try
            {
                int days = GetTimeZoneDate(DateTime.UtcNow).Day + 7;
                int batchid = GetBatchId();
                DateTime dt = GetTimeZoneDate(DateTime.UtcNow);

                var sids = (from a in _context.TStudentAdmission where a.Batchid == batchid select a.Studentid).ToList();
                var q = (from p in _context.Student
                         orderby p.Dob.Value.Day ascending
                         where sids.Contains(p.Id) && p.Dob.Value.Month == dt.Month
                         && p.Dob.Value.Day >= dt.Day && p.Dob.Value.Day <= days
                         select new
                         {
                             Id = p.Id,
                             studentname = p.Fname + " " + p.Lname,
                             dob = p.Dob,
                             dayname = ""
                         }).ToList();
               
                DateTime nowDate = GetTimeZoneDate(DateTime.UtcNow);
                DateTime date;
                string dayName = string.Empty;

                foreach (var item in q)
                {
                    date = new DateTime(nowDate.Year, item.dob.Value.Month, item.dob.Value.Day);
                    if (date.Date == nowDate.Date)
                    {
                        dayName = "Today";
                    }
                    else if (date.Date == nowDate.Date.AddDays(1))
                    {
                        dayName = "Tomorrow";
                    }
                    else
                    {
                        dayName = date.DayOfWeek.ToString();
                    }

                    birthdays.Add(new Birthday()
                    {
                        Id = item.Id,
                        Name = item.studentname,
                        Dob = item.dob.ToString(),
                        DayName = dayName
                    });                    
                }
                
            }
            catch (Exception)
            {
            }
            return birthdays;
        }

        private List<Birthday> GetStaffBirthdays()
        {
            List<Birthday> birthdays = new List<Birthday>();

            try
            {
                int days = GetTimeZoneDate(DateTime.UtcNow).Day + 7;
                int batchid = GetBatchId();
                DateTime dt = GetTimeZoneDate(DateTime.UtcNow);                

                DateTime monthEndDate = new DateTime(dt.Year, dt.Month, DateTime.DaysInMonth(dt.Year, dt.Month));
                DateTime monthStartDate = new DateTime(dt.Year, dt.Month, 1);

                //get staff only whose DOJ is less than selected month and year
                var staff = (from p in _context.MStaff
                                  where p.Disabled == false && p.Doj.Value.Date <= monthEndDate.Date &&
                                        p.Dol.HasValue ? p.Dol.Value.Date >= monthStartDate.Date :
                                        p.Doj.Value.Date <= monthEndDate.Date &&
                                        p.Dob.Value.Month == dt.Month
                                            && p.Dob.Value.Day >= dt.Day && p.Dob.Value.Day <= days
                             select new
                             {
                                 Id = p.Id,
                                 name = p.Staffname,
                                 dob = p.Dob,
                                 dayname = ""
                             }).ToList();

                DateTime nowDate = GetTimeZoneDate(DateTime.UtcNow);
                DateTime date;
                string dayName = string.Empty;

                foreach (var item in staff)
                {
                    date = new DateTime(nowDate.Year, item.dob.Value.Month, item.dob.Value.Day);
                    if (date.Date == nowDate.Date)
                    {
                        dayName = "Today";
                    }
                    else if (date.Date == nowDate.Date.AddDays(1))
                    {
                        dayName = "Tomorrow";
                    }
                    else
                    {
                        dayName = date.DayOfWeek.ToString();
                    }

                    birthdays.Add(new Birthday()
                    {
                        Id = item.Id,
                        Name = item.name,
                        Dob = item.dob.ToString(),
                        DayName = dayName
                    });
                }
                
            }
            catch (Exception)
            {
            }
            return birthdays;
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        private class Notification
        {
            public List<Birthday> Staff { get; set; }
            public List<Birthday> Students { get; set; }
            public int Count { get; set; }
        }

        public class Birthday
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Dob { get; set; }
            public string DayName { get; set; }
        }
    }
}
