using Microsoft.AspNetCore.Mvc;
using test1.Models;

namespace test1.Controllers
{
    public class ReserveController : Controller
    {
        private readonly AppDbContext _context;

        public ReserveController()
        {
            _context = new AppDbContext(); // 初始化 DbContext
        }

        public IActionResult Index()
        {
            // 從資料庫中讀取所有 clients 資料
            var clients = _context.clients.ToList();

            // 將資料傳遞給 View
            return View(clients);
        }
    }
}
