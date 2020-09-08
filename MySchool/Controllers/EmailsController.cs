using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySchool.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using static MySchool.Controllers.EmailController;

namespace MySchool.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailsController : ControllerBase
    {
        private readonly ngSchoolContext _context;
        public IConfiguration Configuration { get; }

        public EmailsController(ngSchoolContext context, IConfiguration configuration)
        {
            _context = context;
            Configuration = configuration;
        }

        public bool SendEmailForgotPassword(MUser user)
        {
            string issuer = Configuration.GetValue<string>("MySettings:ISSUER");

            EmailController emailController = new EmailController(_context);
            emailinfo emailinfo = new emailinfo();
            emailinfo.Address = new List<string>();
            emailinfo.Address.Add(user.Email);
            emailinfo.Subject = "My School App - Password recovery request";

            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append("<span style=\"color: red; font-weight: 800; font-size: 20px;\">My School App</span>");
            stringBuilder.AppendLine("<br/><br/>");
            stringBuilder.Append("If you have requested a password recovery then");
            stringBuilder.AppendLine("<br/><br/>");
            stringBuilder.AppendLine("Please click <a href='" + issuer + "passwordrecovery/" + user.Id + "," + user.Passverificationcode + "' target='blank'>here</a> to verify and create new password");
            stringBuilder.AppendLine("<br/><br/>");
            stringBuilder.AppendLine("Or copy and paste below link in browser window");
            stringBuilder.AppendLine("<br/><br/>");
            stringBuilder.AppendLine("<span style=\"color:blue;\">" + issuer + "passwordrecovery/" + user.Id + "," + user.Passverificationcode + "</span>");
            stringBuilder.AppendLine("<br/><br/>");
            stringBuilder.AppendLine("<span style='font-color:blue; font-size=12px;'><a href='" + issuer + "' target='blank'>visit website</a></span>");
            stringBuilder.AppendLine(" | <span style='font-color:blue; font-size=12px;'><a href='" + issuer + "login' target='blank'>log in to your account</a></span>");
            //stringBuilder.AppendLine(" | <span style='font-color:blue; font-size=12px;'><a href='" + issuer + "contactus' target='blank'>get support</a></span>");

            emailinfo.Body = stringBuilder.ToString();

            return emailController.SendEmail(emailinfo);

        }
    }
}