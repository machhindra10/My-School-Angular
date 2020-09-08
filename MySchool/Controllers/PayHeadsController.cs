using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySchool.Models;

namespace MySchool.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PayHeadsController : BaseController, IDisposable
    {
        private readonly ngSchoolContext _context;

        public PayHeadsController(ngSchoolContext context)
        {
            _context = context;
        }


        // GET: api/PayrollTempletes
        [HttpGet]
        public IEnumerable<MPayHead> GetMPayHead()
        {
            return _context.MPayHead;
        }

        // GET: api/PayrollTempletes/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMPayHead([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var MPayHead = await _context.MPayHead.FindAsync(id);

            if (MPayHead == null)
            {
                return NotFound();
            }

            return Ok(MPayHead);
        }

        // PUT: api/PayrollTempletes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMPayHead([FromRoute] int id, [FromBody] MPayHead MPayHead)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != MPayHead.Id)
            {
                return BadRequest();
            }
            if (MPayHeadNameExists(MPayHead.Id, MPayHead.Head))
            {
                return Ok(new { exists = true });
            }

            if (id == 0)
            {                
                _context.MPayHead.Add(MPayHead);
            }
            else
            {
                _context.Entry(MPayHead).State = EntityState.Modified;
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MPayHeadExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(MPayHead);
        }

        // POST: api/PayrollTempletes
        [HttpPost]
        public async Task<IActionResult> PostMPayHead([FromBody] MPayHead MPayHead)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.MPayHead.Add(MPayHead);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMPayHead", new { id = MPayHead.Id }, MPayHead);
        }

        // DELETE: api/PayrollTempletes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMPayHead([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var MPayHead = await _context.MPayHead.FindAsync(id);
            if (MPayHead == null)
            {
                return NotFound();
            }

            _context.MPayHead.Remove(MPayHead);
            await _context.SaveChangesAsync();

            return Ok(MPayHead);
        }

        private bool MPayHeadExists(int id)
        {
            return _context.MPayHead.Any(e => e.Id == id);
        }

        private bool MPayHeadNameExists(int id, string name)
        {
            return _context.MPayHead.Any(e => e.Id != id && e.Head == name);
        }


        public void Dispose()
        {
            _context.Dispose();
        }
    }
}