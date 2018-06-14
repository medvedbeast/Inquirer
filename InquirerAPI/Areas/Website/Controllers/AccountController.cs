using InquirerAPI.Website.Data;
using InquirerAPI.Website.Models;
using InquirerAPI.Website.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace InquirerAPI.Website.Controllers
{
    [Authorize]
    [Area("Website")]
    public class AccountController : GenericController
    {
        public AccountController(DatabaseContext context, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
            : base(context, configuration, httpContextAccessor)
        {
        }

        [AllowAnonymous]
        public ViewResult Login(string returnUrl)
        {
            return View(new LoginViewModel
            {
                ReturnUrl = returnUrl ?? Url.Action("Index", "Home")
            });
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                string password = Hash(model.Password);
                User user = await Database.User
                    .FirstOrDefaultAsync(u => u.Email == model.Email && u.Password == password);
                if (user != null)
                {
                    await Authenticate(user);

                    return Redirect(model.ReturnUrl);

                }
                ModelState.AddModelError("", "Користувач з такою комбінацією логіну та паролю не знайдений.");
            }
            return View(model);
        }

        [AllowAnonymous]
        public ViewResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                string password = Hash(model.Password);
                User user = Database.User.FirstOrDefault(u => u.Email == model.Email && u.Password == password);
                if (user == null)
                {
                    user = new User
                    {
                        Name = model.Name,
                        Email = model.Email,
                        Password = password
                    };
                    Database.User.Add(user);
                    await Database.SaveChangesAsync();
                    await Authenticate(user);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Користувач з такою комбінацією логіну та паролю не знайдений.");
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<JsonResult> Edit([FromBody]EditAccountViewModel model)
        {
            var user = Database.User
                .Where(u => u.Id == model.Id)
                .FirstOrDefault();

            user.Name = model.Name;
            user.Email = model.Email;

            if (model.Password == model.PasswordConfirmation)
            {
                if (!string.IsNullOrEmpty(model.Password))
                {
                    user.Password = Hash(model.Password);
                }

            }
            else
            {
                return Json(new
                {
                    status = false,
                    message = "Новий пароль та його підтвердження не співпадають."
                });
            }

            await Database.SaveChangesAsync();

            return Json(new
            {
                status = true
            });
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login");
        }

        private async Task Authenticate(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, "user"),
                new Claim("UserId", user.Id.ToString())
            };
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
            user = Database.User
                .Where(u => u.Id == user.Id)
                .FirstOrDefault();
            user.LastSeenOn = DateTime.Now;
            await Database.SaveChangesAsync();
        }
    }
}
