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
    public class AdminModifyPositionController : Controller
    {
        private readonly DBMasterOkContext _context;

        public AdminModifyPositionController(DBMasterOkContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int? page, string searchTitle)
        {
            ViewData["searchTitle"] = searchTitle;

            int pageNumber = (page ?? 1);
            int pageSize = 20;

            IQueryable<Position> positions = _context.Positions;

            if(!String.IsNullOrEmpty(searchTitle))
            {
                positions = positions.Where(c => c.TitlePosition.Contains(searchTitle));
            }

            positions = positions.OrderBy(i => i.Id);

            return View(await positions.ToPagedListAsync(pageNumber, pageSize));
        }

        [HttpGet]
        public IActionResult AddPosition()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddPosition(Position position)
        {
            if(ModelState.IsValid)
            {
                await _context.Positions.AddAsync(position);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(position);
        }

        [HttpGet]
        public async Task<IActionResult> EditPosition(int id)
        {
            Position position = await _context.Positions.FindAsync(id);

            if(position != null)
            {
                return View(position);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> EditPosition(Position position)
        {
            if(ModelState.IsValid)
            {
                _context.Positions.Update(position);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(position);
        }
    }
}
