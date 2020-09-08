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
    public class ExamsController : BaseController, IDisposable
    {
        private readonly ngSchoolContext _context;

        public ExamsController(ngSchoolContext context)
        {
            _context = context;
        }

        // GET: api/Exams
        [HttpGet]
        public IEnumerable<TExam> GetTExam()
        {
            var tExam = (from p in _context.TExam
                         select new TExam
                         {
                             Batchid = p.Batchid,
                             Id = p.Id,
                             Userid = p.Userid,
                             Classid = p.Classid,
                             DateCreated = p.DateCreated,
                             ExamName = p.ExamName,
                             TExamSchedule = _context.TExamSchedule.Where(es=>es.Examid == p.Id).ToList(),
                             Class = _context.MClasses.Where(c=> c.Id == p.Classid).FirstOrDefault()
                         }).ToList();

            return tExam;

        }

        // GET: api/Exams/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTExam([FromRoute] int id)
        {
            var tExam = await (from p in _context.TExam
                               where p.Id == id
                               select new TExam
                               {
                                   Batchid = p.Batchid,
                                   Id = p.Id,
                                   Userid = p.Userid,
                                   Classid = p.Classid,
                                   DateCreated = p.DateCreated,
                                   ExamName = p.ExamName,                                   
                                   TExamSchedule = (from es in _context.TExamSchedule
                                                    where es.Examid == p.Id
                                                    select new TExamSchedule
                                                    {
                                                        Batchid = es.Batchid,
                                                        Endtime = es.Endtime,
                                                        DateCreated = es.DateCreated,
                                                        Examid = es.Examid,
                                                        Classid = es.Classid,
                                                        Examdate = es.Examdate,
                                                        Id = es.Id,
                                                        Starttime = es.Starttime,
                                                        Subjectid = es.Subjectid,
                                                        Userid = es.Userid,
                                                        Totalmarks = es.Totalmarks,
                                                        Passingmarks = es.Passingmarks,
                                                        Subject = _context.MSubjects.Where(s => s.Id == es.Subjectid).FirstOrDefault()
                                                    }).ToList(),
                                   TExamMarkSheet = _context.TExamMarkSheet.Where(m=>m.Examid == p.Id).ToList(),

                               }).FirstOrDefaultAsync();

            if (tExam == null)
            {
                return NotFound();
            }

            return Ok(tExam);
        }

        // GET: api/Exams/5
        [HttpGet, Route("getbyclassid/{classid}")]
        public async Task<IActionResult> GetTExamsByClassId([FromRoute] int classid)
        {
            
            var tExams = await (from p in _context.TExam
                               where (classid == 0 ? true : p.Classid == classid)
                               select p).ToListAsync();            

            return Ok(tExams);
        }

        // PUT: api/Exams/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTExam([FromRoute] int id, [FromBody] TExam tExam)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tExam.Id)
            {
                return BadRequest();
            }

            if (CheckIfExamExists(tExam.Id, tExam.Classid, tExam.Batchid, tExam.ExamName))
            {
                return Ok(new { exists = true });
            }
            var examschedule = tExam.TExamSchedule;

            tExam.TExamMarkSheet = null;
            tExam.TExamSchedule = null;
            tExam.TExamStudentsAdmitCard = null;
            _context.Entry(tExam).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            foreach (var item in examschedule)
            {                
                if (item.Id == 0)
                {
                    item.DateCreated = GetTimeZoneDate(DateTime.UtcNow);
                    item.Batchid = GetBatchId();
                    item.Userid = GetUserId();
                    item.Examid = tExam.Id;
                    item.Examdate = GetTimeZoneDate(item.Examdate);
                    _context.TExamSchedule.Add(item);
                }
                else if (item.Id > 0)
                {
                    item.Examdate = GetTimeZoneDate(item.Examdate);
                    _context.Entry(item).State = EntityState.Modified;
                }
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TExamExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(new { exists = false });
        }

        // POST: api/Exams
        [HttpPost]
        public async Task<IActionResult> PostTExam([FromBody] TExam tExam)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (CheckIfExamExists(tExam.Id, tExam.Classid, GetBatchId(), tExam.ExamName))
            {
                return Ok(new { exists = true });
            }

            tExam.DateCreated = GetTimeZoneDate(DateTime.UtcNow);
            tExam.Userid = GetUserId();
            tExam.Batchid = GetBatchId();

            _context.TExam.Add(tExam);

            foreach (var item in tExam.TExamSchedule)
            {
                item.DateCreated = GetTimeZoneDate(DateTime.UtcNow);
                item.Batchid = GetBatchId();
                item.Userid = GetUserId();
                item.Examid = tExam.Id;
                item.Examdate = GetTimeZoneDate(item.Examdate);

                _context.TExamSchedule.Add(item);
            }
            await _context.SaveChangesAsync();

            return Ok(new { exists = false });
        }

        private bool CheckIfExamExists(int id, int? classid, int? batchid, string examName)
        {
            return _context.TExam.Any(e => e.Id != id && e.Classid == classid && e.Batchid == batchid && e.ExamName == examName);
        }

        // DELETE: api/Exams/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTExam([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tExam = await _context.TExam.FindAsync(id);
            if (tExam == null)
            {
                return NotFound();
            }

            try
            {
                var exam_subjects = (from p in _context.TExamSchedule where p.Examid == tExam.Id select p).ToList();
                foreach (var item in exam_subjects)
                {
                    _context.TExamSchedule.Remove(item);
                }
                _context.TExam.Remove(tExam);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return Ok(new { result = false });
            }

            return Ok(new { result = true });
        }

        private bool TExamExists(int id)
        {
            return _context.TExam.Any(e => e.Id == id);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}