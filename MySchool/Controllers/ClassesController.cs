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
    public class ClassesController : BaseController, IDisposable
    {
        private readonly ngSchoolContext _context;

        public ClassesController(ngSchoolContext context)
        {
            _context = context;
        }

        // GET: api/Classes
        [HttpGet]
        public IEnumerable<MClasses> GetMClasses()
        {
            return _context.MClasses;
        }

        [HttpGet, Route("getclassesenabled")]
        public IEnumerable<MClasses> GetMClassesEnabled()
        {
            return _context.MClasses.Where(e => e.Disabled == false);
        }

        // GET: api/Classes
        [HttpGet, Route("getclassesbyyear/{batchid}")]
        public IEnumerable<MClasses> GetMClassesAll(int batchid)
        {
            batchid = this.GetBatchId();
            return (from p in _context.MClasses
                    select new MClasses()
                    {
                        Capacity = p.Capacity,
                        Datecreated = p.Datecreated,
                        Datemodified = p.Datemodified,
                        Disabled = p.Disabled,
                        Id = p.Id,
                        Standard = p.Standard,
                        TStudentAdmission = _context.TStudentAdmission.Where(t => t.Classid == p.Id && t.Batchid == batchid).ToList(),
                        ClassTeacher = (from ct in _context.ClassTeacher
                                         where ct.Classid == p.Id && ct.Isactive == true
                                         select new ClassTeacher()
                                         {
                                             Staff = _context.MStaff.Where(s => s.Id == ct.Staffid).FirstOrDefault()
                                         }).ToList(),
                        ClassFees = _context.ClassFees.Where(t => t.Classid == p.Id).ToList(),
                        Userid = p.Userid
                    });
        }

        // GET: api/Classes/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMClasses([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var mClasses = await _context.MClasses.FindAsync(id);

            if (mClasses == null)
            {
                return NotFound();
            }

            return Ok(mClasses);
        }

        // GET: api/Classes/5
        [HttpGet(), Route("getclassdetails/{id}/{year}")]
        public async Task<IActionResult> GetMClassDetails([FromRoute] int id, [FromRoute] int year)
        {
            year = this.GetBatchId();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //var mClasses = await _context.MClasses.FindAsync(id);
            var mClasses = await (from p in _context.MClasses
                                  where p.Id == id
                                  select new MClasses()
                                  {
                                      Capacity = p.Capacity,
                                      Datecreated = p.Datecreated,
                                      Datemodified = p.Datemodified,
                                      Disabled = p.Disabled,
                                      Id = p.Id,
                                      Standard = p.Standard,
                                      ClassTeacher = (from ct in _context.ClassTeacher
                                                       where ct.Classid == p.Id
                                                       select new ClassTeacher()
                                                       {
                                                           Isactive = ct.Isactive,
                                                           Staff = _context.MStaff.Where(s => s.Id == ct.Staffid).FirstOrDefault()
                                                       }).ToList(),
                                      ClassFees = _context.ClassFees.Where(t => t.Classid == p.Id).ToList(),
                                      ClassSubjects = (from cs in _context.ClassSubjects
                                                        where cs.Classid == p.Id
                                                        select new ClassSubjects()
                                                        {
                                                            Subject = _context.MSubjects.Where(s => s.Id == cs.Subjectid).FirstOrDefault()
                                                        }).ToList(),
                                      Userid = p.Userid
                                  }).FirstOrDefaultAsync();
            if (mClasses == null)
            {
                return NotFound();
            }

            return Ok(mClasses);
        }

        // PUT: api/Classes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMClasses([FromRoute] int id, [FromBody] MClasses mClasses)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != mClasses.Id)
            {
                return BadRequest();
            }
            if (MClassesNameExists(mClasses.Id, mClasses.Standard))
            {
                return Ok(new { exists = true });
            }

            if (id == 0)
            {
                mClasses.Userid = this.GetUserId();
                mClasses.Disabled = false;
                mClasses.Datecreated = GetTimeZoneDate(DateTime.UtcNow);
                _context.MClasses.Add(mClasses);
            }
            else
            {
                mClasses.Datemodified = GetTimeZoneDate(DateTime.UtcNow);
                _context.Entry(mClasses).State = EntityState.Modified;
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MClassesExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(mClasses);
        }

        // POST: api/Classes
        [HttpPost]
        public async Task<IActionResult> PostMClasses([FromBody] MClasses mClasses)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.MClasses.Add(mClasses);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMClasses", new { id = mClasses.Id }, mClasses);
        }

        // DELETE: api/Classes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMClasses([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var mClasses = await _context.MClasses.FindAsync(id);
            if (mClasses == null)
            {
                return NotFound();
            }

            _context.MClasses.Remove(mClasses);
            await _context.SaveChangesAsync();

            return Ok(mClasses);
        }

        private bool MClassesExists(int id)
        {
            return _context.MClasses.Any(e => e.Id == id);
        }

        private bool MClassesNameExists(int id, string name)
        {
            return _context.MClasses.Any(e => e.Id != id && e.Standard == name);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}