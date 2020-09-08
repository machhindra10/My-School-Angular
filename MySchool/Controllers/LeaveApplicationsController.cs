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
    public class LeaveApplicationsController : BaseController, IDisposable
    {
        private readonly ngSchoolContext _context;

        public LeaveApplicationsController(ngSchoolContext context)
        {
            _context = context;
        }

        // GET: api/LeaveApplications
        [HttpGet]
        public IEnumerable<LeaveApplication> GetLeaveApplication()
        {
            return _context.LeaveApplication;
        }

        // GET: api/LeaveApplications/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetLeaveApplication([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var leaveApplication = await _context.LeaveApplication.FindAsync(id);

            if (leaveApplication == null)
            {
                return NotFound();
            }

            return Ok(leaveApplication);
        }

        // PUT: api/LeaveApplications/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLeaveApplication([FromRoute] int id, [FromBody] LeaveApplication leaveApplication)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != leaveApplication.Id)
            {
                return BadRequest();
            }

            _context.Entry(leaveApplication).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LeaveApplicationExists(id))
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

        // POST: api/LeaveApplications
        [HttpPost]
        public async Task<IActionResult> PostLeaveApplication([FromBody] tempApplication temp)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            LeaveApplication leaveApplication = temp.data;
            List<leavesCount> leavesCounts = temp.leavesRemaining;

            leaveApplication.Datefrom = GetTimeZoneDate(leaveApplication.Datefrom);
            leaveApplication.Dateto = GetTimeZoneDate(leaveApplication.Dateto);

            var count = (from c in leavesCounts where c.leavetypeid == leaveApplication.Leavetypeid select c).FirstOrDefault();

            TimeSpan totaldays = leaveApplication.Dateto.Date - leaveApplication.Datefrom.Date;

            if((totaldays.Days + 1) > count.count)
            {
                return Ok(new {result = false, available = count.count, applied = (totaldays.Days + 1), leavetype =  count.leavetype});
            }           

            _context.LeaveApplication.Add(leaveApplication);
            await _context.SaveChangesAsync();

            return Ok(new { result = true, count = 0 });
        }

        // DELETE: api/LeaveApplications/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLeaveApplication([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var leaveApplication = await _context.LeaveApplication.FindAsync(id);
            if (leaveApplication == null)
            {
                return NotFound();
            }

            if (leaveApplication.Status == "Pending")
            {
                _context.LeaveApplication.Remove(leaveApplication);
            }
            await _context.SaveChangesAsync();

            return Ok(leaveApplication);
        }

        private bool LeaveApplicationExists(int id)
        {
            return _context.LeaveApplication.Any(e => e.Id == id);
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        // GET: api/LeaveApplications/5
        [HttpGet, Route("changestatustorejected/{id}/{reason}")]
        public async Task<IActionResult> ChangeStatus([FromRoute] int id, string reason)
        {
            var leaveApplication = await _context.LeaveApplication.FindAsync(id);

            if (leaveApplication == null)
            {
                return NotFound();
            }

            leaveApplication.Status = "Rejected";
            leaveApplication.Reason = reason;
            _context.Entry(leaveApplication).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(leaveApplication);
        }

        // GET: api/LeaveApplications/5
        [HttpGet, Route("changestatustoapproved/{id}")]
        public async Task<IActionResult> ChangeStatusToApproved([FromRoute] int id)
        {
            var leaveApplication = await _context.LeaveApplication.FindAsync(id);

            if (leaveApplication == null)
            {
                return NotFound();
            }

            StaffAttendance1Controller staffAttendance1Controller = new StaffAttendance1Controller(_context);
            await staffAttendance1Controller.UpdateLeavesOfStaff(id);

            leaveApplication.Status = "Approved";
            _context.Entry(leaveApplication).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(leaveApplication);
        }

        // GET: api/LeaveApplications/5
        [HttpGet, Route("getbystaffid/{staffid}")]
        public async Task<IActionResult> GetLeaveApplicationByStaff([FromRoute] int staffid)
        {
            int year = DateTime.UtcNow.Year;
            var leaveApplication = await (from p in _context.LeaveApplication
                                          where p.Staffid == staffid && p.Datefrom.Year == year
                                          orderby p.Datefrom descending
                                          select new LeaveApplication
                                          {
                                              Staffid = p.Staffid,
                                              Datefrom = p.Datefrom,
                                              Dateto = p.Dateto,
                                              Description = p.Description,
                                              Id = p.Id,
                                              Leavetype = p.Leavetype,
                                              Leavetypeid = p.Leavetypeid,
                                              Staff = p.Staff,
                                              Status = p.Status,
                                              Reason = p.Reason
                                          }).ToListAsync();

            if (leaveApplication == null)
            {
                return NotFound();
            }

            return Ok(leaveApplication);
        }

        // GET: api/LeaveApplications/5
        [HttpGet, Route("getall")]
        public async Task<IActionResult> GetLeaveApplicationAll()
        {
            int year = DateTime.UtcNow.Year;
            var leaveApplication = await (from p in _context.LeaveApplication
                                          where p.Datefrom.Year == year
                                          orderby p.Datefrom descending
                                          select new LeaveApplication
                                          {
                                              Staffid = p.Staffid,
                                              Datefrom = p.Datefrom,
                                              Dateto = p.Dateto,
                                              Description = p.Description,
                                              Id = p.Id,
                                              Leavetype = p.Leavetype,
                                              Leavetypeid = p.Leavetypeid,
                                              Staff = p.Staff,
                                              Status = p.Status,
                                              Reason = p.Reason
                                          }).ToListAsync();

            if (leaveApplication == null)
            {
                return NotFound();
            }

            return Ok(leaveApplication);
        }
    }

    public class tempApplication
    {
        public LeaveApplication data { get; set; }
        public List<leavesCount> leavesRemaining { get; set; }
    }
}