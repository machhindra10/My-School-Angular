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
    public class DesignationsController : BaseController, IDisposable
    {
        private readonly ngSchoolContext _context;

        public DesignationsController(ngSchoolContext context)
        {
            _context = context;
        }

        // GET: api/Designations
        [HttpGet]
        public IEnumerable<MDesignation> GetMDesignation()
        {
            return _context.MDesignation;
        }

        // GET: api/Designations/getdesignationsenabled
        [HttpGet, Route("getdesignationsenabled")]
        public IEnumerable<MDesignation> GetMDesignationEnabled()
        {
            return _context.MDesignation.Where(t => t.Disabled == false);
        }

        // GET: api/Designations/5
        [HttpGet("{id}"), Authorize]
        public async Task<IActionResult> GetMDesignation([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            MDesignation mDesignation = null;
            if (GetIsUserMasterAdmin())
            {
                mDesignation = await _context.MDesignation.FindAsync(id);
            }
            else
            {
                mDesignation = await _context.MDesignation.Where(e => e.Isdefault == false && e.Id == id).FirstOrDefaultAsync();

            }

            if (mDesignation == null)
            {
                return NotFound();
            }

            return Ok(mDesignation);
        }

        // PUT: api/Designations/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMDesignation([FromRoute] int id, [FromBody] MDesignation mDesignation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != mDesignation.Id)
            {
                return BadRequest();
            }
            if (MDesignationNameExists(mDesignation.Id, mDesignation.Designname))
            {
                return Ok(new { exists = true });
            }


            if (id == 0)
            {
                mDesignation.Disabled = false;
                _context.MDesignation.Add(mDesignation);
            }
            else
            {
                if (mDesignation.Isdefault)
                {
                    return Ok(mDesignation);
                }
                _context.Entry(mDesignation).State = EntityState.Modified;
            }


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Ok(mDesignation);
                //if (!MDesignationExists(id))
                //{
                //    return NotFound();
                //}
                //else
                //{
                //    throw;
                //}
            }
            catch(Exception)
            {
                return Ok(new { exists = true });
            }
            return Ok(mDesignation);
            //return NoContent();
        }

        // POST: api/Designations
        [HttpPost, Authorize]
        public async Task<IActionResult> PostMDesignation([FromBody] MDesignation mDesignation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.MDesignation.Add(mDesignation);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMDesignation", new { id = mDesignation.Id }, mDesignation);
        }

        // DELETE: api/Designations/5
        [HttpDelete("{id}"), Authorize]
        public async Task<IActionResult> DeleteMDesignation([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var mDesignation = await _context.MDesignation.FindAsync(id);
            if (mDesignation == null)
            {
                return NotFound();
            }
            if (mDesignation.Isdefault == false)
            {
                _context.MDesignation.Remove(mDesignation);
            }
            await _context.SaveChangesAsync();

            return Ok(mDesignation);
        }

        private bool MDesignationExists(int id)
        {
            return _context.MDesignation.Any(e => e.Id == id);
        }

        private bool MDesignationNameExists(int id, string name)
        {
            return _context.MDesignation.Any(e => e.Id != id && e.Designname == name);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}