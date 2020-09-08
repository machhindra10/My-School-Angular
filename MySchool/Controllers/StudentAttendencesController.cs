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
    public class StudentAttendencesController : BaseController, IDisposable
    {
        private readonly ngSchoolContext _context;

        public StudentAttendencesController(ngSchoolContext context)
        {
            _context = context;
        }

        // GET: api/StudentAttendences
        [HttpGet]
        public IEnumerable<TStudentAttendence> GetTStudentAttendence()
        {
            return _context.TStudentAttendence;
        }

        // GET: api/StudentAttendences/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTStudentAttendence([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tStudentAttendence = await _context.TStudentAttendence.FindAsync(id);

            if (tStudentAttendence == null)
            {
                return NotFound();
            }

            return Ok(tStudentAttendence);
        }

        [HttpGet, Route("getbystudentid/{id}")]
        public async Task<IActionResult> GetByStudentId([FromRoute] int id)
        {
            int batchid = GetBatchId();
            var q = await (from p in _context.TStudentAttendence
                     where p.Studentid == id && p.Batchid == batchid && p.Ispresent == true
                     group p by new { p.Datecreated.Value.Month, p.Ispresent } into g
                     select new AttendenceData()
                     {
                         month = g.Key.Month,
                         datecreated = g.Select(a => a.Datecreated).FirstOrDefault(),
                         total = g.Count().ToString(),
                         //totaldays = 0
                     }).ToListAsync();

            foreach (var item in q)
            {
                item.total = item.total + " / " + DateTime.DaysInMonth(item.datecreated.Value.Year, item.datecreated.Value.Month);
                item.monthname = month_names_short[Convert.ToInt32(item.month)-1];
            }

            if (q == null)
            {
                return NotFound();
            }

            return Ok(q);
        }

        [HttpPut, Route("getbyclassid/{classid}")]
        public async Task<IActionResult> GetAttendanceByStudentId([FromRoute] int classid, [FromBody] DateTime date1)
        {
            DateTime date = date1.ToLocalTime().Date;
            int batchid = GetBatchId();
            var qq = (from p in _context.TStudentAdmission
                      where p.Classid == classid
                        && p.Batchid == batchid
                      select p.Studentid).ToList();

            var q = await (from p in _context.Student
                     orderby p.Datecreated descending
                     where qq.Contains(p.Id)
                     select new Student()
                     {
                         Id = p.Id,
                         Fname = p.Fname,
                         Mname = p.Mname,
                         Lname = p.Lname,
                         TStudentAttendence = (from t in _context.TStudentAttendence
                                               where t.Studentid == p.Id && t.Datecreated.Value.Date == date && t.Classid == classid
                                               select t).ToList()
                     }).ToListAsync();


            if (q == null)
            {
                return NotFound();
            }

            return Ok(q);
        }


        // PUT: api/StudentAttendences/5
        [HttpPut, Route("update/{id}")]
        public async Task<IActionResult> PutTStudentAttendence([FromRoute] int id, [FromBody] TStudentAttendence tStudentAttendence)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tStudentAttendence.Id)
            {
                return BadRequest();
            }
            if (tStudentAttendence.Id == 0)
            {
                tStudentAttendence.Datecreated = GetTimeZoneDate(tStudentAttendence.Datecreated);
                tStudentAttendence.Batchid = GetBatchId();
                _context.TStudentAttendence.Add(tStudentAttendence);
            }
            else
            {
                _context.Entry(tStudentAttendence).State = EntityState.Modified;
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TStudentAttendenceExists(id))
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

        // POST: api/StudentAttendences
        [HttpPost]
        public async Task<IActionResult> PostTStudentAttendence([FromBody] TStudentAttendence tStudentAttendence)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.TStudentAttendence.Add(tStudentAttendence);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTStudentAttendence", new { id = tStudentAttendence.Id }, tStudentAttendence);
        }

        // DELETE: api/StudentAttendences/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTStudentAttendence([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tStudentAttendence = await _context.TStudentAttendence.FindAsync(id);
            if (tStudentAttendence == null)
            {
                return NotFound();
            }

            _context.TStudentAttendence.Remove(tStudentAttendence);
            await _context.SaveChangesAsync();

            return Ok(tStudentAttendence);
        }

        private bool TStudentAttendenceExists(int id)
        {
            return _context.TStudentAttendence.Any(e => e.Id == id);
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        private class AttendenceData
        {
            public int? month { get; set; }
            public DateTime? datecreated { get; set; }
            public string total { get; set; }
            public int totaldays { get; set; }
            public string monthname { get; set; }
        }

        string[] month_names_short = { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
    }
}