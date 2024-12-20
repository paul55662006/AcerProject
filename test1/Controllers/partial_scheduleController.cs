using Microsoft.AspNetCore.Mvc;
using test1.Models;

namespace test1.Controllers
{
    [Route("PartialSchedule")]
    public class PartialScheduleController : Controller
    {
        private readonly AppDbContext _context;

        public PartialScheduleController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("LoadPartial")]
        public IActionResult LoadPartial()
        {
            var Doctors = _context.Doctor_time.ToList();
            return PartialView("~/Views/partial_schedule/Index.cshtml", Doctors);
        }
    }

}
