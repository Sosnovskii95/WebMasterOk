using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebMasterOk.Models.CodeFirst;
using WebMasterOk.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace WebMasterOk.Controllers
{
    public class ClientPersonalAreaController : Controller
    {
        private readonly DBMasterOkContext _context;

        public ClientPersonalAreaController(DBMasterOkContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "client, admin")]
        public IActionResult Index()
        {
            string role = User.FindFirst(x => x.Type == ClaimsIdentity.DefaultRoleClaimType).Value;
            if (role.Equals("admin"))
            {
                return RedirectToAction("Index", "AdminPanel");
            }
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "client")]
        public async Task<IActionResult> History()
        {
            string roleClient = User.FindFirst(x => x.Type == ClaimsIdentity.DefaultRoleClaimType).Value;
            if (roleClient.Equals("client"))
            {
                int id = Convert.ToInt32(User.FindFirst(x => x.Type == ClaimsIdentity.DefaultNameClaimType).Value);
                var clientHistory = await _context.ProductSolds.Where(i => i.ClientId == id).Include(y => y.User).ThenInclude(s => s.Staff).Include(p => p.ProductChecks).ThenInclude(u => u.Product).ToListAsync();
                return View(clientHistory);
            }
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "client")]
        public async Task<IActionResult> UpdateClient(string returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            int clientId = Convert.ToInt32(User.FindFirst(x => x.Type == ClaimsIdentity.DefaultNameClaimType).Value);
            Client client = await _context.Clients.FindAsync(clientId);
            if (client != null)
            {
                return View(client);
            }
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "client")]
        public async Task<IActionResult> UpdateClient(Client client, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                _context.Clients.Update(client);
                await _context.SaveChangesAsync();
                if (!String.IsNullOrEmpty(returnUrl))
                {
                    return Redirect(returnUrl);
                }
            }
            return RedirectToAction(nameof(Index), "Home");
        }
    }
}
