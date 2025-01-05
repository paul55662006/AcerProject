using Microsoft.EntityFrameworkCore;
using test1.Models;

namespace test1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // 添加控制器和視圖
            builder.Services.AddControllersWithViews();

            // 註冊 Session
            builder.Services.AddDistributedMemoryCache(); // 必須的內存緩存
            builder.Services.AddSession(options =>
            {
                options.Cookie.Name = ".YourApp.Session"; // Session 的 Cookie 名稱
                options.IdleTimeout = TimeSpan.FromMinutes(30); // 設置 Session 過期時間
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true; // 確保在隱私選項啟用時依然可以使用
            });

            // 設置資料庫上下文
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // 啟用 CORS（跨域資源共享），允許所有源
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });

            var app = builder.Build();

            // 配置 HTTP 請求管道
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            // 啟用 CORS
            app.UseCors(policy =>
    policy.AllowAnyOrigin()
          .AllowAnyMethod()
          .AllowAnyHeader());


            // 啟用 Session
            app.UseSession();

            // 映射 API 控制器路由
            app.MapControllers();

            // 映射默認路由
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=EmployeeLogIn}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
