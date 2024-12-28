using Microsoft.AspNetCore.Mvc;
using test1.Models;

namespace test1.Controllers
{
    [Route("PartialDoctor")]
    public class PartialDoctorontroller : Controller
    {
        private readonly AppDbContext _context;

        public PartialDoctorontroller(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("DoctorPartial")]
        public IActionResult LoadPartial()
        {
            var Doctors = _context.Doctors.ToList();
            return PartialView("~/Views/partial_Doctors/Index.cshtml", Doctors);
        }
    }
    
}
