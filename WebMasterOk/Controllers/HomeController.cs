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
using X.PagedList;
using Microsoft.AspNetCore.Mvc.Rendering;

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
                    image = new PathImage { NameImage = "maps.png", TypeImage = "image/png" };
                    currentDirectory = "/Content/";
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
            var product = await _context.Products.Include(s => s.Stores).Include(s => s.SubCategory).ThenInclude(s => s.Category).FirstOrDefaultAsync(p => p.Id == id);
            if (product != null)
            {
                return View(product);
            }

            return null;
        }

        [HttpGet]
        public async Task<IActionResult> ShowSubCategory(int? idCategory, int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 20;

            IQueryable<SubCategory> subCategories = _context.SubCategories;

            subCategories = subCategories.Where(i => i.CategoryId == idCategory).Include(p => p.PathImages);

            subCategories = subCategories.OrderBy(i => i.Id);

            return View(await subCategories.ToPagedListAsync(pageNumber, pageSize));
        }

        [HttpGet]
        public async Task<IActionResult> ShowProduct(int? idSubCategory, int? page, int? sorted, int? sizes)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 20;
            ViewData["page"] = pageNumber;
            ViewData["idSubCategory"] = idSubCategory;

            IQueryable<Product> products = _context.Products;

            products = products.Where(i => i.SubCategoryId == idSubCategory).Include(p => p.PathImages);

            ViewBag.SubCategory = await _context.SubCategories.Include(c => c.Category).FirstOrDefaultAsync(i => i.Id == idSubCategory);

            List<SelectListItem> sortedItems = generateSort();

            List<SelectListItem> sizeItems = generateSize();

            if (sorted.HasValue)
            {
                if (sorted == 1)
                {
                    sortedItems[0].Selected = true;
                    products = products.OrderBy(i => i.Id);
                }
                if (sorted == 2)
                {
                    sortedItems[1].Selected = true;
                    products = products.OrderBy(p => p.Price);
                }
                if (sorted == 3)
                {
                    sortedItems[2].Selected = true;
                    products = products.OrderByDescending(p => p.Price);
                }
            }

            if (sizes.HasValue)
            {
                if (sizes == 1)
                {
                    sizeItems[0].Selected = true;
                    pageSize = 20;
                }
                if (sizes == 2)
                {
                    sizeItems[1].Selected = true;
                    pageSize = 40;
                }
            }

            ViewBag.Sorted = new SelectList(sortedItems, "Value", "Text");
            ViewBag.Sizes = new SelectList(sizeItems, "Value", "Text");

            return View(await products.ToPagedListAsync(pageNumber, pageSize));
        }

        private List<SelectListItem> generateSort()
        {
            List<SelectListItem> sortedItems = new List<SelectListItem>();
            sortedItems.Add(new SelectListItem() { Text = "По порядку", Value = "1" });
            sortedItems.Add(new SelectListItem() { Text = "По росту цены", Value = "2" });
            sortedItems.Add(new SelectListItem() { Text = "По снижению цены", Value = "3" });

            return sortedItems;
        }

        private List<SelectListItem> generateSize()
        {
            List<SelectListItem> sizeItems = new List<SelectListItem>();
            sizeItems.Add(new SelectListItem() { Text = "20", Value = "1" });
            sizeItems.Add(new SelectListItem() { Text = "40", Value = "2" });

            return sizeItems;
        }

        [HttpGet]
        public async Task<IActionResult> ShowSearch(int? page, string search)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 20;

            IQueryable<Product> products = _context.Products;

            if (!String.IsNullOrEmpty(search))
            {
                products = products.Where(s => s.TitleProduct.Contains(search) || s.DescriptionProduct.Contains(search));
            }

            return View(await products.ToPagedListAsync(pageNumber, pageSize));
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
