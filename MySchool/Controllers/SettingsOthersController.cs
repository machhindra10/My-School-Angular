using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySchool.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MySchool.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SettingsOthersController : ControllerBase
    {
        private readonly ngSchoolContext _context;

        public SettingsOthersController(ngSchoolContext context)
        {
            _context = context;
        }

        // GET: api/SettingsOthers
        [HttpGet]
        public IEnumerable<SettingsOther> GetSettingsOther()
        {
            return _context.SettingsOther;
        }

        public SettingsOther GetSettingsOthersFirstOnly()
        {
            var settings = _context.SettingsOther.FirstOrDefault();

            return settings;
        }

        // GET: api/SettingsOthers/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSettingsOther([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var settingsOther = await _context.SettingsOther.FindAsync(id);

            if (settingsOther == null)
            {
                return NotFound();
            }

            return Ok(settingsOther);
        }

        // PUT: api/SettingsOthers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSettingsOther([FromRoute] int id, [FromBody] SettingsOther settingsOther)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != settingsOther.Id)
            {
                return BadRequest();
            }

            _context.Entry(settingsOther).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SettingsOtherExists(id))
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


        // GET: api/SettingsOthers/5
        [HttpGet, Route("getsmssettings")]
        public async Task<IActionResult> GetSMSSettings()
        {

            var settingsOther = await (from p in _context.SettingsOther
                                       select new SettingsOther
                                       {
                                           Id = p.Id,
                                           Smsusername = p.Smsusername,                                           
                                           Smspassword = p.Smspassword,

                                           Smskey = p.Smskey,
                                           Smsprofileid = p.Smsprofileid,
                                           Smssenderid = p.Smssenderid
                                       }).FirstOrDefaultAsync();

            if (settingsOther == null)
            {
                return NotFound();
            }

            return Ok(settingsOther);
        }

        // PUT: api/SettingsOthers/5
        [HttpPut, Route("updatesmssettings/{id}")]
        public async Task<IActionResult> UpdateSMSSettings([FromRoute] int id, [FromBody] SettingsOther settingsOther)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != settingsOther.Id)
            {
                return BadRequest();
            }

            var tempSettingsOther = await _context.SettingsOther.FindAsync(id);

            if (tempSettingsOther == null)
            {
                return NotFound();
            }

            tempSettingsOther.Smskey = settingsOther.Smskey;
            //tempSettingsOther.Smspassword = Encryption.EncryptString(settingsOther.Smspassword);
            tempSettingsOther.Smsprofileid = settingsOther.Smsprofileid;
            //tempSettingsOther.Smsusername = settingsOther.Smsusername;
            tempSettingsOther.Smssenderid = settingsOther.Smssenderid;

            _context.Entry(tempSettingsOther).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SettingsOtherExists(id))
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

        // POST: api/SettingsOthers
        [HttpPost]
        public async Task<IActionResult> PostSettingsOther([FromBody] SettingsOther settingsOther)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.SettingsOther.Add(settingsOther);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSettingsOther", new { id = settingsOther.Id }, settingsOther);
        }

        // DELETE: api/SettingsOthers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSettingsOther([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var settingsOther = await _context.SettingsOther.FindAsync(id);
            if (settingsOther == null)
            {
                return NotFound();
            }

            _context.SettingsOther.Remove(settingsOther);
            await _context.SaveChangesAsync();

            return Ok(settingsOther);
        }

        private bool SettingsOtherExists(int id)
        {
            return _context.SettingsOther.Any(e => e.Id == id);
        }
    }
}