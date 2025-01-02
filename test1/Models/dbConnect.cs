using Microsoft.EntityFrameworkCore;
using System.Numerics;
using System.Linq;


namespace test1.Models
{
    public class AppDbContext : DbContext
    {
        public DbSet<Clients> Clients { get; set; } // 對應資料表 Clients
        public DbSet<doctor> Doctor_time { get; set; } // 對應資料表 Doctor_time
        public DbSet<Doctors> Doctors { get; set; }
        public DbSet<Schedules> Schedules { get; set; }
        public DbSet<ScheduleDoctor> ScheduleDoctors { get; set; }

        // 添加構造函數，讓 AddDbContext 可以傳遞配置
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Clients>()
            .HasKey(c => c.ClientId);

            modelBuilder.Entity<ScheduleDoctor>()
            .HasKey(sd => new { sd.ScheduleId, sd.DoctorId });

            modelBuilder.Entity<ScheduleDoctor>()
                .HasOne(sd => sd.Schedule)
                .WithMany(s => s.ScheduleDoctors)
                .HasForeignKey(sd => sd.ScheduleId);

            modelBuilder.Entity<ScheduleDoctor>()
                .HasOne(sd => sd.Doctor)
                .WithMany(d => d.ScheduleDoctors)
                .HasForeignKey(sd => sd.DoctorId);
        }


    }
}
