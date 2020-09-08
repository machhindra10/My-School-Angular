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
    public class StaffLeavesController : BaseController, IDisposable
    {
        private readonly ngSchoolContext _context;

        public StaffLeavesController(ngSchoolContext context)
        {
            _context = context;
        }

        // GET: api/StaffLeaves
        [HttpGet]
        public IEnumerable<StaffLeaves> GetStaffLeaves()
        {
            return _context.StaffLeaves;
        }

        // GET: api/StaffLeaves/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetStaffLeaves([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var staffLeaves = await _context.StaffLeaves.FindAsync(id);

            if (staffLeaves == null)
            {
                return NotFound();
            }

            return Ok(staffLeaves);
        }

        // PUT: api/StaffLeaves/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStaffLeaves([FromRoute] long id, [FromBody] StaffLeaves staffLeaves)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != staffLeaves.Id)
            {
                return BadRequest();
            }

            _context.Entry(staffLeaves).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StaffLeavesExists(id))
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

        // POST: api/StaffLeaves
        [HttpPost]
        public async Task<IActionResult> PostStaffLeaves([FromBody] StaffLeaves staffLeaves)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.StaffLeaves.Add(staffLeaves);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStaffLeaves", new { id = staffLeaves.Id }, staffLeaves);
        }

        // DELETE: api/StaffLeaves/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStaffLeaves([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var staffLeaves = await _context.StaffLeaves.FindAsync(id);
            if (staffLeaves == null)
            {
                return NotFound();
            }

            _context.StaffLeaves.Remove(staffLeaves);
            await _context.SaveChangesAsync();

            return Ok(staffLeaves);
        }

        private bool StaffLeavesExists(long id)
        {
            return _context.StaffLeaves.Any(e => e.Id == id);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}