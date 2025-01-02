using Microsoft.AspNetCore.Mvc;
using System.Linq;
using test1.Models;

namespace test1.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly AppDbContext _context;

        public AppointmentController(AppDbContext context)
        {
            _context = context;
        }

        // 顯示預約表單
        public IActionResult Create()
        {
            return View();
        }

        // 處理預約表單提交
        [HttpPost]
        public IActionResult Create(Clients appointment)
        {
            if (!ModelState.IsValid)
            {
                return View(appointment);
            }

            if (appointment.Date < DateTime.Today.AddDays(1))
            {
                ModelState.AddModelError("", "預約日期必須為當天日期至少一天後！");
                return View(appointment);
            }

            var count = _context.Clients
                .Where(a => a.Date == appointment.Date && a.TimeSlot == appointment.TimeSlot)
                .Count();

            if (count >= 60)
            {
                ModelState.AddModelError("", "該時段已達上限！");
                return View(appointment);
            }

            _context.Clients.Add(appointment);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "預約成功！";
            return View(appointment);
        }
    }
}
