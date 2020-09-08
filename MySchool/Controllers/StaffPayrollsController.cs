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
    public class StaffPayrollsController : BaseController, IDisposable
    {
        private readonly ngSchoolContext _context;

        public StaffPayrollsController(ngSchoolContext context)
        {
            _context = context;
        }

        // GET: api/StaffPayrolls
        [HttpGet]
        public IEnumerable<MStaffPayroll> GetMStaffPayroll()
        {
            return _context.MStaffPayroll;
        }

        // GET: api/StaffPayrolls/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMStaffPayroll([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var mStaffPayroll = await _context.MStaffPayroll.FindAsync(id);

            if (mStaffPayroll == null)
            {
                return NotFound();
            }

            return Ok(mStaffPayroll);
        }

        // GET: api/StaffPayrolls/5
        [HttpGet, Route("getbystaffid/{id}")]
        public async Task<IActionResult> GetMStaffPayrollByStaffId([FromRoute] int id)
        {
            //SetStaffPayrollFirst(id);

            var mStaffPayroll = await (from p in _context.MStaffPayroll
                                       where p.Staffid == id
                                       select p).ToListAsync();

            if (mStaffPayroll == null)
            {
                return NotFound();
            }

            return Ok(mStaffPayroll);
        }

        // GET: api/StaffPayrolls/5
        [HttpGet, Route("setstaffpayroll/{id}")]
        public IActionResult SetStaffPayroll([FromRoute] int id)
        {
            SetStaffPayrollFirst(id);
            return Ok();
        }

        public void SetStaffPayrollFirst(int staffid)
        {
            var payheads = (from p in _context.MPayHead select p).ToList();
            var staffpayroll = (from p in _context.MStaffPayroll where p.Staffid == staffid select p).ToList();

            if (payheads.Count() == staffpayroll.Count())
            {
                return;
            }

            foreach (var templete in payheads)
            {
                var q = (from p in _context.MStaffPayroll
                         where p.Head == templete.Head && p.Staffid == staffid
                         select p).FirstOrDefault();
                if (q == null)
                {
                    MStaffPayroll mStaffPayroll = new MStaffPayroll();
                    mStaffPayroll.Head = templete.Head;
                    mStaffPayroll.Staffid = staffid;
                    mStaffPayroll.Amount = templete.Amount;
                    mStaffPayroll.Type = templete.Type;

                    _context.MStaffPayroll.Add(mStaffPayroll);
                }
            }
            _context.SaveChanges();
        }


        // PUT: api/StaffPayrolls/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMStaffPayroll([FromRoute] int id, [FromBody] MStaffPayroll mStaffPayroll)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != mStaffPayroll.Id)
            {
                return BadRequest();
            }

            _context.Entry(mStaffPayroll).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MStaffPayrollExists(id))
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

        // POST: api/StaffPayrolls
        [HttpPost]
        public async Task<IActionResult> PostMStaffPayroll([FromBody] MStaffPayroll mStaffPayroll)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.MStaffPayroll.Add(mStaffPayroll);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMStaffPayroll", new { id = mStaffPayroll.Id }, mStaffPayroll);
        }

        [HttpPost, Route("updatepayroll")]
        public async Task<IActionResult> PostMStaffPayrollByStaff([FromBody] List<MStaffPayroll> listmStaffPayroll)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            foreach (var mStaffPayroll in listmStaffPayroll)
            {
                _context.Entry(mStaffPayroll).State = EntityState.Modified;
            }
            await _context.SaveChangesAsync();

            return Ok();
        }

        // DELETE: api/StaffPayrolls/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMStaffPayroll([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var mStaffPayroll = await _context.MStaffPayroll.FindAsync(id);
            if (mStaffPayroll == null)
            {
                return NotFound();
            }

            _context.MStaffPayroll.Remove(mStaffPayroll);
            await _context.SaveChangesAsync();

            return Ok(mStaffPayroll);
        }

        private bool MStaffPayrollExists(int id)
        {
            return _context.MStaffPayroll.Any(e => e.Id == id);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}