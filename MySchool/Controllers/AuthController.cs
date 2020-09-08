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
    [Route("api/auth")]
    public class AuthController : Controller
    {
        private readonly ngSchoolContext _context;

        public AuthController(ngSchoolContext context, IConfiguration configuration)
        {
            _context = context;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // GET api/values
        [HttpPost, Route("login")]
        public IActionResult Login([FromBody]LoginModel user)
        {

            if (user == null)
            {
                return BadRequest("Invalid client request");
            }

            MUser mUser = CheckUserLogin(user);

            if (mUser != null)
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
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim("userid", mUser.Id.ToString()),
                    new Claim("userroleid", mUser.RoleId.ToString()),
                    new Claim("ismasteradmin", mUser.IsMasterAdmin.ToString()),
                    new Claim("batchid", GetBatchIdFromDatabase().ToString()),
                    new Claim("rolename", mUser.Role.Rolename.ToString()),
                    new Claim("orgtoken", orgtoken),
                    new Claim("currcode", currencyCode),
                    new Claim("tz", settings.Timezoneid),
                    new Claim("staffid", GetAssociateStaffId(mUser.Id).ToString())
                };

                var tokeOptions = new JwtSecurityToken(
                    issuer: issuer,
                    audience: issuer,
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: signinCredentials
                );

                var tokeOptionRR = new JwtSecurityToken(
                   issuer: issuer,
                   audience: issuer,
                   claims: GetRightIDsToken(mUser.RoleId),
                   expires: DateTime.Now.AddMinutes(30),
                   signingCredentials: signinCredentials
                );

                var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
                var tokenStringRoleRights = new JwtSecurityTokenHandler().WriteToken(tokeOptionRR);

                return Ok(new { Token = tokenString, rrtoken = tokenStringRoleRights });
            }
            else
            {
                return Unauthorized();
            }
        }

        private MUser CheckUserLogin(LoginModel user)
        {
            string username = user.UserName;
            string pass = Encryption.EncryptString(user.Password);

            var q = (from p in _context.MUser
                     where p.Email == username && p.Password == pass && p.Disabled == false
                     select new MUser
                     {
                         Id = p.Id,
                         RoleId = p.RoleId,
                         IsMasterAdmin = p.IsMasterAdmin,
                         UserName = p.UserName,
                         Role = p.Role, Aadharno = p.Aadharno, DateCreated = p.DateCreated, DateModified = p.DateModified,
                         Disabled = p.Disabled, Email = p.Email, Fname = p.Fname, IsAdmin = p.IsAdmin,Lastlogin = p.Lastlogin,
                         Lname = p.Lname,Mname = p.Mname,Passverificationcode = p.Passverificationcode,Password = p.Password,
                         Photo = p.Photo,Userid = p.Userid, Currentlogin = p.Currentlogin
                     }).FirstOrDefault();
            if(q != null)
            {
                Settings settings = GetSettings();
                //To do need to change to Localtime

                q.Lastlogin = q.Currentlogin;

                TimeZoneInfo infotime = TimeZoneInfo.FindSystemTimeZoneById(settings.Timezoneid);                
                q.Currentlogin = TimeZoneInfo.ConvertTime(DateTime.UtcNow, infotime); 
                _context.Entry(q).State = EntityState.Modified;
                _context.SaveChanges();
            }
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
}
