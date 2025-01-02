using Microsoft.EntityFrameworkCore;
using System.Numerics;

namespace test1.Models
{
    public class AppDbContext : DbContext
    {
        public DbSet<Clients> Clients { get; set; } // 對應資料表 Clients
        public DbSet<doctor> Doctor_time { get; set; } // 對應資料表 Doctor_time
        public DbSet<Doctors> Doctors { get; set; } // 對應資料表 Doctors
        public DbSet<Schedules> Schedules { get; set; } // 對應資料表 Schedules

        // 添加構造函數，讓 AddDbContext 可以傳遞配置
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Clients>()
            .HasKey(c => c.ClientId);

            // 指定 Doctors 資料表
            modelBuilder.Entity<Doctors>().ToTable("Doctors");
            modelBuilder.Entity<Doctors>().HasKey(d => d.Doctor_id); // 設定主鍵

            // 指定 Schedules 資料表
            modelBuilder.Entity<Schedules>().ToTable("Schedules");
            modelBuilder.Entity<Schedules>().HasKey(d => d.Schedule_id); // 設定主鍵

            //-------------------------外來鍵設定------------------------

            modelBuilder.Entity<Schedules>()
                .HasOne(r => r.Doctors) // 與 Doctors 的一對一關聯
                .WithMany() // Schedule 不需要反向導航
                .HasForeignKey(r => r.Doctor_id); // 外鍵是 Client_id
        }


    }
}
