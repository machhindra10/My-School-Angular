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
    public class MessagesGuardiansController : BaseController, IDisposable
    {
        private readonly ngSchoolContext _context;

        public MessagesGuardiansController(ngSchoolContext context)
        {
            _context = context;
        }

        // GET: api/MessagesGuardians
        [HttpGet]
        public IEnumerable<MessagesGuardians> GetMessagesGuardians()
        {
            return _context.MessagesGuardians;
        }

        // GET: api/MessagesGuardians/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMessagesGuardians([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var messagesGuardians = await _context.MessagesGuardians.FindAsync(id);

            if (messagesGuardians == null)
            {
                return NotFound();
            }

            return Ok(messagesGuardians);
        }

        // PUT: api/MessagesGuardians/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMessagesGuardians([FromRoute] long id, [FromBody] MessagesGuardians messagesGuardians)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != messagesGuardians.Id)
            {
                return BadRequest();
            }

            _context.Entry(messagesGuardians).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MessagesGuardiansExists(id))
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

        // POST: api/MessagesGuardians
        [HttpPost]
        public async Task<IActionResult> PostMessagesGuardians([FromBody] MessagesGuardians messagesGuardians)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.MessagesGuardians.Add(messagesGuardians);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMessagesGuardians", new { id = messagesGuardians.Id }, messagesGuardians);
        }

        // DELETE: api/MessagesGuardians/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMessagesGuardians([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var messagesGuardians = await _context.MessagesGuardians.FindAsync(id);
            if (messagesGuardians == null)
            {
                return NotFound();
            }

            _context.MessagesGuardians.Remove(messagesGuardians);
            await _context.SaveChangesAsync();

            return Ok(messagesGuardians);
        }

        private bool MessagesGuardiansExists(long id)
        {
            return _context.MessagesGuardians.Any(e => e.Id == id);
        }

        // GET: api/Messages/5
        [HttpGet, Route("updateread/{id}")]
        public async Task<IActionResult> UpdateReadRecepient([FromRoute] long id)
        {
            var messagesGuardians = await _context.MessagesGuardians.FindAsync(id);

            if (messagesGuardians == null)
            {
                return NotFound();
            }

            messagesGuardians.Read = GetTimeZoneDate(DateTime.UtcNow);
            _context.Entry(messagesGuardians).State = EntityState.Modified;

            int i = await _context.SaveChangesAsync();
            if (i > 0)
            {
                return Ok(new { result = true });
            }
            else
            {
                return Ok(new { result = false });
            }

        }

        // GET: api/Messages/5
        [HttpGet, Route("getmessagesbyguardianid/{guardianid}")]
        public async Task<IActionResult> GetMessagesByGuardianId([FromRoute] long guardianid)
        {
            var messageRecepients = await (from p in _context.MessagesGuardians
                                           where p.Guardianid == guardianid
                                           select new MessagesGuardians
                                           {
                                               Id = p.Id,
                                               Delivered = p.Delivered,
                                               Read = p.Read,
                                               Guardian = p.Guardian,
                                               Message = p.Message
                                           }).ToListAsync();

            if (messageRecepients == null)
            {
                return NotFound();
            }

            foreach (var item in messageRecepients.Where(p => p.Delivered == null))
            {
                item.Delivered = GetTimeZoneDate(DateTime.UtcNow);
                _context.Entry(item).State = EntityState.Modified;
            }
            await _context.SaveChangesAsync();

            return Ok(messageRecepients);
        }

        // GET: api/Messages/5
        [HttpGet, Route("getmessagebyid/{id}")]
        public async Task<IActionResult> GetMessageByIDforGuardian([FromRoute] long id)
        {
            var messageGuardians = await (from p in _context.MessagesGuardians
                                          where p.Id == id
                                          select new MessagesGuardians
                                          {
                                              Id = p.Id,
                                              Delivered = p.Delivered,
                                              Read = p.Read,
                                              Guardian = p.Guardian,
                                              Message = p.Message
                                          }).FirstOrDefaultAsync();

            if (messageGuardians == null)
            {
                return NotFound();
            }

            messageGuardians.Read = GetTimeZoneDate(DateTime.UtcNow);
            _context.Entry(messageGuardians).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return Ok(messageGuardians);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}