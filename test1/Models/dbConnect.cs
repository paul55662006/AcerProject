using Microsoft.EntityFrameworkCore;
using System.Numerics;
using System.Linq;
using System.Drawing;


namespace test1.Models
{
    public class AppDbContext : DbContext
    {
        public DbSet<Clients> Clients { get; set; } // 對應資料表 Clients
        public DbSet<Doctors> Doctors { get; set; }
        public DbSet<Schedules> Schedules { get; set; }
        public DbSet<Employees> Employees { get; set; }
        public DbSet<Owner> Owner { get; set; }
        public DbSet<Pet> Pet { get; set; }


        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Clients>()
            .HasKey(c => c.ClientId);

            modelBuilder.Entity<Owner>()
                 .HasKey(o => o.OwnerID);

            modelBuilder.Entity<Pet>()
                .HasKey(p => p.PetID);

            modelBuilder.Entity<Pet>()
                .HasOne(p => p.Owner)
                .WithMany(o => o.Pets)
                .HasForeignKey(p => p.OwnerID)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
    public class OwnerDbContext : DbContext
    {
        public OwnerDbContext(DbContextOptions<OwnerDbContext> options) : base(options) { }

        public DbSet<Owner> Owner { get; set; }
    }
}
