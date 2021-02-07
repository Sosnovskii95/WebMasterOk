using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMasterOk.Models.CodeFirst;
using WebMasterOk.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebMasterOk.Controllers
{
    public class AdminModifyUserController : Controller
    {
        private readonly DBMasterOkContext _context;

        public AdminModifyUserController(DBMasterOkContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _context.Users.Include(r => r.Role).Include(p => p.Staff).ToListAsync();

            return View(users);
        }

        [HttpGet]
        public async Task<IActionResult> AddUser()
        {
            ViewBag.StaffId = new SelectList(await _context.Staffs.ToListAsync(), "Id", "FirstName");
            ViewBag.RoleId = new SelectList(await _context.Roles.ToListAsync(), "Id", "TitleRole");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(User user)
        {
            if (ModelState.IsValid)
            {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(int id)
        {
            User user = await _context.Users.FindAsync(id);

            if (user != null)
            {
                ViewBag.StaffId = new SelectList(await _context.Staffs.ToListAsync(), "Id", "FirstName");
                ViewBag.RoleId = new SelectList(await _context.Roles.ToListAsync(), "Id", "TitleRole");

                return View(user);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(User user)
        {
            if (ModelState.IsValid)
            {
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
