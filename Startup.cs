using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WeatherReport.Data;
using WeatherReport.Models;
using WeatherReport.Services;

namespace WeatherReport
{
    public class Startup
    {
        private string _testSecret;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            _testSecret = Configuration["MySecret"];

            //Google Authentication
            services.AddAuthentication().AddGoogle(googleOptions =>
            {
                googleOptions.ClientId = "184003044969-hjnt2m0t3fdmouet2l6t54ejrj3v3fva.apps.googleusercontent.com";
                googleOptions.ClientSecret = "q4-51ICZG1hIceg6jt8wSvAb";
            });

            services.AddAuthentication().AddFacebook(facebookOptions =>
            {
                facebookOptions.AppId = "851102425083741";
                facebookOptions.AppSecret = "d53da2151028771b6f9f3fd87b20523b";
            });

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();

            services.AddMvc();

            services.AddDbContext<WeatherReportContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("WeatherReportContext")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
