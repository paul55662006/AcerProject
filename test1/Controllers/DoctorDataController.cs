using Microsoft.AspNetCore.Mvc;
using test1.Models;
using System.Linq;

namespace test1.Controllers
{
    public class DoctorDataController : Controller
    {
        private readonly AppDbContext _context;

        public DoctorDataController(AppDbContext context)
        {
            _context = context;
        }

        // 顯示醫師列表，支援專長篩選
        public IActionResult Index(string specialty = null)
        {
            var doctors = string.IsNullOrEmpty(specialty)
                ? _context.Doctors.ToList()
                : _context.Doctors.Where(d => d.Specialty == specialty).ToList();

            ViewBag.SelectedSpecialty = specialty; // 用於維持篩選器選項
            return View(doctors);
        }

        // 刪除醫師資料
        [HttpPost]
        public IActionResult Delete(int id)
        {
            // 查找醫生
            var doctor = _context.Doctors.Find(id);
            if (doctor == null)
            {
                return NotFound();
            }

            // 找到與該醫生相關的班表記錄
            var relatedSchedules = _context.Schedules
                .Where(s => s.DoctorName == doctor.Name)
                .ToList();

            // 刪除相關的班表記錄
            if (relatedSchedules.Any())
            {
                _context.Schedules.RemoveRange(relatedSchedules);
            }

            // 刪除醫生記錄
            _context.Doctors.Remove(doctor);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }


    }
}
