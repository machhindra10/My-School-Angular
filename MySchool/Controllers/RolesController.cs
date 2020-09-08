using Microsoft.AspNetCore.Authorization;
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
    public class RolesController : BaseController, IDisposable
    {
        private readonly ngSchoolContext _context;

        public RolesController(ngSchoolContext context)
        {
            _context = context;
        }

        // GET: api/Roles
        [HttpGet]
        public IEnumerable<MRoles> GetMRoles()
        {
            if (GetIsUserMasterAdmin())
            {
                return _context.MRoles;
            }
            else
            {
                return _context.MRoles.Where(e => e.Isadmin == false);
            }

        }

        [HttpGet, Route("getrolesenabled")]
        public IEnumerable<MRoles> GetMRolesEnabled()
        {
            return _context.MRoles.Where(e => e.Disabled == false);
        }

        // GET: api/Roles/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMRoles([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            MRoles mRoles = null;
            if (GetIsUserMasterAdmin())
            {
                mRoles = await _context.MRoles.FindAsync(id);
            }
            else
            {
                mRoles = await _context.MRoles.Where(e => e.Isadmin == false && e.Id == id).FirstOrDefaultAsync();
            }


            if (mRoles == null)
            {
                return NotFound();
            }

            return Ok(mRoles);
        }

        // PUT: api/Roles/5
        [HttpPut("{id}"), Authorize]
        public async Task<IActionResult> PutMRoles([FromRoute] int id, [FromBody] MRoles mRole)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != mRole.Id)
            {
                return BadRequest();
            }
            if (MRolesNameExists(mRole.Id, mRole.Rolename))
            {
                return Ok(new { exists = true });
            }

            mRole.Userid = this.GetUserId();



            if (id == 0)
            {
                mRole.DateCreated = GetTimeZoneDate(DateTime.UtcNow);
                mRole.Isadmin = false;
                mRole.Isdefault = false;
                mRole.Disabled = false;
                _context.MRoles.Add(mRole);
            }
            else
            {
                if (mRole.Isdefault == false)
                {
                    mRole.DateModified = GetTimeZoneDate(DateTime.UtcNow);
                    _context.Entry(mRole).State = EntityState.Modified;
                }
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MRolesExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return Ok(mRole);
            //return NoContent();
        }

        // POST: api/Roles
        [HttpPost, Authorize]
        public async Task<IActionResult> PostMRoles([FromBody] MRoles mRoles)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.MRoles.Add(mRoles);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMRoles", new { id = mRoles.Id }, mRoles);
        }

        // DELETE: api/Roles/5
        [HttpDelete("{id}"), Authorize]
        public async Task<IActionResult> DeleteMRoles([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var mRoles = await _context.MRoles.FindAsync(id);
            if (mRoles == null)
            {
                return NotFound();
            }
            if (!Convert.ToBoolean(mRoles.Isadmin) || mRoles.Isdefault == false)
            {
                _context.MRoles.Remove(mRoles);
            }
            await _context.SaveChangesAsync();

            return Ok(mRoles);
        }

        private bool MRolesExists(int id)
        {
            return _context.MRoles.Any(e => e.Id == id);
        }

        private bool MRolesNameExists(int id, string rolename)
        {
            return _context.MRoles.Any(e => e.Id != id && e.Rolename == rolename);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}