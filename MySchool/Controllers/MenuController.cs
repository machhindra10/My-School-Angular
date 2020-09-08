using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySchool.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MySchool.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuController : BaseController, IDisposable
    {
        private readonly ngSchoolContext _context;

        public MenuController(ngSchoolContext context)
        {
            _context = context;
        }

        // GET: api/Menu
        //[HttpGet]
        //public List<Menu> Get()
        //{
        //    var q = (from p in db.Menu select p).ToList();

        //    return q;
        //}



        // GET: api/Menu/5
        //[HttpGet("{id}", Name = "Get")]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            //List<Menu> temp = null;

            try
            {
                int roleid = this.GetUserRoleId();
                var rolerights = await (from y in _context.MRoleRights
                                  where y.RoleId == roleid || y.Right.Url == null
                                  select new MRoleRights
                                  {
                                      RightId = y.RightId,
                                      Right = y.Right
                                  }).Distinct().ToListAsync();

                var array = rolerights.Select(x => x.RightId).ToList();

                var arry2 = rolerights.Where(x => x.Right.Parentid != null).Select(x => x.Right.Parentid).Distinct().ToList();

                foreach (int? item in arry2)
                {
                    array.Add(Convert.ToInt32(item));
                }

                var temp = (from m in _context.Menu
                            orderby m.Sort
                            select new
                            {
                                Id = m.Id,
                                Icon = m.Icon,
                                Sort = m.Sort,
                                Menu1 = m.Menu1,
                                Url = m.Url,
                                Visible = m.Visible,
                                MRights = (from sm in _context.MRights
                                           where sm.Visible == true && array.Contains(sm.Id) && sm.Menuid == m.Id
                                           && sm.Parentid == null
                                           orderby sm.Menuid, sm.Sort
                                           select new
                                           {
                                               url = sm.Url,
                                               rname = sm.Rname,
                                               MRights = (from sub in _context.MRights
                                                          where sub.Visible == true && sub.Parentid == sm.Id && sub.Parentid != null
                                                          && array.Contains(sub.Id)
                                                          orderby sub.Menuid, sub.Sort
                                                          select new
                                                          {
                                                              url = sub.Url,
                                                              rname = sub.Rname
                                                          }).ToList()
                                           }).ToList()
                            }).Where(t => t.MRights.Count > 0).ToList();

                return Ok(temp);

            }
            catch (Exception)
            {
                return NotFound();
            }

        }

        [HttpGet, Route("getlogo")]
        public async Task<IActionResult> GetLogo()
        {

            try
            {
                var lg = await (from y in _context.Settings
                                where y.Id == 1
                                select y.Logo).FirstOrDefaultAsync();
                return Ok(new { logo = lg });
            }
            catch (Exception)
            {
                return Ok(new { logo = "" });
                //throw;
            }

        }
        // POST: api/Menu
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Menu/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
