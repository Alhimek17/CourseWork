using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ServiceStationDatabaseImplement.Models;

namespace ServiceStationDatabaseImplement
{
    public class ServiceStationDatabase : DbContext
    {
        public ServiceStationDatabase()
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=otp;Username=postgres;Password=admin",
                o => o.SetPostgresVersion(12, 2));

            base.OnConfiguring(optionsBuilder);

        }

        public virtual DbSet<Car> Cars { get; set; }
        public virtual DbSet<Defect> Defects { get; set; }
        public virtual DbSet<CarDefect> CarDefects { get; set; }
        public virtual DbSet<Executor> Executors { get; set; }
        public virtual DbSet<TechnicalWork> TechnicalWorks { get; set; }
        public virtual DbSet<CarTechnicalWork> CarTechnicalWorks { get; set; }
        public virtual DbSet<SparePart> SpareParts { get; set; }
        public virtual DbSet<Repair> Repairs { get; set; }
        public virtual DbSet<SparePartRepair> SparePartRepairs { get; set; }
        public virtual DbSet<Guarantor> Guarantors { get; set; }
        public virtual DbSet<Work> Works { get; set; }
        public virtual DbSet<SparePartWork> SparePartWorks { get; set; }
    }
}
