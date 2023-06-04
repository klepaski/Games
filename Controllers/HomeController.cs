using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using task6x.Models;

namespace task6x.Controllers
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
            return View();
        }

        [HttpPost]
        public IActionResult Login()
        {
            var userName = HttpContext.Request.Form["userName"];
            HttpContext.Session.SetString("userName", userName);
            return RedirectPermanent("~/Home/Hangman");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("userName");
            return RedirectPermanent("~/Home/Index");
        }

        public IActionResult Crocodile()
        {
            var currentUser = HttpContext.Session.GetString("userName");
            if (currentUser == null) return RedirectPermanent("~/Home/Index");
            ViewBag.CurrentUser = currentUser;
            return View();
        }

        public IActionResult Hangman()
        {
            var currentUser = HttpContext.Session.GetString("userName");
            if (currentUser == null) return RedirectPermanent("~/Home/Index");
            ViewBag.CurrentUser = currentUser;
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}