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
    public class StudentGuardiansController : BaseController, IDisposable
    {
        private readonly ngSchoolContext _context;

        public StudentGuardiansController(ngSchoolContext context)
        {
            _context = context;
        }

        // GET: api/StudentGuardians
        [HttpGet]
        public IEnumerable<StudentGuardian> GetStudentGuardian()
        {
            return _context.StudentGuardian;
        }

        // GET: api/StudentGuardians
        [HttpGet, Route("getenabled")]
        public IEnumerable<StudentGuardian> GetStudentGuardianEnabled()
        {
            return _context.StudentGuardian.Where(p => p.Disabled == false);
        }

        // GET: api/StudentGuardians/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetStudentGuardian([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var studentGuardian = await _context.StudentGuardian.FindAsync(id);

            if (studentGuardian == null)
            {
                return NotFound();
            }

            return Ok(studentGuardian);
        }

        // PUT: api/StudentGuardians/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudentGuardian([FromRoute] long id, [FromBody] StudentGuardian studentGuardian)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != studentGuardian.Id)
            {
                return BadRequest();
            }

            if (StudentGuardianNameExists(studentGuardian.Id, studentGuardian.Name))
            {
                return Ok(new { result = false, nameExists = true });
            }
            else if (StudentGuardianMobileExists(studentGuardian.Id, studentGuardian.Mobile))
            {
                return Ok(new { result = false, nameExists = false, mobileExists = true });
            }

            _context.Entry(studentGuardian).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentGuardianExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(new { result = true, nameExists = false, mobileExists = false, id = studentGuardian.Id });

        }

        // POST: api/StudentGuardians
        [HttpPost]
        public async Task<IActionResult> PostStudentGuardian([FromBody] StudentGuardian studentGuardian)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if(StudentGuardianNameExists(studentGuardian.Id, studentGuardian.Name))
            {
                return Ok(new { result = false, nameExists = true });
            }
            else if (StudentGuardianMobileExists(studentGuardian.Id, studentGuardian.Mobile))
            {
                return Ok(new { result = false, nameExists = false, mobileExists = true });
            }
            studentGuardian.Userid = GetUserId();
            studentGuardian.Datecreated = GetTimeZoneDate(DateTime.UtcNow);
            studentGuardian.Disabled = false;
            studentGuardian.Password = Encryption.EncryptString(studentGuardian.Mobile.ToString());

            _context.StudentGuardian.Add(studentGuardian);
            await _context.SaveChangesAsync();

            return Ok(new { result = true, nameExists = false, mobileExists = false, id = studentGuardian.Id });
        }

        // DELETE: api/StudentGuardians/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudentGuardian([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var studentGuardian = await _context.StudentGuardian.FindAsync(id);
            if (studentGuardian == null)
            {
                return NotFound();
            }

            _context.StudentGuardian.Remove(studentGuardian);
            await _context.SaveChangesAsync();

            return Ok(studentGuardian);
        }

        private bool StudentGuardianExists(long id)
        {
            return _context.StudentGuardian.Any(e => e.Id == id);
        }

        private bool StudentGuardianNameExists(long id, string name)
        {
            return _context.StudentGuardian.Any(e => e.Id != id && e.Name == name);
        }

        private bool StudentGuardianMobileExists(long id, long mobile)
        {
            return _context.StudentGuardian.Any(e => e.Id != id && e.Mobile == mobile);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}