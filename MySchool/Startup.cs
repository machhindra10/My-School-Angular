using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MySchool.Models;
using Newtonsoft.Json;
using System.Text;

namespace MySchool
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            string issuer = Configuration.GetValue<string>("MySettings:ISSUER");
            string secret_key = Configuration.GetValue<string>("MySettings:SECRET_KEY");

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp";
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,
                            ValidIssuer = issuer,
                            ValidAudience = issuer,
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret_key))
                        };
                    });

            services.AddMvc();
            services.AddCors(options =>
            {
                options.AddPolicy("EnableCORS", builder =>
                {
                    builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
                    .Build();
                });
            });

            //services.AddMvc().AddJsonOptions(options =>
            //{
            //    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            //});

            ////Use connection string from appsettings.json
            //services.AddDbContext<ngSchoolContext>(options =>
            //          options.UseSqlServer(Configuration.GetConnectionString("DefaultDatabase")));  

            ////Use dynamic connection string passed from header or querystring parameters
            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
            services.AddDbContext<ngSchoolContext>(((serviceProvider, options) =>
            {
                var httpContext = serviceProvider.GetService<IHttpContextAccessor>().HttpContext;
                if (httpContext != null)
                {
                    var httpRequest = httpContext.Request;

                    // Get the 'database' header parameter from the request (if supplied - default is empty). 
                    var databaseQuerystringParameter = httpRequest.Headers["database"].ToString();

                    if (string.IsNullOrEmpty(databaseQuerystringParameter))
                    {
                        // Get the 'database' querystring parameter from the request (if supplied - default is empty).                
                        databaseQuerystringParameter = httpRequest.Query["database"].ToString();
                    }

                    // Get the base, formatted connection string with the 'DATABASE' paramter missing.
                    var db2ConnectionString = Configuration.GetConnectionString("IbmDb2Formatted");

                    if (!string.IsNullOrEmpty(databaseQuerystringParameter))
                    {
                        // We have a 'database' param, stick it in.
                        db2ConnectionString = string.Format(db2ConnectionString, databaseQuerystringParameter);
                    }
                    else
                    {
                        // We havent been given a 'database' param, use the default.
                        var db2DefaultDatabaseValue = Configuration.GetConnectionString("DefaultDatabase");
                        db2ConnectionString = string.Format(db2ConnectionString, db2DefaultDatabaseValue);
                    }

                    // Build the EF DbContext using the built conn string.
                    options.UseSqlServer(db2ConnectionString);

                }
                else
                {
                    // We havent been given a 'database' param, use the default.
                    var db2DefaultDatabaseValue = Configuration.GetConnectionString("DefaultDatabase");

                    // Build the EF DbContext using the built conn string.
                    options.UseSqlServer(db2DefaultDatabaseValue);
                }
            }));


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();
            app.UseAuthentication();
            app.UseCors("EnableCORS");
            app.UseMvc();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                //spa.Options.SourcePath = "ClientApp";

                //if (env.IsDevelopment())
                //{
                ////spa.UseAngularCliServer(npmScript: "start");
                //}
            });

            //// Migrate and seed the database during startup. Must be synchronous.
            //using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            //{
            //    serviceScope.ServiceProvider.GetService<ngSchoolContext>().Database.Migrate();
            //    //serviceScope.ServiceProvider.GetService<ISeedService>().SeedDatabase().Wait();
            //}


        }
    }

}
