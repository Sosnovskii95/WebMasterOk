using Microsoft.AspNetCore.Mvc;
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
    public class AdminModifyClientController : Controller
    {
        private readonly DBMasterOkContext _context;

        public AdminModifyClientController(DBMasterOkContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int? page, string searchFullName, string searchTelephone)
        {
            ViewData["searchFullName"] = searchFullName;
            ViewData["searchTelephone"] = searchTelephone;

            int pageNumber = (page ?? 1);
            int pageSize = 20;

            IQueryable<Client> clients = _context.Clients;

            if (!String.IsNullOrEmpty(searchFullName))
            {
                clients = clients.Where(c => c.FamClient.Contains(searchFullName) && c.FirstNameClient.Contains(searchFullName) && c.LastNameClient.Contains(searchFullName));
            }
            if (!String.IsNullOrEmpty(searchTelephone))
            {
                clients = clients.Where(n => n.NumberTelephone.Contains(searchTelephone));
            }

            clients = clients.OrderBy(i => i.Id);

            return View(await clients.ToPagedListAsync(pageNumber, pageSize));
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
                await _context.Clients.AddAsync(client);
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

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _context.Clients
                .FirstOrDefaultAsync(m => m.Id == id);
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        // POST: Clients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var client = await _context.Clients.FindAsync(id);
            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
