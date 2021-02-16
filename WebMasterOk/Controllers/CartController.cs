using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMasterOk.Data;
using WebMasterOk.Models.CodeFirst;
using WebMasterOk.Models;
using Microsoft.AspNetCore.Http;

namespace WebMasterOk.Controllers
{
    public class CartController : Controller
    {
        private readonly DBMasterOkContext _context;
        private List<Cart> cart;

        public CartController(DBMasterOkContext context)
        {
            _context = context;
        }

        public IActionResult Index(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            ViewBag.TotalValue = ModifyCart.GetTotalValue(GetCart());
            return View(GetCart());
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId, string returnUrl)
        {
            Product product = await _context.Products.FindAsync(productId);
            if (product != null)
            {
                cart = GetCart();
                cart = ModifyCart.Additem(cart, product, 1);
                SetCard(cart);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(int productId, string returnUrl)
        {
            Product product = await _context.Products.FindAsync(productId);
            if(product != null)
            {
                cart = GetCart();
                cart = ModifyCart.RemoveItem(cart, product);
                SetCard(cart);
            }

            return RedirectToAction("Index", new { returnUrl });
        }

        [HttpGet]
        public PartialViewResult Summary()
        {
            cart = GetCart();
            ViewBag.CountProduct = cart.Count().ToString();
            ViewBag.TotalValue = ModifyCart.GetTotalValue(GetCart());
            return PartialView();
        }

        private List<Cart> GetCart()
        {
            List<Cart> cart = HttpContext.Session.Get<List<Cart>>("Cart");

            if (cart == null)
            {
                cart = new List<Cart>();
            }

            return cart;
        }

        private void SetCard(List<Cart> carts)
        {
            HttpContext.Session.Set<List<Cart>>("Cart", carts);
        }
    }
}
