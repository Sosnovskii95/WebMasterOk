using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebMasterOk.Data;
using WebMasterOk.Models;
using WebMasterOk.Models.CodeFirst;
using X.PagedList;

namespace WebMasterOk.Controllers.Admin
{
    public class AdminModifyOrderController : Controller
    {
        private readonly DBMasterOkContext _context;

        public AdminModifyOrderController(DBMasterOkContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int? page, string searchUser)
        {
            ViewData["searchUser"] = searchUser;

            int pageNumber = (page ?? 1);
            int pageSize = 20;

            IQueryable<ProductSold> orders = _context.ProductSolds.Include(u => u.User).ThenInclude(s => s.Staff);

            if (orders.Where(u => u.UserId == null).Count() > 0)
            {
                orders = orders.Where(u => u.UserId == null);
            }
            if(!String.IsNullOrEmpty(searchUser))
            {
                orders = orders.Where(u => u.User.Staff.FullNameStaff.Contains(searchUser));
            }

            orders = orders.OrderBy(i => i.Id);

            return View(await orders.ToPagedListAsync(pageNumber, pageSize));
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var productSold = await _context.ProductSolds.Include(u => u.User).ThenInclude(s => s.Staff).Include(c => c.Client).FirstOrDefaultAsync(i => i.Id == id);
            ViewBag.ProductChecks = await _context.ProductChecks.Include(i => i.Product).Where(p => p.ProductSoldId == productSold.Id).ToListAsync();
            ViewBag.StateOrder = new SelectList(new SelectListItem[]
            {
                new SelectListItem(){Text = "В ожидании", Value="В ожидании", Selected = true},
                new SelectListItem(){Text = "Одобрен", Value="Одобрен"},
                new SelectListItem(){Text="Отменен", Value="Отменен"}
            }, "Value", "Text");

            return View(productSold);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, string stateOrder)
        {
            var productSold = await _context.ProductSolds.FindAsync(id);
            productSold.StateOrder = stateOrder;
            int userId = Convert.ToInt32(User.FindFirst(x => x.Type == ClaimsIdentity.DefaultNameClaimType).Value);
            productSold.UserId = userId;

            _context.ProductSolds.Update(productSold);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
