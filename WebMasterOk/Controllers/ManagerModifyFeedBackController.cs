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
using X.PagedList;

namespace WebMasterOk.Controllers.Manager
{
    public class ManagerModifyFeedBackController : Controller
    {
        private readonly DBMasterOkContext _context;

        public ManagerModifyFeedBackController(DBMasterOkContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int? page, string searchUser)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 20;

            ViewData["searchUser"] = searchUser;

            IQueryable<FeedBack> feedBacks = _context.FeedBacks.Include(u => u.User).ThenInclude(s => s.Staff);

            if (feedBacks.Where(u => u.UserId == null).Count() > 0)
            {
                feedBacks = feedBacks.Where(u => u.UserId == null);
            }
            if (!String.IsNullOrEmpty(searchUser))
            {
                feedBacks = feedBacks.Where(u => u.User.Staff.FullNameStaff.Contains(searchUser));
            }

            feedBacks = feedBacks.OrderBy(i => i.Id);

            return View(await feedBacks.ToPagedListAsync(pageNumber, pageSize));
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
