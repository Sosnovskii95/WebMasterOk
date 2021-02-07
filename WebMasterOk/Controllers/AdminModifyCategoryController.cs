using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMasterOk.Data;
using WebMasterOk.Models.CodeFirst;

namespace WebMasterOk.Controllers
{
    public class AdminModifyCategoryController : Controller
    {
        private readonly DBMasterOkContext _context;

        public AdminModifyCategoryController(DBMasterOkContext context)
        {
            _context = context;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AddCategory()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddCategory(Category category)
        {
            if(ModelState.IsValid)
            {
                _context.Categories.Add(category);
                await _context.SaveChangesAsync();
            }
            return null;
        }

        [HttpGet]
        public async Task<IActionResult> EditCategory(int id)
        {
            Category category = await _context.Categories.FindAsync(id);
            if(category!=null)
            {
                return View(category);
            }

            return null;
        }

        [HttpPost]
        public async Task<IActionResult> EditCategory(Category category)
        {
            if(ModelState.IsValid)
            {
                _context.Categories.Update(category);
                await _context.SaveChangesAsync();
            }
            return null;
        }
    }
}
