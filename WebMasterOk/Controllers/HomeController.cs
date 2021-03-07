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
            string currentDirectory = "";
            PathImage image = null;

            if (!String.IsNullOrEmpty(typeObject))
            {
                if (typeObject.Equals("product"))
                {
                    image = await _context.PathImages.FirstOrDefaultAsync(i => i.ProductId == id);
                    currentDirectory += "/Content/Product/" + id;
                }
                else if (typeObject.Equals("subCategory"))
                {
                    image = await _context.PathImages.FirstOrDefaultAsync(i => i.SubCategoryId == id);
                    currentDirectory += "/Content/SubCategory/" + id;
                }
                else if (typeObject.Equals("category"))
                {
                    image = await _context.PathImages.FirstOrDefaultAsync(i => i.CategoryId == id);
                    currentDirectory += "/Content/Category/" + id;
                }
                else if (typeObject.Equals("slider"))
                {
                    image = await _context.PathImages.FindAsync(id);
                    currentDirectory += "/Content/Slider/" + image.Id;
                }
                else if (typeObject.Equals("maps"))
                {
                    image = new PathImage { NameImage = "maps.png" };
                    currentDirectory = "~/Content/";
                }
            }
            if (CheckFile(_appEnvironment.WebRootPath + currentDirectory, image.NameImage))
            {
                return File(Path.Combine("~" + currentDirectory, image.NameImage), image.TypeImage, image.NameImage);
            }
            else
            {
                currentDirectory = "~/Content/";
                image = new PathImage { NameImage = "image-aborted.jpg" };
                return File(Path.Combine(currentDirectory, image.NameImage), "image/jpg", image.NameImage);
            }
        }

        private bool CheckFile(string currentDirectory, string fileName)
        {
            bool result = true;


            if (!(System.IO.File.Exists(Path.Combine(currentDirectory, fileName))))
            {
                result = false;
            }

            return result;
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
            var products = await _context.Products.Where(i => i.SubCategoryId == idSubCategory).Include(p => p.PathImages).ToListAsync();

            return View(products);
        }

        [HttpPost]
        public async Task<IActionResult> ShowSearch(string search)
        {
            List<Product> listProduct = null;

            if (!String.IsNullOrEmpty(search))
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
            if (ModelState.IsValid)
            {
                feedBack.UserId = null;
                feedBack.StateFeedBack = "В обработке";
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
