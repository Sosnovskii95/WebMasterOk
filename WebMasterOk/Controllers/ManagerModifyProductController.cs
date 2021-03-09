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
    public class ManagerModifyProductController : Controller
    {
        private readonly DBMasterOkContext _context;
        private readonly IWebHostEnvironment _appEnvironment;

        public ManagerModifyProductController(IWebHostEnvironment appEnvironment, DBMasterOkContext context)
        {
            _context = context;
            _appEnvironment = appEnvironment;
        }

        public async Task<IActionResult> Index(int? page, string searchTitle, int? searchSubCategoryId)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 20;

            IQueryable<Product> products = _context.Products;

            if (!String.IsNullOrEmpty(searchTitle))
            {
                products = products.Where(p => p.TitleProduct.Contains(searchTitle));
            }
            if (searchSubCategoryId.HasValue && searchSubCategoryId > 0)
            {
                products = products.Where(i => i.SubCategoryId == searchSubCategoryId);
            }
            products = products.Include(s => s.SubCategory).OrderBy(i => i.Id);

            var subCategories = await _context.SubCategories.ToListAsync();
            subCategories.Insert(0, new SubCategory { Id = 0, TitleSubCategory = "Все" });
            ViewBag.SubCategories = new SelectList(subCategories, "Id", "TitleSubCategory");

            return View(await products.ToPagedListAsync(pageNumber, pageSize));
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
                await _context.SaveChangesAsync();

                PathImage nameImage = new PathImage
                {
                    NameImage = pathImage.FileName,
                    ProductId = product.Id,
                    Slider = false,
                    CategoryId = null,
                    SubCategoryId = null,
                    TypeImage = pathImage.ContentType
                };
                await _context.PathImages.AddAsync(nameImage);
                await SaveFile(product, pathImage);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(product);
        }

        [HttpGet]
        public async Task<IActionResult> EditProduct(int id)
        {
            ViewBag.SubCategoryId = new SelectList(await _context.SubCategories.ToListAsync(), "Id", "TitleSubCategory");
            Product product = await _context.Products.Include(p => p.PathImages).FirstOrDefaultAsync(i => i.Id == id);

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
                PathImage image = await _context.PathImages.FirstOrDefaultAsync(p => p.ProductId == product.Id);

                if (pathImages != null)
                {
                    image.NameImage = pathImages.FileName;
                    await SaveFile(product, pathImages);

                    _context.PathImages.Update(image);
                }

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
