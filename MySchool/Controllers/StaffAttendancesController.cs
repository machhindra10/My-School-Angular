using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySchool.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MySchool.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StaffAttendancesController : BaseController, IDisposable
    {
        private readonly ngSchoolContext _context;

        public StaffAttendancesController(ngSchoolContext context)
        {
            _context = context;
        }

        // GET: api/StaffAttendances
        [HttpGet]
        public IEnumerable<StaffAttendance> GetStaffAttendance()
        {
            return _context.StaffAttendance;
        }

        // GET: api/StaffAttendances/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetStaffAttendance([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var staffAttendance = await _context.StaffAttendance.FindAsync(id);

            if (staffAttendance == null)
            {
                return NotFound();
            }

            return Ok(staffAttendance);
        }       

        // PUT: api/StaffAttendances/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStaffAttendance([FromRoute] int id, [FromBody] StaffAttendance staffAttendance)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != staffAttendance.Id)
            {
                return BadRequest();
            }

            _context.Entry(staffAttendance).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StaffAttendanceExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/StaffAttendances
        [HttpPost]
        public async Task<IActionResult> PostStaffAttendance([FromBody] StaffAttendance staffAttendance)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.StaffAttendance.Add(staffAttendance);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStaffAttendance", new { id = staffAttendance.Id }, staffAttendance);
        }

        // DELETE: api/StaffAttendances/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStaffAttendance([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var staffAttendance = await _context.StaffAttendance.FindAsync(id);
            if (staffAttendance == null)
            {
                return NotFound();
            }

            _context.StaffAttendance.Remove(staffAttendance);
            await _context.SaveChangesAsync();

            return Ok(staffAttendance);
        }

        private bool StaffAttendanceExists(int id)
        {
            return _context.StaffAttendance.Any(e => e.Id == id);
        }

        public void Dispose()
        {
            _context.Dispose();
        }



        // GET: api/StaffAttendances/5
        [HttpGet, Route("getbymonth/{month}/{year}")]
        public IActionResult GetStaffAttendanceOfAllStaff([FromRoute] int month, int year)
        {
            DateTime monthEndDate = new DateTime(year, month, DateTime.DaysInMonth(year, month));
            DateTime monthStartDate = new DateTime(year, month, 1);

            WeeklyOffsController weeklyOffsController = new WeeklyOffsController(_context);
            List<DateTime> dateTimes = weeklyOffsController.GetWeeklyOffDaysForMonth(month, year);

            var staffAttendance = (from p in _context.MStaff
                                   where p.Disabled == false && p.Doj.Value.Date <= monthEndDate.Date &&
                                     p.Dol.HasValue ? p.Dol.Value.Date >= monthStartDate.Date :
                                     p.Doj.Value.Date <= monthEndDate.Date
                                   select new SAData
                                   {
                                       Id = p.Id,
                                       Staffname = p.Staffname,

                                   }).ToList();

            foreach (var staff in staffAttendance)
            {
                foreach (var col in staff.GetType().GetProperties().Where(p => p.Name.StartsWith("_")))
                {
                    col.SetValue(staff, GetDayStatus(staff.Id, month, year, Convert.ToInt32(col.Name.Split('_')[1]), dateTimes));
                }
            }

            return Ok(staffAttendance);
        }

        private string GetDayStatus(int id, int month, int year, int day, List<DateTime> weekdays)
        {
            DateTime dateTime = new DateTime();
            string value = string.Empty;
            if (day <= DateTime.DaysInMonth(year, month))
            {
                dateTime = new DateTime(year, month, day);
            }
            else
            {
                return value;
            }
            value = (from sa in _context.StaffLeaves
                     where sa.Staffid == id && sa.Datecreated.Value == dateTime.Date
                     select sa.Leavetype).FirstOrDefault();

            if (value == null)
            {
                value = (from p in weekdays where p == dateTime select "WO").FirstOrDefault();
                if (value == null)
                {
                    value = (from sa in _context.StaffAttendance
                             where sa.Staffid == id && sa.Datecreated.Value == dateTime.Date
                             select sa.Ispresent).FirstOrDefault();
                }
            }


            return value;
        }

        // GET: api/StaffAttendances/5
        [HttpGet, Route("getbymonth-notuse/{month}/{year}")]
        public IActionResult GetStaffAttendanceOfAllStaff_nouse([FromRoute] int month, int year)
        {
            WeeklyOffsController weeklyOffsController = new WeeklyOffsController(_context);
            List<DateTime> dateTimes = weeklyOffsController.GetWeeklyOffDaysForMonth(month, year);

            var staffAttendance = (from p in _context.MStaff
                                   where p.Disabled == false
                                   select new SAData
                                   {
                                       Id = p.Id,
                                       Staffname = p.Staffname,
                                       _1 = (from sa in _context.StaffAttendance
                                             where sa.Staffid == p.Id
                                                     && sa.Datecreated.Value.Month == month && sa.Datecreated.Value.Year == year
                                                     && sa.Datecreated.Value.Day == 1
                                             select sa.Ispresent).FirstOrDefault(),
                                       _2 = (from sa in _context.StaffAttendance
                                             where sa.Staffid == p.Id
                                                       && sa.Datecreated.Value.Month == month && sa.Datecreated.Value.Year == year
                                                       && sa.Datecreated.Value.Day == 2
                                             select sa.Ispresent).FirstOrDefault(),
                                       _3 = (from sa in _context.StaffAttendance
                                             where sa.Staffid == p.Id
                                                       && sa.Datecreated.Value.Month == month && sa.Datecreated.Value.Year == year
                                                       && sa.Datecreated.Value.Day == 3
                                             select sa.Ispresent).FirstOrDefault(),
                                       _4 = (from sa in _context.StaffAttendance
                                             where sa.Staffid == p.Id
                                                       && sa.Datecreated.Value.Month == month && sa.Datecreated.Value.Year == year
                                                       && sa.Datecreated.Value.Day == 4
                                             select sa.Ispresent).FirstOrDefault(),
                                       _5 = (from sa in _context.StaffAttendance
                                             where sa.Staffid == p.Id
                                                       && sa.Datecreated.Value.Month == month && sa.Datecreated.Value.Year == year
                                                       && sa.Datecreated.Value.Day == 5
                                             select sa.Ispresent).FirstOrDefault(),
                                       _6 = (from sa in _context.StaffAttendance
                                             where sa.Staffid == p.Id
                                                       && sa.Datecreated.Value.Month == month && sa.Datecreated.Value.Year == year
                                                       && sa.Datecreated.Value.Day == 6
                                             select sa.Ispresent).FirstOrDefault(),
                                       _7 = (from sa in _context.StaffAttendance
                                             where sa.Staffid == p.Id
                                                       && sa.Datecreated.Value.Month == month && sa.Datecreated.Value.Year == year
                                                       && sa.Datecreated.Value.Day == 7
                                             select sa.Ispresent).FirstOrDefault(),
                                       _8 = (from sa in _context.StaffAttendance
                                             where sa.Staffid == p.Id
                                                       && sa.Datecreated.Value.Month == month && sa.Datecreated.Value.Year == year
                                                       && sa.Datecreated.Value.Day == 8
                                             select sa.Ispresent).FirstOrDefault(),
                                       _9 = (from sa in _context.StaffAttendance
                                             where sa.Staffid == p.Id
                                                       && sa.Datecreated.Value.Month == month && sa.Datecreated.Value.Year == year
                                                       && sa.Datecreated.Value.Day == 9
                                             select sa.Ispresent).FirstOrDefault(),
                                       _10 = (from sa in _context.StaffAttendance
                                              where sa.Staffid == p.Id
                                                        && sa.Datecreated.Value.Month == month && sa.Datecreated.Value.Year == year
                                                        && sa.Datecreated.Value.Day == 10
                                              select sa.Ispresent).FirstOrDefault(),
                                       _11 = (from sa in _context.StaffAttendance
                                              where sa.Staffid == p.Id
                                                      && sa.Datecreated.Value.Month == month && sa.Datecreated.Value.Year == year
                                                      && sa.Datecreated.Value.Day == 11
                                              select sa.Ispresent).FirstOrDefault(),
                                       _12 = (from sa in _context.StaffAttendance
                                              where sa.Staffid == p.Id
                                                        && sa.Datecreated.Value.Month == month && sa.Datecreated.Value.Year == year
                                                        && sa.Datecreated.Value.Day == 12
                                              select sa.Ispresent).FirstOrDefault(),
                                       _13 = (from sa in _context.StaffAttendance
                                              where sa.Staffid == p.Id
                                                        && sa.Datecreated.Value.Month == month && sa.Datecreated.Value.Year == year
                                                        && sa.Datecreated.Value.Day == 13
                                              select sa.Ispresent).FirstOrDefault(),
                                       _14 = (from sa in _context.StaffAttendance
                                              where sa.Staffid == p.Id
                                                        && sa.Datecreated.Value.Month == month && sa.Datecreated.Value.Year == year
                                                        && sa.Datecreated.Value.Day == 14
                                              select sa.Ispresent).FirstOrDefault(),
                                       _15 = (from sa in _context.StaffAttendance
                                              where sa.Staffid == p.Id
                                                        && sa.Datecreated.Value.Month == month && sa.Datecreated.Value.Year == year
                                                        && sa.Datecreated.Value.Day == 15
                                              select sa.Ispresent).FirstOrDefault(),
                                       _16 = (from sa in _context.StaffAttendance
                                              where sa.Staffid == p.Id
                                                        && sa.Datecreated.Value.Month == month && sa.Datecreated.Value.Year == year
                                                        && sa.Datecreated.Value.Day == 16
                                              select sa.Ispresent).FirstOrDefault(),
                                       _17 = (from sa in _context.StaffAttendance
                                              where sa.Staffid == p.Id
                                                        && sa.Datecreated.Value.Month == month && sa.Datecreated.Value.Year == year
                                                        && sa.Datecreated.Value.Day == 17
                                              select sa.Ispresent).FirstOrDefault(),
                                       _18 = (from sa in _context.StaffAttendance
                                              where sa.Staffid == p.Id
                                                        && sa.Datecreated.Value.Month == month && sa.Datecreated.Value.Year == year
                                                        && sa.Datecreated.Value.Day == 18
                                              select sa.Ispresent).FirstOrDefault(),
                                       _19 = (from sa in _context.StaffAttendance
                                              where sa.Staffid == p.Id
                                                        && sa.Datecreated.Value.Month == month && sa.Datecreated.Value.Year == year
                                                        && sa.Datecreated.Value.Day == 19
                                              select sa.Ispresent).FirstOrDefault(),
                                       _20 = (from sa in _context.StaffAttendance
                                              where sa.Staffid == p.Id
                                                        && sa.Datecreated.Value.Month == month && sa.Datecreated.Value.Year == year
                                                        && sa.Datecreated.Value.Day == 20
                                              select sa.Ispresent).FirstOrDefault(),
                                       _21 = (from sa in _context.StaffAttendance
                                              where sa.Staffid == p.Id
                                                      && sa.Datecreated.Value.Month == month && sa.Datecreated.Value.Year == year
                                                      && sa.Datecreated.Value.Day == 21
                                              select sa.Ispresent).FirstOrDefault(),
                                       _22 = (from sa in _context.StaffAttendance
                                              where sa.Staffid == p.Id
                                                        && sa.Datecreated.Value.Month == month && sa.Datecreated.Value.Year == year
                                                        && sa.Datecreated.Value.Day == 22
                                              select sa.Ispresent).FirstOrDefault(),
                                       _23 = (from sa in _context.StaffAttendance
                                              where sa.Staffid == p.Id
                                                        && sa.Datecreated.Value.Month == month && sa.Datecreated.Value.Year == year
                                                        && sa.Datecreated.Value.Day == 23
                                              select sa.Ispresent).FirstOrDefault(),
                                       _24 = (from sa in _context.StaffAttendance
                                              where sa.Staffid == p.Id
                                                        && sa.Datecreated.Value.Month == month && sa.Datecreated.Value.Year == year
                                                        && sa.Datecreated.Value.Day == 24
                                              select sa.Ispresent).FirstOrDefault(),
                                       _25 = (from sa in _context.StaffAttendance
                                              where sa.Staffid == p.Id
                                                        && sa.Datecreated.Value.Month == month && sa.Datecreated.Value.Year == year
                                                        && sa.Datecreated.Value.Day == 25
                                              select sa.Ispresent).FirstOrDefault(),
                                       _26 = (from sa in _context.StaffAttendance
                                              where sa.Staffid == p.Id
                                                        && sa.Datecreated.Value.Month == month && sa.Datecreated.Value.Year == year
                                                        && sa.Datecreated.Value.Day == 26
                                              select sa.Ispresent).FirstOrDefault(),
                                       _27 = (from sa in _context.StaffAttendance
                                              where sa.Staffid == p.Id
                                                        && sa.Datecreated.Value.Month == month && sa.Datecreated.Value.Year == year
                                                        && sa.Datecreated.Value.Day == 27
                                              select sa.Ispresent).FirstOrDefault(),
                                       _28 = (from sa in _context.StaffAttendance
                                              where sa.Staffid == p.Id
                                                        && sa.Datecreated.Value.Month == month && sa.Datecreated.Value.Year == year
                                                        && sa.Datecreated.Value.Day == 28
                                              select sa.Ispresent).FirstOrDefault(),
                                       _29 = (from sa in _context.StaffAttendance
                                              where sa.Staffid == p.Id
                                                        && sa.Datecreated.Value.Month == month && sa.Datecreated.Value.Year == year
                                                        && sa.Datecreated.Value.Day == 29
                                              select sa.Ispresent).FirstOrDefault(),
                                       _30 = (from sa in _context.StaffAttendance
                                              where sa.Staffid == p.Id
                                                        && sa.Datecreated.Value.Month == month && sa.Datecreated.Value.Year == year
                                                        && sa.Datecreated.Value.Day == 30
                                              select sa.Ispresent).FirstOrDefault(),
                                       _31 = (from sa in _context.StaffAttendance
                                              where sa.Staffid == p.Id
                                                        && sa.Datecreated.Value.Month == month && sa.Datecreated.Value.Year == year
                                                        && sa.Datecreated.Value.Day == 31
                                              select sa.Ispresent).FirstOrDefault(),
                                   }).ToList();

            if (staffAttendance == null)
            {
                return NotFound();
            }

            return Ok(staffAttendance);
        }

        [HttpPut, Route("updateattendance/{month}/{year}")]
        public async Task<IActionResult> GetStaffAttendanceUpdate(int month, int year, [FromBody] List<SAData> listsAData)
        {
            bool result = false;
            string[] status = { "A", "P", "H", "WO" };
            foreach (var sAData in listsAData)
            {
                foreach (var col in sAData.GetType().GetProperties().Where(p => p.Name.StartsWith("_")))
                {
                    string value = (string)col.GetValue(sAData, null);
                    if (!string.IsNullOrEmpty(value))
                    {
                        if (status.Contains(value))     // Save only P A H WO
                        {
                            int daysinmonth = DateTime.DaysInMonth(year, month);
                            int day = Convert.ToInt32(Convert.ToInt32(col.Name.Split('_')[1]));

                            if (day <= daysinmonth)
                            {
                                DateTime dateTime = new DateTime(year, month, day);
                                //if (dateTime <= GetTimeZoneDate(DateTime.UtcNow).Date)
                                //{
                                    var staffAttendance = await _context.StaffAttendance.Where(p => p.Datecreated.Value == dateTime.Date && p.Staffid == sAData.Id).FirstOrDefaultAsync();
                                    if (staffAttendance == null)
                                    {
                                        staffAttendance = new StaffAttendance();
                                        staffAttendance.Staffid = sAData.Id;
                                        staffAttendance.Ispresent = value;
                                        staffAttendance.Datecreated = dateTime.Date;
                                        _context.StaffAttendance.Add(staffAttendance);
                                        //result = true;
                                    }
                                    else
                                    {
                                        staffAttendance.Ispresent = value;
                                        _context.Entry(staffAttendance).State = EntityState.Modified;
                                        //result = true;
                                    }
                                //}
                            }
                        }
                    }
                }
            }

            int i = await _context.SaveChangesAsync();
            if (i > 0) { result = true; }
            return Ok(new { result = result });
        }

        public decimal GetTotalPresentyByStaffId(int? staffid, int? month, int? year)
        {
            string[] status = { "P", "WO", "NH" }; //take Only present == true and full day

            var AprovedLeaves = (from p in _context.StaffLeaves
                                 where p.Staffid == staffid && p.Datecreated.Value.Year == year
                                       && p.Datecreated.Value.Month == month
                                 select p).ToList();
            int totalAprovedLeaves = AprovedLeaves.Count();

            int daysPresent = (from p in _context.StaffAttendance
                               where p.Staffid == staffid && p.Datecreated.Value.Year == year
                                     && p.Datecreated.Value.Month == month
                                     && status.Contains(p.Ispresent) && !AprovedLeaves.Select(x => x.Datecreated).Contains(p.Datecreated)
                               select p).Count();

            var halfDays = (from p in _context.StaffAttendance
                            where p.Staffid == staffid && p.Datecreated.Value.Year == year
                                  && p.Datecreated.Value.Month == month
                                  && p.Ispresent == "H" && !AprovedLeaves.Select(x => x.Datecreated).Contains(p.Datecreated)
                            select p).Count();

            decimal halfDaysActual = (Convert.ToDecimal(halfDays) / 2);
            return (daysPresent + halfDaysActual + totalAprovedLeaves);
        }
        
    }

    public class SAData
    {
        public int Id { get; set; }
        public string Staffname { get; set; }
        public string _1 { get; set; }
        public string _2 { get; set; }
        public string _3 { get; set; }
        public string _4 { get; set; }
        public string _5 { get; set; }
        public string _6 { get; set; }
        public string _7 { get; set; }
        public string _8 { get; set; }
        public string _9 { get; set; }
        public string _10 { get; set; }
        public string _11 { get; set; }
        public string _12 { get; set; }
        public string _13 { get; set; }
        public string _14 { get; set; }
        public string _15 { get; set; }
        public string _16 { get; set; }
        public string _17 { get; set; }
        public string _18 { get; set; }
        public string _19 { get; set; }
        public string _20 { get; set; }
        public string _21 { get; set; }
        public string _22 { get; set; }
        public string _23 { get; set; }
        public string _24 { get; set; }
        public string _25 { get; set; }
        public string _26 { get; set; }
        public string _27 { get; set; }
        public string _28 { get; set; }
        public string _29 { get; set; }
        public string _30 { get; set; }
        public string _31 { get; set; }
    }
}