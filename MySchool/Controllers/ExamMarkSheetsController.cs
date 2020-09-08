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
    public class ExamMarkSheetsController : BaseController, IDisposable
    {
        private readonly ngSchoolContext _context;

        public ExamMarkSheetsController(ngSchoolContext context)
        {
            _context = context;
        }

        // GET: api/ExamMarkSheets
        [HttpGet]
        public IEnumerable<TExamMarkSheet> GetTExamMarkSheet()
        {
            return _context.TExamMarkSheet;
        }

        // GET: api/ExamMarkSheets/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTExamMarkSheet([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tExamMarkSheet = await _context.TExamMarkSheet.FindAsync(id);

            if (tExamMarkSheet == null)
            {
                return NotFound();
            }

            return Ok(tExamMarkSheet);
        }

        // GET: api/ExamMarkSheets/5
        [HttpGet, Route("getmarksheetreport/{batchid}/{classid}/{examid}/{studentids}")]
        public IActionResult GetTExamMarkSheetReport([FromRoute] int batchid, int classid, int examid, string studentids)
        {
            List<int?> sids = new List<int?>(Array.ConvertAll(studentids.Split(','),
                            new Converter<string, int?>((s) => { return Convert.ToInt32(s); })));


            var students = (from s in _context.Student
                            where sids.Contains(s.Id)
                            select new Student
                            {
                                Id = s.Id,
                                Fname = s.Fname + " " + s.Mname + " " + s.Lname,
                                Prnno = s.Prnno,
                                Photo = s.Photo,
                                Guardian = s.Guardian,
                                TExamMarkSheet = (from p in _context.TExamMarkSheet
                                                  where p.Batchid == batchid && p.Classid == classid && p.Examid == examid &&
                                                  p.Studentid == s.Id
                                                  select new TExamMarkSheet()
                                                  {
                                                      Batchid = p.Batchid,
                                                      Classid = p.Classid,
                                                      Grade = p.Grade,
                                                      Id = p.Id,
                                                      Obtained = p.Obtained,
                                                      Practical = p.Practical,
                                                      Remarks = p.Remarks,
                                                      Studentid = p.Studentid,
                                                      Totalmarks = p.Totalmarks,
                                                      Subjectid = p.Subjectid,
                                                      Userid = p.Userid,
                                                      Subject = p.Subject,
                                                      Exam = p.Exam,
                                                      Exmsch = p.Exmsch,
                                                      Class = p.Class,
                                                  }).ToList(),
                                TStudentAdmission = (from a in _context.TStudentAdmission
                                                     where a.Studentid == s.Id && a.Batchid == batchid
                                                     select new TStudentAdmission()
                                                     {
                                                         Rollno = a.Rollno,
                                                     }).ToList()

                            }).Where(p => p.TExamMarkSheet.Count > 0).ToList();

            if (students == null)
            {
                return NotFound();
            }

            return Ok(students);
        }

        // PUT: api/ExamMarkSheets/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTExamMarkSheet([FromRoute] int id, [FromBody] TExamMarkSheet tExamMarkSheet)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tExamMarkSheet.Id)
            {
                return BadRequest();
            }

            _context.Entry(tExamMarkSheet).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TExamMarkSheetExists(id))
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

        // POST: api/ExamMarkSheets
        [HttpPost]
        public async Task<IActionResult> PostTExamMarkSheet([FromBody] TExamMarkSheet tExamMarkSheet)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.TExamMarkSheet.Add(tExamMarkSheet);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTExamMarkSheet", new { id = tExamMarkSheet.Id }, tExamMarkSheet);
        }

        // POST: api/ExamMarkSheets
        [HttpPost, Route("insertmany")]
        public async Task<IActionResult> PostTExamMarkSheets([FromBody] List<TExamMarkSheet> tExamMarkSheets)
        {
            foreach (var item in tExamMarkSheets)
            {
                item.Student = null;
                if (item.Id == 0)
                {
                    if (item.Obtained != null)
                    {
                        item.Userid = GetUserId();
                        item.DateCreated = GetTimeZoneDate(DateTime.UtcNow);

                        _context.TExamMarkSheet.Add(item);
                    }
                }
                else
                {
                    if (item.Obtained != null)
                    {
                        _context.Entry(item).State = EntityState.Modified;
                    }
                    else
                    {
                        _context.TExamMarkSheet.Remove(item);
                    }
                }
            }

            await _context.SaveChangesAsync();

            return Ok();
        }

        // DELETE: api/ExamMarkSheets/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTExamMarkSheet([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tExamMarkSheet = await _context.TExamMarkSheet.FindAsync(id);
            if (tExamMarkSheet == null)
            {
                return NotFound();
            }

            _context.TExamMarkSheet.Remove(tExamMarkSheet);
            await _context.SaveChangesAsync();

            return Ok(tExamMarkSheet);
        }

        private bool TExamMarkSheetExists(int id)
        {
            return _context.TExamMarkSheet.Any(e => e.Id == id);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}