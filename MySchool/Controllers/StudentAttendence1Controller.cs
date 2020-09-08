using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySchool.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace MySchool.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentAttendence1Controller : BaseController, IDisposable
    {
        private readonly ngSchoolContext _context;

        public StudentAttendence1Controller(ngSchoolContext context)
        {
            _context = context;
        }

        // GET: api/StudentAttendence1
        [HttpGet]
        public IEnumerable<TStudentAttendence1> GetTStudentAttendence1()
        {
            return _context.TStudentAttendence1;
        }

        // GET: api/StudentAttendence1/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTStudentAttendence1([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tStudentAttendence1 = await _context.TStudentAttendence1.FindAsync(id);

            if (tStudentAttendence1 == null)
            {
                return NotFound();
            }

            return Ok(tStudentAttendence1);
        }

        [HttpGet, Route("get/{month}/{classid}")]
        public async Task<IActionResult> GetTStudentAttendence12([FromRoute] int month, [FromRoute] int classid)
        {

            var tStudentAttendence1 = await _context.TStudentAttendence1.Where(e => e.Month == month && e.Classid == classid).FirstOrDefaultAsync();


            return Ok(tStudentAttendence1);
        }


        [HttpGet, Route("getbystudentid/{id}")]
        public async Task<IActionResult> GetByStudentId([FromRoute] int id)
        {
            int batchid = GetBatchId();
            var q = await (from p in _context.TStudentAttendence1
                           orderby p.Month ascending
                           where p.Studentid == id && p.Batchid == batchid
                           select p).ToListAsync();

            List<string> allowedStatus = new List<string>() { "P", "WO", "NH" };

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
                        else if (Convert.ToString(value) == "A")
                        {
                            totalA++;
                        }
                        else if (Convert.ToString(value) == "L")
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


        [HttpGet, Route("getstudents/{classid}/{month}/{batchid}")]
        public async Task<IActionResult> GetAttendanceByStudentId([FromRoute] int classid, [FromRoute] int month, [FromRoute] int batchid)
        {
            if (batchid == 0)
            {
                batchid = GetBatchId();
            }

            var qq = (from p in _context.TStudentAdmission
                      where p.Classid == classid
                        && p.Batchid == batchid
                      select p.Studentid).ToList();

            var qqq = (from t in _context.TStudentAttendence1
                       where t.Month == month && t.Classid == classid && t.Batchid == batchid
                       select t).ToList();

            //set data for each student firsttime
            if (qq.Count != qqq.Count)
            {
                foreach (var item in qq)
                {
                    SetDataFirst(classid, month, batchid, item.Value);
                }
            }           

            var listAtt = await (from p in _context.TStudentAttendence1
                                 where qq.Contains(p.Studentid) && p.Month == month && p.Classid == classid && p.Batchid == batchid
                                 select new
                                 {
                                     Id = p.Id,
                                     StudentId = p.Studentid,
                                     Classid = p.Classid,
                                     Batchid = p.Batchid,
                                     Year = p.Year,
                                     Month = p.Month,
                                     name = (from s in _context.Student where s.Id == p.Studentid select new { name = s.Fname + " " + s.Mname + " " + s.Lname }).Select(x => x.name).FirstOrDefault(),
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

            return Ok(listAtt);
        }

        [HttpGet, Route("getbyclassid/{classid}/{month}")]
        public async Task<IActionResult> GetAttendanceByStudentId1([FromRoute] int classid, [FromRoute] int month)
        {

            int batchid = GetBatchId();

            var TStudentAttendence1 = await (from t in _context.TStudentAttendence1
                                             where t.Month == month && t.Classid == classid && t.Batchid == batchid
                                             select t).ToListAsync();
            //string sb = string.Empty;

            //foreach (var item in TStudentAttendence1)
            //{
            //    foreach (var col in item.GetType().GetProperties().Where(p => p.Name.Contains("1")))
            //    {
            //        sb = sb + " " + col.GetValue(item, null);
            //    }
            //}

            if (TStudentAttendence1 == null)
            {
                return NotFound();
            }

            return Ok(TStudentAttendence1);
        }

        private void SetDataFirst(int classid, int month, int batchid, int studentid)
        {
            int year = DateTime.UtcNow.Year;

            var q = (from p in _context.TStudentAttendence1
                     where p.Classid == classid && p.Month == month
                                        && p.Batchid == batchid && p.Studentid == studentid
                     select p).FirstOrDefault();

            WeeklyOffsController weeklyOffsController = new WeeklyOffsController(_context);
            List<Adata> weeklyOffs = weeklyOffsController.GetWeeklyOffDaysForMonth1(month, year);
            weeklyOffsController = null;

            HolidaysController holidaysController = new HolidaysController(_context);
            List<Adata> holidays = holidaysController.GetHoliDaysForMonth(month, year);
            holidaysController = null;

            weeklyOffs.AddRange(holidays);

            if (q == null)
            {
                TStudentAttendence1 studentAttendence1 = new TStudentAttendence1();
                studentAttendence1.Studentid = studentid;
                studentAttendence1.Classid = classid;
                studentAttendence1.Month = month;
                studentAttendence1.Batchid = batchid;

                foreach (var col in studentAttendence1.GetType().GetProperties().Where(p => p.Name.StartsWith("_")))
                {
                    int day = Convert.ToInt32(Convert.ToInt32(col.Name.Split('_')[1]));
                    int daysinmonth = DateTime.DaysInMonth(year, month);
                    if (day <= daysinmonth)
                    {
                        col.SetValue(studentAttendence1, (from d in weeklyOffs where d.day == day select d.status).FirstOrDefault());
                    }
                }

                _context.TStudentAttendence1.Add(studentAttendence1);
            }
            _context.SaveChanges();
        }


        // PUT: api/StudentAttendence1/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTStudentAttendence1([FromRoute] int id, [FromBody] TStudentAttendence1 tStudentAttendence1)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tStudentAttendence1.Id)
            {
                return BadRequest();
            }

            _context.Entry(tStudentAttendence1).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TStudentAttendence1Exists(id))
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

        // POST: api/StudentAttendence1
        [HttpPost]
        public async Task<IActionResult> PostTStudentAttendence1([FromBody] TStudentAttendence1 tStudentAttendence1)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.TStudentAttendence1.Add(tStudentAttendence1);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTStudentAttendence1", new { id = tStudentAttendence1.Id }, tStudentAttendence1);
        }

        [HttpPost, Route("updateattendence")]
        public async Task<IActionResult> PostTStudentAttendence1All([FromBody] List<TStudentAttendence1> student)
        {

            foreach (TStudentAttendence1 s in student)
            {
                //TStudentAttendence1 studentAttendence1 = s.TStudentAttendence1.FirstOrDefault();
                _context.Entry(s).State = EntityState.Modified;
            }

            await _context.SaveChangesAsync();

            return Ok();
        }


        // DELETE: api/StudentAttendence1/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTStudentAttendence1([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tStudentAttendence1 = await _context.TStudentAttendence1.FindAsync(id);
            if (tStudentAttendence1 == null)
            {
                return NotFound();
            }

            _context.TStudentAttendence1.Remove(tStudentAttendence1);
            await _context.SaveChangesAsync();

            return Ok(tStudentAttendence1);
        }

        [HttpGet, Route("getmonths")]
        public IActionResult GetMonths()
        {
            List<yr> listmonths = new List<yr>();
            yr yr = null;
            int month = 1;
            do
            {
                yr = new yr { id = month, dyr = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month) };
                listmonths.Add(yr);
            }
            while (month++ < 12);//DateTime.Now.Month);

            return Ok(listmonths);
        }

        private class yr
        {
            public int id { get; set; }
            public string dyr { get; set; }
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

        private bool TStudentAttendence1Exists(int id)
        {
            return _context.TStudentAttendence1.Any(e => e.Id == id);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}