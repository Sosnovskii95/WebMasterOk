using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
    public class ManagerModifySubCategoryController : Controller
    {
        private readonly DBMasterOkContext _context;
        private readonly IWebHostEnvironment _appEnvironment;

        public ManagerModifySubCategoryController(IWebHostEnvironment appEnvironment, DBMasterOkContext context)
        {
            _context = context;
            _appEnvironment = appEnvironment;
        }

        public async Task<IActionResult> Index(int? page, string searchTitle, int? searchCategoryId)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 20;

            IQueryable<SubCategory> subCategories = _context.SubCategories;

            if (!String.IsNullOrEmpty(searchTitle))
            {
                subCategories = subCategories.Where(s => s.TitleSubCategory.Contains(searchTitle));
            }
            if (searchCategoryId.HasValue && searchCategoryId > 0)
            {
                subCategories = subCategories.Where(s => s.CategoryId == searchCategoryId);
            }

            subCategories = subCategories.Include(c => c.Category).OrderBy(i => i.Id);

            var categories = await _context.Categories.ToListAsync();
            categories.Insert(0, new Category { Id = 0, TitleCategory = "Все" });
            ViewBag.Categories = new SelectList(categories, "Id", "TitleCategory");

            return View(await subCategories.ToPagedListAsync(pageNumber, pageSize));
        }

        [HttpGet]
        public async Task<IActionResult> AddSubCategory()
        {
            ViewBag.CategoryId = new SelectList(await _context.Categories.ToListAsync(), "Id", "TitleCategory");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddSubCategory(SubCategory subCategory, IFormFile pathImage)
        {
            if (ModelState.IsValid)
            {
                await _context.SubCategories.AddAsync(subCategory);
                await _context.SaveChangesAsync();

                PathImage image = new PathImage
                {
                    NameImage = pathImage.FileName,
                    SubCategoryId = subCategory.Id,
                    ProductId = null,
                    Slider = false,
                    CategoryId = null,
                    TypeImage = pathImage.ContentType
                };
                await _context.PathImages.AddAsync(image);
                await SaveFile(subCategory, pathImage);

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
        public async Task<IActionResult> EditSubCategory(SubCategory subCategory, IFormFile pathImage)
        {
            if (ModelState.IsValid)
            {
                PathImage image = await _context.PathImages.FirstOrDefaultAsync(i => i.SubCategoryId == subCategory.Id);
                image.NameImage = pathImage.FileName;
                await SaveFile(subCategory, pathImage);

                _context.SubCategories.Update(subCategory);
                _context.PathImages.Update(image);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(subCategory);
        }

        private async Task<bool> SaveFile(SubCategory subCategory, IFormFile formFile)
        {
            bool result = false;
            string pathSaveFile = _appEnvironment.WebRootPath + "/Content/SubCategory/" + subCategory.Id;
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
