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
    public class RoleRightsController : BaseController, IDisposable
    {
        private readonly ngSchoolContext _context;

        public RoleRightsController(ngSchoolContext context)
        {
            _context = context;
        }

        // GET: api/RoleRights
        [HttpGet]
        public IEnumerable<MRoleRights> GetMRoleRights()
        {
            return _context.MRoleRights;
        }

        // GET: api/RoleRights/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMRoleRights([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var mRoleRights = await _context.MRoleRights.FindAsync(id);

            if (mRoleRights == null)
            {
                return NotFound();
            }

            return Ok(mRoleRights);
        }

        // GET: api/RoleRights/isuserauthorized/5
        [HttpGet(), Route("isuserauthorized/{rightid}")]
        public async Task<IActionResult> IsUserAuthorizzed([FromRoute] int rightid)
        {
            bool isuserauthorized = false;
            int roleid = GetUserRoleId();
            //decimal rightid = _context.MRights.Where(e => e.Authid == authid).Select(e => e.Id).FirstOrDefault();
            if (rightid > 0)
            {
                isuserauthorized = await _context.MRoleRights.AnyAsync(e => e.RoleId == roleid && e.RightId == rightid);
            }

            return Ok(isuserauthorized);
        }

        // GET: api/RoleRights/isuserauthorized/5
        [HttpGet, Route("getrightids/{roleid}")]
        public List<RightIDs> GetRightIDsofRole([FromRoute] int roleid)
        {

            var q = (from p in _context.MRoleRights
                     where p.RoleId == roleid
                     select new RightIDs
                     {
                         Id = Convert.ToInt32(p.RightId)
                     }).ToList();

            return q;
        }



        // PUT: api/RoleRights/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMRoleRights([FromRoute] long id, [FromBody] MRoleRights mRoleRights)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != mRoleRights.Id)
            {
                return BadRequest();
            }

            _context.Entry(mRoleRights).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MRoleRightsExists(id))
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

        // POST: api/RoleRights
        [HttpPost]
        public async Task<IActionResult> PostMRoleRights([FromBody] MRoleRights mRoleRights)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.MRoleRights.Add(mRoleRights);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMRoleRights", new { id = mRoleRights.Id }, mRoleRights);
        }

        // DELETE: api/RoleRights/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMRoleRights([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var mRoleRights = await _context.MRoleRights.FindAsync(id);
            if (mRoleRights == null)
            {
                return NotFound();
            }

            _context.MRoleRights.Remove(mRoleRights);
            await _context.SaveChangesAsync();

            return Ok(mRoleRights);
        }

        private bool MRoleRightsExists(decimal id)
        {
            return _context.MRoleRights.Any(e => e.Id == id);
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public class RightIDs
        {
            public int Id { get; set; }
        }
    }
}