﻿using Microsoft.AspNet.Mvc;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace PollR.Controllers
{
    public class VoteController : Controller
    {
        public VoteController()
        {
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
