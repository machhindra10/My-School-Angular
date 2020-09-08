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
    public class LeavesController : BaseController, IDisposable
    {
        private readonly ngSchoolContext _context;

        public LeavesController(ngSchoolContext context)
        {
            _context = context;
        }

        // GET: api/Leaves
        [HttpGet]
        public IEnumerable<TLeaves> GetTLeaves()
        {
            return _context.TLeaves;
        }

        // GET: api/Leaves/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTLeaves([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tLeaves = await _context.TLeaves.FindAsync(id);

            if (tLeaves == null)
            {
                return NotFound();
            }

            return Ok(tLeaves);
        }

        // GET: api/Leaves/5
        [HttpGet, Route("getbyyear/{year}")]
        public async Task<IActionResult> GetTLeavesByYear([FromRoute] long year)
        {
            var tLeaves = await (from p in _context.TLeaves
                                 where p.Year == year
                                 select new TLeaves
                                 {
                                     Id = p.Id,
                                     Year = p.Year,
                                     Leaves = p.Leaves,
                                     Leavetype = p.Leavetype,
                                     Leavetypeid = p.Leavetypeid
                                 }).ToListAsync();

            if (tLeaves == null)
            {
                return NotFound();
            }

            return Ok(tLeaves);
        }

        // PUT: api/Leaves/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTLeaves([FromRoute] long id, [FromBody] TLeaves tLeaves)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tLeaves.Id)
            {
                return BadRequest();
            }

            _context.Entry(tLeaves).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TLeavesExists(id))
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

        // POST: api/Leaves
        [HttpPost]
        public async Task<IActionResult> PostTLeaves([FromBody] TLeaves tLeaves)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (DateTime.UtcNow.Year != tLeaves.Year)
            {
                return Ok();
            }

            var leaves = _context.TLeaves.Where(p => p.Year == tLeaves.Year && p.Leavetypeid == tLeaves.Leavetypeid).FirstOrDefault();
            if (leaves != null)
            {
                leaves.Leaves = tLeaves.Leaves;
                _context.Entry(leaves).State = EntityState.Modified;
            }
            else
            {
                _context.TLeaves.Add(tLeaves);
            }
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTLeaves", new { id = tLeaves.Id }, tLeaves);
        }

        // DELETE: api/Leaves/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTLeaves([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tLeaves = await _context.TLeaves.FindAsync(id);
            if (tLeaves == null)
            {
                return NotFound();
            }

            _context.TLeaves.Remove(tLeaves);
            await _context.SaveChangesAsync();

            return Ok(tLeaves);
        }

        private bool TLeavesExists(long id)
        {
            return _context.TLeaves.Any(e => e.Id == id);
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        [HttpGet, Route("getbyyear/{staffid}/{year}")]
        public async Task<IActionResult> GetRemainingLeavesOfStaff([FromRoute]int staffid, int year)
        {
            var leavesremaining = await (from p in _context.MLeaveType
                                         select new leavesCount
                                         {
                                             leavetypeid = p.Id,
                                             code = p.Code,
                                             leavetype = p.Leavetype,
                                             iscarryforward = p.Iscarryforward,
                                             
                                             count = 0
                                         }).ToListAsync();

            

            foreach (var item in leavesremaining)
            {
                decimal leavestaken = 0;   string value = string.Empty;
                try
                {
                    if (!Convert.ToBoolean(item.iscarryforward))
                    {
                        var attendances = await (from a in _context.StaffAttendance1
                                                 where a.Staffid == staffid && a.Year == year
                                                 select a).ToListAsync();
                        foreach (var attendance in attendances)
                        {
                            foreach (var col in attendance.GetType().GetProperties().Where(p => p.Name.StartsWith("_")))
                            {
                                value = Convert.ToString(col.GetValue(attendance, null));
                                if (value == item.code)
                                {
                                    leavestaken++;
                                }
                            }
                        }

                        var totalleaves = (from l in _context.TLeaves where l.Year == year && l.Leavetypeid == item.leavetypeid select l.Leaves).FirstOrDefault();
                        item.count = (Convert.ToDecimal(totalleaves) - leavestaken);
                        item.total = Convert.ToDecimal(totalleaves);
                    }
                    else
                    {
                        var attendances = await (from a in _context.StaffAttendance1
                                                 where a.Staffid == staffid && a.Year <= year
                                                 select a).ToListAsync();
                        foreach (var attendance in attendances)
                        {
                            foreach (var col in attendance.GetType().GetProperties().Where(p => p.Name.StartsWith("_")))
                            {
                                value = Convert.ToString(col.GetValue(attendance, null));
                                if (value == item.code)
                                {
                                    leavestaken++;
                                }
                            }
                        }

                        var totalleaves = (from l in _context.TLeaves where l.Year <= year && l.Leavetypeid == item.leavetypeid select l.Leaves).Sum();
                        item.count = (Convert.ToDecimal(totalleaves) - leavestaken);
                        item.total = Convert.ToDecimal(totalleaves);
                    }
                    if (item.count < 0)
                    {
                        item.count = 0;
                    }
                }
                catch (Exception )
                {

                    //throw;
                }
            }

            return Ok(leavesremaining);
        }
    }

    public class leavesCount
    {
        public long leavetypeid { get; set; }
        public string code { get; set; }
        public string leavetype { get; set; }
        public bool? iscarryforward { get; set; }
        public decimal count { get; set; }
        public decimal total { get; set; }
    }
}