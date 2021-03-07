using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMasterOk.Data;
using WebMasterOk.Models.CodeFirst;

namespace WebMasterOk.Controllers.Admin
{
    public class AdminModifyStoreController : Controller
    {
        private readonly DBMasterOkContext _context;

        public AdminModifyStoreController(DBMasterOkContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Stores.ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.ProductId = new SelectList(await _context.Products.ToListAsync(), "Id", "TitleProduct");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Store store)
        {
            if (ModelState.IsValid)
            {
                _context.Stores.Add(store);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
