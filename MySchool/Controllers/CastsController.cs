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
    public class CastsController : BaseController, IDisposable
    {
        private readonly ngSchoolContext _context;

        public CastsController(ngSchoolContext context)
        {
            _context = context;
        }

        // GET: api/Casts
        [HttpGet]
        public IEnumerable<MCast> GetMCast()
        {
            return _context.MCast.Where(e => e.Disabled == false);
        }

        // GET: api/Casts/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMCast([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var mCast = await _context.MCast.FindAsync(id);

            if (mCast == null)
            {
                return NotFound();
            }

            return Ok(mCast);
        }

        // PUT: api/Casts/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMCast([FromRoute] int id, [FromBody] MCast mCast)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != mCast.Id)
            {
                return BadRequest();
            }

            _context.Entry(mCast).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MCastExists(id))
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

        // POST: api/Casts
        [HttpPost]
        public async Task<IActionResult> PostMCast([FromBody] MCast mCast)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.MCast.Add(mCast);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMCast", new { id = mCast.Id }, mCast);
        }

        // DELETE: api/Casts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMCast([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var mCast = await _context.MCast.FindAsync(id);
            if (mCast == null)
            {
                return NotFound();
            }

            _context.MCast.Remove(mCast);
            await _context.SaveChangesAsync();

            return Ok(mCast);
        }

        private bool MCastExists(int id)
        {
            return _context.MCast.Any(e => e.Id == id);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}