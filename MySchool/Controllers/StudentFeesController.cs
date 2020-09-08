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
    public class StudentFeesController : BaseController, IDisposable
    {
        private readonly ngSchoolContext _context;

        public StudentFeesController(ngSchoolContext context)
        {
            _context = context;
        }

        // GET: api/StudentFees
        [HttpGet]
        public IEnumerable<TStudentFees> GetTStudentFees()
        {
            return _context.TStudentFees;
        }

        // GET: api/StudentFees/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTStudentFees([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tStudentFees = await _context.TStudentFees.FindAsync(id);

            if (tStudentFees == null)
            {
                return NotFound();
            }

            return Ok(tStudentFees);
        }

        // GET: api/StudentFees/5
        [HttpGet(), Route("getbystudentid/{id}")]
        public async Task<IActionResult> GetTStudentFeesByStudentId([FromRoute] int id)
        {
            int batchid = GetBatchId();
            var tStudentFees = await _context.TStudentFees.Where(e=>e.Studentid == id && e.Batchid == batchid).ToListAsync();

            if (tStudentFees == null)
            {
                return NotFound();
            }

            return Ok(tStudentFees);
        }

        // PUT: api/StudentFees/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTStudentFees([FromRoute] int id, [FromBody] TStudentFees tStudentFees)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tStudentFees.Id)
            {
                return BadRequest();
            }

            _context.Entry(tStudentFees).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TStudentFeesExists(id))
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

        // POST: api/StudentFees
        [HttpPost]
        public async Task<IActionResult> PostTStudentFees([FromBody] TStudentFees tStudentFees)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            int batchid = GetBatchId();
            tStudentFees.Classid = _context.TStudentAdmission.Where(e => e.Studentid == tStudentFees.Studentid && e.Batchid == batchid).Select(e => e.Classid).FirstOrDefault();
            tStudentFees.Userid = GetUserId();
            tStudentFees.Datecreated = GetTimeZoneDate(DateTime.UtcNow);
            tStudentFees.Batchid = GetBatchId();
            
            _context.TStudentFees.Add(tStudentFees);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTStudentFees", new { id = tStudentFees.Id }, tStudentFees);
        }

        // POST: api/StudentFees
        [HttpGet, Route("updatefromclasstemplete/{studentid}")]
        public async Task<IActionResult> PostTStudentFeesFromClassTemplete([FromRoute] int studentid)
        {
            bool result1 = false;
            int batchid = GetBatchId();
            var admission = (from a in _context.TStudentAdmission
                     where a.Studentid == studentid && a.Batchid == batchid
                     select a).FirstOrDefault();

            var classfees = (from cf in _context.ClassFees where cf.Classid == admission.Classid select cf).ToList();

            foreach (var classfee in classfees)
            {
                var q = (from sf in _context.TStudentFees where sf.FeesType == classfee.FeesType && 
                                                            sf.Studentid == studentid && 
                                                            sf.Classid == admission.Classid &&
                                                            sf.Batchid == batchid
                                                    select sf).FirstOrDefault();
                if(q == null)
                {
                    TStudentFees tStudentFees = new TStudentFees();

                    tStudentFees.Studentid = studentid;
                    tStudentFees.Classid = admission.Classid;
                    tStudentFees.Userid = GetUserId();
                    tStudentFees.Datecreated = GetTimeZoneDate(DateTime.UtcNow);
                    tStudentFees.Batchid = batchid;
                    tStudentFees.Amount = classfee.Amount;
                    tStudentFees.FeesType = classfee.FeesType;

                    _context.TStudentFees.Add(tStudentFees);

                    tStudentFees = null;
                    result1 = true;
                }
            }
            
            await _context.SaveChangesAsync();

            return Ok(new { result = result1 });
        }

        // DELETE: api/StudentFees/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTStudentFees([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tStudentFees = await _context.TStudentFees.FindAsync(id);
            if (tStudentFees == null)
            {
                return NotFound();
            }

            _context.TStudentFees.Remove(tStudentFees);
            await _context.SaveChangesAsync();

            return Ok(tStudentFees);
        }

        private bool TStudentFeesExists(int id)
        {
            return _context.TStudentFees.Any(e => e.Id == id);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}