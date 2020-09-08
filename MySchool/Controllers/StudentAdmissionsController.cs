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
    public class StudentAdmissionsController : BaseController, IDisposable
    {
        private readonly ngSchoolContext _context;

        public StudentAdmissionsController(ngSchoolContext context)
        {
            _context = context;
        }

        // GET: api/StudentAdmissions
        [HttpGet]
        public IEnumerable<TStudentAdmission> GetTStudentAdmission()
        {
            return _context.TStudentAdmission;
        }

        // GET: api/StudentAdmissions/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTStudentAdmission([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tStudentAdmission = await _context.TStudentAdmission.FindAsync(id);

            if (tStudentAdmission == null)
            {
                return NotFound();
            }

            return Ok(tStudentAdmission);
        }

        // GET: api/StudentAdmissions/5
        [HttpGet(), Route("checkstudentadmitted/{id}")]
        public  IActionResult CheckStudentAdmission([FromRoute] int id)
        {
            int batchid = GetBatchId();           

            var admitted = _context.TStudentAdmission.Any(e => e.Studentid == id && e.Batchid == batchid && e.Cancelled == false);            

            return Ok(admitted);
        }

        // PUT: api/StudentAdmissions/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTStudentAdmission([FromRoute] int id, [FromBody] TStudentAdmission tStudentAdmission)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tStudentAdmission.Id)
            {
                return BadRequest();
            }

            _context.Entry(tStudentAdmission).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TStudentAdmissionExists(id))
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

        // POST: api/StudentAdmissions
        [HttpPost]
        public async Task<IActionResult> PostTStudentAdmission([FromBody] TStudentAdmission tStudentAdmission)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            int batchid = GetBatchId();

            int rollno = (from p in _context.TStudentAdmission
                          where p.Classid == tStudentAdmission.Classid && p.Batchid == batchid
                          select p).Count();

            tStudentAdmission.Rollno = rollno + 1;
            tStudentAdmission.Userid = GetUserId();
            tStudentAdmission.Batchid = batchid;
            tStudentAdmission.Cancelled = false;
            tStudentAdmission.Datecreated = GetTimeZoneDate(tStudentAdmission.Datecreated);

            _context.TStudentAdmission.Add(tStudentAdmission);
            await _context.SaveChangesAsync();

            SaveStudentFees(tStudentAdmission.Studentid, tStudentAdmission.Classid);

            return CreatedAtAction("GetTStudentAdmission", new { id = tStudentAdmission.Id }, tStudentAdmission);
        }

        private void SaveStudentFees(int? studentid, int? classid)
        {
            
            var q = (from p in _context.ClassFees where p.Classid == classid select p).ToList();

            foreach (var item in q)
            {
                TStudentFees studentFees = new TStudentFees();
                studentFees.FeesType = item.FeesType;
                studentFees.Amount = item.Amount;
                studentFees.Classid = item.Classid;
                studentFees.Datecreated = GetTimeZoneDate(DateTime.UtcNow);
                studentFees.Userid = GetUserId();
                studentFees.Batchid = GetBatchId();
                studentFees.Studentid = studentid;

                _context.TStudentFees.Add(studentFees);
                studentFees = null;
            }
            _context.SaveChanges();
        }

        // DELETE: api/StudentAdmissions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTStudentAdmission([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tStudentAdmission = await _context.TStudentAdmission.FindAsync(id);
            if (tStudentAdmission == null)
            {
                return NotFound();
            }

            _context.TStudentAdmission.Remove(tStudentAdmission);
            await _context.SaveChangesAsync();

            return Ok(tStudentAdmission);
        }

        private bool TStudentAdmissionExists(int id)
        {
            return _context.TStudentAdmission.Any(e => e.Id == id);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}