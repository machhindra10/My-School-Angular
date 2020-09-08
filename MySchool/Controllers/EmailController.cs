using MySchool.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace MySchool.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly ngSchoolContext _context;


        public EmailController(ngSchoolContext context)
        {
            _context = context;
        }

        [HttpGet, Route("send")]
        public bool SendEmail(emailinfo emailinfo)
        {
            SettingsOthersController settingsOthersController = new SettingsOthersController(_context);
            SettingsOther settings = settingsOthersController.GetSettingsOthersFirstOnly();

            try
            {
                string emailid = settings.Smtpemailid;
                string pwd = Encryption.DecryptString(settings.Smtppassword);

                MailMessage objeto_mail = new MailMessage();
                objeto_mail.IsBodyHtml = true;

                if (emailinfo.BccToSupport)
                {
                    if (!string.IsNullOrEmpty(settings.Smtpbccid))
                    {
                        objeto_mail.Bcc.Add(settings.Smtpbccid);
                    }
                }

                SmtpClient client = new SmtpClient();
                client.Port = settings.Smtpport;
                client.Timeout = 10000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Host = settings.Smtphost;
                client.EnableSsl = settings.Smtpenablessl;
                client.Credentials = new System.Net.NetworkCredential(emailid, pwd);
                if(string.IsNullOrEmpty(emailinfo.From))
                {
                    objeto_mail.From = new MailAddress(emailid);
                }
                else
                {
                    objeto_mail.From = new MailAddress(emailinfo.From);
                }
                objeto_mail.BodyEncoding = UTF8Encoding.UTF8;
                objeto_mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                foreach (var item in emailinfo.Address)
                {
                    objeto_mail.To.Add(new MailAddress(item));
                }
                objeto_mail.Subject = emailinfo.Subject;

                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.AppendLine("<br/><br/>");
                stringBuilder.AppendLine("<span style='color:gray;font-style:italic;font-size:12px;'>" + "please do not reply to this email, in case any issue please reply directly to " + settings.Smtpbccid + "</span>");

                objeto_mail.Body = emailinfo.Body + stringBuilder.ToString();

                client.Send(objeto_mail);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                settings = null;
                settingsOthersController = null;
            }
        }

        public class emailinfo
        {
            private bool _bcctosupport = true;

            public string From { get; set; }
            public string Subject { get; set; }
            public string Body { get; set; }
            public List<string> Address { get; set; }
            public bool BccToSupport { get { return _bcctosupport; } set { _bcctosupport = value; } }
        }

    }
}