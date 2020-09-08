using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MySchool.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MySchool.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : BaseController, IDisposable
    {
        private readonly ngSchoolContext _context;
        public IConfiguration _configuration { get; }

        public UsersController(ngSchoolContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: api/MUsers
        [HttpGet, Authorize]
        public IEnumerable<MUser> GetUser()
        {
            if (GetIsUserMasterAdmin())
            {
                return (from p in _context.MUser
                        select new MUser
                        {
                            Aadharno = p.Aadharno,
                            DateCreated = p.DateCreated,
                            Disabled = p.Disabled,
                            Email = p.Email,
                            Fname = p.Fname,
                            Id = p.Id,
                            IsAdmin = p.IsAdmin,
                            Lname = p.Lname,
                            Mname = p.Mname,
                            Role = p.Role,
                            RoleId = p.RoleId,
                            UserName = p.UserName,
                            Userid = p.Userid,
                            IsMasterAdmin = p.IsMasterAdmin,
                        }).ToList();
            }
            else
            {
                return (from p in _context.MUser
                        where p.IsMasterAdmin == false
                        select new MUser
                        {
                            Aadharno = p.Aadharno,
                            DateCreated = p.DateCreated,
                            Disabled = p.Disabled,
                            Email = p.Email,
                            Fname = p.Fname,
                            Id = p.Id,
                            IsAdmin = p.IsAdmin,
                            Lname = p.Lname,
                            Mname = p.Mname,
                            Role = p.Role,
                            RoleId = p.RoleId,
                            UserName = p.UserName,
                            Userid = p.Userid,
                            IsMasterAdmin = p.IsMasterAdmin,
                        }).ToList();
            }
        }

        // GET: api/MUsers/5
        [HttpGet("{id}"), Authorize]
        public async Task<IActionResult> GetUser([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            MUser mUser = null;
            if (GetIsUserMasterAdmin())
            {
                mUser = await _context.MUser.FindAsync(id);
            }
            else
            {
                mUser = await _context.MUser.Where(e => e.IsMasterAdmin == false && e.Id == id).FirstOrDefaultAsync();
            }


            if (mUser == null)
            {
                return NotFound();
            }

            return Ok(mUser);
        }

        // GET: api/MUsers/5
        [HttpGet(), Route("getuserphoto/{id}"), Authorize]
        public async Task<IActionResult> GetUserPhoto([FromRoute] int id)
        {
            try
            {
                var photo = await (from y in _context.MUser
                                   where y.Id == id
                                   select y.Photo).FirstOrDefaultAsync();
                return Ok(new { userphoto = photo });
            }
            catch (Exception)
            {
                return Ok(new { userphoto = "" });
            }
        }


        // PUT: api/MUsers/5
        [HttpPut("{id}"), Authorize]
        public async Task<IActionResult> PutUser([FromRoute] int id, [FromBody] MUser mUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != mUser.Id)
            {
                return BadRequest();
            }
            if (UserNameEmailExists(mUser.Email, mUser.Id))
            {
                return Ok(new { exists = true });
            }

            mUser.Userid = this.GetUserId();


            if (id == 0)
            {
                mUser.IsAdmin = false;
                mUser.IsMasterAdmin = false;
                mUser.Disabled = false;
                mUser.DateCreated = GetTimeZoneDate(DateTime.UtcNow);
                mUser.Password = Encryption.EncryptString(mUser.Password);
                _context.MUser.Add(mUser);
            }
            else
            {
                //var tempUser = await _context.MUser.FindAsync(id);
                //mUser.RoleId = tempUser.RoleId;
                //mUser.Disabled = tempUser.Disabled;
                //mUser.IsAdmin = tempUser.IsAdmin;
                //mUser.IsMasterAdmin = tempUser.IsMasterAdmin;
                //mUser.Password = tempUser.Password;
                mUser.DateModified = GetTimeZoneDate(DateTime.UtcNow);
                _context.Entry(mUser).State = EntityState.Modified;
            }
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return Ok(mUser);
            //return NoContent();
        }

        // PUT: api/MUsers/5
        [HttpGet, Route("disableuser/{id}"), Authorize]
        public async Task<IActionResult> PutUpdateUser([FromRoute] int id)
        {
            MUser mUser = await _context.MUser.FindAsync(id);
            if (mUser == null)
            {
                return NotFound();
            }
            mUser.Disabled = !mUser.Disabled;
            _context.Entry(mUser).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return Ok(mUser);
            //return NoContent();
        }

        // POST: api/MUsers
        [HttpPost, Authorize]
        public async Task<IActionResult> PostUser([FromBody] MUser mUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //mUser.Userid = mUser.Id;
            //mUser.IsMasterAdmin = false;
            //mUser.Disabled = false;
            //mUser.DateCreated = DateTime.UtcNow;

            _context.MUser.Add(mUser);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMUser", new { id = mUser.Id }, mUser);
        }

        // POST: api/MUsers
        [HttpPost, Route("addfirstuser")]
        public async Task<IActionResult> PostAddUser([FromBody] MUser mUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            mUser.Userid = mUser.Id;
            mUser.IsAdmin = true;
            mUser.IsMasterAdmin = false;
            mUser.Disabled = false;
            mUser.DateCreated = DateTime.UtcNow;
            mUser.Password = Encryption.EncryptString(mUser.Password);

            _context.MUser.Add(mUser);
            await _context.SaveChangesAsync();

            return Ok(new { result = true, user = mUser });
        }

        // DELETE: api/MUsers/5
        [HttpDelete("{id}"), Authorize]
        public async Task<IActionResult> DeleteUser([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var mUser = await _context.MUser.FindAsync(id);
            if (mUser == null)
            {
                return NotFound();
            }
            if (!mUser.IsAdmin)
            {
                _context.MUser.Remove(mUser);
            }
            await _context.SaveChangesAsync();

            return Ok(mUser);
        }

        // PUT: api/MUsers/5
        [HttpPut(), Route("updatepassword/{id}"), Authorize]
        public async Task<IActionResult> PutUpdatePassword([FromRoute] int id, [FromBody] changepassword mUser)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            if (mUser.newpassword != mUser.confirmpassword)
            {
                return Ok(new { comparepassword = false });
            }

            var user = _context.MUser.Find(id);
            if (Encryption.EncryptString(mUser.oldpassword) != user.Password)
            {
                return Ok(new { invalidpassword = true });
            }

            user.Password = Encryption.EncryptString(mUser.newpassword);
            user.DateModified = GetTimeZoneDate(DateTime.UtcNow);

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return Ok(mUser);
            //return NoContent();
        }

        // GET: api/Organisations/5
        [HttpGet, Route("forgotpassword/{email}")]
        public async Task<IActionResult> forgotpassword([FromRoute] string email)
        {
            var user = await (from p in _context.MUser where p.Email == email select p).FirstOrDefaultAsync();
            if (user != null)
            {
                user.Passverificationcode = Guid.NewGuid().ToString("n");

                _context.Entry(user).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                EmailsController emailsController = new EmailsController(_context, _configuration);
                emailsController.SendEmailForgotPassword(user);
                return Ok(new { result = true });
            }
            else
            {
                return Ok(new { result = false });
            }
        }

        // GET: api/Organisations/5
        [HttpGet, Route("checkpasswordverificationcode/{userid}/{code}")]
        public async Task<IActionResult> checkpasswordverificationcode([FromRoute] int userid, [FromRoute] string code)
        {
            var user = await (from p in _context.MUser where p.Id == userid && p.Passverificationcode == code select p).FirstOrDefaultAsync();
            if (user != null)
            {
                return Ok(new { result = true });
            }
            else
            {
                return Ok(new { result = false });
            }

        }

        // GET: api/Organisations/5
        [HttpGet, Route("recoverpassword/{userid}/{newp}/{confirm}")]
        public async Task<IActionResult> recoverpasswordfinal([FromRoute] int userid, [FromRoute] string newp, [FromRoute] string confirm)
        {
            if (newp != confirm)
            {
                return Ok(new { result = false, match = false });
            }
            var user = await (from p in _context.MUser where p.Id == userid select p).FirstOrDefaultAsync();
            if (user != null)
            {
                user.Password = Encryption.EncryptString(newp);
                user.Passverificationcode = string.Empty;

                _context.Entry(user).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Ok(new { result = true, match = true });
            }
            else
            {
                return NotFound();
            }

        }


        private bool UserExists(int id)
        {
            return _context.MUser.Any(e => e.Id == id);
        }

        private bool UserNameEmailExists(string email, int id)
        {
            return _context.MUser.Any(e => e.Email == email && e.Id != id);
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        // GET: api/MUsers/5
        [HttpGet, Route("getforselector")]
        public async Task<IActionResult> GetUserForselector()
        {
            var staffassociateuserids = await (from p in _context.MStaff select p.Associateuserid).Distinct().ToListAsync();
            List<MUser> mUser = null;
            if (GetIsUserMasterAdmin())
            {
                mUser = await _context.MUser.Where(x => !staffassociateuserids.Contains(x.Id)).ToListAsync();
            }
            else
            {
                mUser = await _context.MUser.Where(e => e.IsMasterAdmin == false && !staffassociateuserids.Contains(e.Id)).ToListAsync();
            }

            return Ok(mUser);
        }

        public class changepassword
        {
            public string oldpassword { get; set; }
            public string newpassword { get; set; }
            public string confirmpassword { get; set; }
        }
    }
}