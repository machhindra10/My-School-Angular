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
    public class BatchesController : BaseController, IDisposable
    {
        private readonly ngSchoolContext _context;

        public BatchesController(ngSchoolContext context)
        {
            _context = context;
        }

        // GET: api/Batches
        [HttpGet]
        public IEnumerable<Batches> GetBatches()
        {
            return _context.Batches.OrderByDescending(e => e.Id);
        }

        // GET: api/Batches/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBatches([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var batches = await _context.Batches.FindAsync(id);

            if (batches == null)
            {
                return NotFound();
            }

            return Ok(batches);
        }

        // GET: api/Batches/5
        [HttpGet, Route("getlastbatchdate")]
        public async Task<IActionResult> GetLastBatchDate()
        {
            var batches = await _context.Batches.Where(e => e.Isactive == true).Select(e => e.Startdate).FirstOrDefaultAsync();

            if (batches == null)
            {
                batches = GetTimeZoneDate(DateTime.UtcNow);
            }
            else
            {
                batches = batches.Value.AddDays(1);
            }

            return Ok(batches);
        }

        // PUT: api/Batches/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBatches([FromRoute] int id, [FromBody] Batches batches)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != batches.Id)
            {
                return BadRequest();
            }
            if (BatchesNameExists(batches.Id, batches.Batch))
            {
                return Ok(new { exists = true });
            }

            if (id == 0)
            {
                batches.Startdate = GetTimeZoneDate(batches.Startdate);
                batches.Datecreated = GetTimeZoneDate(DateTime.UtcNow);
                batches.Isactive = true;
                batches.Userid = GetUserId();
                _context.Batches.Add(batches);

                var q = _context.Batches.Where(e => e.Id != batches.Id && e.Isactive == true).FirstOrDefault();
                if (q != null)
                {
                    q.Isactive = false;
                    q.Enddate = GetTimeZoneDate(batches.Startdate).AddDays(-1);
                    _context.Entry(q).State = EntityState.Modified;
                }

            }
            else
            {
                _context.Entry(batches).State = EntityState.Modified;
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BatchesExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(batches);
        }

        // PUT: api/Batches/5
        [HttpPut, Route("setactive/{id}")]
        public async Task<IActionResult> PutSetActiveBatches([FromRoute] int id, [FromBody] Batches batches)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != batches.Id)
            {
                return BadRequest();
            }

            _context.Entry(batches).State = EntityState.Modified;
            var q = _context.Batches.Where(e => e.Id != batches.Id).ToList();
            foreach (var item in q)
            {
                item.Isactive = false;
                _context.Entry(item).State = EntityState.Modified;
            }
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BatchesExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(batches);
        }

        // POST: api/Batches
        [HttpPost]
        public async Task<IActionResult> PostBatches([FromBody] Batches batches)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            batches.Datecreated = DateTime.UtcNow;

            _context.Batches.Add(batches);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBatches", new { id = batches.Id }, batches);
        }

        // POST: api/Batches
        [HttpPost, Route("addfirstbatch")]
        public async Task<IActionResult> PostAddFirstBatches([FromBody] Batches batches)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            batches.Datecreated = GetTimeZoneDate(DateTime.UtcNow);
            batches.Startdate = GetTimeZoneDate(batches.Startdate);
            _context.Batches.Add(batches);
            await _context.SaveChangesAsync();

            return Ok(new { result = true });
        }
        
        // DELETE: api/Batches/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBatches([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var batches1 = await _context.Batches.ToListAsync();
            if (batches1.Count == 1)
            {
                return Ok(new { status = "notdelete" });
            }
            var batches = await _context.Batches.FindAsync(id);
            if (batches.Isactive == true)
            {
                return Ok(new { status = "activenotdelete" });
            }
            if (batches == null)
            {
                return NotFound();
            }


            _context.Batches.Remove(batches);
            await _context.SaveChangesAsync();

            return Ok(batches);
        }

        private bool BatchesExists(int id)
        {
            return _context.Batches.Any(e => e.Id == id);
        }

        private bool BatchesNameExists(int id, string name)
        {
            return _context.Batches.Any(e => e.Id != id && e.Batch == name);
        }


        public void Dispose()
        {
            _context.Dispose();
        }
    }
}