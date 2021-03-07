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
using X.PagedList;

namespace WebMasterOk.Controllers.Manager
{
    public class ManagerModifyCategoryController : Controller
    {
        private readonly DBMasterOkContext _context;
        private readonly IWebHostEnvironment _appEnvironment;

        public ManagerModifyCategoryController(IWebHostEnvironment appEnvironment, DBMasterOkContext context)
        {
            _context = context;
            _appEnvironment = appEnvironment;
        }

        public async Task<ActionResult> Index(int? page, string searchCategory)
        {
            ViewData["searchCategory"] = searchCategory;

            int pageNumber = (page ?? 1);
            int pageSize = 20;

            IQueryable<Category> categories = _context.Categories;

            if (!String.IsNullOrEmpty(searchCategory))
            {
                categories = categories.Where(c => c.TitleCategory.Contains(searchCategory));
            }

            categories = categories.OrderBy(i => i.Id);

            return View(await categories.ToPagedListAsync(pageNumber, pageSize));
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
                await _context.SaveChangesAsync();

                PathImage image = new PathImage
                {
                    NameImage = pathImage.FileName,
                    ProductId = null,
                    CategoryId = category.Id,
                    SubCategoryId = null,
                    Slider = false,
                    TypeImage = pathImage.ContentType
                };
                await _context.PathImages.AddAsync(image);
                await SaveFile(category, pathImage);

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
