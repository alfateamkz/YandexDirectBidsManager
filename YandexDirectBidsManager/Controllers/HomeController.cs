using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using YandexDirectBidsManager.Models;

namespace YandexDirectBidsManager.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Dashboard", "Dashboard");
            return RedirectToAction("Login", "Home");
        }

        [Route("Login")]
        public IActionResult Login()
        {
            return View();
        }

        [Route("Registration")]
        public IActionResult Registration()
        {
            return View();
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}