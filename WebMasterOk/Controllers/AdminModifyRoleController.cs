using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMasterOk.Data;
using WebMasterOk.Models.CodeFirst;

namespace WebMasterOk.Controllers
{
    public class AdminModifyRoleController : Controller
    {
        private readonly DBMasterOkContext _context;

        public AdminModifyRoleController(DBMasterOkContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View(_context.Roles.ToList());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Role role)
        {
            if(ModelState.IsValid)
            {
                await _context.Roles.AddAsync(role);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(role);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            Role role = await _context.Roles.FindAsync(id);
            if(role != null)
            {
                return View(role);
            }
            return null;
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Role role)
        {
            if(ModelState.IsValid)
            {
                _context.Roles.Update(role);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return null;
        }
    }
}
