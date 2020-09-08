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
    public class WeeklyOffsController : BaseController, IDisposable
    {
        private readonly ngSchoolContext _context;

        public WeeklyOffsController(ngSchoolContext context)
        {
            _context = context;
        }

        // GET: api/WeeklyOffs
        [HttpGet]
        public IEnumerable<MWeeklyOff> GetMWeeklyOff()
        {
            return _context.MWeeklyOff;
        }

        // GET: api/WeeklyOffs/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMWeeklyOff([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var mWeeklyOff = await _context.MWeeklyOff.FindAsync(id);

            if (mWeeklyOff == null)
            {
                return NotFound();
            }

            return Ok(mWeeklyOff);
        }

        // PUT: api/WeeklyOffs/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMWeeklyOff([FromRoute] int id, [FromBody] MWeeklyOff mWeeklyOff)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != mWeeklyOff.Id)
            {
                return BadRequest();
            }

            _context.Entry(mWeeklyOff).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MWeeklyOffExists(id))
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

        // POST: api/WeeklyOffs
        [HttpPost]
        public async Task<IActionResult> PostMWeeklyOff([FromBody] MWeeklyOff mWeeklyOff)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var q = _context.MWeeklyOff.Where(x => x.Weekday == mWeeklyOff.Weekday).FirstOrDefault();
            if (q != null)
            {
                q.PosInMonth = mWeeklyOff.PosInMonth;
                _context.Entry(q).State = EntityState.Modified;
            }
            else
            {
                _context.MWeeklyOff.Add(mWeeklyOff);
            }

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMWeeklyOff", new { id = mWeeklyOff.Id }, mWeeklyOff);
        }

        // DELETE: api/WeeklyOffs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMWeeklyOff([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var mWeeklyOff = await _context.MWeeklyOff.FindAsync(id);
            if (mWeeklyOff == null)
            {
                return NotFound();
            }

            _context.MWeeklyOff.Remove(mWeeklyOff);
            await _context.SaveChangesAsync();

            return Ok(mWeeklyOff);
        }

        private bool MWeeklyOffExists(int id)
        {
            return _context.MWeeklyOff.Any(e => e.Id == id);
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public List<DateTime> GetWeeklyOffDaysForMonth(int month, int year)
        {
            List<DateTime> weekdaysDates = new List<DateTime>();

            List<DateTime> MyCalendar = new List<DateTime>();           //create list
            DateTime currDate = new DateTime(year, month, 1);           //initial value                                                                        

            while (currDate <= new DateTime(year, month, DateTime.DaysInMonth(year, month))) //add days to MyCalendar
            {
                MyCalendar.Add(currDate);
                currDate = currDate.AddDays(1);
            }

            var weeklyoffs = _context.MWeeklyOff.ToList();

            foreach (var weeklyoff in weeklyoffs)
            {
                List<int?> listPositions = new List<int?>(Array.ConvertAll(weeklyoff.PosInMonth.Split(','),
                            new Converter<string, int?>((s) => { return Convert.ToInt32(s); })));

                var result = MyCalendar.Where(x => x.DayOfWeek.ToString() == weeklyoff.Weekday)
                            .GroupBy(x => x.Month)
                            .SelectMany(grp =>
                                grp.Select((d, counter) => new
                                {
                                    Month = grp.Key,
                                    PosInMonth = counter + 1,
                                    Day = d
                                }))
                                .Where(x => listPositions.Contains(x.PosInMonth))
                            .ToList();

                foreach (var item in result)
                {
                    weekdaysDates.Add(item.Day);
                }
            }

            return weekdaysDates;
        }

        public List<Adata> GetWeeklyOffDaysForMonth1(int month, int year)
        {
            List<Adata> weekdaysDates = new List<Adata>();

            List<DateTime> MyCalendar = new List<DateTime>();           //create list
            DateTime currDate = new DateTime(year, month, 1);           //initial value                                                                        

            while (currDate <= new DateTime(year, month, DateTime.DaysInMonth(year, month))) //add days to MyCalendar
            {
                MyCalendar.Add(currDate);
                currDate = currDate.AddDays(1);
            }

            var weeklyoffs = _context.MWeeklyOff.ToList();

            foreach (var weeklyoff in weeklyoffs)
            {
                List<int?> listPositions = new List<int?>(Array.ConvertAll(weeklyoff.PosInMonth.Split(','),
                            new Converter<string, int?>((s) => { return Convert.ToInt32(s); })));

                var result = MyCalendar.Where(x => x.DayOfWeek.ToString() == weeklyoff.Weekday)
                            .GroupBy(x => x.Month)
                            .SelectMany(grp =>
                                grp.Select((d, counter) => new
                                {
                                    Month = grp.Key,
                                    PosInMonth = counter + 1,
                                    Day = d,
                                }))
                                .Where(x => listPositions.Contains(x.PosInMonth)).
                                Select((v) => new Adata { day = v.Day.Day, status = "WO" })
                            .ToList();

                weekdaysDates.AddRange(result);

            }

            return weekdaysDates;
        }

    }
}