using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using test1.Models;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.PixelFormats;

namespace test1.Controllers
{
    public class CustomerLogInController : Controller
    {
        private readonly AppDbContext _context;

        public CustomerLogInController(AppDbContext context) => _context = context;

        [HttpGet]
        public IActionResult Index() => View();

        [HttpGet]
        public IActionResult Dashboard()
        {
            var account = HttpContext.Session.GetString("Account");

            if (string.IsNullOrEmpty(account))
            {
                return RedirectToAction("Index", "CustomerLogIn"); // 如果未登入，跳轉到登入頁面
            }

            ViewData["Account"] = account;
            return View();
        }

        [HttpPost]
        public IActionResult Index(string Account, string Password, string captcha)
        {
            // 驗證驗證碼
            var sessionCaptcha = HttpContext.Session.GetString("CaptchaCode");
            if (string.IsNullOrEmpty(sessionCaptcha) || captcha != sessionCaptcha)
            {
                TempData["Error"] = "驗證碼錯誤";
                return View();
            }

            var customer = _context.Owner.FirstOrDefault(e => e.Account == Account && e.Password == Password);

            if (customer == null)
            {
                TempData["Error"] = "帳號或密碼錯誤";
                return View();
            }

            // 設定 Session
            HttpContext.Session.SetString("Account", customer.Account); // 存入帳號
            HttpContext.Session.SetInt32("OwnerId", customer.OwnerID); // 存入員工 ID

            // 重定向到首頁
            return RedirectToAction("Create", "Appointment");
        }

        [HttpGet]
        public IActionResult Captcha()
        {
            var random = new Random();
            var captchaCode = random.Next(1000, 9999).ToString();

            // 儲存驗證碼到 Session
            HttpContext.Session.SetString("CaptchaCode", captchaCode);

            // 生成驗證碼圖片
            using var image = new Image<Rgba32>(100, 40);
            image.Mutate(ctx =>
            {
                ctx.Fill(SixLabors.ImageSharp.Color.White);
                var font = SixLabors.Fonts.SystemFonts.CreateFont("Arial", 18);
                ctx.DrawText(captchaCode, font, SixLabors.ImageSharp.Color.Black, new SixLabors.ImageSharp.PointF(10, 5));
            });

            using var ms = new MemoryStream();
            image.Save(ms, new PngEncoder());
            return File(ms.ToArray(), "image/png");
        }
    }
}
