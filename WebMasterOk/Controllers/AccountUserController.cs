using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebMasterOk.Data;
using WebMasterOk.Models.AuthoriziationUser;
using WebMasterOk.Models.CodeFirst;

namespace WebMasterOk.Controllers
{
    public class AccountUserController : Controller
    {
        private readonly DBMasterOkContext _context;

        public AccountUserController(DBMasterOkContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                User user = await _context.Users.Include(r => r.Role).FirstOrDefaultAsync(e => e.LoginUser == loginModel.Login && e.PasswordUser == loginModel.Password);
                if (user != null)
                {
                    await Authenticate(user);

                    return RedirectToAction(nameof(Index), nameof(HomeController));
                }
            }
            else
            {
                ModelState.AddModelError("", "данные не верны");
            }
            return View(loginModel);
        }

        private async Task Authenticate(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.LoginUser),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role.TitleRole)
            };
            ClaimsIdentity id = new ClaimsIdentity(claims, "AppCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }
    }
}