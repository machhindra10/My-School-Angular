using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MySchool.Models;

namespace MySchool
{
    public class Program
    {
        public static void Main(string[] args)
        {

            CreateWebHostBuilder(args).Build().Run();
            //var host = CreateWebHostBuilder(args).Build();
            //using (var serviceScope = host.Services.CreateScope())
            //{
            //    var context = serviceScope.ServiceProvider.GetRequiredService<ngSchoolContext>();
            //    context.Database.EnsureCreated();
            //}
            //host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
