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
    public class SMSsController : BaseController, IDisposable
    {
        private readonly ngSchoolContext _context;

        public SMSsController(ngSchoolContext context)
        {
            _context = context;
        }

        [HttpPost, Route("sendcommonsms")]
        public async Task<IActionResult> SendSMS([FromBody] messagevm messagevm)
        {
            int batchid = this.GetBatchId();            

            var studentids = await (from p in _context.TStudentAdmission
                                    where messagevm.Classids.Contains(p.Classid) && p.Batchid == batchid
                                    select p.Studentid).ToListAsync();

            var students = await (from p in _context.Student
                                  where studentids.Contains(p.Id)
                                  select new Student
                                  {
                                      Id = p.Id,
                                      Guardianid = p.Guardianid,
                                      Guardian = p.Guardian,
                                      Mobile = p.Mobile
                                  }).ToListAsync();

            List<string> numbers = new List<string>();
            var guar_numbers = students.Select(x => x.Guardian.Mobile.ToString()).ToList();
            var stud_numbers = students.Select(x => x.Mobile).ToList();

            if (messagevm.Towhom.Count == 1)
            {
                if (messagevm.Towhom.Contains("P"))
                {
                    numbers.AddRange(guar_numbers);
                }
                else if (messagevm.Towhom.Contains("S"))
                {
                    numbers.AddRange(stud_numbers);
                }
            }
            else
            {
                numbers.AddRange(guar_numbers);
                numbers.AddRange(stud_numbers);
            }

            string str_numbers = string.Join(",", numbers.Where(x => x != null || x != "").Distinct());

            SMSController sMSController = new SMSController(_context);
            string result = sMSController.SendSMS(messagevm.Message, str_numbers);

            return Ok(result);
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public class messagevm
        {
            public ICollection<int?> Classids { get; set; }
            public ICollection<string> Towhom { get; set; }
            public string Message { get; set; }
        }
    }
}