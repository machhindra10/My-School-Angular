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
    public class StudentPaymentsController : BaseController, IDisposable
    {
        private readonly ngSchoolContext _context;

        public StudentPaymentsController(ngSchoolContext context)
        {
            _context = context;
        }

        // GET: api/StudentPayments
        [HttpGet]
        public IEnumerable<TStudentPayment> GetTStudentPayment()
        {
            return _context.TStudentPayment;
        }

        // GET: api/StudentPayments/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTStudentPayment([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tStudentPayment = await _context.TStudentPayment.FindAsync(id);

            if (tStudentPayment == null)
            {
                return NotFound();
            }

            return Ok(tStudentPayment);
        }

        // GET: api/StudentPayments/5
        [HttpGet(), Route("getbystudentid/{id}")]
        public async Task<IActionResult> GetTStudentPaymentByStudentId([FromRoute] int id)
        {
            int batchid = GetBatchId();
            var tStudentPayment = await _context.TStudentPayment.Where(e => e.Studentid == id && e.Batchid == batchid).OrderByDescending(e => e.Datecreated).ToListAsync();

            if (tStudentPayment == null)
            {
                return NotFound();
            }

            return Ok(tStudentPayment);
        }

        // GET: api/StudentPayments/5
        [HttpGet(), Route("getsummarybystudentid/{id}")]
        public async Task<IActionResult> GetSummaryByStudentId([FromRoute] int id)
        {
            int batchid = GetBatchId();
            List<summary> summaries = new List<summary>();

            var pSum = await (from e in _context.TStudentPayment
                              where e.Studentid == id && e.Batchid == batchid
                              select e.Amount).SumAsync();
            var fSum = await (from e in _context.TStudentFees
                              where e.Studentid == id && e.Batchid == batchid
                              select e.Amount).SumAsync();

            var prevpSum = await (from e in _context.TStudentPayment
                                  where e.Studentid == id && e.Batchid < batchid
                                  select e.Amount).SumAsync();
            var prevfSum = await (from e in _context.TStudentFees
                                  where e.Studentid == id && e.Batchid < batchid
                                  select e.Amount).SumAsync();

            var outstanding = prevfSum - prevpSum;
            var tobepaid = (fSum - pSum) + outstanding;


            summaries.Add(new summary() { Description = "Total Fees", Amount = fSum });
            summaries.Add(new summary() { Description = "Total Paid", Amount = pSum });
            summaries.Add(new summary() { Description = "Outstandings", Amount = outstanding });
            summaries.Add(new summary() { Description = "To be Paid", Amount = tobepaid });



            return Ok(summaries);
        }

        // GET: api/StudentPayments/5
        [HttpGet(), Route("getreceiptbyid/{id}")]
        public async Task<IActionResult> GetReceiptByStudentId([FromRoute] int id)
        {
            var tStudentPayment = await (from p in _context.TStudentPayment
                                         where p.Id == id
                                         select new TStudentPayment
                                         {
                                             Id = p.Id,
                                             Amount = p.Amount,
                                             Chtrno = p.Chtrno,
                                             Datecreated = p.Datecreated,
                                             Description = p.Description,
                                             Mode = p.Mode,
                                             Student = (from s in _context.Student
                                                        where s.Id == p.Studentid
                                                        select new Student
                                                        {
                                                            Id = s.Id,
                                                            Fname = s.Fname,
                                                            Mname = s.Mname,
                                                            Lname = s.Lname,
                                                            Prnno = s.Prnno,
                                                            Address = s.Address,

                                                        }).FirstOrDefault(),
                                             Batchid = p.Batchid
                                         }).ToListAsync();

            if (tStudentPayment == null)
            {
                return NotFound();
            }

            return Ok(tStudentPayment);
        }

        // GET: api/StudentPayments/5
        [HttpGet(), Route("getmonthlyreport/{classid}/{month}/{year}/{batchid}")]
        public async Task<IActionResult> GetMonthlyReport([FromRoute] int classid, int month, int year, int batchid)
        {
            List<int?> sids;
            if (classid > 0)
            {
                sids = (from a in _context.TStudentAdmission where a.Batchid == batchid && a.Classid == classid select a.Studentid).ToList();
            }
            else
            {
                sids = (from a in _context.TStudentAdmission where a.Batchid == batchid select a.Studentid).ToList();
            }
            var tStudentPayment = await (from p in _context.TStudentPayment
                                         where p.Datecreated.Value.Month == month &&
                                               p.Datecreated.Value.Year == year &&
                                               sids.Contains(p.Studentid)
                                         select new TStudentPayment
                                         {
                                             Id = p.Id,
                                             Amount = p.Amount,
                                             Chtrno = p.Chtrno,
                                             Datecreated = p.Datecreated,
                                             Description = p.Description,
                                             Mode = p.Mode,
                                             Student = (from s in _context.Student
                                                        where s.Id == p.Studentid
                                                        select new Student
                                                        {
                                                            Id = s.Id,
                                                            Fname = s.Fname,
                                                            Mname = s.Mname,
                                                            Lname = s.Lname,
                                                            Prnno = s.Prnno,
                                                            Address = s.Address,

                                                        }).FirstOrDefault(),
                                             Batchid = p.Batchid
                                         }).ToListAsync();

            if (tStudentPayment == null)
            {
                return NotFound();
            }

            return Ok(tStudentPayment);
        }

        // PUT: api/StudentPayments/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTStudentPayment([FromRoute] int id, [FromBody] TStudentPayment tStudentPayment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tStudentPayment.Id)
            {
                return BadRequest();
            }

            _context.Entry(tStudentPayment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TStudentPaymentExists(id))
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

        // POST: api/StudentPayments
        [HttpPost]
        public async Task<IActionResult> PostTStudentPayment([FromBody] TStudentPayment tStudentPayment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            tStudentPayment.Userid = GetUserId();
            tStudentPayment.Datecreated = GetTimeZoneDate(tStudentPayment.Datecreated);
            tStudentPayment.Batchid = GetBatchId();

            _context.TStudentPayment.Add(tStudentPayment);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTStudentPayment", new { id = tStudentPayment.Id }, tStudentPayment);
        }

        // DELETE: api/StudentPayments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTStudentPayment([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tStudentPayment = await _context.TStudentPayment.FindAsync(id);
            if (tStudentPayment == null)
            {
                return NotFound();
            }

            _context.TStudentPayment.Remove(tStudentPayment);
            await _context.SaveChangesAsync();

            return Ok(tStudentPayment);
        }

        private bool TStudentPaymentExists(int id)
        {
            return _context.TStudentPayment.Any(e => e.Id == id);
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        private class summary
        {
            public string Description { get; set; }
            public decimal? Amount { get; set; }
        }
    }



}