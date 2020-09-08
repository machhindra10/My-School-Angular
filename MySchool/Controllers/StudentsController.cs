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
    public class StudentsController : BaseController, IDisposable
    {
        private readonly ngSchoolContext _context;

        public StudentsController(ngSchoolContext context)
        {
            _context = context;
        }

        // GET: api/Students
        [HttpGet]
        public IEnumerable<Student> GetStudent()
        {
            return (from p in _context.Student
                    select new Student()
                    {
                        Id = p.Id,
                        Aadharno = p.Aadharno,
                        Address = p.Address,
                        Cast = _context.MCast.Where(e => e.Id == p.Castid).FirstOrDefault(),
                        Castid = p.Castid,
                        Datecreated = p.Datecreated,
                        Datemodified = p.Datemodified,
                        Disabled = p.Disabled,
                        Dob = p.Dob,
                        Email = p.Email,
                        Fname = p.Fname,
                        Guardianid = p.Guardianid,
                        GuardianRelation = p.GuardianRelation,
                        Lname = p.Lname,
                        Mname = p.Mname,
                        Mobile = p.Mobile,
                        Phone = p.Phone,
                        Prnno = p.Prnno,
                        Userid = p.Userid
                    }).ToList();
        }

        // GET: api/Students
        [HttpGet(), Route("getstudentforsearch")]
        public IEnumerable<Student> GetStudentForSearch()
        {
            return (from p in _context.Student
                    where p.Disabled == false
                    select new Student()
                    {
                        Id = p.Id,
                        Aadharno = p.Aadharno,
                        Fname = p.Fname,
                        Lname = p.Lname,
                        Mname = p.Mname,
                        Prnno = p.Prnno
                    }).ToList();
        }

        // GET: api/Students
        [HttpGet(), Route("getbyguardianid/{guardianid}")]
        public IEnumerable<Student> GetStudentByGuardianId(long guardianid)
        {
            int batchid = GetBatchId();
            return (from p in _context.Student
                    where p.Disabled == false && p.Guardianid == guardianid
                    select new Student()
                    {
                        Id = p.Id,
                        Aadharno = p.Aadharno,
                        Fname = p.Fname,
                        Lname = p.Lname,
                        Mname = p.Mname,
                        Prnno = p.Prnno,
                        TStudentAdmission = (from a in _context.TStudentAdmission
                                             where a.Studentid == p.Id && a.Batchid == batchid
                                             select new TStudentAdmission()
                                             {
                                                 Class = a.Class,
                                                 Classid = a.Classid,
                                                 Rollno = a.Rollno,
                                                 Datecreated = a.Datecreated,
                                                 Id = a.Id,
                                             }).ToList()
                    }).ToList();
        }

        // GET: api/Students/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetStudent([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var student = await (from p in _context.Student
                                 where p.Id == id
                                 select new Student()
                                 {
                                     Id = p.Id,
                                     Aadharno = p.Aadharno,
                                     Address = p.Address,
                                     Cast = _context.MCast.Where(e => e.Id == p.Castid).FirstOrDefault(),
                                     Castid = p.Castid,
                                     Datecreated = p.Datecreated,
                                     Datemodified = p.Datemodified,
                                     Disabled = p.Disabled,
                                     Dob = p.Dob,
                                     Email = p.Email,
                                     Fname = p.Fname,
                                     Guardianid = p.Guardianid,
                                     GuardianRelation = p.GuardianRelation,
                                     Lname = p.Lname,
                                     Mname = p.Mname,
                                     Mobile = p.Mobile,
                                     Phone = p.Phone,
                                     Prnno = p.Prnno,
                                     Userid = p.Userid,
                                     Gender = p.Gender

                                 }).FirstOrDefaultAsync();

            if (student == null)
            {
                return NotFound();
            }

            return Ok(student);
        }

        // GET: api/Students/5
        [HttpGet, Route("getdetails/{id}")]
        public async Task<IActionResult> GetStudentDetails([FromRoute] int id)
        {
            int batchid = GetBatchId();

            var student = await (from p in _context.Student
                                 where p.Id == id
                                 select new Student()
                                 {
                                     Id = p.Id,
                                     Aadharno = p.Aadharno,
                                     Address = p.Address,
                                     Cast = _context.MCast.Where(e => e.Id == p.Castid).FirstOrDefault(),
                                     Castid = p.Castid,
                                     Datecreated = p.Datecreated,
                                     Datemodified = p.Datemodified,
                                     Disabled = p.Disabled,
                                     Dob = p.Dob,
                                     Email = p.Email,
                                     Fname = p.Fname,
                                     Guardianid = p.Guardianid,
                                     GuardianRelation = p.GuardianRelation,
                                     Lname = p.Lname,
                                     Mname = p.Mname,
                                     Mobile = p.Mobile,
                                     Phone = p.Phone,
                                     Prnno = p.Prnno,
                                     Userid = p.Userid,
                                     Gender = p.Gender,
                                     TStudentAdmission = (from a in _context.TStudentAdmission
                                                          where a.Studentid == p.Id && a.Batchid == batchid
                                                          select new TStudentAdmission()
                                                          {
                                                              Class = a.Class,
                                                              Classid = a.Classid,
                                                              Rollno = a.Rollno,
                                                              Datecreated = a.Datecreated,
                                                              Id = a.Id,
                                                          }).ToList()
                                 }).FirstOrDefaultAsync();

            if (student == null)
            {
                return NotFound();
            }

            return Ok(student);
        }
        // GET: api/Students/5
        [HttpGet, Route("getbyclass/{id}/{batchid}")]
        public async Task<IActionResult> GetStudentsByClass([FromRoute] int id, int batchid)
        {
            if (batchid == 0)
            {
                batchid = GetBatchId();
            }
            var sids = await (from p in _context.TStudentAdmission where p.Classid == id && p.Batchid == batchid select p.Studentid).ToListAsync();

            var students = await (from p in _context.Student
                                  where sids.Contains(p.Id)
                                  select new Student()
                                  {
                                      Id = p.Id,
                                      Aadharno = p.Aadharno,
                                      Address = p.Address,
                                      Cast = _context.MCast.Where(e => e.Id == p.Castid).FirstOrDefault(),
                                      Castid = p.Castid,
                                      Datecreated = p.Datecreated,
                                      Datemodified = p.Datemodified,
                                      Disabled = p.Disabled,
                                      Dob = p.Dob,
                                      Email = p.Email,
                                      Fname = p.Fname,
                                      Guardianid = p.Guardianid,
                                      GuardianRelation = p.GuardianRelation,
                                      Lname = p.Lname,
                                      Mname = p.Mname,
                                      Mobile = p.Mobile,
                                      Phone = p.Phone,
                                      Prnno = p.Prnno,
                                      Userid = p.Userid,
                                      Gender = p.Gender
                                  }).ToListAsync();

            if (students == null)
            {
                return NotFound();
            }

            return Ok(students);
        }

        [HttpGet, Route("getstudentsforidentitycards/{id}/{batchid}")]
        public async Task<IActionResult> GetStudentsForIdentityCards([FromRoute] int id, int batchid)
        {
            if (batchid == 0)
            {
                batchid = GetBatchId();
            }
            var sids = await (from p in _context.TStudentAdmission where p.Classid == id && p.Batchid == batchid select p.Studentid).ToListAsync();

            var students = await (from p in _context.Student
                                  where sids.Contains(p.Id)
                                  select new 
                                  {
                                      Id = p.Id,
                                      Aadharno = p.Aadharno,
                                      Address = p.Address,
                                      Cast = _context.MCast.Where(e => e.Id == p.Castid).FirstOrDefault(),
                                      Castid = p.Castid,
                                      Datecreated = p.Datecreated,
                                      Datemodified = p.Datemodified,
                                      Disabled = p.Disabled,
                                      Dob = p.Dob,
                                      Email = p.Email,
                                      Fname = p.Fname,
                                      Guardianid = p.Guardianid,
                                      GuardianRelation = p.GuardianRelation,
                                      Lname = p.Lname,
                                      Mname = p.Mname,
                                      Mobile = p.Mobile,
                                      Phone = p.Phone,
                                      Prnno = p.Prnno,
                                      Userid = p.Userid,
                                      Gender = p.Gender,
                                      Photo = p.Photo,
                                      classname = (from sa in _context.TStudentAdmission
                                                           where sa.Classid == id &&
                                                          sa.Batchid == batchid && sa.Studentid == p.Id
                                                           select new
                                                           {
                                                              standard = sa.Class.Standard
                                                           }).FirstOrDefault(),
                                      guardian = (from g in _context.StudentGuardian 
                                               where g.Id == p.Guardianid
                                               select new
                                               {
                                                   gname = g.Name,
                                                   mobile = g.Mobile
                                               }).FirstOrDefault()
                                  }).ToListAsync();

            if (students == null)
            {
                return NotFound();
            }

            return Ok(students);
        }


        // GET: api/Students/5
        [HttpGet, Route("getbyclassid/{classid}/{batchid}")]
        public async Task<IActionResult> GetStudentsByClassAndBatchId([FromRoute] int classid, int batchid)
        {
            var sids = await (from p in _context.TStudentAdmission where p.Classid == classid && p.Batchid == batchid select p.Studentid).ToListAsync();

            var students = await (from p in _context.Student
                                  where sids.Contains(p.Id)
                                  select new
                                  {
                                      Id = p.Id,
                                      Name = p.Fname + " " + p.Mname + " " + p.Lname
                                  }).ToListAsync();

            if (students == null)
            {
                return NotFound();
            }

            return Ok(students);
        }


        // GET: api/Students/5
        [HttpGet(), Route("getstudentphoto/{id}")]
        public async Task<IActionResult> GetStudentPhoto([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var student = await (from p in _context.Student
                                 where p.Id == id
                                 select new Student()
                                 {
                                     Photo = p.Photo
                                 }).FirstOrDefaultAsync();

            if (student == null)
            {
                return NotFound();
            }

            return Ok(student);
        }

        // GET: api/Students/5
        [HttpGet, Route("enabledisable/{id}")]
        public async Task<IActionResult> EnabledDisabledStudent([FromRoute] int id)
        {
            var student = await (from p in _context.Student
                                 where p.Id == id
                                 select p).FirstOrDefaultAsync();

            if (student == null)
            {
                return NotFound();
            }
            student.Disabled = !student.Disabled;
            _context.Entry(student).State = EntityState.Modified;
            _context.SaveChanges();
            return Ok(student);
        }

        // GET: api/Students/5
        [HttpGet, Route("getbyage/{classid}/{batchid}/{fromyear}/{toyear}")]
        public async Task<IActionResult> GetByAge([FromRoute] int classid, [FromRoute] int batchid, [FromRoute] int fromyear, [FromRoute] int toyear)
        {
            DateTime dt = GetTimeZoneDate(DateTime.UtcNow);
            List<int?> sids;
            if (classid > 0)
            {
                sids = (from a in _context.TStudentAdmission where a.Batchid == batchid && a.Classid == classid select a.Studentid).ToList();
            }
            else
            {
                sids = (from a in _context.TStudentAdmission where a.Batchid == batchid select a.Studentid).ToList();
            }
            var q = await (from p in _context.Student
                           where sids.Contains(p.Id)
                           select new
                           {
                               id = p.Id,
                               fname = p.Fname + " " + p.Mname + " " + p.Lname,
                               prnno = p.Prnno,
                               age = dt.Year - p.Dob.Value.Year
                           }).Where(x => x.age >= fromyear && x.age <= toyear).OrderBy(x => x.age).ToArrayAsync();


            return Ok(q);
        }

        // PUT: api/Students/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudent([FromRoute] int id, [FromBody] Student student)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != student.Id)
            {
                return BadRequest();
            }


            student.Dob = GetTimeZoneDate(student.Dob);
            if (id == 0)
            {
                student.Prnno = GetPRNNumber();
                student.Userid = this.GetUserId();
                student.Datecreated = GetTimeZoneDate(DateTime.UtcNow);
                student.Disabled = false;
                _context.Student.Add(student);
            }
            else
            {
                student.Datemodified = GetTimeZoneDate(DateTime.UtcNow);
                _context.Entry(student).State = EntityState.Modified;
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(student);
        }

        // POST: api/Students
        [HttpPost]
        public async Task<IActionResult> PostStudent([FromBody] Student student)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Student.Add(student);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStudent", new { id = student.Id }, student);
        }

        // DELETE: api/Students/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var student = await _context.Student.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            _context.Student.Remove(student);
            await _context.SaveChangesAsync();

            return Ok(student);
        }

        // GET: api/Casts
        [HttpGet, Route("getgender")]
        public IEnumerable<MGender> GetGender()
        {
            return _context.MGender;
        }

        private bool StudentExists(int id)
        {
            return _context.Student.Any(e => e.Id == id);
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        private string GetPRNNumber()
        {
            return DateTime.Now.ToString("yyyyMMddHHmmssf");
        }
    }
}