using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using test1.Models;

namespace test1.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;

		public HomeController(ILogger<HomeController> logger)
		{
			_logger = logger;
		}

		public IActionResult Index()
		{
			return View();
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}

        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); // 清除所有 Session 資訊
            return RedirectToAction("Index", "EmployeeLogIn"); // 跳轉到登入頁面
        }
    }
}
