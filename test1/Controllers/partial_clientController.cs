﻿using Microsoft.AspNetCore.Mvc;
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
            var query = _context.Clients
                .Where(c => c.Date.Date >= startDate.Date && c.Date.Date <= endDate.Date);

            // 時段篩選
            if (timeSlot != "allDay")
            {
                query = query.Where(c => c.TimeSlot == timeSlot);
            }

            // 執行查詢
            var clients = query.ToList();

            // 返回部分檢視
            return PartialView("~/Views/partial_client/Index.cshtml", clients);

        }

        [HttpPost("delete/{ClientId}")]
        public IActionResult Delete(int ClientId)
        {
            var client = _context.Clients.FirstOrDefault(c => c.ClientId == ClientId);
            if (client == null)
            {
                return Json(new { success = false, message = "找不到該資料！" });
            }

            _context.Clients.Remove(client);
            _context.SaveChanges(); // 確保刪除操作被儲存到資料庫

            return Json(new { success = true });
        }




    }
}
