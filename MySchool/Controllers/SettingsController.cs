using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySchool.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace MySchool.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SettingsController : BaseController, IDisposable
    {
        private readonly ngSchoolContext _context;

        public SettingsController(ngSchoolContext context)
        {
            _context = context;
        }

        // GET: api/Settings
        [HttpGet]
        public IEnumerable<Settings> GetSettings()
        {
            return _context.Settings;
        }

        // GET: api/Settings/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSettings([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var settings = await _context.Settings.FirstOrDefaultAsync();

            if (settings == null)
            {
                return NotFound();
            }

            return Ok(settings);
        }

        // GET: api/Settings/5
        [HttpGet, Route("getdefault")]
        public async Task<IActionResult> GetDefaultSettings([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var settings = await _context.Settings.FirstOrDefaultAsync();

            if (settings == null)
            {
                return Ok(new { result = false });
            }

            return Ok(new { result = true, settings = settings}); 
        }

        // GET: api/Settings/5
        [HttpGet, Route("getorgtoken")]
        public async Task<IActionResult> GetSettingsOrgToken()
        {
            Settings q = null;
            try
            {
                q = await _context.Settings.FirstOrDefaultAsync();
                if (q == null)
                {
                    return Ok(new { tk = "" });
                }
            }
            catch (Exception)
            {
                return Ok(new { tk = "" });
            }
            return Ok(new { tk = q.Token });
        }

        // GET: api/Settings/5
        [HttpGet(), Route("getbykey/{key}")]
        public async Task<IActionResult> GetSettingsByKey([FromRoute] string key)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var settings = await _context.Settings.Where(e => e.Key == key).FirstOrDefaultAsync();

            if (settings == null)
            {
                return NotFound();
            }

            return Ok(settings);
        }

        // GET: api/Settings/5
        [HttpGet(), Route("getyear")]
        public IEnumerable<Data> GetYear1()
        {
            List<Data> lyears = new List<Data>();
            for (int i = 2018; i <= GetTimeZoneDate(DateTime.UtcNow).Year; i++)
            {
                lyears.Add(new Data() { id = i, name = i.ToString() + "-" + (i + 1).ToString() });
            }
            return lyears;
        }

        // GET: api/Settings/5
        [HttpGet(), Route("getyearonly")]
        public IEnumerable<Data> GetYear123()
        {
            List<Data> lyears = new List<Data>();
            for (int i = 2018; i <= GetTimeZoneDate(DateTime.UtcNow).Year; i++)
            {
                lyears.Add(new Data() { id = i, name = i.ToString() });
            }
            return lyears;
        }

        [HttpGet, Route("getmonths")]
        public IActionResult GetMonths()
        {
            List<Data> listmonths = new List<Data>();
            Data yr = null;
            int month = 1;
            do
            {
                yr = new Data { id = month, name = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month) };
                listmonths.Add(yr);
            }
            while (month++ < 12);//DateTime.Now.Month);

            return Ok(listmonths);
        }

        // PUT: api/Settings/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSettings([FromRoute] int id, [FromBody] Settings settings)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != settings.Id)
            {
                return BadRequest();
            }
            if (id > 0)
            {
                _context.Entry(settings).State = EntityState.Modified;
            }
            else
            {
                _context.Settings.Add(settings);
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SettingsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(settings);
        }

        // POST: api/Settings
        [HttpPost]
        public async Task<IActionResult> PostSettings([FromBody] Settings settings)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            _context.Settings.Add(settings);
            await _context.SaveChangesAsync();

            return Ok(new { result = true });
        }

        // DELETE: api/Settings/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSettings([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var settings = await _context.Settings.FindAsync(id);
            if (settings == null)
            {
                return NotFound();
            }

            _context.Settings.Remove(settings);
            await _context.SaveChangesAsync();

            return Ok(settings);
        }

        private bool SettingsExists(int id)
        {
            return _context.Settings.Any(e => e.Id == id);
        }

        // GET: api/Settings/5
        [HttpGet(), Route("gettimezones")]
        public IActionResult GetTimeZones()
        {
            List<TimeZoneInfo> tz;
            tz = TimeZoneInfo.GetSystemTimeZones().ToList();

            return Ok(tz);
        }

        // GET: api/Settings/5
        [HttpGet(), Route("getsetupcount")]
        public IActionResult GetSetupCount()
        {
            int settings_count = 0; int batches_count = 0; int user_count = 0;
            try
            {
                settings_count = (from s in _context.Settings select s).Count();
                batches_count = (from b in _context.Batches select b).Count();
                user_count = (from u in _context.MUser where u.IsMasterAdmin == false select u).Count();
            }
            catch (Exception)
            {
                return Ok(new { settingscount = settings_count, batchescount = batches_count, usercount = user_count });
            }

            return Ok(new { settingscount = settings_count, batchescount = batches_count, usercount = user_count });
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }

    public class Data
    {
        public int id { get; set; }
        public string name { get; set; }
    }
}