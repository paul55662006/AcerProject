using Microsoft.AspNetCore.Mvc;

namespace test1.Controllers
{
    public class DoctorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
