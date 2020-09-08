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
    public class ClassSubjectsController : BaseController, IDisposable
    {
        private readonly ngSchoolContext _context;

        public ClassSubjectsController(ngSchoolContext context)
        {
            _context = context;
        }

        // GET: api/ClassSubjects
        [HttpGet]
        public IEnumerable<ClassSubjects> GetTClassSubjects()
        {
            return _context.ClassSubjects;
        }

        // GET: api/Classes/5
        [HttpGet(), Route("getbyclassid/{id}")]
        public async Task<IActionResult> GetTClassSubjectsByClassId([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var mClassSubjects = await (from cs in _context.ClassSubjects
                                        where cs.Classid == id
                                        select new ClassSubjects()
                                        {
                                            Subject = _context.MSubjects.Where(s => s.Id == cs.Subjectid).FirstOrDefault(),
                                            Classid = cs.Classid,
                                            Datecreated = cs.Datecreated,
                                            Userid = cs.Userid,
                                            Id = cs.Id,
                                            Subjectid = cs.Subjectid,
                                            Staff = cs.Staff,
                                            Staffid = cs.Staffid

                                        }).ToListAsync();
            if (mClassSubjects == null)
            {
                return NotFound();
            }

            return Ok(mClassSubjects);
        }

        // GET: api/Classes/5
        [HttpGet, Route("assignteacher/{id}/{staffid}")]
        public async Task<IActionResult> GetTClassSubjectsByClassId([FromRoute] int id, int staffid)
        {
            var mClassSubjects = await (from cs in _context.ClassSubjects
                                        where cs.Id == id
                                        select cs).FirstOrDefaultAsync();
            if (mClassSubjects == null)
            {
                return NotFound();
            }

            mClassSubjects.Staffid = staffid;
            _context.Entry(mClassSubjects).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(new { result = true });
        }

        // GET: api/ClassSubjects/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTClassSubjects([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ClassSubjects = await _context.ClassSubjects.FindAsync(id);

            if (ClassSubjects == null)
            {
                return NotFound();
            }

            return Ok(ClassSubjects);
        }

        // PUT: api/ClassSubjects/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTClassSubjects([FromRoute] int id, [FromBody] ClassSubjects ClassSubjects)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != ClassSubjects.Id)
            {
                return BadRequest();
            }

            _context.Entry(ClassSubjects).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TClassSubjectsExists(id))
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

        // POST: api/ClassSubjects
        [HttpPost]
        public async Task<IActionResult> PostTClassSubjects([FromBody] ClassSubjects ClassSubjects)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (TClassSubjectsNameExists(ClassSubjects.Classid, ClassSubjects.Subjectid))
            {
                return Ok(new { exists = true });
            }

            ClassSubjects.Userid = this.GetUserId();
            ClassSubjects.Datecreated = GetTimeZoneDate(DateTime.UtcNow);

            _context.ClassSubjects.Add(ClassSubjects);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTClassSubjects", new { id = ClassSubjects.Id }, ClassSubjects);
        }

        // POST: api/ClassSubjects
        [HttpGet, Route("addmultiple/{classid}/{subjectIds}")]
        public async Task<IActionResult> AddClassSubjects([FromRoute] int classid, string subjectIds)
        {
            List<int?> subIds = new List<int?>(Array.ConvertAll(subjectIds.Split(','),
                            new Converter<string, int?>((s) => { return Convert.ToInt32(s); })));

            foreach (int subjectid in subIds)
            {
                ClassSubjects ClassSubjects = new ClassSubjects();
                ClassSubjects.Classid = classid;
                ClassSubjects.Subjectid = subjectid;
                ClassSubjects.Userid = this.GetUserId();
                ClassSubjects.Datecreated = GetTimeZoneDate(DateTime.UtcNow);

                if (!TClassSubjectsNameExists(ClassSubjects.Classid, ClassSubjects.Subjectid))
                {
                    _context.ClassSubjects.Add(ClassSubjects);
                    //return Ok(new { exists = true });
                }
                ClassSubjects = null;
            }

            await _context.SaveChangesAsync();

            return Ok(new { result = true });
        }
        // DELETE: api/ClassSubjects/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTClassSubjects([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ClassSubjects = await _context.ClassSubjects.FindAsync(id);
            if (ClassSubjects == null)
            {
                return NotFound();
            }

            _context.ClassSubjects.Remove(ClassSubjects);
            await _context.SaveChangesAsync();

            return Ok(ClassSubjects);
        }

        private bool TClassSubjectsExists(int id)
        {
            return _context.ClassSubjects.Any(e => e.Id == id);
        }

        private bool TClassSubjectsNameExists(int? classid, int? subjectid)
        {
            return _context.ClassSubjects.Any(e => e.Classid == classid && e.Subjectid == subjectid);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}