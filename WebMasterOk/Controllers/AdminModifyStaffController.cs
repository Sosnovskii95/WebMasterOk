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

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> AddStaff()
        {
            ViewBag.PositionId = new SelectList(await _context.Positions.ToListAsync(), "Id", "TitlePosition");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddStaff(Staff newStaff)
        {
            if(ModelState.IsValid)
            {
                _context.Staffs.Add(newStaff);
                await _context.SaveChangesAsync();
            }
            return View();
        }
    }
}
