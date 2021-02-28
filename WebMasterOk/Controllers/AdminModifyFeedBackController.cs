using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
    public class AdminModifyFeedBackController : Controller
    {
        private readonly DBMasterOkContext _context;

        public AdminModifyFeedBackController(DBMasterOkContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var feedBacks = await _context.FeedBacks.Where(u => u.UserId == null).ToListAsync();
            if (feedBacks.Count() <= 0)
            {
                feedBacks = await _context.FeedBacks.Include(u => u.User).ThenInclude(s => s.Staff).ToListAsync();
            }

            return View(feedBacks);
        }

        public async Task<IActionResult> Details(int id)
        {
            var feedBack = await _context.FeedBacks.Include(x => x.User).ThenInclude(s => s.Staff).FirstOrDefaultAsync(i => i.Id == id);

            ViewBag.StateFeedBack = new SelectList(new SelectListItem[]
            {
                new SelectListItem(){Text = "В ожидании", Value="В ожидании", Selected = true},
                new SelectListItem(){Text = "Одобрен", Value="Одобрен"}
            }, "Value", "Text");

            return View(feedBack);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, string stateFeedBack)
        {
            FeedBack feedBack = await _context.FeedBacks.FindAsync(id);
            feedBack.StateFeedBack = stateFeedBack;

            int userId = Convert.ToInt32(User.FindFirst(x => x.Type == ClaimsIdentity.DefaultNameClaimType).Value);
            feedBack.UserId = userId;
            _context.FeedBacks.Update(feedBack);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

    }
}
