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
    public class TimeTablesController : BaseController
    {
        private readonly ngSchoolContext _context;

        public TimeTablesController(ngSchoolContext context)
        {
            _context = context;
        }

        // GET: api/TimeTables
        [HttpGet]
        public IEnumerable<MTimeTable> GetTimeTable()
        {
            return _context.MTimeTable;
        }

        // GET: api/MTimeTables/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMTimeTable([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var timeTable = await _context.MTimeTable.FindAsync(id);

            if (timeTable == null)
            {
                return NotFound();
            }

            return Ok(timeTable);
        }

        // GET: api/TimeTables/5
        [HttpGet, Route("getbyclassid/{classid}/{batchid}")]
        public async Task<IActionResult> GetTimeTableData([FromRoute] int classid, int batchid)
        {
            try
            {
                var timeTable = await (from p in _context.MTimeTable
                                       where p.Classid == classid && p.Batchid == batchid
                                       group p by new { p.Fromtime, p.Totime } into g
                                       select new
                                       {
                                           Sunday = (from t in g
                                                     select new
                                                     {
                                                         Id = t.Id,
                                                         Subjectid = t.Sunday,
                                                         Classid = t.Classid,
                                                         Subject = (from s in _context.MSubjects where s.Id == t.Sunday select s).FirstOrDefault(),
                                                         Class = t.Class,
                                                         Batchid = t.Batchid,
                                                         Fromtime = t.Fromtime,
                                                         Totime = t.Totime
                                                     }).FirstOrDefault(),
                                           Monday = (from t in g
                                                     select new
                                                     {
                                                         Id = t.Id,
                                                         Subjectid = t.Monday,
                                                         Classid = t.Classid,
                                                         Subject = (from s in _context.MSubjects where s.Id == t.Monday select s).FirstOrDefault(),
                                                         Class = t.Class,
                                                         Batchid = t.Batchid,
                                                         Fromtime = t.Fromtime,
                                                         Totime = t.Totime
                                                     }).FirstOrDefault(),
                                           Tuesday = (from t in g
                                                      select new
                                                      {
                                                          Id = t.Id,
                                                          Subjectid = t.Tuesday,
                                                          Classid = t.Classid,
                                                          Subject = (from s in _context.MSubjects where s.Id == t.Tuesday select s).FirstOrDefault(),
                                                          Class = t.Class,
                                                          Batchid = t.Batchid,
                                                          Fromtime = t.Fromtime,
                                                          Totime = t.Totime
                                                      }).FirstOrDefault(),
                                           Wednesday = (from t in g
                                                        select new
                                                        {
                                                            Id = t.Id,
                                                            Subjectid = t.Wednesday,
                                                            Classid = t.Classid,
                                                            Subject = (from s in _context.MSubjects where s.Id == t.Wednesday select s).FirstOrDefault(),
                                                            Class = t.Class,
                                                            Batchid = t.Batchid,
                                                            Fromtime = t.Fromtime,
                                                            Totime = t.Totime
                                                        }).FirstOrDefault(),
                                           Thursday = (from t in g
                                                       select new
                                                       {
                                                           Id = t.Id,
                                                           Subjectid = t.Thursday,
                                                           Classid = t.Classid,
                                                           Subject = (from s in _context.MSubjects where s.Id == t.Thursday select s).FirstOrDefault(),
                                                           Class = t.Class,
                                                           Batchid = t.Batchid,
                                                           Fromtime = t.Fromtime,
                                                           Totime = t.Totime
                                                       }).FirstOrDefault(),
                                           Friday = (from t in g
                                                     select new
                                                     {
                                                         Id = t.Id,
                                                         Subjectid = t.Friday,
                                                         Classid = t.Classid,
                                                         Subject = (from s in _context.MSubjects where s.Id == t.Friday select s).FirstOrDefault(),
                                                         Class = t.Class,
                                                         Batchid = t.Batchid,
                                                         Fromtime = t.Fromtime,
                                                         Totime = t.Totime
                                                     }).FirstOrDefault(),
                                           Saturday = (from t in g
                                                       select new
                                                       {
                                                           Id = t.Id,
                                                           Subjectid = t.Saturday,
                                                           Classid = t.Classid,
                                                           Subject = (from s in _context.MSubjects where s.Id == t.Saturday select s).FirstOrDefault(),
                                                           Class = t.Class,
                                                           Batchid = t.Batchid,
                                                           Fromtime = t.Fromtime,
                                                           Totime = t.Totime
                                                       }).FirstOrDefault(),
                                           Fromtime = g.Key.Fromtime,
                                           Totime = g.Key.Totime
                                       }).ToListAsync();

                return Ok(timeTable);
            }
            catch (Exception)
            {
                return Ok();
            }
        }

        // GET: api/TimeTables/5
        [HttpGet, Route("getbyclassidreport/{classid}/{batchid}")]
        public async Task<IActionResult> GetTimeTableDataReport([FromRoute] int classid, int batchid)
        {
            try
            {
                var timeTable = await (from p in _context.MTimeTable
                                       where p.Classid == classid && p.Batchid == batchid
                                       group p by new { p.Fromtime, p.Totime } into g
                                       select new
                                       {
                                           Sunday = (from t in g
                                                     select new
                                                     {
                                                         Id = t.Id,
                                                         Subjectid = t.Sunday,
                                                         Classid = t.Classid,
                                                         Subject = (from s in _context.MSubjects where s.Id == t.Sunday select s).FirstOrDefault(),
                                                         Class = t.Class,
                                                         Batchid = t.Batchid,
                                                         Fromtime = t.Fromtime,
                                                         Totime = t.Totime
                                                     }).FirstOrDefault(),
                                           Monday = (from t in g
                                                     select new
                                                     {
                                                         Id = t.Id,
                                                         Subjectid = t.Monday,
                                                         Classid = t.Classid,
                                                         Subject = (from s in _context.MSubjects where s.Id == t.Monday select s).FirstOrDefault(),
                                                         Class = t.Class,
                                                         Batchid = t.Batchid,
                                                         Fromtime = t.Fromtime,
                                                         Totime = t.Totime
                                                     }).FirstOrDefault(),
                                           Tuesday = (from t in g
                                                      select new
                                                      {
                                                          Id = t.Id,
                                                          Subjectid = t.Tuesday,
                                                          Classid = t.Classid,
                                                          Subject = (from s in _context.MSubjects where s.Id == t.Tuesday select s).FirstOrDefault(),
                                                          Class = t.Class,
                                                          Batchid = t.Batchid,
                                                          Fromtime = t.Fromtime,
                                                          Totime = t.Totime
                                                      }).FirstOrDefault(),
                                           Wednesday = (from t in g
                                                        select new
                                                        {
                                                            Id = t.Id,
                                                            Subjectid = t.Wednesday,
                                                            Classid = t.Classid,
                                                            Subject = (from s in _context.MSubjects where s.Id == t.Wednesday select s).FirstOrDefault(),
                                                            Class = t.Class,
                                                            Batchid = t.Batchid,
                                                            Fromtime = t.Fromtime,
                                                            Totime = t.Totime
                                                        }).FirstOrDefault(),
                                           Thursday = (from t in g
                                                       select new
                                                       {
                                                           Id = t.Id,
                                                           Subjectid = t.Thursday,
                                                           Classid = t.Classid,
                                                           Subject = (from s in _context.MSubjects where s.Id == t.Thursday select s).FirstOrDefault(),
                                                           Class = t.Class,
                                                           Batchid = t.Batchid,
                                                           Fromtime = t.Fromtime,
                                                           Totime = t.Totime
                                                       }).FirstOrDefault(),
                                           Friday = (from t in g
                                                     select new
                                                     {
                                                         Id = t.Id,
                                                         Subjectid = t.Friday,
                                                         Classid = t.Classid,
                                                         Subject = (from s in _context.MSubjects where s.Id == t.Friday select s).FirstOrDefault(),
                                                         Class = t.Class,
                                                         Batchid = t.Batchid,
                                                         Fromtime = t.Fromtime,
                                                         Totime = t.Totime
                                                     }).FirstOrDefault(),
                                           Saturday = (from t in g
                                                       select new
                                                       {
                                                           Id = t.Id,
                                                           Subjectid = t.Saturday,
                                                           Classid = t.Classid,
                                                           Subject = (from s in _context.MSubjects where s.Id == t.Saturday select s).FirstOrDefault(),
                                                           Class = t.Class,
                                                           Batchid = t.Batchid,
                                                           Fromtime = t.Fromtime,
                                                           Totime = t.Totime
                                                       }).FirstOrDefault(),
                                           Fromtime = g.Key.Fromtime,
                                           Totime = g.Key.Totime
                                       }).ToListAsync();

                return Ok(timeTable);
            }
            catch (Exception)
            {
                return Ok();
            }
        }

        // GET: api/TimeTables/5
        [HttpGet, Route("assignsubject/{id}/{subjectid}/{index}")]
        public IActionResult AssignSubject([FromRoute] int id, int subjectid, int index)
        {
            int batchid = GetBatchId();
            var q = _context.MTimeTable.Find(id);

            var staffid = (from p in _context.ClassSubjects where p.Subjectid == subjectid && p.Classid == q.Classid select p.Staffid).FirstOrDefault();
            var subjectids = (from p in _context.ClassSubjects where p.Staffid == staffid select p.Subjectid).Distinct();


            if (index == 1)
            {
                var qq = (from p in _context.MTimeTable
                          where p.Fromtime == q.Fromtime && p.Totime == q.Totime
                           && p.Batchid == batchid
                          && subjectids.Contains(p.Sunday) && p.Id != q.Id
                          select p).ToList();
                if (qq.Count > 0)
                {
                    return Ok(new { exists = true });
                }

                q.Sunday = subjectid;
            }
            else if (index == 2)
            {
                var qq = (from p in _context.MTimeTable
                          where p.Fromtime == q.Fromtime && p.Totime == q.Totime
                           && p.Batchid == batchid
                          && subjectids.Contains(p.Monday) && p.Id != q.Id
                          select p).ToList();
                if (qq.Count > 0)
                {
                    return Ok(new { exists = true });
                }
                q.Monday = subjectid;
            }
            else if (index == 3)
            {
                var qq = (from p in _context.MTimeTable
                          where p.Fromtime == q.Fromtime && p.Totime == q.Totime
                           && p.Batchid == batchid
                          && subjectids.Contains(p.Tuesday) && p.Id != q.Id
                          select p).ToList();
                if (qq.Count > 0)
                {
                    return Ok(new { exists = true });
                }
                q.Tuesday = subjectid;
            }
            else if (index == 4)
            {
                var qq = (from p in _context.MTimeTable
                          where p.Fromtime == q.Fromtime && p.Totime == q.Totime
                           && p.Batchid == batchid
                          && subjectids.Contains(p.Wednesday) && p.Id != q.Id
                          select p).ToList();
                if (qq.Count > 0)
                {
                    return Ok(new { exists = true });
                }
                q.Wednesday = subjectid;
            }
            else if (index == 5)
            {
                var qq = (from p in _context.MTimeTable
                          where p.Fromtime == q.Fromtime && p.Totime == q.Totime
                           && p.Batchid == batchid
                          && subjectids.Contains(p.Thursday) && p.Id != q.Id
                          select p).ToList();
                if (qq.Count > 0)
                {
                    return Ok(new { exists = true });
                }
                q.Thursday = subjectid;
            }
            else if (index == 6)
            {
                var qq = (from p in _context.MTimeTable
                          where p.Fromtime == q.Fromtime && p.Totime == q.Totime
                           && p.Batchid == batchid
                          && subjectids.Contains(p.Friday) && p.Id != q.Id
                          select p).ToList();
                if (qq.Count > 0)
                {
                    return Ok(new { exists = true });
                }
                q.Friday = subjectid;
            }
            else if (index == 7)
            {
                var qq = (from p in _context.MTimeTable
                          where p.Fromtime == q.Fromtime && p.Totime == q.Totime
                           && p.Batchid == batchid
                          && subjectids.Contains(p.Saturday) && p.Id != q.Id
                          select p).ToList();
                if (qq.Count > 0)
                {
                    return Ok(new { exists = true });
                }
                q.Saturday = subjectid;
            }

            _context.Entry(q).State = EntityState.Modified;
            _context.SaveChangesAsync();

            return Ok(new { result = true });
        }

        // PUT: api/TimeTables/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTimeTable([FromRoute] int id, [FromBody] MTimeTable timeTable)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != timeTable.Id)
            {
                return BadRequest();
            }

            _context.Entry(timeTable).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TimeTableExists(id))
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

        // POST: api/TimeTables
        [HttpPost]
        public async Task<IActionResult> PostTimeTable([FromBody] MTimeTable timeTable)
        {
            timeTable.Batchid = GetBatchId();
            if (CheckExists(timeTable))
            {
                return Ok(new { exists = true });
            }


            _context.MTimeTable.Add(timeTable);
            await _context.SaveChangesAsync();

            return Ok(new { exists = false });
        }

        private bool CheckExists(MTimeTable timeTable)
        {
            bool exists = false;

            exists = _context.MTimeTable.Any(e => e.Fromtime == timeTable.Fromtime
                                            && e.Totime == timeTable.Totime
                                            && e.Batchid == timeTable.Batchid
                                            && e.Classid == timeTable.Classid);
            if (exists)
            {
                return exists;
            }
            else
            {
                exists = _context.MTimeTable.Any(e => e.Fromtime < timeTable.Fromtime
                                            && e.Totime > timeTable.Fromtime
                                            && e.Batchid == timeTable.Batchid
                                            && e.Classid == timeTable.Classid);
                if (exists)
                {
                    return exists;
                }
                else
                {
                    exists = _context.MTimeTable.Any(e => e.Fromtime < timeTable.Totime
                                                && e.Totime > timeTable.Totime
                                                && e.Batchid == timeTable.Batchid
                                                && e.Classid == timeTable.Classid);
                    return exists;
                }
            }

        }

        // DELETE: api/TimeTables/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTimeTable([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var timeTable = await _context.MTimeTable.FindAsync(id);
            if (timeTable == null)
            {
                return NotFound();
            }

            _context.MTimeTable.Remove(timeTable);
            await _context.SaveChangesAsync();

            return Ok(timeTable);
        }

        private bool TimeTableExists(int id)
        {
            return _context.MTimeTable.Any(e => e.Id == id);
        }

        // GET: api/TimeTables/5
        [HttpGet, Route("gettimespans/{slot}")]
        public IActionResult GettimeSpans(int slot)
        {
            TimeSpan timeSpan = new TimeSpan(0, 0, 0);
            List<TimeSpan> timeSpans = new List<TimeSpan>();
            //timeSpans.Add(timeSpan);

            while (timeSpan.TotalDays < 1)
            {                
                timeSpans.Add(timeSpan);
                timeSpan = timeSpan.Add(new TimeSpan(0, slot, 0));
            }

            timeSpan = timeSpan.Add(new TimeSpan(0, -slot, 0));
            timeSpan = timeSpan.Add(new TimeSpan(0, ((1440 - Convert.ToInt32(timeSpan.TotalMinutes)) - 1), 0));
            timeSpans.Add(timeSpan);

            return Ok(timeSpans);
        }


        // GET: api/TimeTables/5
        [HttpGet, Route("getteacherstimetable/{staffid}")]
        public IActionResult getteacherstimetable(int staffid)
        {
            int batchid = GetBatchId();
            var list = (from p in _context.ClassSubjects where p.Staffid == staffid select p.Subjectid).ToArray();
            List<int?> subjectids = list.ToList();
            subjectids.Add(0);
            try
            {


                var q = (from p in _context.MTimeTable
                         where p.Batchid == batchid &&
                            subjectids.Contains(p.Sunday) ||
                            subjectids.Contains(p.Monday) ||
                            subjectids.Contains(p.Tuesday) ||
                            subjectids.Contains(p.Wednesday) ||
                            subjectids.Contains(p.Thursday) ||
                            subjectids.Contains(p.Friday) ||
                            subjectids.Contains(p.Saturday)
                         group p by new { p.Fromtime, p.Totime } into g
                         select new
                         {
                             Sunday = (from t in g
                                       where (from cs in _context.ClassSubjects
                                              where cs.Subjectid == t.Sunday && cs.Classid == t.Classid
                                              select cs.Staffid).FirstOrDefault() == staffid
                                       select new
                                       {
                                           Id = t.Id,
                                           Subjectid = t.Sunday,
                                           Classid = t.Classid,
                                           Subject = (from s in _context.MSubjects where s.Id == t.Sunday select s).FirstOrDefault(),
                                           Class = t.Class,
                                           Batchid = t.Batchid,
                                           Fromtime = t.Fromtime,
                                           Totime = t.Totime
                                       }).FirstOrDefault(),
                             Monday = (from t in g
                                       where (from cs in _context.ClassSubjects
                                              where cs.Subjectid == t.Monday && cs.Classid == t.Classid
                                              select cs.Staffid).FirstOrDefault() == staffid
                                       select new
                                       {
                                           Id = t.Id,
                                           Subjectid = t.Monday,
                                           Classid = t.Classid,
                                           Subject = (from s in _context.MSubjects where s.Id == t.Monday select s).FirstOrDefault(),
                                           Class = t.Class,
                                           Batchid = t.Batchid,
                                           Fromtime = t.Fromtime,
                                           Totime = t.Totime
                                       }).FirstOrDefault(),
                             Tuesday = (from t in g
                                        where (from cs in _context.ClassSubjects
                                               where cs.Subjectid == t.Tuesday && cs.Classid == t.Classid
                                               select cs.Staffid).FirstOrDefault() == staffid
                                        select new
                                        {
                                            Id = t.Id,
                                            Subjectid = t.Tuesday,
                                            Classid = t.Classid,
                                            Subject = (from s in _context.MSubjects where s.Id == t.Tuesday select s).FirstOrDefault(),
                                            Class = t.Class,
                                            Batchid = t.Batchid,
                                            Fromtime = t.Fromtime,
                                            Totime = t.Totime
                                        }).FirstOrDefault(),
                             Wednesday = (from t in g
                                          where (from cs in _context.ClassSubjects
                                                 where cs.Subjectid == t.Wednesday && cs.Classid == t.Classid
                                                 select cs.Staffid).FirstOrDefault() == staffid
                                          select new
                                          {
                                              Id = t.Id,
                                              Subjectid = t.Wednesday,
                                              Classid = t.Classid,
                                              Subject = (from s in _context.MSubjects where s.Id == t.Wednesday select s).FirstOrDefault(),
                                              Class = t.Class,
                                              Batchid = t.Batchid,
                                              Fromtime = t.Fromtime,
                                              Totime = t.Totime
                                          }).FirstOrDefault(),
                             Thursday = (from t in g
                                         where (from cs in _context.ClassSubjects
                                                where cs.Subjectid == t.Thursday && cs.Classid == t.Classid
                                                select cs.Staffid).FirstOrDefault() == staffid
                                         select new
                                         {
                                             Id = t.Id,
                                             Subjectid = t.Thursday,
                                             Classid = t.Classid,
                                             Subject = (from s in _context.MSubjects where s.Id == t.Thursday select s).FirstOrDefault(),
                                             Class = t.Class,
                                             Batchid = t.Batchid,
                                             Fromtime = t.Fromtime,
                                             Totime = t.Totime
                                         }).FirstOrDefault(),
                             Friday = (from t in g
                                       where (from cs in _context.ClassSubjects
                                              where cs.Subjectid == t.Friday && cs.Classid == t.Classid
                                              select cs.Staffid).FirstOrDefault() == staffid
                                       select new
                                       {
                                           Id = t.Id,
                                           Subjectid = t.Friday,
                                           Classid = t.Classid,
                                           Subject = (from s in _context.MSubjects where s.Id == t.Friday select s).FirstOrDefault(),
                                           Class = t.Class,
                                           Batchid = t.Batchid,
                                           Fromtime = t.Fromtime,
                                           Totime = t.Totime
                                       }).FirstOrDefault(),
                             Saturday = (from t in g
                                         where (from cs in _context.ClassSubjects
                                                where cs.Subjectid == t.Saturday && cs.Classid == t.Classid
                                                select cs.Staffid).FirstOrDefault() == staffid
                                         select new
                                         {
                                             Id = t.Id,
                                             Subjectid = t.Saturday,
                                             Classid = t.Classid,
                                             Subject = (from s in _context.MSubjects where s.Id == t.Saturday select s).FirstOrDefault(),
                                             Class = t.Class,
                                             Batchid = t.Batchid,
                                             Fromtime = t.Fromtime,
                                             Totime = t.Totime
                                         }).FirstOrDefault(),
                             Fromtime = g.Key.Fromtime,
                             Totime = g.Key.Totime
                         }).ToList();

                return Ok(q);
            }
            catch (Exception)
            {
                return Ok();
                //throw;
            }
        }

        // GET: api/TimeTables/5
        [HttpGet, Route("getteacherstimetablereport/{staffid}/{batchid}")]
        public IActionResult getteacherstimetableReport(int staffid, int batchid)
        {            
            var list = (from p in _context.ClassSubjects where p.Staffid == staffid select p.Subjectid).ToArray();
            List<int?> subjectids = list.ToList();
            subjectids.Add(0);
            try
            {


                var q = (from p in _context.MTimeTable
                         where p.Batchid == batchid &&
                            subjectids.Contains(p.Sunday) ||
                            subjectids.Contains(p.Monday) ||
                            subjectids.Contains(p.Tuesday) ||
                            subjectids.Contains(p.Wednesday) ||
                            subjectids.Contains(p.Thursday) ||
                            subjectids.Contains(p.Friday) ||
                            subjectids.Contains(p.Saturday)
                         group p by new { p.Fromtime, p.Totime } into g
                         select new
                         {
                             Sunday = (from t in g
                                       where (from cs in _context.ClassSubjects
                                              where cs.Subjectid == t.Sunday && cs.Classid == t.Classid
                                              select cs.Staffid).FirstOrDefault() == staffid
                                       select new
                                       {
                                           Id = t.Id,
                                           Subjectid = t.Sunday,
                                           Classid = t.Classid,
                                           Subject = (from s in _context.MSubjects where s.Id == t.Sunday select s).FirstOrDefault(),
                                           Class = t.Class,
                                           Batchid = t.Batchid,
                                           Fromtime = t.Fromtime,
                                           Totime = t.Totime
                                       }).FirstOrDefault(),
                             Monday = (from t in g
                                       where (from cs in _context.ClassSubjects
                                              where cs.Subjectid == t.Monday && cs.Classid == t.Classid
                                              select cs.Staffid).FirstOrDefault() == staffid
                                       select new
                                       {
                                           Id = t.Id,
                                           Subjectid = t.Monday,
                                           Classid = t.Classid,
                                           Subject = (from s in _context.MSubjects where s.Id == t.Monday select s).FirstOrDefault(),
                                           Class = t.Class,
                                           Batchid = t.Batchid,
                                           Fromtime = t.Fromtime,
                                           Totime = t.Totime
                                       }).FirstOrDefault(),
                             Tuesday = (from t in g
                                        where (from cs in _context.ClassSubjects
                                               where cs.Subjectid == t.Tuesday && cs.Classid == t.Classid
                                               select cs.Staffid).FirstOrDefault() == staffid
                                        select new
                                        {
                                            Id = t.Id,
                                            Subjectid = t.Tuesday,
                                            Classid = t.Classid,
                                            Subject = (from s in _context.MSubjects where s.Id == t.Tuesday select s).FirstOrDefault(),
                                            Class = t.Class,
                                            Batchid = t.Batchid,
                                            Fromtime = t.Fromtime,
                                            Totime = t.Totime
                                        }).FirstOrDefault(),
                             Wednesday = (from t in g
                                          where (from cs in _context.ClassSubjects
                                                 where cs.Subjectid == t.Wednesday && cs.Classid == t.Classid
                                                 select cs.Staffid).FirstOrDefault() == staffid
                                          select new
                                          {
                                              Id = t.Id,
                                              Subjectid = t.Wednesday,
                                              Classid = t.Classid,
                                              Subject = (from s in _context.MSubjects where s.Id == t.Wednesday select s).FirstOrDefault(),
                                              Class = t.Class,
                                              Batchid = t.Batchid,
                                              Fromtime = t.Fromtime,
                                              Totime = t.Totime
                                          }).FirstOrDefault(),
                             Thursday = (from t in g
                                         where (from cs in _context.ClassSubjects
                                                where cs.Subjectid == t.Thursday && cs.Classid == t.Classid
                                                select cs.Staffid).FirstOrDefault() == staffid
                                         select new
                                         {
                                             Id = t.Id,
                                             Subjectid = t.Thursday,
                                             Classid = t.Classid,
                                             Subject = (from s in _context.MSubjects where s.Id == t.Thursday select s).FirstOrDefault(),
                                             Class = t.Class,
                                             Batchid = t.Batchid,
                                             Fromtime = t.Fromtime,
                                             Totime = t.Totime
                                         }).FirstOrDefault(),
                             Friday = (from t in g
                                       where (from cs in _context.ClassSubjects
                                              where cs.Subjectid == t.Friday && cs.Classid == t.Classid
                                              select cs.Staffid).FirstOrDefault() == staffid
                                       select new
                                       {
                                           Id = t.Id,
                                           Subjectid = t.Friday,
                                           Classid = t.Classid,
                                           Subject = (from s in _context.MSubjects where s.Id == t.Friday select s).FirstOrDefault(),
                                           Class = t.Class,
                                           Batchid = t.Batchid,
                                           Fromtime = t.Fromtime,
                                           Totime = t.Totime
                                       }).FirstOrDefault(),
                             Saturday = (from t in g
                                         where (from cs in _context.ClassSubjects
                                                where cs.Subjectid == t.Saturday && cs.Classid == t.Classid
                                                select cs.Staffid).FirstOrDefault() == staffid
                                         select new
                                         {
                                             Id = t.Id,
                                             Subjectid = t.Saturday,
                                             Classid = t.Classid,
                                             Subject = (from s in _context.MSubjects where s.Id == t.Saturday select s).FirstOrDefault(),
                                             Class = t.Class,
                                             Batchid = t.Batchid,
                                             Fromtime = t.Fromtime,
                                             Totime = t.Totime
                                         }).FirstOrDefault(),
                             Fromtime = g.Key.Fromtime,
                             Totime = g.Key.Totime
                         }).ToList();

                return Ok(q);
            }
            catch (Exception)
            {
                return Ok();
                //throw;
            }
        }
    }
}