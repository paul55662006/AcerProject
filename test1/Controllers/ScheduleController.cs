using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using test1.Models;

namespace test1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScheduleController : Controller
    {
        private readonly AppDbContext _context;

        public ScheduleController(AppDbContext context)
        {
            _context = context;
        }

        // 返回排班頁面
        [HttpGet("/Schedule")]
        public IActionResult Index()
        {
            return View(); // 返回 Views/Schedule/Index.cshtml
        }

        // GET: api/schedule/doctors
        [HttpGet("doctors")]
        public async Task<IActionResult> GetDoctors()
        {
            var doctors = await _context.Doctors
                .Select(d => new
                {
                    d.Id,
                    d.Name,
                    d.Email,
                    d.Phone,
                    d.Specialty,
                })
                .ToListAsync();

            return Ok(doctors);
        }

        // GET: api/schedule
        [HttpGet]
        public async Task<IActionResult> GetSchedules([FromQuery] string? specialty, [FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            var query = _context.Schedules.AsQueryable();

            if (!string.IsNullOrEmpty(specialty) && specialty != "all")
            {
                query = query.Where(s => s.DoctorSpecialty == specialty);
            }

            if (startDate.HasValue)
            {
                query = query.Where(s => s.Date >= startDate.Value);
            }
            if (endDate.HasValue)
            {
                query = query.Where(s => s.Date <= endDate.Value);
            }

            var schedules = await query.Select(s => new
            {
                s.Id,
                s.Date,
                s.TimeSlot,
                s.DoctorName,
                s.DoctorEmail,
                s.DoctorPhone,
                s.DoctorSpecialty
            }).ToListAsync();

            return Ok(schedules);
        }

        // POST: api/schedule
        [HttpPost]
        public async Task<IActionResult> CreateSchedule([FromBody] Schedules schedule)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // 驗證是否有重複的排班
            var exists = await _context.Schedules.AnyAsync(s =>
                s.Date == schedule.Date &&
                s.TimeSlot == schedule.TimeSlot &&
                s.DoctorName == schedule.DoctorName);

            if (exists)
            {
                return Conflict("該醫生在此日期和時段已經有班表安排。");
            }

            _context.Schedules.Add(schedule);
            await _context.SaveChangesAsync();

            return Ok(schedule);
        }

        // DELETE: api/schedule/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSchedule(int id)
        {
            var schedule = await _context.Schedules.FindAsync(id);
            if (schedule == null)
            {
                return NotFound();
            }

            _context.Schedules.Remove(schedule);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}
