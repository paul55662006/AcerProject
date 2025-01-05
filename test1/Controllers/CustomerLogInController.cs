using Microsoft.AspNetCore.Mvc;
using System.Linq;
using test1.Models;

namespace test1.Controllers
{
    public class CustomerLogInController : Controller
    {
        private readonly OwnerDbContext _context;

        public CustomerLogInController(OwnerDbContext context) => _context = context;

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
        public IActionResult Index(string Account, string Password)
        {
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


    }
}
