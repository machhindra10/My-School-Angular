using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MySchool.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace MySchool.Controllers
{
    [Route("api/authmobile")]
    public class AuthMobileController : Controller
    {
        private readonly ngSchoolContext _context;

        public AuthMobileController(ngSchoolContext context, IConfiguration configuration)
        {
            _context = context;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // GET api/values
        [HttpPost, Route("parentlogin")]
        public IActionResult Login([FromBody]Guardian guardianUser)
        {
            if (guardianUser == null)
            {
                return BadRequest("Invalid client request");
            }

            StudentGuardian guardian = CheckGuardianLogin(guardianUser);

            if (guardian != null)
            {
                string orgtoken = string.Empty;
                string currencyCode = "INR";
                Settings settings = GetSettings();
                if (settings != null)
                {
                    orgtoken = settings.Token;
                    currencyCode = settings.Currency;
                }

                string issuer = Configuration.GetValue<string>("MySettings:ISSUER");
                string secret_key = Configuration.GetValue<string>("MySettings:SECRET_KEY");

                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret_key));
                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

                var claims = new List<Claim>
                {
                    new Claim("guardianid", guardian.Id.ToString()),
                    new Claim("guardianname", guardian.Name),                    
                    new Claim("mobile", guardian.Mobile.ToString()),                                    
                    new Claim("orgtoken", orgtoken),
                    new Claim("currcode", currencyCode),
                    new Claim("tz", settings.Timezoneid),
                    new Claim("batchid", GetBatchIdFromDatabase().ToString()),
                };

                var tokeOptions = new JwtSecurityToken(
                    issuer: issuer,
                    audience: issuer,
                    claims: claims,
                    expires: DateTime.Now.AddDays(60),
                    signingCredentials: signinCredentials
                );                

                var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);              

                return Ok(new { Token = tokenString });
            }
            else
            {
                return Unauthorized();
            }
        }

        private StudentGuardian CheckGuardianLogin(Guardian user)
        {            
            string pass = Encryption.EncryptString(user.Password);

            var q = (from p in _context.StudentGuardian
                     where p.Mobile == user.Mobile && p.Password == pass && p.Disabled == false
                     select p).FirstOrDefault();            
            return q;
        }

        private int? GetBatchIdFromDatabase()
        {
            int? q = (from p in _context.Batches
                      where p.Isactive == true
                      select p.Id).FirstOrDefault();
            if (q == null)
            {
                q = 0;
            }
            return q;
        }

        private Settings GetSettings()
        {
            return (from p in _context.Settings
                        //where p.Id == 1
                    select p).FirstOrDefault();
        }

        private List<Claim> GetRightIDsToken(int roleid)
        {
            RoleRightsController roleRightsController = new RoleRightsController(_context);

            var ids = roleRightsController.GetRightIDsofRole(roleid);

            string jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(ids);

            var claims = new List<Claim>
                {
                    new Claim("ids", jsonString)
                };

            return claims;
        }

        private int GetAssociateStaffId(int userid)
        {
            int id = 0;
            var q = _context.MStaff.Where(p => p.Associateuserid == userid).FirstOrDefault();
            if(q != null)
            {
                id = q.Id;
            }
            return id;
        }
    }

    public class Guardian
    {
        public long Mobile { get; set; }
        public string Password { get; set; }
    }
}
