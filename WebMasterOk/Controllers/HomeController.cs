using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebMasterOk.Models;
using WebMasterOk.Models.CodeFirst;
using WebMasterOk.Data;
using Microsoft.AspNetCore.Authorization;

namespace WebMasterOk.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DBMasterOkContext db;

        public HomeController(ILogger<HomeController> logger, DBMasterOkContext db)
        {
            _logger = logger;
            this.db = db;

        }

        //[Authorize]
        public IActionResult Index()
        {


            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
