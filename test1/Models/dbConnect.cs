using Microsoft.EntityFrameworkCore;

namespace test1.Models
{
    public class AppDbContext : DbContext
    {
        public DbSet<client> clients { get; set; } // 對應資料表 clients
        public DbSet<doctor> Doctor_time { get; set; } // 對應資料表 Doctor_time

        // 添加構造函數，讓 AddDbContext 可以傳遞配置
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }        
    }
}
