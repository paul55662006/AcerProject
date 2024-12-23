using Microsoft.AspNetCore.Mvc;
using test1.Models;
using System.Linq;

namespace test1.Controllers
{
    [Route("partial_client")]
    public class partial_clientController : Controller
    {
        private readonly AppDbContext _context;

        public partial_clientController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("partial_client")]
        public IActionResult partial_client(DateTime startDate, DateTime endDate, string timeSlot)
        {
            // 基本查詢條件：日期範圍
            var query = _context.client
                .Where(c => c.Date.Date >= startDate.Date && c.Date.Date <= endDate.Date);

            // 時段篩選
            if (timeSlot != "allDay")
            {
                query = query.Where(c => c.Time_period == timeSlot);
            }

            // 執行查詢
            var clients = query.ToList();

            // 返回部分檢視
            return PartialView("~/Views/partial_client/Index.cshtml", clients);
        }
    }
}
