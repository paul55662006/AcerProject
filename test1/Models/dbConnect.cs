using Microsoft.EntityFrameworkCore;
namespace test1.Models
{
    public class AppDbContext : DbContext
    {
        public DbSet<client> clients { get; set; }   // 對應資料表 clients
        public DbSet<doctor> Doctor_time { get; set; }   // 對應資料表 Doctor_time
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=Reserve;Integrated Security=True;TrustServerCertificate=True;");
        }
        
    }
    

}


