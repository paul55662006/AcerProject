using Microsoft.AspNetCore.Mvc;
using test1.Models;
using System.Linq;

namespace test1.Controllers
{
    public class DoctorsController : Controller
    {
        private readonly AppDbContext _context;


        public DoctorsController(AppDbContext context)
        {
            _context = context;
        }

        // 顯示建檔表單
        public IActionResult Create()
        {
            return View();
        }

        // 提交醫師資料
        [HttpPost]
        public IActionResult Create(Doctors doctor)
        {
            if (ModelState.IsValid)
            {
                _context.Doctors.Add(doctor);
                _context.SaveChanges();
                return RedirectToAction("Index", "Home"); // 回到首頁或其他頁面
            }
            return View(doctor);
        }

        // 更新資料表單
        public IActionResult Edit(int id)
        {
            var doctor = _context.Doctors.Find(id);
            if (doctor == null)
            {
                return NotFound();
            }
            return View(doctor);
        }

        // 提交更新資料
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Doctors doctor)
        {
            if (ModelState.IsValid)
            {
                _context.Doctors.Update(doctor);
                _context.SaveChanges();
                return RedirectToAction("Create", "Doctors");
            }
            return View(doctor);
        }
    }
}
