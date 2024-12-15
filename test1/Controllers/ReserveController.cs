using Microsoft.AspNetCore.Mvc;

namespace test1.Controllers
{
    public class ReserveController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
