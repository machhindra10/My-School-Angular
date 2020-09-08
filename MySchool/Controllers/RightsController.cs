using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySchool.Models;
using MySchool.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MySchool.Controllers
{
    [Route("api/rights")]
    [ApiController]
    public class RightsController : BaseController
    {
        private readonly ngSchoolContext _context;

        public RightsController(ngSchoolContext context)
        {
            _context = context;
        }

        // GET: api/Rights
        [HttpGet]
        public IEnumerable<MRights> GetMRights()
        {
            return _context.MRights;
        }

        // GET: api/Rights
        [HttpGet, Route("rightsbyroleid/{roleid}")]
        public IActionResult RightsByRoleId([FromRoute] int roleid)
        {
            List<VMRoleRights> q = (from mright in _context.MRights where mright.Url != null
                                    select new VMRoleRights()
                                    {
                                        Id = mright.Id,
                                        Menuname = mright.Menu.Menu1,
                                        Displayname = mright.Displayname,
                                        Groupid = mright.Groupid,
                                        Menuid = mright.Menuid,
                                        Rname = mright.Rname,
                                        Sort = mright.Menu.Sort,
                                        Url = mright.Url,
                                        Visible = mright.Visible,
                                        IsEnabled = _context.MRoleRights.Any(e => e.RoleId == roleid && e.RightId == mright.Id),
                                        RoleRightId = _context.MRoleRights.Where(e => e.RoleId == roleid && e.RightId == mright.Id).Select(e => e.Id).FirstOrDefault(),
                                    }).OrderBy(e => e.Sort).ToList();
            return Ok(q);
        }

        // GET: api/Rights/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMRights([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var mRights = await _context.MRights.FindAsync(id);

            if (mRights == null)
            {
                return NotFound();
            }

            return Ok(mRights);
        }

        // PUT: api/Rights/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMRights([FromRoute] int id, [FromBody] MRights mRights)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != mRights.Id)
            {
                return BadRequest();
            }

            _context.Entry(mRights).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MRightsExists(id))
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

        // POST: api/Rights
        [HttpPost]
        public async Task<IActionResult> PostMRights([FromBody] MRights mRights)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.MRights.Add(mRights);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMRights", new { id = mRights.Id }, mRights);
        }

        // DELETE: api/Rights/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMRights([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var mRights = await _context.MRights.FindAsync(id);
            if (mRights == null)
            {
                return NotFound();
            }

            _context.MRights.Remove(mRights);
            await _context.SaveChangesAsync();

            return Ok(mRights);
        }

        private bool MRightsExists(decimal id)
        {
            return _context.MRights.Any(e => e.Id == id);
        }
    }
}