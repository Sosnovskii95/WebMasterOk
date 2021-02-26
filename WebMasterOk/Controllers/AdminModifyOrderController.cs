using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMasterOk.Data;
using WebMasterOk.Models;
using WebMasterOk.Models.CodeFirst;

namespace WebMasterOk.Controllers
{
    public class AdminModifyOrderController : Controller
    {
        private readonly DBMasterOkContext _context;

        public AdminModifyOrderController(DBMasterOkContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var order = await _context.ProductSolds.Where(u => u.UserId == null).ToListAsync();
            return View(order);
        }

        public async Task<IActionResult> Details(int id)
        {
            var productSold = await _context.ProductSolds.Include(u => u.User).ThenInclude(s => s.Staff).Include(c => c.Client).FirstOrDefaultAsync(i => i.Id == id);
            var productChecks = await _context.ProductChecks.Include(i=>i.Product).Where(p => p.ProductSoldId == productSold.Id).ToListAsync();

            return View(productSold);
        }
    }
}
