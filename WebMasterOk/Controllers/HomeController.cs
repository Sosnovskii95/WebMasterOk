using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebMasterOk.Models;
using WebMasterOk.Models.CodeFirst;
using WebMasterOk.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Security.Claims;

namespace WebMasterOk.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DBMasterOkContext _context;
        private readonly IWebHostEnvironment _appEnvironment;

        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment appEnvironment, DBMasterOkContext context)
        {
            _logger = logger;
            _context = context;
            _appEnvironment = appEnvironment;

        }

        //[Authorize]
        public async Task<IActionResult> Index()
        {
            var categories = await _context.Categories.Include(s => s.SubCategories).Include(p => p.PathImages).ToListAsync();
            ViewBag.Slider = await _context.PathImages.Where(s => s.Slider == true).ToListAsync();

            return View(categories);
        }

        public async Task<VirtualFileResult> GetImage(int id, string typeObject)
        {
            string currentDirectory = "~/Content/";
            string openFileName = null;

            if (!String.IsNullOrEmpty(typeObject))
            {
                if (typeObject.Equals("product"))
                {
                    var temp = await _context.Products.FindAsync(id);
                    var image = await _context.PathImages.FirstOrDefaultAsync(i => i.ProductId == id);
                    currentDirectory += "Product/" + temp.Id;
                    openFileName = image.NameImage;
                }
                if (typeObject.Equals("subCategory"))
                {
                    var temp = await _context.SubCategories.FindAsync(id);
                    var image = await _context.PathImages.FirstOrDefaultAsync(i => i.SubCategoryId == id);
                    currentDirectory += "SubCategory/" + temp.Id;
                    openFileName = image.NameImage;
                }
                if (typeObject.Equals("category"))
                {
                    var temp = await _context.Categories.FindAsync(id);
                    var image = await _context.PathImages.FirstOrDefaultAsync(i => i.CategoryId == id);
                    currentDirectory += "Category/" + temp.Id;
                    openFileName = image.NameImage;
                }
                if (typeObject.Equals("slider"))
                {
                    var temp = await _context.PathImages.FindAsync(id);
                    currentDirectory += "Slider/" + temp.Id;
                    openFileName = temp.NameImage;
                }

                return File(Path.Combine(currentDirectory, openFileName), "application/png", openFileName);
            }
            else
            {
                openFileName = "maps.png";
                return File(Path.Combine(currentDirectory, openFileName), "application/png", openFileName);

            }

            return null;
        }

        public async Task<IActionResult> Details(int id)
        {
            var product = await _context.Products.Include(s => s.Stores).FirstOrDefaultAsync(p => p.Id == id);
            if (product != null)
            {
                return View(product);
            }

            return null;
        }

        [HttpGet]
        public async Task<IActionResult> ShowSubCategory(int? idCategory)
        {
            var subCategories = await _context.SubCategories.Where(i => i.CategoryId == idCategory).Include(p => p.PathImages).ToListAsync();

            return View(subCategories);
        }

        [HttpGet]
        public async Task<IActionResult> ShowProduct(int? idSubCategory)
        {
            var products = await _context.Products.Where(i => i.SubCategoryId == idSubCategory).Include(p=>p.PathImages).ToListAsync();

            return View(products);
        }

        [HttpPost]
        public async Task<IActionResult> ShowSearch(string search)
        {
            List<Product> listProduct = null;

            if(!String.IsNullOrEmpty(search))
            {
                listProduct = await _context.Products.Where(i => i.TitleProduct.Contains(search)).ToListAsync();
            }
            return View(listProduct);
        }

        [HttpGet]
        public IActionResult ShowFeedBack()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ShowFeedBack(FeedBack feedBack)
        {
            if(ModelState.IsValid)
            {
                feedBack.UserId = null;
                await _context.FeedBacks.AddAsync(feedBack);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public IActionResult Contact()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Delivery()
        {
            return View();
        }
        
        [HttpGet]
        public IActionResult Pay()
        {
            return View();
        }
    }
}
