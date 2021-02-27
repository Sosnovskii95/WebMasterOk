using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebMasterOk.Data;
using WebMasterOk.Models.CodeFirst;

namespace WebMasterOk.Controllers
{
    public class AdminPanelController : Controller
    {
        private readonly DBMasterOkContext _context;

        public AdminPanelController(DBMasterOkContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Index()
        {
            int userId = Convert.ToInt32(User.FindFirst(x => x.Type == ClaimsIdentity.DefaultNameClaimType).Value);
            User user = await _context.Users.Include(s => s.Staff).FirstOrDefaultAsync(i => i.Id == userId);

            return View(user);
        }
    }
}
