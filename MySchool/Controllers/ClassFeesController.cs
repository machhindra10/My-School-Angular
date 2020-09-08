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
    public class ClassFeesController : BaseController, IDisposable
    {
        private readonly ngSchoolContext _context;

        public ClassFeesController(ngSchoolContext context)
        {
            _context = context;
        }

        // GET: api/ClassFees
        [HttpGet]
        public IEnumerable<ClassFees> GetTClassFees()
        {
            return _context.ClassFees;
        }

        // GET: api/ClassFees/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTClassFees([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ClassFees = await _context.ClassFees.FindAsync(id);

            if (ClassFees == null)
            {
                return NotFound();
            }

            return Ok(ClassFees);
        }

        // GET: api/ClassFees/5
        [HttpGet(), Route("getbyclassid/{id}")]
        public async Task<IActionResult> GetTClassFeesByClassId([FromRoute] int id)
        {
            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tClassFees = await _context.ClassFees.Where(e => e.Classid == id ).ToListAsync();

            if (tClassFees == null)
            {
                return NotFound();
            }

            return Ok(tClassFees);
        }

        // PUT: api/ClassFees/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTClassFees([FromRoute] int id, [FromBody] ClassFees ClassFees)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != ClassFees.Id)
            {
                return BadRequest();
            }

            _context.Entry(ClassFees).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TClassFeesExists(id))
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

        // POST: api/ClassFees
        [HttpPost]
        public async Task<IActionResult> PostTClassFees([FromBody] ClassFees ClassFees)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ClassFees.Userid = GetUserId();
            ClassFees.Datecreated = GetTimeZoneDate(DateTime.UtcNow);
            
            _context.ClassFees.Add(ClassFees);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTClassFees", new { id = ClassFees.Id }, ClassFees);
        }

        // DELETE: api/ClassFees/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTClassFees([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ClassFees = await _context.ClassFees.FindAsync(id);
            if (ClassFees == null)
            {
                return NotFound();
            }

            _context.ClassFees.Remove(ClassFees);
            await _context.SaveChangesAsync();

            return Ok(ClassFees);
        }

        private bool TClassFeesExists(int id)
        {
            return _context.ClassFees.Any(e => e.Id == id);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}