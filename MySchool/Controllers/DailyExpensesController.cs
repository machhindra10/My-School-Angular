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
    public class DailyExpensesController : BaseController, IDisposable
    {
        private readonly ngSchoolContext _context;

        public DailyExpensesController(ngSchoolContext context)
        {
            _context = context;
        }

        // GET: api/DailyExpenses
        [HttpGet]
        public IEnumerable<DailyExpenses> GetDailyExpenses()
        {
            return _context.DailyExpenses.OrderByDescending(e => e.Datecreated);
        }

        // GET: api/DailyExpenses
        [HttpPost, Route("getreport")]
        public IEnumerable<DailyExpenses> GetDailyExpensesReport([FromBody] Params @params)
        {
            var q = (from p in _context.DailyExpenses
                     where p.Datecreated.Value.Date >= @params.fromdate.Date && 
                     p.Datecreated.Value.Date <= @params.todate.Date
                     select p).ToList().OrderBy(e => e.Datecreated);

            return q;
        }

        // GET: api/DailyExpenses/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDailyExpenses([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var dailyExpenses = await _context.DailyExpenses.FindAsync(id);

            if (dailyExpenses == null)
            {
                return NotFound();
            }

            return Ok(dailyExpenses);
        }

        // PUT: api/DailyExpenses/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDailyExpenses([FromRoute] int id, [FromBody] DailyExpenses dailyExpenses)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != dailyExpenses.Id)
            {
                return BadRequest();
            }

            _context.Entry(dailyExpenses).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DailyExpensesExists(id))
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

        // POST: api/DailyExpenses
        [HttpPost]
        public async Task<IActionResult> PostDailyExpenses([FromBody] DailyExpenses dailyExpenses)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            dailyExpenses.Userid = GetUserId();
            dailyExpenses.Datecreated = GetTimeZoneDate(dailyExpenses.Datecreated);

            _context.DailyExpenses.Add(dailyExpenses);
            await _context.SaveChangesAsync();

            return Ok(dailyExpenses);
        }

        // DELETE: api/DailyExpenses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDailyExpenses([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var dailyExpenses = await _context.DailyExpenses.FindAsync(id);
            if (dailyExpenses == null)
            {
                return NotFound();
            }

            _context.DailyExpenses.Remove(dailyExpenses);
            await _context.SaveChangesAsync();

            return Ok(dailyExpenses);
        }

        private bool DailyExpensesExists(int id)
        {
            return _context.DailyExpenses.Any(e => e.Id == id);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }

    public class Params
    {
        public DateTime fromdate { get; set; }
        public DateTime todate { get; set; }
    }
}