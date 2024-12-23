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
           return View();
        }

    }
}
