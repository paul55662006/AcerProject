using Microsoft.AspNetCore.Mvc;

namespace test1.Controllers
{
    public class ChartController : Controller
    {
        public IActionResult Index()
        {
            return View(); 
        }

        public IActionResult PetTypeImage()
        {
            string imagePath = "/img/pet_types.png";
            ViewBag.ImagePath = imagePath;
            return View();
        }

        public IActionResult TimeSlotImage()
        {
            string imagePath = "/img/time_slot.png";
            ViewBag.ImagePath = imagePath;
            return View();
        }
    }
}
