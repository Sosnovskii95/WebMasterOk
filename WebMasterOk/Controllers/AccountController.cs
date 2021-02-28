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
        public IActionResult Login(string returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel loginModel, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                Client client = await _context.Clients.FirstOrDefaultAsync(e => e.LoginClient == loginModel.Login && e.PasswordClient == loginModel.Password);

                if (client != null)
                {
                    await Authenticate(client);

                    if (!String.IsNullOrEmpty(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction(nameof(Index), "ClientPersonalArea");
                    }
                }
                else
                {
                    User user = await _context.Users.Include(r => r.Role).FirstOrDefaultAsync(e => e.LoginUser == loginModel.Login && e.PasswordUser == loginModel.Password);
                    if (user != null)
                    {
                        if(user.Role.DescriptionRole.Equals("admin"))
                        {
                            await Authenticate(user);

                            return RedirectToAction(nameof(Index), "AdminPanel");
                        }
                        else
                        {
                            await Authenticate(user);

                            return RedirectToAction(nameof(Index), "ManagerPanel");
                        }
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
        public IActionResult Register(string returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel registerModel, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                Client client = await _context.Clients.FirstOrDefaultAsync(u => u.LoginClient == registerModel.Login);
                if (client == null)
                {
                    client = new Client
                    {
                        LoginClient = registerModel.Login,
                        EmailClient = registerModel.EmailClient,
                        PasswordClient = registerModel.Password,
                        Address = registerModel.Address,
                        FamClient = registerModel.FamClient,
                        FirstNameClient = registerModel.FirstNameClient,
                        LastNameClient = registerModel.LastNameClient,
                        NumberTelephone = registerModel.NumberTelephone
                    };
                    await _context.Clients.AddAsync(client);

                    await _context.SaveChangesAsync();

                    await Authenticate(client);

                    if (!String.IsNullOrEmpty(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction(nameof(Index), "Home");
                    }
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
                new Claim(ClaimsIdentity.DefaultNameClaimType, client.Id.ToString()),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, "client")
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
