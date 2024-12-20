using Microsoft.AspNetCore.Mvc;
using test1.Models;

namespace test1.Controllers
{
    public class DoctorController : Controller
    {
        private readonly AppDbContext _context;

        public DoctorController(AppDbContext context)
        {
            _context =  context; // 初始化 DbContext
        }

        public IActionResult Index()
        {
            try
            {
                var Doctors = _context.Doctor_time.ToList(); // 嘗試從資料庫讀取數據
                Console.WriteLine($"成功連接資料庫，讀取到 {Doctors.Count} 條資料");
                return View(Doctors);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"資料庫連接失敗: {ex.Message}");
                throw;
            }
        }

    }
}
