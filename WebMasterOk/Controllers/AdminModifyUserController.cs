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

        public IActionResult Index()
        {
           return View();
        }

        [HttpGet]
        public async Task<IActionResult> AddUser()
        {
            ViewBag.StaffId = new SelectList(await _context.Staffs.ToListAsync(),"Id", "FirstName");
            ViewBag.RoleId = new SelectList(await _context.Roles.ToListAsync(), "Id", "TitleRole");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(User newUser)
        {
            if(ModelState.IsValid)
            {

            }
            return View();
        }
    }
}
