using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using test1.Models;

namespace test1.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly AppDbContext _context;

        public EmployeeController(AppDbContext context)
        {
            _context = context;
        }

        // 顯示註冊頁面
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // 處理員工註冊資料
        [HttpPost]
        public async Task<IActionResult> Create(Employees employee)
        {
            if (!ModelState.IsValid)
            {
                return View(employee);
            }

            // 檢查帳號或 Email 是否已存在
            bool isExisting = await _context.Employees.AnyAsync(e =>
                e.Account == employee.Account || e.Email == employee.Email);
            if (isExisting)
            {
                ModelState.AddModelError(string.Empty, "帳號或 Email 已被註冊");
                return View(employee);
            }

            // 新增員工資料
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            // 設置成功提示
            TempData["Message"] = "註冊成功！";

            return RedirectToAction("Create");
        }

    }
}
