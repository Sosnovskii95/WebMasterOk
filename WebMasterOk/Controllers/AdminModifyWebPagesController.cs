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
    public class AdminModifyWebPagesController : Controller
    {
        private readonly DBMasterOkContext _context;
        private readonly IWebHostEnvironment _appEnvironment;

        public AdminModifyWebPagesController(DBMasterOkContext context, IWebHostEnvironment appEnvironment)
        {
            _context = context;
            _appEnvironment = appEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            var pathImages = await _context.PathImages.Where(s => s.Slider == true).ToListAsync();
            return View(pathImages);
        }

        [HttpGet]
        public async Task<VirtualFileResult> GetImage(int? id)
        {
            if (id.HasValue)
            {
                PathImage image = await _context.PathImages.FindAsync(id);
                string currentDirectory = "/Content/Slider/" + image.Id;

                if (CheckFile(_appEnvironment.WebRootPath + currentDirectory, image.NameImage))
                {
                    return File(Path.Combine("~" + currentDirectory, image.NameImage), image.TypeImage, image.NameImage);
                }
                else
                {
                    currentDirectory = "~/Content/";
                    image = new PathImage { NameImage = "image-aborted.jpg", TypeImage = "image/jpeg" };
                    return File(Path.Combine(currentDirectory, image.NameImage), image.TypeImage, image.NameImage);
                }
            }
            else
            {
                string currentDirectory = "~/Content/";
                PathImage image = new PathImage { NameImage = "plus.png", TypeImage = "image/png" };
                return File(Path.Combine(currentDirectory, image.NameImage), image.TypeImage, image.NameImage);
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteImage(int id)
        {
            PathImage image = await _context.PathImages.FindAsync(id);
            string currentDirectory = "/Content/Slider/" + image.Id;

            if (CheckFile(_appEnvironment.WebRootPath + currentDirectory, image.NameImage))
            {
                FileInfo file = new FileInfo(_appEnvironment.WebRootPath + currentDirectory);
                file.Delete();
            }
            _context.PathImages.Remove(image);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> AddImage(IFormFile pathImage)
        {
            PathImage image = new PathImage
            {
                NameImage = pathImage.FileName,
                TypeImage = pathImage.ContentType,
                Slider = true,
                CategoryId = null,
                ProductId = null,
                SubCategoryId = null
            };

            await _context.PathImages.AddAsync(image);
            await _context.SaveChangesAsync();

            SaveFile(image, pathImage);

            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> SaveFile(PathImage pathImage, IFormFile formFile)
        {
            bool result = false;
            string pathSaveFile = _appEnvironment.WebRootPath + "/Content/Slider/" + pathImage.Id;
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

        private bool CheckFile(string currentDirectory, string fileName)
        {
            bool result = true;


            if (!(System.IO.File.Exists(Path.Combine(currentDirectory, fileName))))
            {
                result = false;
            }

            return result;
        }
    }
}
