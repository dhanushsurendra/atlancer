﻿using Microsoft.AspNetCore.Mvc;

namespace Atlancer.Controllers
{
    public class PaymentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}