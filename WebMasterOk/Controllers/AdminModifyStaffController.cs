using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMasterOk.Data;
using WebMasterOk.Models.CodeFirst;

namespace WebMasterOk.Controllers
{
    public class AdminModifyStaffController : Controller
    {
        private readonly DBMasterOkContext _context;

        public AdminModifyStaffController(DBMasterOkContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var staffList = await _context.Staffs.Include(p => p.Position).ToListAsync();
            return View(staffList);
        }

        [HttpGet]
        public async Task<IActionResult> AddStaff()
        {
            ViewBag.PositionId = new SelectList(await _context.Positions.ToListAsync(), "Id", "TitlePosition");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddStaff(Staff staff)
        {
            if (ModelState.IsValid)
            {
                _context.Staffs.Add(staff);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(staff);
        }

        [HttpGet]
        public async Task<IActionResult> EditStaff(int id)
        {
            Staff staff = await _context.Staffs.FindAsync(id);

            if (staff != null)
            {
                ViewBag.PositionId = new SelectList(await _context.Positions.ToListAsync(), "Id", "TitlePosition");

                return View(staff);
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> EditStaff(Staff staff)
        {
            if (ModelState.IsValid)
            {
                _context.Staffs.Update(staff);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
