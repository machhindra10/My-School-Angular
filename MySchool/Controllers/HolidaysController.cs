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
    public class HolidaysController : BaseController, IDisposable
    {
        private readonly ngSchoolContext _context;

        public HolidaysController(ngSchoolContext context)
        {
            _context = context;
        }

        // GET: api/MHolidays
        [HttpGet]
        public IEnumerable<MHoliday> GetMHoliday()
        {
            return _context.MHoliday;
        }

        // GET: api/MHolidays/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMHoliday([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var mHoliday = await _context.MHoliday.FindAsync(id);

            if (mHoliday == null)
            {
                return NotFound();
            }

            return Ok(mHoliday);
        }

        // GET: api/MHolidays/5
        [HttpGet, Route("getbyyear/{year}")]
        public async Task<IActionResult> GetMHolidayByYear([FromRoute] int year)
        {

            var mHoliday = await (from p in _context.MHoliday orderby p.Dates where p.Dates.Value.Year == year select p).ToListAsync();

            if (mHoliday == null)
            {
                return NotFound();
            }

            return Ok(mHoliday);
        }

        // PUT: api/MHolidays/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMHoliday([FromRoute] int id, [FromBody] MHoliday mHoliday)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != mHoliday.Id)
            {
                return BadRequest();
            }

            _context.Entry(mHoliday).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MHolidayExists(id))
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

        // POST: api/MHolidays
        [HttpPost]
        public async Task<IActionResult> PostMHoliday([FromBody] MHoliday mHoliday)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (mHoliday.Dates.Value.Year != DateTime.UtcNow.Year)
            {
                return Ok();
            }

            mHoliday.Dates = GetTimeZoneDate(mHoliday.Dates);
            _context.MHoliday.Add(mHoliday);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMHoliday", new { id = mHoliday.Id }, mHoliday);
        }

        // DELETE: api/MHolidays/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMHoliday([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var mHoliday = await _context.MHoliday.FindAsync(id);
            if (mHoliday == null)
            {
                return NotFound();
            }

            _context.MHoliday.Remove(mHoliday);
            await _context.SaveChangesAsync();

            return Ok(mHoliday);
        }

        private bool MHolidayExists(int id)
        {
            return _context.MHoliday.Any(e => e.Id == id);
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public List<Adata> GetHoliDaysForMonth(int month, int year)
        {
            List<Adata> holidays = new List<Adata>();

            holidays = (from p in _context.MHoliday
                        where p.Dates.Value.Year == year && p.Dates.Value.Month == month
                        select new Adata
                        {
                            day = p.Dates.Value.Day,
                            status = "NH"
                        }).ToList();

            return holidays;
        }

        // GET: api/MHolidays/5
        [HttpGet, Route("getcountbyear/{year}")]
        public async Task<IActionResult> GetMHolidayAndWeeklyOffCount([FromRoute] int year)
        {
            bool weeklyoffs = true; bool holidays = true;

            var mHoliday = await (from p in _context.MHoliday where p.Dates.Value.Year == year select p).CountAsync();
            var mWeeklyOffs = await (from p in _context.MWeeklyOff select p).CountAsync();

            if (mHoliday == 0)
            {
                holidays = false;
            }
            if (mWeeklyOffs == 0)
            {
                weeklyoffs = false;
            }

            return Ok(new { weeklyoffsExists = weeklyoffs, holidaysExists = holidays });
        }
    }
}