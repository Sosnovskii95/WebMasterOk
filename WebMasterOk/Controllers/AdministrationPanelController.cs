using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMasterOk.Controllers
{
    public class AdministrationPanelController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetUser()
        {
            return RedirectToAction(nameof(Index), "AdminModifyUser");
        }
    }
}
