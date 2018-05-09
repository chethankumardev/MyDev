using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace WeatherReport.Models
{
    public class WeatherReportContext : DbContext
    {
        public WeatherReportContext (DbContextOptions<WeatherReportContext> options)
            : base(options)
        {
        }

        public DbSet<WeatherReport.Models.WearherReport> WearherReport { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
