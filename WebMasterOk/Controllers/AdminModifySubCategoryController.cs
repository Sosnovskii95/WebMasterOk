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
    public class AdminModifySubCategoryController : Controller
    {
        private readonly DBMasterOkContext _context;

        public AdminModifySubCategoryController(DBMasterOkContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var subCategories = await _context.SubCategories.Include(c => c.Category).ToListAsync();

            return View(subCategories);
        }

        [HttpGet]
        public async Task<IActionResult> AddSubCategory()
        {
            ViewBag.CategoryId = new SelectList(await _context.Categories.ToListAsync(), "Id", "TitleCategory");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddSubCategory(SubCategory subCategory)
        {
            if(ModelState.IsValid)
            {
                _context.SubCategories.Add(subCategory);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(subCategory);
        }

        [HttpGet]
        public async Task<IActionResult> EditSubCategory(int id)
        {
            ViewBag.CategoryId = new SelectList(await _context.Categories.ToListAsync(), "Id", "TitleCategory");
            SubCategory subCategory = await _context.SubCategories.FindAsync(id);

            return View(subCategory);
        }

        [HttpPost]
        public async Task<IActionResult> EditSubCategory(SubCategory subCategory)
        {
            if(ModelState.IsValid)
            {
                _context.SubCategories.Update(subCategory);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(subCategory);
        }
    }
}
