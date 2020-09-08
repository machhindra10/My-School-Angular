using Microsoft.AspNetCore.Mvc;
using MySchool.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace MySchool.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashbaordController : BaseController, IDisposable
    {
        private readonly ngSchoolContext _context;

        public DashbaordController(ngSchoolContext context)
        {
            _context = context;
        }

        // GET api/values/5
        [HttpGet, Route("getadmindashboarddata/{month}")]
        public IActionResult GetAdminDashboardData(int month)
        {
            int batchid = GetBatchId();
            DateTime dt = GetTimeZoneDate(DateTime.UtcNow);

            dataCount dataCount = new dataCount();

            dataCount.students = (from p in _context.Student select p).Count();
            dataCount.admitted = (from p in _context.TStudentAdmission
                                  where p.Batchid == batchid
                                  select p.Id).Count();
            dataCount.collections = (from p in _context.TStudentPayment
                                     where p.Datecreated.Value.Year == dt.Year && p.Datecreated.Value.Month == dt.Month
                                     && p.Batchid == batchid
                                     select p.Amount).Sum();
            dataCount.expenses = (from p in _context.DailyExpenses
                                  where p.Datecreated.Value.Year == dt.Year && p.Datecreated.Value.Month == dt.Month                                  
                                  select p.Amount).Sum();
            dataCount.salaries = (from p in _context.StaffSalary
                                  where p.Datepaid.Value.Year == dt.Year && p.Datepaid.Value.Month == dt.Month && p.Ispaid == true
                                  select p.Netpay).Sum();
            dataCount.profit = (dataCount.collections - (dataCount.expenses + dataCount.salaries));
            dataCount.month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(dt.Month)
                                    + " " + dt.Year;

            return Ok(dataCount);
        }

        // GET api/values/5
        [HttpGet, Route("getadmindashboardstatsdata/{month}")]
        public IActionResult GetAdminDashboardStatsData(int month)
        {
            //int year = GetYear();
            DateTime dt = GetTimeZoneDate(DateTime.UtcNow);
            int batchid = GetBatchId();

            StatsData statsData = new StatsData();
            List<chartdata> chartdatasE = new List<chartdata>();
            List<chartdata> chartdatasS = new List<chartdata>();
            List<chartdata> chartdatasC = new List<chartdata>();

            var lExpenses = (from p in _context.DailyExpenses
                             where p.Datecreated.Value.Year == dt.Year && p.Datecreated.Value.Month == dt.Month
                             group p by new { p.Datecreated.Value.Day } into g
                             select new
                             {
                                 name = g.Key.Day,
                                 y = g.Select(e => e.Amount).Sum()
                             }).ToList();

            var lCollections = (from p in _context.TStudentPayment
                                where p.Datecreated.Value.Year == dt.Year && p.Datecreated.Value.Month == dt.Month
                                && p.Batchid == batchid
                                group p by new { p.Datecreated.Value.Day } into g
                                select new
                                {
                                    name = g.Key.Day,
                                    y = g.Select(e => e.Amount).Sum()
                                }).ToList();

            var lSalaries = (from p in _context.StaffSalary
                             where p.Datepaid.Value.Year == dt.Year && p.Datepaid.Value.Month == dt.Month && p.Ispaid == true
                             group p by new { p.Datepaid.Value.Day } into g
                             select new
                             {
                                 name = g.Key.Day,
                                 y = g.Select(e => e.Netpay).Sum()
                             }).ToList();

            for (int i = 1; i <= DateTime.DaysInMonth(dt.Year, dt.Month); i++)
            {
                var e1 = lExpenses.Where(e => e.name == i).FirstOrDefault();
                if (e1 == null)
                    chartdatasE.Add(new chartdata { name = i, y = 0 });
                else
                    chartdatasE.Add(new chartdata { name = i, y = e1.y });

                var s = lSalaries.Where(e => e.name == i).FirstOrDefault();
                if (s == null)
                    chartdatasS.Add(new chartdata { name = i, y = 0 });
                else
                    chartdatasS.Add(new chartdata { name = i, y = s.y });


                var c = lCollections.Where(e => e.name == i).FirstOrDefault();
                if (c == null)
                    chartdatasC.Add(new chartdata { name = i, y = 0 });
                else
                    chartdatasC.Add(new chartdata { name = i, y = c.y });


            }

            statsData.collections = chartdatasC;
            statsData.salaries = chartdatasS;
            statsData.expenses = chartdatasE;

            return Ok(statsData);
        }


        public void Dispose()
        {
            _context.Dispose();
        }

        private class dataCount
        {
            public int students { get; set; }
            public int admitted { get; set; }
            public decimal? collections { get; set; }
            public decimal? expenses { get; set; }
            public decimal? salaries { get; set; }
            public decimal? profit { get; set; }
            public string month { get; set; }
        }

        private class StatsData
        {
            public List<chartdata> collections { get; set; }
            public List<chartdata> salaries { get; set; }
            public List<chartdata> expenses { get; set; }
        }

        private class chartdata
        {
            public int name { get; set; }
            public decimal? y { get; set; }
        }
    }
}
