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
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace WebMasterOk.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DBMasterOkContext _context;

        public HomeController(ILogger<HomeController> logger, DBMasterOkContext context)
        {
            _logger = logger;
            _context = context;

        }

        //[Authorize]
        public async Task<IActionResult> Index()
        {
            var products = await _context.Products.ToListAsync();
            ViewBag.Categories = await _context.Categories.ToListAsync();

            return View(products);
        }

        public VirtualFileResult GetImage(int id)
        {
            Product product = _context.Products.Find(id);

            if(product!=null)
            {
                return File(Path.Combine(product.PathImage), "application /png","1");
            }

            return null;
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
