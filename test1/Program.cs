using Microsoft.EntityFrameworkCore;
using test1.Models;

namespace test1
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllersWithViews();


            // ���U Session
            builder.Services.AddDistributedMemoryCache(); // ���������s�w�s
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30); // �]�m Session �L���ɶ�
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true; // �T�O�b���p�ﶵ�ҥήɨ̵M�i�H�ϥ�
            });


            // Add services to the container.
            builder.Services.AddDbContext<AppDbContext>(options =>
			options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


            var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (!app.Environment.IsDevelopment())
			{
				app.UseExceptionHandler("/Home/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			//app.UseAuthorization();

			app.MapControllerRoute(
				name: "default",
				pattern: "{controller=Schedule}/{action=Index}/{id?}");

			app.Run();

		}
	}
}
