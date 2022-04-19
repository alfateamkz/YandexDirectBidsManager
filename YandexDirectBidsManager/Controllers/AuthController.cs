using BidsManager.Database.Models;
using BidsManager.Database.Modules;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace YandexDirectBidsManager.Controllers
{
    public class AuthController : Controller
    {
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Home");
        }

        [Route("CheckUser")]
        [HttpGet]
        public async Task<IActionResult> CheckUser(string login)
        {
            var user = UsersModule.GetUserByEmail(login);
            if (user != null)
                return new JsonResult(403);
            else
                return new JsonResult(200);
        }
        [Route("CheckUserCredentials")]
        [HttpGet]
        public async Task<IActionResult> CheckUserCredentials(string login, string password)
        {
            var user = UsersModule.GetUserByCredentials(login, password);
            if (user != null)
                return new JsonResult(200);
            else
                return new JsonResult(404);
        }




        [HttpPost]
        public async Task<IActionResult> Auth(UserModel model)
        {
            var found = UsersModule.GetUserByCredentials(model.Email, model.Password);
            if (found != null)
            {
                var principal = await MakeAuth(found);
                return RedirectToAction("Dashboard", "Dashboard");
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(UserModel model)
        {
            var found = UsersModule.GetUserByEmail(model.Email);
            if (found != null)
                return RedirectToAction("Registration", "Home");

            UsersModule.CreateUser(model);

            return RedirectToAction("Index", "Home");
        }




        private async Task<ClaimsPrincipal> MakeAuth(UserModel user)
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // создаем один claim
            var claims = new List<Claim>
            {
                 new Claim(ClaimsIdentity.DefaultNameClaimType, user.Id.ToString()),
            };
            // создаем объект ClaimsIdentity
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            var principal = new ClaimsPrincipal(id);
            // установка аутентификационных куки
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
            return principal;
        }
    }
}
