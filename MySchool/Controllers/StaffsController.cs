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
    public class StaffsController : BaseController, IDisposable
    {
        private readonly ngSchoolContext _context;

        public StaffsController(ngSchoolContext context)
        {
            _context = context;
        }

        // GET: api/Staffs
        [HttpGet]
        public IEnumerable<MStaff> GetMStaff()
        {
            return (from p in _context.MStaff
                    select new MStaff()
                    {
                        Aadharno = p.Aadharno,
                        Desig = _context.MDesignation.Where(t => t.Id == p.Desigid).FirstOrDefault(),
                        Disabled = p.Disabled,
                        Email = p.Email,
                        Mobile = p.Mobile,
                        Id = p.Id,
                        Code = p.Code,
                        Dob = p.Dob,
                        Staffname = p.Staffname,
                        Address = p.Address,
                        Desigid = p.Desigid,
                        Datecreated = p.Datecreated,
                        Datemodified = p.Datemodified,
                        Phone = p.Phone,
                        Userid = p.Userid,
                        Doj = p.Doj,
                        Dol = p.Dol,
                        Associateuserid = p.Associateuserid
                    });
        }

        // GET: api/staffs/getstaffsenabled
        [HttpGet, Route("getstaffsenabled")]
        public IEnumerable<MStaff> GetMStaffEnabled()
        {
            return _context.MStaff.Where(e => e.Disabled == false);
        }

        // GET: api/staffs/getstaffsenabled
        [HttpGet, Route("getteachersstaffsenabled")]
        public IEnumerable<MStaff> GetTeachersOnlyMStaffEnabled()
        {
            return _context.MStaff.Where(e => e.Disabled == false
                                            && e.Desig.Designname.ToLower() == "teacher");
        }

        // GET: api/Staffs/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMStaff([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var mStaff = await _context.MStaff.FindAsync(id);

            if (mStaff == null)
            {
                return NotFound();
            }

            return Ok(mStaff);
        }

        // PUT: api/Staffs/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMStaff([FromRoute] int id, [FromBody] MStaff mStaff)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != mStaff.Id)
            {
                return BadRequest();
            }
            
            if (MStaffNameExists(mStaff.Id, mStaff.Staffname))
            {
                return Ok(new { exists = true });
            }
            if (MStaffCodeExists(mStaff.Id, mStaff.Code))
            {
                return Ok(new { codeexists = true });
            }
            if (MStaffEmailExists(mStaff.Id, mStaff.Email))
            {
                return Ok(new { emailexists = true });
            }
            mStaff.Dob = GetTimeZoneDate(mStaff.Dob);
            mStaff.Doj = GetTimeZoneDate(mStaff.Doj);
            if (mStaff.Dol != null)
            {
                mStaff.Dol = GetTimeZoneDate(mStaff.Dol);
            }
            if (id == 0)
            {
                mStaff.Datecreated = GetTimeZoneDate(DateTime.UtcNow);
                mStaff.Userid = this.GetUserId();
                mStaff.Disabled = false;
                mStaff.Associateuserid = 0;

                _context.MStaff.Add(mStaff);
            }
            else
            {
                mStaff.Datemodified = GetTimeZoneDate(DateTime.UtcNow);
                _context.Entry(mStaff).State = EntityState.Modified;
            }

            try
            {
                await _context.SaveChangesAsync();
                if (id == 0)
                {
                    StaffPayrollsController controller = new StaffPayrollsController(_context);
                    controller.SetStaffPayrollFirst(mStaff.Id);
                }
            }
            catch (DbUpdateConcurrencyException )
            {
                if (!MStaffExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            catch(Exception )
            {

            }

            return Ok(mStaff);
            //return NoContent();
        }

        [HttpGet, Route("updateassociateuserid/{id}/{ass_id}")]
        public async Task<IActionResult> PutUpdateAssociateUserIdMStaff([FromRoute] int id, int ass_id)
        {
            var mStaff = await _context.MStaff.FindAsync(id);
            if (mStaff == null)
            {
                return BadRequest();
            }

            mStaff.Associateuserid = ass_id;
            mStaff.Datemodified = GetTimeZoneDate(DateTime.UtcNow);
            _context.Entry(mStaff).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MStaffExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(mStaff);
            //return NoContent();
        }

        [HttpGet, Route("getbyassociateuserid/{id}")]
        public async Task<IActionResult> GetByAssociateUserId([FromRoute] int id)
        {
            var mStaff = await _context.MStaff.Where(e => e.Associateuserid == id).FirstOrDefaultAsync();


            return Ok(mStaff);
        }


        // POST: api/Staffs
        [HttpPost]
        public async Task<IActionResult> PostMStaff([FromBody] MStaff mStaff)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.MStaff.Add(mStaff);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMStaff", new { id = mStaff.Id }, mStaff);
        }

        // DELETE: api/Staffs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMStaff([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var mStaff = await _context.MStaff.FindAsync(id);
            if (mStaff == null)
            {
                return NotFound();
            }

            _context.MStaff.Remove(mStaff);
            await _context.SaveChangesAsync();

            return Ok(mStaff);
        }

        // GET: api/Students/5
        [HttpGet, Route("enabledisable/{id}")]
        public async Task<IActionResult> EnabledDisabled([FromRoute] int id)
        {
            var mStaff = await (from p in _context.MStaff
                                where p.Id == id
                                select p).FirstOrDefaultAsync();

            if (mStaff == null)
            {
                return NotFound();
            }
            mStaff.Disabled = !mStaff.Disabled;
            _context.Entry(mStaff).State = EntityState.Modified;
            _context.SaveChanges();
            return Ok(mStaff);
        }

        private bool MStaffExists(int id)
        {
            return _context.MStaff.Any(e => e.Id == id);
        }

        private bool MStaffNameExists(int id, string name)
        {
            return _context.MStaff.Any(e => e.Id != id && e.Staffname == name);
        }

        private bool MStaffEmailExists(int id, string email)
        {
            return _context.MStaff.Any(e => e.Id != id && e.Email == email);
        }

        private bool MStaffCodeExists(int id, string code)
        {
            return _context.MStaff.Any(e => e.Id != id && e.Code == code);
        }

        public void Dispose()
        {
            _context.Dispose();
        }


        // GET: api/Students
        [HttpGet(), Route("getstaffforsearch")]
        public async Task<IActionResult> GetStaffForSearch()
        {
            var result =await (from p in _context.MStaff
                    where p.Disabled == false
                    select new MStaff()
                    {
                        Id = p.Id,                        
                        Staffname = p.Staffname                       
                    }).ToListAsync();

            return Ok(result);
        }

        // GET: api/Students/5
        [HttpGet, Route("getdetails/{id}")]
        public async Task<IActionResult> GetStaffDetails(int id)
        {
            var result = await (from p in _context.MStaff
                                where p.Id == id
                                select new MStaff()
                                {
                                    Id = p.Id,
                                    Staffname = p.Staffname,
                                    Aadharno = p.Aadharno,
                                    Address = p.Address,
                                    Associateuserid = p.Associateuserid,
                                    Desig = p.Desig,
                                    Doj = p.Doj,
                                    Dol = p.Dol,
                                    Email = p.Email,
                                    Mobile = p.Mobile,
                                    Phone = p.Phone,
                                    Code = p.Code,
                                    Dob = p.Dob,
                                }).FirstOrDefaultAsync();

            return Ok(result);
        }

        // GET: api/Students/5
        [HttpGet, Route("getstaffphoto/{id}")]
        public async Task<IActionResult> GetStaffPhoto(int id)
        {
            var result = await (from p in _context.MStaff
                                where p.Id == id
                                select new MStaff()
                                {
                                    Photo = p.Photo
                                }).FirstOrDefaultAsync();

            return Ok(result);
        }
    }
}