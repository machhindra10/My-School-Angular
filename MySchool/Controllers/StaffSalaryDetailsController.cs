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
    public class StaffSalaryDetailsController : BaseController, IDisposable
    {
        private readonly ngSchoolContext _context;

        public StaffSalaryDetailsController(ngSchoolContext context)
        {
            _context = context;
        }

        // GET: api/StaffSalaryDetails
        [HttpGet]
        public IEnumerable<StaffSalaryDetails> GetStaffSalaryDetails()
        {
            return _context.StaffSalaryDetails;
        }

        // GET: api/StaffSalaryDetails/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetStaffSalaryDetails([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var staffSalaryDetails = await _context.StaffSalaryDetails.FindAsync(id);

            if (staffSalaryDetails == null)
            {
                return NotFound();
            }

            return Ok(staffSalaryDetails);
        }

        // GET: api/StaffSalaryDetails/5
        [HttpGet, Route("getbystaffsalaryid/{id}")]
        public async Task<IActionResult> GetStaffSalaryDetailsByStaffSalaryId([FromRoute] int id)
        {

            var staffSalaryDetails = await _context.StaffSalaryDetails.Where(e => e.SsId == id).ToListAsync();

            if (staffSalaryDetails == null)
            {
                return NotFound();
            }

            return Ok(staffSalaryDetails);
        }

        // PUT: api/StaffSalaryDetails/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStaffSalaryDetails([FromRoute] int id, [FromBody] StaffSalaryDetails staffSalaryDetails)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != staffSalaryDetails.Id)
            {
                return BadRequest();
            }

            _context.Entry(staffSalaryDetails).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StaffSalaryDetailsExists(id))
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

        // POST: api/StaffSalaryDetails
        [HttpPost]
        public async Task<IActionResult> PostStaffSalaryDetails([FromBody] StaffSalaryDetails staffSalaryDetails)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.StaffSalaryDetails.Add(staffSalaryDetails);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStaffSalaryDetails", new { id = staffSalaryDetails.Id }, staffSalaryDetails);
        }

        // DELETE: api/StaffSalaryDetails/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStaffSalaryDetails([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var staffSalaryDetails = await _context.StaffSalaryDetails.FindAsync(id);
            if (staffSalaryDetails == null)
            {
                return NotFound();
            }

            _context.StaffSalaryDetails.Remove(staffSalaryDetails);
            await _context.SaveChangesAsync();

            return Ok(staffSalaryDetails);
        }

        private bool StaffSalaryDetailsExists(int id)
        {
            return _context.StaffSalaryDetails.Any(e => e.Id == id);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}