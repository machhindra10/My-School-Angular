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
    public class ClassTeachersController : BaseController, IDisposable
    {
        private readonly ngSchoolContext _context;

        public ClassTeachersController(ngSchoolContext context)
        {
            _context = context;
        }
        // GET: api/ClassTeachers
        [HttpGet]
        public IEnumerable<ClassTeacher> GetTClassTeacher()
        {
            return _context.ClassTeacher;
        }

        // GET: api/Classes/5
        [HttpGet(), Route("getbyclassid/{id}/{year}")]
        public async Task<IActionResult> GetTClassTeacherByClassId([FromRoute] int id, [FromRoute] int year)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tClassTeacher = await (from ct in _context.ClassTeacher
                                       where ct.Classid == id orderby ct.From descending
                                       select new ClassTeacher()
                                       {
                                           Staff = _context.MStaff.Where(s => s.Id == ct.Staffid).FirstOrDefault(),
                                           Classid = ct.Classid,
                                           From = ct.From,
                                           To = ct.To,
                                           Userid = ct.Userid,
                                           Id = ct.Id,
                                           Staffid = ct.Staffid,
                                           
                                           Isactive = ct.Isactive
                                       }).ToListAsync();
            if (tClassTeacher == null)
            {
                return NotFound();
            }

            return Ok(tClassTeacher);
        }

        // GET: api/ClassTeachers/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTClassTeacher([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ClassTeacher = await _context.ClassTeacher.FindAsync(id);

            if (ClassTeacher == null)
            {
                return NotFound();
            }

            return Ok(ClassTeacher);
        }

        // PUT: api/ClassTeachers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTClassTeacher([FromRoute] int id, [FromBody] ClassTeacher ClassTeacher)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != ClassTeacher.Id)
            {
                return BadRequest();
            }

            _context.Entry(ClassTeacher).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TClassTeacherExists(id))
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

        // POST: api/ClassTeachers
        [HttpPost]
        public async Task<IActionResult> PostTClassTeacher([FromBody] ClassTeacher tClassTeacher)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var q = _context.ClassTeacher.Where(e => e.Classid == tClassTeacher.Classid &&
            e.Isactive == true).FirstOrDefault();
            if (q != null)
            {
                q.Isactive = false;
                q.To = GetTimeZoneDate(DateTime.UtcNow);

                _context.Entry(q).State = EntityState.Modified;
            }

            tClassTeacher.Userid = this.GetUserId();
            tClassTeacher.From = GetTimeZoneDate(DateTime.UtcNow);
            tClassTeacher.Isactive = true;
            

            _context.ClassTeacher.Add(tClassTeacher);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTClassTeacher", new { id = tClassTeacher.Id }, tClassTeacher);
        }

        // DELETE: api/ClassTeachers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTClassTeacher([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ClassTeacher = await _context.ClassTeacher.FindAsync(id);
            if (ClassTeacher == null)
            {
                return NotFound();
            }

            _context.ClassTeacher.Remove(ClassTeacher);
            await _context.SaveChangesAsync();

            return Ok(ClassTeacher);
        }

        private bool TClassTeacherExists(int id)
        {
            return _context.ClassTeacher.Any(e => e.Id == id);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}