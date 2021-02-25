using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMasterOk.Data;
using WebMasterOk.Models.CodeFirst;
using WebMasterOk.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Rendering;

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

        public async Task<IActionResult> Index(string returnUrl)
        {
            if (GetCart().Count == 0)
            {
                return RedirectToAction(nameof(Index), "Home");
            }
            else
            {
                ViewBag.ReturnUrl = returnUrl;
                ViewBag.TotalValue = ModifyCart.GetTotalValue(GetCart());
                ViewBag.Cart = GetCart();
                ViewBag.PayMethods = new SelectList(await _context.PayMethods.ToListAsync(), "Id", "TitlePayMethod");
                ViewBag.DeliveryMethods = new SelectList(await _context.DeliveryMethods.ToListAsync(), "Id", "TitleDeliveryMethod");

                string role = User.FindFirst(x => x.Type == ClaimsIdentity.DefaultRoleClaimType)?.Value;
                if (!String.IsNullOrEmpty(role))
                {
                    if (role.Equals("client"))
                    {
                        int clientId = Convert.ToInt32(User.FindFirst(x => x.Type == ClaimsIdentity.DefaultNameClaimType).Value);
                        Client client = await _context.Clients.FindAsync(clientId);
                        if (client != null)
                        {
                            return View(client);
                        }
                    }
                }
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId, int quanity, string returnUrl)
        {
            Product product = await _context.Products.FindAsync(productId);
            if (product != null)
            {
                cart = GetCart();
                if (quanity > 0)
                {
                    cart = ModifyCart.RemoveItem(cart, product);
                    cart = ModifyCart.Additem(cart, product, quanity);
                }
                else
                {
                    cart = ModifyCart.Additem(cart, product, 1);
                }
                SetCard(cart);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(int productId, string returnUrl)
        {
            Product product = await _context.Products.FindAsync(productId);
            if (product != null)
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

        [HttpPost]
        public async Task<IActionResult> ProductCheck(Client client, int payMethod, int deliveryMethod)
        {
            cart = GetCart();

            if (client.Id == 0)
            {
                client.LoginClient = client.EmailClient;
                client.PasswordClient = client.EmailClient;

                await _context.Clients.AddAsync(client);
                await _context.SaveChangesAsync();
            }

            ProductSold productSold = new ProductSold { DateSale = DateTime.Now, ClientId = client.Id, UserId = null, StateOrder = "В обработке" };
            await _context.ProductSolds.AddAsync(productSold);
            await _context.SaveChangesAsync();

            foreach (Cart item in cart)
            {
                await _context.ProductChecks.AddAsync(new ProductCheck
                {
                    ProductId = item.Product.Id,
                    QuantitySale = item.Quantity,
                    ProductSoldId = productSold.Id,
                    PayMethodId = payMethod,
                    DeliveryMethodId = deliveryMethod
                });
                Store store = await _context.Stores.FirstOrDefaultAsync(p => p.ProductId == item.Product.Id);
                store.Quantity -= item.Quantity;
                _context.Stores.Update(store);
            }
            await _context.SaveChangesAsync();

            SetCard(null);

            return RedirectToAction("History", "ClientPersonalArea");
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
