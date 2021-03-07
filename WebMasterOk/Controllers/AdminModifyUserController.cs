using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMasterOk.Models.CodeFirst;
using WebMasterOk.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using X.PagedList;

namespace WebMasterOk.Controllers.Admin
{
    public class AdminModifyUserController : Controller
    {
        private readonly DBMasterOkContext _context;

        public AdminModifyUserController(DBMasterOkContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int? page, string searchFullName, int? searchRoleId)
        {
            ViewData["searchFullName"] = searchFullName;

            int pageNumber = (page ?? 1);
            int pageSize = 20;

            IQueryable<User> users = _context.Users;

            if(!String.IsNullOrEmpty(searchFullName))
            {
                users = users.Where(s => s.Staff.FullNameStaff.Contains(searchFullName));
            }
            if(searchRoleId.HasValue && searchRoleId>0)
            {
                users = users.Where(r => r.RoleId == searchRoleId);
            }

            users = users.Include(r => r.Role).Include(p => p.Staff).OrderBy(i => i.Id);

            var roles = await _context.Roles.ToListAsync();
            roles.Insert(0, new Role { Id = 0, TitleRole = "Все" });
            ViewBag.SearchRoles = new SelectList(roles, "Id", "TitleRole");

            return View(await users.ToPagedListAsync(pageNumber, pageSize));
        }

        [HttpGet]
        public async Task<IActionResult> AddUser()
        {
            ViewBag.StaffId = new SelectList(await _context.Staffs.ToListAsync(), "Id", "FullNameStaff");
            ViewBag.RoleId = new SelectList(await _context.Roles.ToListAsync(), "Id", "TitleRole");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(User user)
        {
            if (ModelState.IsValid)
            {
                await _context.Users.AddAsync(user);
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
                ViewBag.StaffId = new SelectList(await _context.Staffs.ToListAsync(), "Id", "FullNameStaff");
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
