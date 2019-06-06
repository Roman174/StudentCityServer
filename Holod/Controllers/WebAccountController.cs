using Holod.Models;
using Holod.Models.Database;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Holod.Controllers
{
    [Route("web/account")]
    public class WebAccountController : Controller
    {
        private readonly DatabaseContext database;

        public WebAccountController(DatabaseContext database)
        {
            this.database = database;
        }

        [Route("login")]
        public IActionResult Login() => View("Views/Account/Login.cshtml");

        [Route("login")]
        [HttpPost]
        public async Task<IActionResult> LoginAsync(LoginModel model)
        {
            string redirectUrl = $"{Request.Scheme}://{Request.Host.Host}:{Request.Host.Port}/web/account/login";

            if (model is null) return View("Views/Error.cshtml", new ErrorViewModel
            {
                Message = "Введите данные для авторизации",
                RedirectUrl = redirectUrl
            });

            User user = await database.Users
                .FirstOrDefaultAsync(u => u.Login == model.Login);

            if (user is null) return View("Views/Error.cshtml", new ErrorViewModel
            {
                Message = "Пользователь не найден",
                RedirectUrl = redirectUrl
            });

            if (user.Password == model.Password)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, model.Login)
                };

                ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));

                return RedirectToAction("Index", "Home");
            }
            else return View("Views/Error.cshtml", new ErrorViewModel
            {
                Message = "Неверный пароль",
                RedirectUrl = redirectUrl
            });
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        [Authorize]
        [Route("add")]
        public IActionResult Add() => View("Views/Account/Add.cshtml");

        [Authorize]
        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> Add(User user)
        {
            string redirectUrl = $"{Request.Scheme}://{Request.Host.Host}/web/account/add";

            if (user is null) return View("Views/Error.cshtml", new ErrorViewModel
            {
                Message = "Введите данные пользователя",
                RedirectUrl = redirectUrl
            });

            User foundUser = await database.Users
                .FirstOrDefaultAsync(u => u.Login == user.Login);

            if (foundUser != null) return View("Views/Error.cshtml", new ErrorViewModel
            {
                Message = "Пользователь уже существует",
                RedirectUrl = redirectUrl
            });

            await database.Users.AddAsync(user);
            await database.SaveChangesAsync();

            return RedirectToAction("Add");
        }
    }
}