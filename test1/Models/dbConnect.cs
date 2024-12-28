using Microsoft.EntityFrameworkCore;
using System.Numerics;

namespace test1.Models
{
    public class AppDbContext : DbContext
    {
        public DbSet<client> client { get; set; } // 對應資料表 clients
        public DbSet<doctor> Doctor_time { get; set; } // 對應資料表 Doctor_time
        public DbSet<Doctors> Doctors { get; set; } // 對應資料表 Doctors
        public DbSet<Reservations> Reservations { get; set; } // 對應資料表 Reservation
        public DbSet<Schedules> Schedules { get; set; } // 對應資料表 Schedules

        // 添加構造函數，讓 AddDbContext 可以傳遞配置
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 指定資料表名稱
            modelBuilder.Entity<client>().ToTable("client");

            // 設定主鍵
            modelBuilder.Entity<client>().HasKey(c => c.Client_id);

            // 指定 Doctors 資料表
            modelBuilder.Entity<Doctors>().ToTable("Doctors");
            modelBuilder.Entity<Doctors>().HasKey(d => d.Doctor_id); // 設定主鍵

            // 指定 Reservation 資料表
            modelBuilder.Entity<Reservations>().ToTable("Reservation");
            modelBuilder.Entity<Reservations>().HasKey(d => d.Reserve_id); // 設定主鍵

            // 指定 Schedules 資料表
            modelBuilder.Entity<Schedules>().ToTable("Schedules");
            modelBuilder.Entity<Schedules>().HasKey(d => d.Schedule_id); // 設定主鍵

            //-------------------------外來鍵設定-------------------------------------
            modelBuilder.Entity<Reservations>()
            .HasOne(r => r.client) // 與 client 的一對一關聯
            .WithMany() // client 不需要反向導航
            .HasForeignKey(r => r.Client_id); // 外鍵是 Client_id

            modelBuilder.Entity<Reservations>()
                .HasOne(r => r.Schedules) // 與 Schedule 的一對一關聯
                .WithMany() // Schedule 不需要反向導航
                .HasForeignKey(r => r.Schedule_id); // 外鍵是 Schedule_id

            modelBuilder.Entity<Schedules>()
                .HasOne(r => r.Doctors) // 與 Doctors 的一對一關聯
                .WithMany() // Schedule 不需要反向導航
                .HasForeignKey(r => r.Doctor_id); // 外鍵是 Client_id
        }


    }
}
