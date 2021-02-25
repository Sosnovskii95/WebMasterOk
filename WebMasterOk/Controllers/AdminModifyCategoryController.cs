using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebMasterOk.Data;
using WebMasterOk.Models.CodeFirst;

namespace WebMasterOk.Controllers
{
    public class AdminModifyCategoryController : Controller
    {
        private readonly DBMasterOkContext _context;
        private readonly IWebHostEnvironment _appEnvironment;

        public AdminModifyCategoryController(IWebHostEnvironment appEnvironment, DBMasterOkContext context)
        {
            _context = context;
            _appEnvironment = appEnvironment;
        }

        public async Task<ActionResult> Index()
        {
            var categories = await _context.Categories.ToListAsync();
            return View(categories);
        }

        [HttpGet]
        public IActionResult AddCategory()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddCategory(Category category, IFormFile pathImage)
        {
            if (ModelState.IsValid)
            {
                await _context.Categories.AddAsync(category);

                PathImage image = new PathImage
                {
                    NameImage = pathImage.FileName,
                    ProductId = null,
                    CategoryId = category.Id,
                    SubCategoryId = null,
                    Slider = false
                };
                await _context.PathImages.AddAsync(image);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        [HttpGet]
        public async Task<IActionResult> EditCategory(int id)
        {
            Category category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                return View(category);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> EditCategory(Category category, IFormFile pathImage)
        {
            if (ModelState.IsValid)
            {
                PathImage image = await _context.PathImages.FirstOrDefaultAsync(c => c.CategoryId == category.Id);
                image.NameImage = pathImage.FileName;

                await SaveFile(category, pathImage);

                _context.PathImages.Update(image);
                _context.Categories.Update(category);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        private async Task<bool> SaveFile(Category category, IFormFile formFile)
        {
            bool result = false;
            string pathSaveFile = _appEnvironment.WebRootPath + "/Content/Category/" + category.Id;
            if (!Directory.Exists(pathSaveFile))
            {
                Directory.CreateDirectory(pathSaveFile);
            }
            pathSaveFile += "/" + formFile.FileName;

            using (var fileStream = new FileStream(pathSaveFile, FileMode.Create))
            {
                await formFile.CopyToAsync(fileStream);
                result = true;
            }
            return result;
        }
    }
}
