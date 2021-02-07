using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMasterOk.Data;
using WebMasterOk.Models.CodeFirst;

namespace WebMasterOk.Controllers
{
    public class AdminClientModifyController : Controller
    {
        private readonly DBMasterOkContext _context;

        public AdminClientModifyController(DBMasterOkContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AddClient()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddClient(Client client)
        {
            if (ModelState.IsValid)
            {
                _context.Clients.Add(client);
                await _context.SaveChangesAsync();
            }
            return null;
        }

        [HttpGet]
        public async Task<IActionResult> EditClient(int id)
        {
            Client client = await _context.Clients.FindAsync(id);
            if (client != null)
            {
                return View(client);
            }
            return null;
        }

        [HttpPost]
        public async Task<IActionResult> EditClient(Client client)
        {
            if (ModelState.IsValid)
            {
                _context.Clients.Update(client);
                await _context.SaveChangesAsync();
            }
            return null;
        }
    }
}
