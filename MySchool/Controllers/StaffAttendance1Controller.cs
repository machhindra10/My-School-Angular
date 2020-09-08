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
    public class StaffAttendance1Controller : BaseController, IDisposable
    {
        private readonly ngSchoolContext _context;

        public StaffAttendance1Controller(ngSchoolContext context)
        {
            _context = context;
        }

        // GET: api/StaffAttendance1
        [HttpGet]
        public IEnumerable<StaffAttendance1> GetStaffAttendance1()
        {
            return _context.StaffAttendance1;
        }

        // GET: api/StaffAttendance1/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetStaffAttendance1([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var staffAttendance1 = await _context.StaffAttendance1.FindAsync(id);

            if (staffAttendance1 == null)
            {
                return NotFound();
            }

            return Ok(staffAttendance1);
        }

        // PUT: api/StaffAttendance1/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStaffAttendance1([FromRoute] long id, [FromBody] StaffAttendance1 staffAttendance1)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != staffAttendance1.Id)
            {
                return BadRequest();
            }

            _context.Entry(staffAttendance1).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StaffAttendance1Exists(id))
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

        // POST: api/StaffAttendance1
        [HttpPost]
        public async Task<IActionResult> PostStaffAttendance1([FromBody] StaffAttendance1 staffAttendance1)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.StaffAttendance1.Add(staffAttendance1);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStaffAttendance1", new { id = staffAttendance1.Id }, staffAttendance1);
        }

        // DELETE: api/StaffAttendance1/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStaffAttendance1([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var staffAttendance1 = await _context.StaffAttendance1.FindAsync(id);
            if (staffAttendance1 == null)
            {
                return NotFound();
            }

            _context.StaffAttendance1.Remove(staffAttendance1);
            await _context.SaveChangesAsync();

            return Ok(staffAttendance1);
        }

        private bool StaffAttendance1Exists(long id)
        {
            return _context.StaffAttendance1.Any(e => e.Id == id);
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        // GET: api/StaffAttendances/5
        [HttpGet, Route("generate/{month}/{year}")]
        public async Task<IActionResult> GenerateAttendanceOfAllStaff([FromRoute] int month, int year)
        {
            DateTime monthEndDate = new DateTime(year, month, DateTime.DaysInMonth(year, month));
            DateTime monthStartDate = new DateTime(year, month, 1);

            var mStaff = await (from p in _context.MStaff
                                where p.Disabled == false && p.Doj.Value.Date <= monthEndDate.Date &&
                                  p.Dol.HasValue ? p.Dol.Value.Date >= monthStartDate.Date :
                                  p.Doj.Value.Date <= monthEndDate.Date
                                select p).ToListAsync();

            if (new DateTime(year, month, 1).Date <= GetTimeZoneDate(DateTime.UtcNow.Date).Date)
            {
                InsertDataFirst(mStaff, month, year);
            }

            var staffIDsA = mStaff.Select(p => p.Id).ToList();

            var staffAttendance = await (from p in _context.StaffAttendance1
                                         where staffIDsA.Contains(p.Staffid) && p.Year == year &&
                                           p.Month == month
                                         select new StaffAttendance1
                                         {
                                             Id = p.Id,
                                             Month = p.Month,
                                             Staff = (from s in _context.MStaff where s.Id == p.Staffid select new MStaff { Staffname = s.Staffname }).FirstOrDefault(),
                                             Staffid = p.Staffid,
                                             Year = p.Year,
                                             _1 = p._1,
                                             _10 = p._10,
                                             _11 = p._11,
                                             _12 = p._12,
                                             _13 = p._13,
                                             _14 = p._14,
                                             _15 = p._15,
                                             _16 = p._16,
                                             _17 = p._17,
                                             _18 = p._18,
                                             _19 = p._19,
                                             _2 = p._2,
                                             _20 = p._20,
                                             _21 = p._21,
                                             _22 = p._22,
                                             _23 = p._23,
                                             _24 = p._24,
                                             _25 = p._25,
                                             _26 = p._26,
                                             _27 = p._27,
                                             _28 = p._28,
                                             _29 = p._29,
                                             _3 = p._3,
                                             _30 = p._30,
                                             _31 = p._31,
                                             _4 = p._4,
                                             _5 = p._5,
                                             _6 = p._6,
                                             _7 = p._7,
                                             _8 = p._8,
                                             _9 = p._9
                                         }).ToListAsync();


            return Ok(staffAttendance);
        }


        // GET: api/StaffAttendances/5
        [HttpGet, Route("getbymonth/{month}/{year}")]
        public async Task<IActionResult> GetStaffAttendanceOfAllStaff([FromRoute] int month, int year)
        {
            DateTime monthEndDate = new DateTime(year, month, DateTime.DaysInMonth(year, month));
            DateTime monthStartDate = new DateTime(year, month, 1);

            var staffidsA = await (from p in _context.MStaff
                                   where p.Disabled == false && p.Doj.Value.Date <= monthEndDate.Date &&
                                     p.Dol.HasValue ? p.Dol.Value.Date >= monthStartDate.Date :
                                     p.Doj.Value.Date <= monthEndDate.Date
                                   select p.Id).ToArrayAsync();

            var staffAttendance = await (from p in _context.StaffAttendance1
                                         where staffidsA.Contains(p.Staffid) && p.Year == year &&
                                           p.Month == month
                                         select new StaffAttendance1
                                         {
                                             Id = p.Id,
                                             Month = p.Month,
                                             Staff = (from s in _context.MStaff where s.Id == p.Staffid select new MStaff { Staffname = s.Staffname }).FirstOrDefault(),
                                             Staffid = p.Staffid,
                                             Year = p.Year,
                                             _1 = p._1,
                                             _10 = p._10,
                                             _11 = p._11,
                                             _12 = p._12,
                                             _13 = p._13,
                                             _14 = p._14,
                                             _15 = p._15,
                                             _16 = p._16,
                                             _17 = p._17,
                                             _18 = p._18,
                                             _19 = p._19,
                                             _2 = p._2,
                                             _20 = p._20,
                                             _21 = p._21,
                                             _22 = p._22,
                                             _23 = p._23,
                                             _24 = p._24,
                                             _25 = p._25,
                                             _26 = p._26,
                                             _27 = p._27,
                                             _28 = p._28,
                                             _29 = p._29,
                                             _3 = p._3,
                                             _30 = p._30,
                                             _31 = p._31,
                                             _4 = p._4,
                                             _5 = p._5,
                                             _6 = p._6,
                                             _7 = p._7,
                                             _8 = p._8,
                                             _9 = p._9
                                         }).ToListAsync();


            return Ok(staffAttendance);
        }

        private void InsertDataFirst(List<MStaff> mStaff, int month, int year)
        {
            var q = (from p in _context.StaffAttendance1 where p.Month == month && p.Year == year select p.Id).ToList();

            if (q.Count == mStaff.Count)
            {
                return;
            }

            WeeklyOffsController weeklyOffsController = new WeeklyOffsController(_context);
            List<Adata> weeklyOffs = weeklyOffsController.GetWeeklyOffDaysForMonth1(month, year);
            weeklyOffsController = null;

            HolidaysController holidaysController = new HolidaysController(_context);
            List<Adata> holidays = holidaysController.GetHoliDaysForMonth(month, year);
            holidaysController = null;

            weeklyOffs.AddRange(holidays);

            foreach (var staff in mStaff)
            {
                var tempAttendance = (from t in _context.StaffAttendance1 where t.Staffid == staff.Id && t.Month == month && t.Year == year select t).FirstOrDefault();
                if (tempAttendance == null)
                {
                    StaffAttendance1 staffAttendance1 = new StaffAttendance1();
                    staffAttendance1.Staffid = staff.Id;
                    staffAttendance1.Month = month;
                    staffAttendance1.Year = year;
                    foreach (var col in staffAttendance1.GetType().GetProperties().Where(p => p.Name.StartsWith("_")))
                    {
                        int day = Convert.ToInt32(Convert.ToInt32(col.Name.Split('_')[1]));
                        int daysinmonth = DateTime.DaysInMonth(year, month);
                        if (day <= daysinmonth)
                        {

                            DateTime date = new DateTime(year, month, day);
                            if (staff.Doj.Value.Date > date.Date || (staff.Dol.HasValue ? staff.Dol.Value.Date < date.Date : false))
                            {
                                col.SetValue(staffAttendance1, "-");
                            }
                            else
                            {
                                col.SetValue(staffAttendance1, (from d in weeklyOffs where d.day == day select d.status).FirstOrDefault());
                            }
                        }
                    }

                    _context.StaffAttendance1.Add(staffAttendance1);
                    staffAttendance1 = null;
                }
            }
            _context.SaveChanges();
        }

        [HttpPut, Route("updateattendance/{month}/{year}")]
        public async Task<IActionResult> GetStaffAttendanceUpdate(int month, int year, [FromBody] List<StaffAttendance1> listsAttendance)
        {
            bool result = false;

            foreach (var attendace in listsAttendance)
            {
                var isSalaryGeneratedForCurrentMonthAndYearOfThisStaff = (from p in _context.StaffSalary where p.Year == year && p.Month == month && p.Staffid == attendace.Staffid select p.Staffid).Count();
                if (isSalaryGeneratedForCurrentMonthAndYearOfThisStaff == 0)
                {
                    _context.Entry(attendace).State = EntityState.Modified;
                }
            }

            int i = await _context.SaveChangesAsync();
            if (i > 0) { result = true; }
            return Ok(new { result = result });
        }

        public decimal GetTotalPresentyByStaffId(int? staffid, int? month, int? year)
        {
            List<string> allowedStatus = new List<string>() { "P", "WO", "NH" };

            var leaveTypes = _context.MLeaveType.Select(p => p.Code).ToList();
            allowedStatus.AddRange(leaveTypes);

            int daysPresent = 0; int halfDays = 0;

            var AttendanceList = (from p in _context.StaffAttendance1
                                  where p.Staffid == staffid && p.Year == year
                                        && p.Month == month
                                  select p).FirstOrDefault();

            foreach (var col in AttendanceList.GetType().GetProperties().Where(p => p.Name.StartsWith("_")))
            {
                int day = Convert.ToInt32(Convert.ToInt32(col.Name.Split('_')[1]));
                string value = Convert.ToString(col.GetValue(AttendanceList));
                if (allowedStatus.Contains(value))
                {
                    daysPresent++;
                }
                else if (value == "H")
                {
                    halfDays++;
                }
            }

            decimal halfDaysActual = (Convert.ToDecimal(halfDays) / 2);
            return (daysPresent + halfDaysActual);
        }



        public async Task<IActionResult> UpdateLeavesOfStaff(int id)
        {
            var leaveapplication = await (from l in _context.LeaveApplication where l.Id == id select l).FirstOrDefaultAsync();
            var leavetype = await _context.MLeaveType.FindAsync(leaveapplication.Leavetypeid);

            if (leaveapplication == null)
            {
                return Ok(new { result = false });
            }

            List<DateTime> dateTimes = new List<DateTime>();
            DateTime date = leaveapplication.Datefrom;
            while (date <= leaveapplication.Dateto)
            {
                dateTimes.Add(date);
                date = date.AddDays(1);
            }
            try
            {
                int year = dateTimes.Select(x => x.Year).FirstOrDefault();
                var months = (from d in dateTimes select d.Month).Distinct().ToList();

                //insert data first if month not exists
                foreach (int tempMonth in months)
                {
                    var attendance = await (from p in _context.StaffAttendance1
                                            where p.Staffid == leaveapplication.Staffid
                                                   && p.Year == year
                                                   && p.Month == tempMonth
                                            select p).FirstOrDefaultAsync();
                    if (attendance == null)
                    {
                        DateTime monthEndDate = new DateTime(year, tempMonth, DateTime.DaysInMonth(year, tempMonth));
                        DateTime monthStartDate = new DateTime(year, tempMonth, 1);

                        var mstaff = await (from p in _context.MStaff
                                            where p.Disabled == false &&
                                              p.Doj.Value.Date <= monthEndDate.Date &&
                                              p.Dol.HasValue ? p.Dol.Value.Date >= monthStartDate.Date :
                                              p.Doj.Value.Date <= monthEndDate.Date
                                            select p).Where(x=>x.Id == leaveapplication.Staffid).ToListAsync();

                        InsertDataFirst(mstaff, tempMonth, year);
                    }
                }

                var attendances = await (from p in _context.StaffAttendance1
                                         where p.Staffid == leaveapplication.Staffid
                                                && p.Year == year
                                                && months.Contains(p.Month)
                                         select p).ToListAsync();

                foreach (var attendance in attendances)
                {
                    foreach (var col in attendance.GetType().GetProperties().Where(p => p.Name.StartsWith("_")))
                    {
                        string value = Convert.ToString(col.GetValue(attendance, null));
                        if(value != "-")
                        {
                            int day = Convert.ToInt32(Convert.ToInt32(col.Name.Split('_')[1]));
                            int daysinmonth = DateTime.DaysInMonth(attendance.Year, attendance.Month);
                            if (day <= daysinmonth)
                            {
                                DateTime tempDate = new DateTime(attendance.Year, attendance.Month, day);

                                var actualDate = (from d in dateTimes where d.Date == tempDate.Date select d).FirstOrDefault();
                                if (actualDate != new DateTime(0001, 1, 1))
                                {
                                    col.SetValue(attendance, leavetype.Code);
                                }
                            }
                        }
                        
                    }

                    _context.Entry(attendance).State = EntityState.Modified;

                }
            }
            catch (Exception)
            {


            }



            bool result = false;
            int i = await _context.SaveChangesAsync();
            if (i > 0) { result = true; }
            return Ok(new { result = result });
        }

        [HttpGet, Route("getbystaffid/{id}")]
        public async Task<IActionResult> GetByStaffId([FromRoute] int id)
        {
            int year = DateTime.UtcNow.Year;
            int month = DateTime.UtcNow.Month;
            var q = await (from p in _context.StaffAttendance1
                           orderby p.Month ascending
                           where p.Staffid == id && p.Year == year && p.Month <= month
                           select p).ToListAsync();

            List<string> allowedStatus = new List<string>() { "P", "WO", "NH" };
            var leaveTypes = _context.MLeaveType.Select(p => p.Code).ToList();

            AttendenceData attendenceData = new AttendenceData();
            List<chartdata> lchartdataP = new List<chartdata>();
            List<chartdata> lchartdataA = new List<chartdata>();
            List<chartdata> lchartdataL = new List<chartdata>();

            foreach (var item in q)
            {
                chartdata chartdataP = null; chartdata chartdataA = null; chartdata chartdataL = null;
                int totalP = 0; int totalA = 0; int totalL = 0;
                foreach (var col in item.GetType().GetProperties().Where(p => p.Name.StartsWith("_")))
                {
                    var value = col.GetValue(item, null);
                    if (value != null)
                    {
                        if (allowedStatus.Contains(Convert.ToString(value)))
                        {
                            totalP++;
                        }
                        else if (Convert.ToString(value) == "A" || Convert.ToString(value) == "-")
                        {
                            totalA++;
                        }
                        //else if (Convert.ToString(value) == "H")
                        //{
                        //    totalL++;
                        //}
                        else if(leaveTypes.Contains(Convert.ToString(value)))
                        {
                            totalL++;
                        }
                    }
                    else
                    {
                        totalA++;
                    }
                }

                chartdataP = new chartdata(); chartdataA = new chartdata(); chartdataL = new chartdata();

                chartdataP.name = month_names_short[Convert.ToInt32(item.Month) - 1];
                chartdataP.y = totalP;
                lchartdataP.Add(chartdataP);

                chartdataA.name = month_names_short[Convert.ToInt32(item.Month) - 1];
                chartdataA.y = totalA;
                lchartdataA.Add(chartdataA);

                chartdataL.name = month_names_short[Convert.ToInt32(item.Month) - 1];
                chartdataL.y = totalL;
                lchartdataL.Add(chartdataL);
            }

            attendenceData.present = lchartdataP;
            attendenceData.absent = lchartdataA;
            attendenceData.leaves = lchartdataL;

            if (q == null)
            {
                return NotFound();
            }

            return Ok(attendenceData);
        }

        private class AttendenceData
        {
            public List<chartdata> present { get; set; }
            public List<chartdata> absent { get; set; }
            public List<chartdata> leaves { get; set; }
        }

        private class chartdata
        {
            public string name { get; set; }
            public int y { get; set; }
        }

        string[] month_names_short = { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };

    }

    public class Adata
    {
        public int? staffid { get; set; }
        public int day { get; set; }
        public string status { get; set; }
    }    

}