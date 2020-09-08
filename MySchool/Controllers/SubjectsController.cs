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
    public class SubjectsController : BaseController
    {
        private readonly ngSchoolContext _context;

        public SubjectsController(ngSchoolContext context)
        {
            _context = context;
        }

        // GET: api/Subjects
        [HttpGet]
        public IEnumerable<MSubjects> GetMSubjects()
        {
            return (from p in _context.MSubjects
                    select new MSubjects()
                    {
                        Code = p.Code,
                        Datecreated = p.Datecreated,
                        Disabled = p.Disabled,
                        Id = p.Id,
                        Staff = _context.MStaff.Where(t => t.Id == p.Staffid).FirstOrDefault(),
                        Staffid = p.Staffid,
                        Subject = p.Subject,
                        Userid = p.Userid,
                    });
        }

        // GET: api/staffs/getstaffsenabled
        [HttpGet, Route("getsubjectsenabled")]
        public IEnumerable<MSubjects> GetMSubjectsEnabled()
        {
            return (from p in _context.MSubjects
                    where p.Disabled == false
                    select new MSubjects()
                    {
                        Code = p.Code,
                        Datecreated = p.Datecreated,
                        Disabled = p.Disabled,
                        Id = p.Id,
                        Staff = _context.MStaff.Where(t => t.Id == p.Staffid).FirstOrDefault(),
                        Staffid = p.Staffid,
                        Subject = p.Subject,
                        Userid = p.Userid
                    });
        }

        // GET: api/staffs/getstaffsenabled
        [HttpGet, Route("getclasssubjectsonly/{classid}")]
        public IEnumerable<MSubjects> GetClassSubjectsOnly(int classid)
        {
            var classes = _context.ClassSubjects.Where(p => p.Classid == classid).Select(p => p.Subjectid).ToArray();

            return (from p in _context.MSubjects
                    where classes.Contains(p.Id)
                    select new MSubjects()
                    {
                        Code = p.Code,
                        Id = p.Id, 
                        Subject = p.Subject
                    }).ToList();
        }

        // GET: api/Subjects/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMSubjects([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var mSubjects = await _context.MSubjects.FindAsync(id);

            if (mSubjects == null)
            {
                return NotFound();
            }

            return Ok(mSubjects);
        }

        // PUT: api/Subjects/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMSubjects([FromRoute] int id, [FromBody] MSubjects mSubjects)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != mSubjects.Id)
            {
                return BadRequest();
            }
            if (MSubjectNameExists(mSubjects.Id, mSubjects.Subject))
            {
                return Ok(new { exists = true });
            }
            mSubjects.Datecreated = DateTime.UtcNow;
            mSubjects.Userid = this.GetUserId();
            if (id == 0)
            {
                mSubjects.Disabled = false;
                _context.MSubjects.Add(mSubjects);
            }
            else
            {
                _context.Entry(mSubjects).State = EntityState.Modified;
            }


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MSubjectsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return Ok(mSubjects);
            //return NoContent();
        }

        // POST: api/Subjects
        [HttpPost]
        public async Task<IActionResult> PostMSubjects([FromBody] MSubjects mSubjects)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.MSubjects.Add(mSubjects);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMSubjects", new { id = mSubjects.Id }, mSubjects);
        }

        // DELETE: api/Subjects/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMSubjects([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var mSubjects = await _context.MSubjects.FindAsync(id);
            if (mSubjects == null)
            {
                return NotFound();
            }

            _context.MSubjects.Remove(mSubjects);
            await _context.SaveChangesAsync();

            return Ok(mSubjects);
        }

        private bool MSubjectsExists(int id)
        {
            return _context.MSubjects.Any(e => e.Id == id);
        }
        private bool MSubjectNameExists(int id, string name)
        {
            return _context.MSubjects.Any(e => e.Id != id && e.Subject == name);
        }
    }
}