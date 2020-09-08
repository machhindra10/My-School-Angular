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
    public class PaymentModesController : BaseController, IDisposable
    {
        private readonly ngSchoolContext _context;

        public PaymentModesController(ngSchoolContext context)
        {
            _context = context;
        }

        // GET: api/PaymentModes
        [HttpGet]
        public IEnumerable<MPaymentModes> GetMPaymentModes()
        {
            return _context.MPaymentModes;
        }

        // GET: api/PaymentModes/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMPaymentModes([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var mPaymentModes = await _context.MPaymentModes.FindAsync(id);

            if (mPaymentModes == null)
            {
                return NotFound();
            }

            return Ok(mPaymentModes);
        }

        // PUT: api/PaymentModes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMPaymentModes([FromRoute] int id, [FromBody] MPaymentModes mPaymentModes)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != mPaymentModes.Id)
            {
                return BadRequest();
            }

            _context.Entry(mPaymentModes).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MPaymentModesExists(id))
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

        // POST: api/PaymentModes
        [HttpPost]
        public async Task<IActionResult> PostMPaymentModes([FromBody] MPaymentModes mPaymentModes)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.MPaymentModes.Add(mPaymentModes);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMPaymentModes", new { id = mPaymentModes.Id }, mPaymentModes);
        }

        // DELETE: api/PaymentModes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMPaymentModes([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var mPaymentModes = await _context.MPaymentModes.FindAsync(id);
            if (mPaymentModes == null)
            {
                return NotFound();
            }

            _context.MPaymentModes.Remove(mPaymentModes);
            await _context.SaveChangesAsync();

            return Ok(mPaymentModes);
        }

        private bool MPaymentModesExists(int id)
        {
            return _context.MPaymentModes.Any(e => e.Id == id);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}