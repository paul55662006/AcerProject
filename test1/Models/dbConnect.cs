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
        public DbSet<Employees> Employees { get; set; }
  
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Clients>()
            .HasKey(c => c.ClientId);
        }       
    }
    public class OwnerDbContext : DbContext
    {
        public OwnerDbContext(DbContextOptions<OwnerDbContext> options) : base(options) { }

        public DbSet<Owner> Owner { get; set; }
    }
}
