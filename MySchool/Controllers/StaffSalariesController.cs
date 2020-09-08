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
    public class StaffSalariesController : BaseController, IDisposable
    {
        private readonly ngSchoolContext _context;

        public StaffSalariesController(ngSchoolContext context)
        {
            _context = context;
        }

        // GET: api/StaffSalaries
        [HttpGet]
        public IEnumerable<StaffSalary> GetStaffSalary()
        {
            return _context.StaffSalary;
        }

        // GET: api/StaffSalaries/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetStaffSalary([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var staffSalary = await _context.StaffSalary.FindAsync(id);

            if (staffSalary == null)
            {
                return NotFound();
            }

            return Ok(staffSalary);
        }

        // GET: api/StaffSalaries/5
        [HttpGet, Route("getsalariesbyidreport/{id}")]
        public async Task<IActionResult> GetStaffSalaryById([FromRoute] int id)
        {

            var staffSalary = await (from p in _context.StaffSalary
                                     where p.Id == id
                                     select new StaffSalary
                                     {
                                         Adjustments = p.Adjustments,
                                         Datecreated = p.Datecreated,
                                         Datepaid = p.Datepaid,
                                         Deductions = p.Deductions,
                                         Earnings = p.Earnings,
                                         Id = p.Id,
                                         Ispaid = p.Ispaid,
                                         Netpay = p.Netpay,
                                         Userid = p.Userid,
                                         Month = p.Month,
                                         Year = p.Year,
                                         Staffid = p.Staffid,
                                         Staff = (from s in _context.MStaff
                                                  where s.Id == p.Staffid
                                                  select s).FirstOrDefault(),
                                         StaffSalaryDetails = (from ss in _context.StaffSalaryDetails
                                                               where ss.SsId == p.Id
                                                               select ss).ToList(),
                                     }).FirstOrDefaultAsync();

            if (staffSalary == null)
            {
                return NotFound();
            }

            //StaffAttendancesController staffAttendancesController = new StaffAttendancesController(_context);
            //decimal daysPayable = staffAttendancesController.GetTotalPresentyByStaffId(staffSalary.Staffid, staffSalary.Month, staffSalary.Year);

            StaffAttendance1Controller staffAttendance1Controller = new StaffAttendance1Controller(_context);
            decimal daysPayable = staffAttendance1Controller.GetTotalPresentyByStaffId(staffSalary.Staffid, staffSalary.Month, staffSalary.Year);


            return Ok(
                new
                {
                    staffSalary = staffSalary,
                    totalDays = DateTime.DaysInMonth(Convert.ToInt32(staffSalary.Year), Convert.ToInt32(staffSalary.Month)),
                    daysPayable = daysPayable
                });
        }

        // GET: api/StaffSalaries/5
        [HttpGet, Route("getsalarydetailsbyid/{id}")]
        public async Task<IActionResult> GetStaffSalaryDetailsById([FromRoute] int id)
        {

            var staffSalary = await (from ss in _context.StaffSalaryDetails
                                     where ss.SsId == id
                                     select ss).ToListAsync();

            if (staffSalary == null)
            {
                return NotFound();
            }

            return Ok(staffSalary);
        }

        // GET: api/StaffSalaries/5
        [HttpGet, Route("getsalariesbymonth/{month}/{year}")]
        public async Task<IActionResult> GetStaffSalaryByMonth([FromRoute] int month, int year)
        {
            var staffSalary = await (from p in _context.StaffSalary
                                     where p.Month == month && p.Year == year
                                     select new StaffSalary
                                     {
                                         Adjustments = p.Adjustments,
                                         Datecreated = p.Datecreated,
                                         Datepaid = p.Datepaid,
                                         Deductions = p.Deductions,
                                         Earnings = p.Earnings,
                                         Netpay = p.Netpay,
                                         Id = p.Id,
                                         Ispaid = p.Ispaid,
                                         Month = p.Month,
                                         Year = p.Year,
                                         Staff = (from t in _context.MStaff
                                                  where t.Id == p.Staffid
                                                  select new MStaff
                                                  {
                                                      Staffname = t.Staffname
                                                  }).FirstOrDefault(),
                                         Staffid = p.Staffid,
                                         Userid = p.Userid,

                                     }).ToListAsync();

            if (staffSalary == null)
            {
                return NotFound();
            }

            return Ok(staffSalary);
        }

        // GET: api/StaffSalaries/5
        [HttpGet, Route("getreportbymonth/{month}/{year}")]
        public async Task<IActionResult> GetStaffSalaryReportByMonth([FromRoute] int month, int year)
        {
            var staffSalary = await (from p in _context.StaffSalary
                                     where p.Datepaid.Value.Month == month && p.Datepaid.Value.Year == year
                                     select new StaffSalary
                                     {
                                         Adjustments = p.Adjustments,
                                         Datecreated = p.Datecreated,
                                         Datepaid = p.Datepaid,
                                         Deductions = p.Deductions,
                                         Earnings = p.Earnings,
                                         Netpay = p.Netpay,
                                         Id = p.Id,
                                         Ispaid = p.Ispaid,
                                         Month = p.Month,
                                         Year = p.Year,
                                         Staff = (from t in _context.MStaff
                                                  where t.Id == p.Staffid
                                                  select new MStaff
                                                  {
                                                      Staffname = t.Staffname
                                                  }).FirstOrDefault(),
                                         Staffid = p.Staffid,
                                         Userid = p.Userid,

                                     }).ToListAsync();

            if (staffSalary == null)
            {
                return NotFound();
            }

            return Ok(staffSalary);
        }

        // GET: api/StaffSalaries/5
        [HttpGet, Route("generatesalariesbymonth/{month}/{year}")]
        public async Task<IActionResult> GenerateSalaryByMonth([FromRoute] int month, int year)
        {
            int userid = GetUserId();
            DateTime monthEndDate = new DateTime(year, month, DateTime.DaysInMonth(year, month));
            DateTime monthStartDate = new DateTime(year, month, 1);

            //get staff only whose DOJ is less than selected month and year
            var staff = await (from p in _context.MStaff
                               where p.Disabled == false && p.Doj.Value.Date <= monthEndDate.Date &&
                                     p.Dol.HasValue ? p.Dol.Value.Date >= monthStartDate.Date :
                                     p.Doj.Value.Date <= monthEndDate.Date
                               select p).ToListAsync();

            var staffSalaries = await (from p in _context.StaffSalary
                                       where p.Month == month  && p.Year == year
                                       select p).ToListAsync();

            foreach (var s in staff)
            {
                var q = staffSalaries.Where(e => e.Staffid == s.Id).FirstOrDefault();
                if (q == null)
                {
                    //StaffAttendancesController staffAttendancesController = new StaffAttendancesController(_context);
                    //decimal totalPresenty = staffAttendancesController.GetTotalPresentyByStaffId(s.Id, month, year);

                    StaffAttendance1Controller staffAttendance1Controller = new StaffAttendance1Controller(_context);
                    decimal totalPresenty = staffAttendance1Controller.GetTotalPresentyByStaffId(s.Id, month, year);


                    int totalDaysInMonth = DateTime.DaysInMonth(year, month);

                    decimal absenty = totalDaysInMonth - totalPresenty;


                    StaffSalary staffSalary = new StaffSalary();
                    staffSalary.StaffSalaryDetails = GetStaffSalaryDetails(s.Id, month);

                    decimal? earnings = staffSalary.StaffSalaryDetails.Where(e => e.Type == "Earnings").Select(e => e.Amount).Sum();

                    decimal? totalAbsentyAmount = (absenty * (earnings / totalDaysInMonth));

                    if (totalAbsentyAmount > 0)
                    {
                        StaffSalaryDetails staffSalaryDetails = new StaffSalaryDetails();
                        staffSalaryDetails.Amount = totalAbsentyAmount;
                        staffSalaryDetails.Head = "Loss of Pay";
                        staffSalaryDetails.Staffid = s.Id;
                        staffSalaryDetails.Type = "Deductions";

                        staffSalary.StaffSalaryDetails.Add(staffSalaryDetails);
                    }


                    staffSalary.Ispaid = false;
                    staffSalary.Adjustments = 0;
                    staffSalary.Datecreated = GetTimeZoneDate(DateTime.UtcNow);
                    staffSalary.Deductions = staffSalary.StaffSalaryDetails.Where(e => e.Type == "Deductions").Select(e => e.Amount).Sum();
                    staffSalary.Earnings = staffSalary.StaffSalaryDetails.Where(e => e.Type == "Earnings").Select(e => e.Amount).Sum();
                    staffSalary.Netpay = (staffSalary.Earnings - staffSalary.Deductions);
                    staffSalary.Staffid = s.Id;
                    staffSalary.Userid = userid;
                    staffSalary.Month = month;
                    staffSalary.Year = year;

                    _context.StaffSalary.Add(staffSalary);
                }

            }
            _context.SaveChanges();
            return Ok();
        }


        // GET: api/StaffSalaries/5
        [HttpGet, Route("generatesalariesbymonth_notusing/{month}/{year}")]
        public async Task<IActionResult> GenerateSalaryByMonth_notusing([FromRoute] int month, int year)
        {
            int userid = GetUserId();
            DateTime monthEndDate = new DateTime(year, month, DateTime.DaysInMonth(year, month));
            DateTime monthStartDate = new DateTime(year, month, 1);

            //get staff only whose DOJ is less than selected month and year
            var staff = await (from p in _context.MStaff
                               where p.Disabled == false && p.Doj.Value.Date <= monthEndDate.Date &&
                                     p.Dol.HasValue ? p.Dol.Value.Date >= monthStartDate.Date :
                                     p.Doj.Value.Date <= monthEndDate.Date
                               select p).ToListAsync();
            var staffSalaries = await (from p in _context.StaffSalary
                                       where p.Month == month  && p.Year == year
                                       select p).ToListAsync();

            foreach (var s in staff)
            {
                var q = staffSalaries.Where(e => e.Staffid == s.Id).FirstOrDefault();
                if (q == null)
                {
                    StaffSalary staffSalary = new StaffSalary();
                    staffSalary.StaffSalaryDetails = GetStaffSalaryDetails(s.Id, month);

                    staffSalary.Ispaid = false;
                    staffSalary.Adjustments = 0;
                    staffSalary.Datecreated = GetTimeZoneDate(DateTime.UtcNow);
                    staffSalary.Deductions = staffSalary.StaffSalaryDetails.Where(e => e.Type == "Deductions").Select(e => e.Amount).Sum();
                    staffSalary.Earnings = staffSalary.StaffSalaryDetails.Where(e => e.Type == "Earnings").Select(e => e.Amount).Sum();
                    staffSalary.Netpay = (staffSalary.Earnings - staffSalary.Deductions);
                    staffSalary.Staffid = s.Id;
                    staffSalary.Userid = userid;
                    staffSalary.Month = month;
                    staffSalary.Year = year;

                    _context.StaffSalary.Add(staffSalary);
                }

            }
            _context.SaveChanges();
            return Ok();
        }

        private ICollection<StaffSalaryDetails> GetStaffSalaryDetails(int staffid, int month)
        {
            List<StaffSalaryDetails> list = new List<StaffSalaryDetails>();

            var staffpayroll = (from p in _context.MStaffPayroll where p.Staffid == staffid select p).ToList();
            foreach (var spayroll in staffpayroll)
            {
                StaffSalaryDetails staffSalaryDetails = new StaffSalaryDetails();
                staffSalaryDetails.Amount = spayroll.Amount;
                staffSalaryDetails.Head = spayroll.Head;
                staffSalaryDetails.Staffid = staffid;
                staffSalaryDetails.Type = spayroll.Type;


                list.Add(staffSalaryDetails);
            }

            return list;
        }
       

        // PUT: api/StaffSalaries/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStaffSalary([FromRoute] int id, [FromBody] StaffSalary staffSalary)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != staffSalary.Id)
            {
                return BadRequest();
            }

            staffSalary.Datecreated = GetTimeZoneDate(DateTime.UtcNow);
            _context.Entry(staffSalary).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StaffSalaryExists(id))
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

        // PUT: api/StaffSalaries/5
        [HttpPut, Route("salarypaid/{id}")]
        public async Task<IActionResult> PaidStaffSalary([FromRoute] int id, [FromBody] StaffSalary staffSalary)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != staffSalary.Id)
            {
                return BadRequest();
            }

            staffSalary.Ispaid = true;
            staffSalary.Datepaid = GetTimeZoneDate(DateTime.UtcNow);
            _context.Entry(staffSalary).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StaffSalaryExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok();
        }

        // POST: api/StaffSalaries
        [HttpPost]
        public async Task<IActionResult> PostStaffSalary([FromBody] StaffSalary staffSalary)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.StaffSalary.Add(staffSalary);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStaffSalary", new { id = staffSalary.Id }, staffSalary);
        }

        // DELETE: api/StaffSalaries/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStaffSalary([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var staffSalary = await _context.StaffSalary.FindAsync(id);

            if (staffSalary == null)
            {
                return NotFound();
            }

            staffSalary.StaffSalaryDetails = await _context.StaffSalaryDetails.Where(e => e.SsId == staffSalary.Id).ToListAsync();

            foreach (var item in staffSalary.StaffSalaryDetails)
            {
                _context.StaffSalaryDetails.Remove(item);
            }

            _context.StaffSalary.Remove(staffSalary);

            await _context.SaveChangesAsync();

            return Ok(staffSalary);
        }

        private bool StaffSalaryExists(int id)
        {
            return _context.StaffSalary.Any(e => e.Id == id);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}