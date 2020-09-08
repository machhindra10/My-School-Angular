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
    public class MessagesController : BaseController, IDisposable
    {
        private readonly ngSchoolContext _context;

        public MessagesController(ngSchoolContext context)
        {
            _context = context;
        }

        // GET: api/Messages
        [HttpGet]
        public IEnumerable<Messages> GetMessages()
        {
            return _context.Messages;
        }

        // GET: api/Messages/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMessages([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var messages = await _context.Messages.FindAsync(id);


            if (messages == null)
            {
                return NotFound();
            }

            return Ok(messages);
        }

        // PUT: api/Messages/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMessages([FromRoute] long id, [FromBody] Messages messages)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != messages.Id)
            {
                return BadRequest();
            }

            _context.Entry(messages).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MessagesExists(id))
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

        // POST: api/Messages
        [HttpPost]
        public async Task<IActionResult> PostMessages([FromBody] Messages messages)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            messages.Datecreated = GetTimeZoneDate(DateTime.UtcNow);
            messages.Userid = GetUserId();
            _context.Messages.Add(messages);

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMessages", new { id = messages.Id }, messages);
        }

        // DELETE: api/Messages/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMessages([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var messages = await _context.Messages.FindAsync(id);
            if (messages == null)
            {
                return NotFound();
            }

            var g = await (from p in _context.MessagesGuardians where p.Messageid == id select p).ToListAsync();
            foreach (var item in g)
            {
                _context.MessagesGuardians.Remove(item);
            }
            var s = await (from p in _context.MessagesStudents where p.Messageid == id select p).ToListAsync();
            foreach (var item in s)
            {
                _context.MessagesStudents.Remove(item);
            }

            _context.Messages.Remove(messages);
            await _context.SaveChangesAsync();

            return Ok(messages);
        }

        // GET: api/Messages/5
        [HttpGet, Route("getmessagedetails/{id}")]
        public async Task<IActionResult> GetMessagesById([FromRoute] long id)
        {
            var message = await (from p in _context.Messages
                                 where p.Id == id
                                 select new
                                 {
                                     Id = p.Id,
                                     Datecreated = p.Datecreated,
                                     Message = p.Message,
                                     Type = p.Type,
                                     Userid = p.Userid,
                                     MessagesStudents = (from ms in _context.MessagesStudents orderby ms.Delivered descending orderby ms.Read descending
                                                         where ms.Messageid == id && ms.Delivered != null
                                                         select new 
                                                         {
                                                             Id = ms.Id,                                                            
                                                             Delivered = ms.Delivered,
                                                             Read = ms.Read,
                                                             Student = (from s in _context.Student where s.Id == ms.Studentid 
                                                                        select new
                                                                        {
                                                                            Id = s.Id,
                                                                            Name = s.Fname + " " + s.Mname + " " + s.Lname,
                                                                            Mobile = s.Mobile
                                                                        }).FirstOrDefault()
                                                         }).ToList(),
                                     StudentsUnreadCount = (from ms in _context.MessagesStudents
                                                         where ms.Messageid == id && ms.Delivered == null
                                                         select ms).Count(),
                                     MessagesGuardians = (from mg in _context.MessagesGuardians
                                                          orderby mg.Delivered descending
                                                          orderby mg.Read descending
                                                          where mg.Messageid == id && mg.Delivered != null
                                                          select new
                                                          {
                                                              Id = mg.Id,
                                                              Delivered = mg.Delivered,
                                                              Read = mg.Read,
                                                              Guardian = (from g in _context.StudentGuardian
                                                                         where g.Id == mg.Guardianid
                                                                         select new
                                                                         {
                                                                             Id = g.Id,
                                                                             Name = g.Name,
                                                                             Mobile = g.Mobile
                                                                         }).FirstOrDefault()
                                                          }).ToList(),
                                     GuardiansUnreadCount = (from mg in _context.MessagesGuardians
                                                          where mg.Messageid == id && mg.Delivered == null
                                                             select mg).Count(),
                                 }).FirstOrDefaultAsync();


            if (message == null)
            {
                return NotFound();
            }

            return Ok(message);
        }

        // GET: api/Messages/5
        [HttpGet, Route("getallmessages")]
        public async Task<IActionResult> GetAllMessages()
        {
            var messages = await (from p in _context.Messages
                                  select new
                                  {
                                      Id = p.Id,
                                      Datecreated = p.Datecreated,
                                      Message = p.Message,
                                      Type = p.Type,
                                      Userid = p.Userid,
                                      MessagesStudents = p.MessagesStudents,
                                      MessagesGuardians = p.MessagesGuardians,
                                      CountRecepients = p.MessagesGuardians.Count() + p.MessagesStudents.Count()
                                  }).ToListAsync();


            if (messages == null)
            {
                return NotFound();
            }

            return Ok(messages);
        }

        [HttpPost, Route("sendcommonmessage")]
        public async Task<IActionResult> SendSMS([FromBody] messagevm messagevm)
        {
            int batchid = GetBatchId();
            //List<int?> classidsArray = new List<int?>(Array.ConvertAll(messagevm.classids.Split(','),
            //                new Converter<string, int?>((s) => { return Convert.ToInt32(s); })));            

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

            Messages messages = new Messages();
            messages.Message = messagevm.Message;
            messages.Type = "WEB";
            messages.Datecreated = GetTimeZoneDate(DateTime.UtcNow);
            messages.Userid = GetUserId();
            _context.Messages.Add(messages);


            if (messagevm.Towhom.Contains("P"))
            {
                foreach (var guardianid in students.Select(p => p.Guardianid).Distinct())
                {
                    MessagesGuardians messagesGuardians = new MessagesGuardians();
                    messagesGuardians.Messageid = messages.Id;
                    messagesGuardians.Guardianid = Convert.ToInt64(guardianid);
                    _context.MessagesGuardians.Add(messagesGuardians);
                    messagesGuardians = null;
                }
            }
            if (messagevm.Towhom.Contains("S"))
            {
                foreach (var student in students)
                {
                    MessagesStudents messagesStudents = new MessagesStudents();
                    messagesStudents.Messageid = messages.Id;
                    messagesStudents.Studentid = student.Id;
                    _context.MessagesStudents.Add(messagesStudents);
                    messagesStudents = null;
                }
            }


            await _context.SaveChangesAsync();

            return Ok(new { result = true });
        }

        private bool MessagesExists(long id)
        {
            return _context.Messages.Any(e => e.Id == id);
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