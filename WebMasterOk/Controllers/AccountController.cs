using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMasterOk.Data;
using WebMasterOk.Models.Authoriziation;
using WebMasterOk.Models.CodeFirst;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace WebMasterOk.Controllers
{
    public class AccountController : Controller
    {
        private readonly DBMasterOkContext _context;

        public AccountController(DBMasterOkContext context)
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
            if(ModelState.IsValid)
            {
                Client client = await _context.Clients.FirstOrDefaultAsync(e => e.LoginClient == loginModel.Login && e.PasswordClient == loginModel.Password);
                
                if(client != null)
                {
                    await Authenticate(client);

                    return RedirectToAction(nameof(Index), "Home");
                }
                else
                {
                    User user = await _context.Users.Include(r => r.Role).FirstOrDefaultAsync(e => e.LoginUser == loginModel.Login && e.PasswordUser == loginModel.Password);
                    if(user != null)
                    {
                        await Authenticate(user);

                        return RedirectToAction(nameof(Index), "AdminPanel");
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "Некорреткные данные");
            }
            return View(loginModel);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel registerModel)
        {
            if (ModelState.IsValid)
            {
                Client client = await _context.Clients.FirstOrDefaultAsync(u => u.EmailClient == registerModel.Email);
                if (client == null)
                {

                    _context.Clients.Add(new Client { EmailClient = registerModel.Email, PasswordClient = registerModel.Password });

                    await _context.SaveChangesAsync();

                    await Authenticate(client);

                    return RedirectToAction(nameof(Index), "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Некорректные данные");
                }
            }
            return View(registerModel);
        }

        private async Task Authenticate(Client client)
        {
            // создаем один claim
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, client.Id.ToString())
            };
            // создаем объект ClaimsIdentity
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            // установка аутентификационных куки
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        private async Task Authenticate(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Id.ToString()),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role.DescriptionRole)
            };
            // создаем объект ClaimsIdentity
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            // установка аутентификационных куки
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }
    }
}
