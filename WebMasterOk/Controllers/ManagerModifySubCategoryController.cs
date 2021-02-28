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

namespace WebMasterOk.Controllers
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
                    CategoryId = null
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
