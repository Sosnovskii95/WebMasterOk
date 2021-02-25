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
    public class AdminModifyProductController : Controller
    {
        private readonly DBMasterOkContext _context;
        private readonly IWebHostEnvironment _appEnvironment;

        public AdminModifyProductController(IWebHostEnvironment appEnvironment, DBMasterOkContext context)
        {
            _context = context;
            _appEnvironment = appEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _context.Products.Include(s => s.SubCategory).ToListAsync();

            return View(products);
        }

        [HttpGet]
        public async Task<IActionResult> AddProduct()
        {
            ViewBag.SubCategoryId = new SelectList(await _context.SubCategories.ToListAsync(), "Id", "TitleSubCategory");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(Product product, IFormFile pathImage)
        {
            if (ModelState.IsValid)
            {
                await _context.Products.AddAsync(product);

                PathImage nameImage = new PathImage
                {
                    NameImage = pathImage.FileName,
                    ProductId = product.Id,
                    Slider = false,
                    CategoryId = null,
                    SubCategoryId = null
                };
                await _context.PathImages.AddAsync(nameImage);

                await _context.SaveChangesAsync();
                await SaveFile(product, pathImage);

                return RedirectToAction(nameof(Index));
            }

            return View(product);
        }

        [HttpGet]
        public async Task<IActionResult> EditProduct(int id)
        {
            ViewBag.SubCategoryId = new SelectList(await _context.SubCategories.ToListAsync(), "Id", "TitleSubCategory");
            Product product = await _context.Products.FindAsync(id);

            if (product != null)
            {
                return View(product);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> EditProduct(Product product, IFormFile pathImages)
        {
            if (ModelState.IsValid)
            {
                await SaveFile(product, pathImages);

                PathImage image = await _context.PathImages.FirstOrDefaultAsync(p => p.ProductId == product.Id);
                image.NameImage = pathImages.FileName;

                _context.PathImages.Update(image);
                _context.Products.Update(product);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(product);
        }

        private async Task<bool> SaveFile(Product product, IFormFile formFile)
        {
            bool result = false;
            string pathSaveFile = _appEnvironment.WebRootPath + "/Content/Product/" + product.Id;
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
