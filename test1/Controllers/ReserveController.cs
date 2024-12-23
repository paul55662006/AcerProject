using Microsoft.AspNetCore.Mvc;
using test1.Models;

namespace test1.Controllers
{
    public class ReserveController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        //日期選擇器
        [HttpPost]
        public IActionResult UpdateTime(DateTime date, string timeSlot)
        {
            // 根據日期和時段更新資料庫
            // 可以在這裡寫入業務邏輯
            Console.WriteLine($"選擇的日期: {date}, 選擇的時段: {timeSlot}");

            // 返回到檢視或進行其他操作
            return RedirectToAction("Index");
        }

    }
}
