using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMasterOk.Data;
using WebMasterOk.Models.CodeFirst;
using X.PagedList;

namespace WebMasterOk.Controllers.Admin
{
    public class AdminModifyStaffController : Controller
    {
        private readonly DBMasterOkContext _context;

        public AdminModifyStaffController(DBMasterOkContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int? page, string searchFullName, int? searchPositionId)
        {
            ViewData["searchFullName"] = searchFullName;

            int pageNumber = (page ?? 1);
            int pageSize = 20;

            IQueryable<Staff> staffs = _context.Staffs;

            if(!String.IsNullOrEmpty(searchFullName))
            {
                staffs = staffs.Where(f => f.FullNameStaff.Contains(searchFullName));
            }
            if(searchPositionId.HasValue && searchPositionId>0)
            {
                staffs = staffs.Where(i => i.PositionId == searchPositionId);
            }

            staffs = staffs.Include(p => p.Position).OrderBy(i => i.Id);

            var positions = await _context.Positions.ToListAsync();
            positions.Insert(0, new Position { Id = 0, TitlePosition = "Все" });
            ViewBag.SearchPosition = new SelectList(positions, "Id", "TitlePosition");

            return View(await staffs.ToPagedListAsync(pageNumber, pageSize));
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
                await _context.Staffs.AddAsync(staff);
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

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var staff = await _context.Staffs
                .Include(s => s.Position)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (staff == null)
            {
                return NotFound();
            }

            return View(staff);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var staff = await _context.Staffs.FindAsync(id);
            _context.Staffs.Remove(staff);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
