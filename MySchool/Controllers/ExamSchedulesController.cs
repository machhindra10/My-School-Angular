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
    public class ExamSchedulesController : BaseController, IDisposable
    {
        private readonly ngSchoolContext _context;

        public ExamSchedulesController(ngSchoolContext context)
        {
            _context = context;
        }

        // GET: api/ExamSchedules
        [HttpGet]
        public IEnumerable<TExamSchedule> GetTExamSchedule()
        {
            return _context.TExamSchedule;
        }

        public class temp
        {
            public DateTime date { get; set; }
            public int classid { get; set; }
        }

        // GET: api/ExamSchedules
        [HttpPut, Route("getbyclassid")]
        public IEnumerable<TExamSchedule> GetTExamScheduleByDromDateClassId([FromBody] temp temp)
        {
            int batchid = GetBatchId();
            DateTime dt = GetTimeZoneDate(temp.date);

            var q = (from p in _context.TExamSchedule
                     where p.Examdate.Value.Date >= dt.Date && p.Batchid == batchid
                     select new TExamSchedule
                     {
                         Batchid = p.Batchid,
                         Classid = p.Classid,
                         DateCreated = p.DateCreated,
                         Endtime = p.Endtime,
                         Examdate = p.Examdate,
                         Examid = p.Examid,
                         Id = p.Id,
                         Starttime = p.Starttime,
                         Subjectid = p.Subjectid,
                         Userid = p.Userid,
                         Exam = _context.TExam.Where(e => e.Id == p.Examid).FirstOrDefault(),
                         Subject = _context.MSubjects.Where(s => s.Id == p.Subjectid).FirstOrDefault(),
                         Class = _context.MClasses.Where(c => c.Id == p.Classid).FirstOrDefault()
                     }).ToList();

            if (temp.classid > 0)
            {
                q = q.Where(p => p.Classid == temp.classid).ToList();
            }

            return q;
        }

        // GET: api/ExamSchedules
        [HttpGet, Route("getbyid/{id}")]
        public TExamSchedule GetTExamScheduleById([FromRoute] int id)
        {
            int batchid = GetBatchId();

            var q = (from es in _context.TExamSchedule
                     where es.Id == id && es.Batchid == batchid
                     select new TExamSchedule
                     {
                         Batchid = es.Batchid,
                         Classid = es.Classid,
                         DateCreated = es.DateCreated,
                         Endtime = es.Endtime,
                         Examdate = es.Examdate,
                         Examid = es.Examid,
                         Id = es.Id,
                         Starttime = es.Starttime,
                         Subjectid = es.Subjectid,
                         Userid = es.Userid,
                         Totalmarks = es.Totalmarks,
                         Passingmarks = es.Passingmarks,
                         Exam = _context.TExam.Where(e => e.Id == es.Examid).FirstOrDefault(),
                         Subject = _context.MSubjects.Where(s => s.Id == es.Subjectid).FirstOrDefault(),
                         Class = _context.MClasses.Where(c => c.Id == es.Classid).FirstOrDefault(),
                         TExamMarkSheet = (from s in _context.Student
                                           where (from sa in _context.TStudentAdmission
                                                  where sa.Classid == es.Classid && sa.Batchid == batchid
                                                  select sa.Studentid).Contains(s.Id)
                                           select new TExamMarkSheet
                                           {
                                               Id = (_context.TExamMarkSheet.Where(em => em.Studentid == s.Id && em.ExmschId == es.Id).FirstOrDefault() == null) ? 0 :
                                               _context.TExamMarkSheet.Where(em => em.Studentid == s.Id && em.ExmschId == es.Id).FirstOrDefault().Id,
                                               Batchid = (_context.TExamMarkSheet.Where(em => em.Studentid == s.Id && em.ExmschId == es.Id).FirstOrDefault() == null) ? es.Batchid :
                                               _context.TExamMarkSheet.Where(em => em.Studentid == s.Id && em.ExmschId == es.Id).FirstOrDefault().Batchid,
                                               Classid = (_context.TExamMarkSheet.Where(em => em.Studentid == s.Id && em.ExmschId == es.Id).FirstOrDefault() == null) ? es.Classid :
                                               _context.TExamMarkSheet.Where(em => em.Studentid == s.Id && em.ExmschId == es.Id).FirstOrDefault().Classid,
                                               DateCreated = (_context.TExamMarkSheet.Where(em => em.Studentid == s.Id && em.ExmschId == es.Id).FirstOrDefault() == null) ? null :
                                               _context.TExamMarkSheet.Where(em => em.Studentid == s.Id && em.ExmschId == es.Id).FirstOrDefault().DateCreated,
                                               ExmschId = (_context.TExamMarkSheet.Where(em => em.Studentid == s.Id && em.ExmschId == es.Id).FirstOrDefault() == null) ? es.Id :
                                               _context.TExamMarkSheet.Where(em => em.Studentid == s.Id && em.ExmschId == es.Id).FirstOrDefault().ExmschId,
                                               Studentid = (_context.TExamMarkSheet.Where(em => em.Studentid == s.Id && em.ExmschId == es.Id).FirstOrDefault() == null) ? s.Id :
                                               _context.TExamMarkSheet.Where(em => em.Studentid == s.Id && em.ExmschId == es.Id).FirstOrDefault().Studentid,
                                               Examid = (_context.TExamMarkSheet.Where(em => em.Studentid == s.Id && em.ExmschId == es.Id).FirstOrDefault() == null) ? es.Examid :
                                               _context.TExamMarkSheet.Where(em => em.Studentid == s.Id && em.ExmschId == es.Id).FirstOrDefault().Examid,
                                               Grade = (_context.TExamMarkSheet.Where(em => em.Studentid == s.Id && em.ExmschId == es.Id).FirstOrDefault() == null) ? null :
                                               _context.TExamMarkSheet.Where(em => em.Studentid == s.Id && em.ExmschId == es.Id).FirstOrDefault().Grade,
                                               Obtained = (_context.TExamMarkSheet.Where(em => em.Studentid == s.Id && em.ExmschId == es.Id).FirstOrDefault() == null) ? null :
                                               _context.TExamMarkSheet.Where(em => em.Studentid == s.Id && em.ExmschId == es.Id).FirstOrDefault().Obtained,
                                               Practical = (_context.TExamMarkSheet.Where(em => em.Studentid == s.Id && em.ExmschId == es.Id).FirstOrDefault() == null) ? null :
                                               _context.TExamMarkSheet.Where(em => em.Studentid == s.Id && em.ExmschId == es.Id).FirstOrDefault().Practical,
                                               Subjectid = (_context.TExamMarkSheet.Where(em => em.Studentid == s.Id && em.ExmschId == es.Id).FirstOrDefault() == null) ? es.Subjectid :
                                               _context.TExamMarkSheet.Where(em => em.Studentid == s.Id && em.ExmschId == es.Id).FirstOrDefault().Subjectid,
                                               Totalmarks = (_context.TExamMarkSheet.Where(em => em.Studentid == s.Id && em.ExmschId == es.Id).FirstOrDefault() == null) ? 0 :
                                               _context.TExamMarkSheet.Where(em => em.Studentid == s.Id && em.ExmschId == es.Id).FirstOrDefault().Totalmarks,
                                               Userid = (_context.TExamMarkSheet.Where(em => em.Studentid == s.Id && em.ExmschId == es.Id).FirstOrDefault() == null) ? 0 :
                                               _context.TExamMarkSheet.Where(em => em.Studentid == s.Id && em.ExmschId == es.Id).FirstOrDefault().Userid,
                                               Remarks = (_context.TExamMarkSheet.Where(em => em.Studentid == s.Id && em.ExmschId == es.Id).FirstOrDefault() == null) ? "" :
                                               _context.TExamMarkSheet.Where(em => em.Studentid == s.Id && em.ExmschId == es.Id).FirstOrDefault().Remarks,

                                               Student = new Student { Fname = s.Fname, Mname = s.Mname, Lname = s.Lname },

                                           }).ToList()
                     }).FirstOrDefault();

            return q;
        }

        // GET: api/ExamSchedules/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTExamSchedule([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tExamSchedule = await _context.TExamSchedule.FindAsync(id);

            if (tExamSchedule == null)
            {
                return NotFound();
            }

            return Ok(tExamSchedule);
        }

        // PUT: api/ExamSchedules/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTExamSchedule([FromRoute] int id, [FromBody] TExamSchedule tExamSchedule)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tExamSchedule.Id)
            {
                return BadRequest();
            }

            tExamSchedule.Exam = null;
            tExamSchedule.Class = null;
            tExamSchedule.Subject = null;
            tExamSchedule.TExamMarkSheet = null;

            _context.Entry(tExamSchedule).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TExamScheduleExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok();
        }

        // POST: api/ExamSchedules
        [HttpPost]
        public async Task<IActionResult> PostTExamSchedule([FromBody] TExamSchedule tExamSchedule)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.TExamSchedule.Add(tExamSchedule);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTExamSchedule", new { id = tExamSchedule.Id }, tExamSchedule);
        }

        // DELETE: api/ExamSchedules/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTExamSchedule([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tExamSchedule = await _context.TExamSchedule.FindAsync(id);
            if (tExamSchedule == null)
            {
                return NotFound();
            }

            try
            {
                _context.TExamSchedule.Remove(tExamSchedule);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return Ok(new { result = false });
            }

            return Ok(new { result = true });
        }

        private bool TExamScheduleExists(int id)
        {
            return _context.TExamSchedule.Any(e => e.Id == id);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}