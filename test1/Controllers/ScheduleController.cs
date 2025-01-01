using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace test1.Controllers
{
    public class ScheduleController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}