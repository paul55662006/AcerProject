using Microsoft.EntityFrameworkCore;
using System.Numerics;
using System.Linq;


namespace test1.Models
{
    public class AppDbContext : DbContext
    {
        public DbSet<Clients> Clients { get; set; } // 對應資料表 Clients
        public DbSet<Doctors> Doctors { get; set; }
        public DbSet<Schedules> Schedules { get; set; }

        // 添加構造函數，讓 AddDbContext 可以傳遞配置
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Clients>()
            .HasKey(c => c.ClientId);
        }


    }
}
