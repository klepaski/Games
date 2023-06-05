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

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Index()
        {
            var currentUser = HttpContext.Session.GetString("userName");
            if (currentUser == null) return RedirectPermanent("~/Home/Login");
            ViewBag.CurrentUser = currentUser;
            return View();
        }

        [HttpPost]
        public IActionResult Authorise()
        {
            var userName = HttpContext.Request.Form["userName"];
            HttpContext.Session.SetString("userName", userName);
            return RedirectPermanent("~/Home/Index");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("userName");
            return RedirectPermanent("~/Home/Login");
        }

        public IActionResult Crocodile(string role)
        {
            ViewBag.Role = role;
            return View();
        }

        public IActionResult Hangman(string role)
        {
            ViewBag.Role = role;
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}