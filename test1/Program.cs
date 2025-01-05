using Microsoft.EntityFrameworkCore;
using test1.Models;

namespace test1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // �K�[����M����
            builder.Services.AddControllersWithViews();

            // ���U Session
            builder.Services.AddDistributedMemoryCache(); // ���������s�w�s
            builder.Services.AddSession(options =>
            {
                options.Cookie.Name = ".YourApp.Session"; // Session �� Cookie �W��
                options.IdleTimeout = TimeSpan.FromMinutes(30); // �]�m Session �L���ɶ�
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true; // �T�O�b���p�ﶵ�ҥήɨ̵M�i�H�ϥ�
            });

            // �]�m��Ʈw�W�U��
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // �ҥ� CORS�]���귽�@�ɡ^�A���\�Ҧ���
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

            // �t�m HTTP �ШD�޹D
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            // �ҥ� CORS
            app.UseCors(policy =>
    policy.AllowAnyOrigin()
          .AllowAnyMethod()
          .AllowAnyHeader());


            // �ҥ� Session
            app.UseSession();

            // �M�g API �������
            app.MapControllers();

            // �M�g�q�{����
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=EmployeeLogIn}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
