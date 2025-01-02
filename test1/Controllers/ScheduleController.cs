using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using test1.Models;
using System.Threading.Tasks;
using System.Linq;

namespace test1.Controllers
{
    public class ScheduleController : Controller
    {
        private readonly AppDbContext _context;

        public ScheduleController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        // 取得所有醫師
        [HttpGet("api/doctors")]
        public async Task<IActionResult> GetDoctors()
        {
            var doctors = await _context.Doctors
                .Select(d => new { d.Id, d.Name })
                .ToListAsync();

            return Ok(doctors);
        }

        // 新增排班
        [HttpPost("api/schedules")]
        public async Task<IActionResult> AddSchedule([FromBody] ScheduleWithDoctorsDto dto)
        {
            if (dto == null || !dto.DoctorIds.Any())
            {
                return BadRequest("請提供排班資訊及至少一位醫師");
            }

            // 新增排班
            var schedule = new Schedules
            {
                Date = dto.Date,
                TimeSlot = dto.TimeSlot,
                PetType = dto.PetType,
                Title = dto.Title
            };
            _context.Schedules.Add(schedule);
            await _context.SaveChangesAsync();

            // 新增 ScheduleDoctors
            foreach (var doctorId in dto.DoctorIds)
            {
                _context.ScheduleDoctors.Add(new ScheduleDoctor
                {
                    ScheduleId = schedule.Id,
                    DoctorId = doctorId
                });
            }

            await _context.SaveChangesAsync();

            return Ok(schedule);
        }
    }
}
