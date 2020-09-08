using Microsoft.AspNetCore.Http;
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
    public class LeaveTypesController : BaseController, IDisposable
    {
        private readonly ngSchoolContext _context;

        public LeaveTypesController(ngSchoolContext context)
        {
            _context = context;
        }

        // GET: api/LeaveTypes
        [HttpGet]
        public IEnumerable<MLeaveType> GetMLeaveType()
        {
            return _context.MLeaveType;
        }

        // GET: api/LeaveTypes/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMLeaveType([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var mLeaveType = await _context.MLeaveType.FindAsync(id);

            if (mLeaveType == null)
            {
                return NotFound();
            }

            return Ok(mLeaveType);
        }

        // PUT: api/LeaveTypes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMLeaveType([FromRoute] long id, [FromBody] MLeaveType mLeaveType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != mLeaveType.Id)
            {
                return BadRequest();
            }

            if (MLeaveTypeExistsName(mLeaveType.Id, mLeaveType.Leavetype))
            {
                return Ok(new { exists = true });
            }
            if (MLeaveTypeExistsCode(mLeaveType.Id, mLeaveType.Code))
            {
                return Ok(new { codeexists = true });
            }

            _context.Entry(mLeaveType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MLeaveTypeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(new { nameexists = false, codeexists = false });
        }

        // POST: api/LeaveTypes
        [HttpPost]
        public async Task<IActionResult> PostMLeaveType([FromBody] MLeaveType mLeaveType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (MLeaveTypeExistsName(mLeaveType.Id, mLeaveType.Leavetype))
            {
                return Ok(new { nameexists = true });
            }
            if (MLeaveTypeExistsCode(mLeaveType.Id, mLeaveType.Code))
            {
                return Ok(new { codeexists = true });
            }

            _context.MLeaveType.Add(mLeaveType);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (MLeaveTypeExists(mLeaveType.Id))
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }

            return Ok(new { nameexists = false, codeexists = false });
        }

        // DELETE: api/LeaveTypes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMLeaveType([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var mLeaveType = await _context.MLeaveType.FindAsync(id);
            if (mLeaveType == null)
            {
                return NotFound();
            }

            _context.MLeaveType.Remove(mLeaveType);
            await _context.SaveChangesAsync();

            return Ok(mLeaveType);
        }

        private bool MLeaveTypeExists(long id)
        {
            return _context.MLeaveType.Any(e => e.Id == id);
        }

        private bool MLeaveTypeExistsName(long id, string name)
        {
            return _context.MLeaveType.Any(e => e.Id != id & e.Leavetype == name);
        }

        private bool MLeaveTypeExistsCode(long id, string code)
        {
            return _context.MLeaveType.Any(e => e.Id != id & e.Code == code);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}