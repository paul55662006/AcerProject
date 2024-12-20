using Microsoft.AspNetCore.Mvc;
using test1.Models;

namespace test1.Controllers
{
    public class DoctorController : Controller
    {
        private readonly AppDbContext _context;

        public DoctorController()
        {
            _context = new AppDbContext(); // 初始化 DbContext
        }

        public IActionResult Index()
        {
            // 從資料庫中讀取所有 clients 資料
            var Doctors = _context.Doctor_time.ToList();

            // 將資料傳遞給 View
            return View(Doctors);
        }
    }
}
