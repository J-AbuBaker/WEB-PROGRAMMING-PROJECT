using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using BookStore.Sessions;
using BookStore.Models;

namespace BookStore.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUserService _userService;

        public LoginController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost] // Login logic
        public IActionResult Index(string username, string password)
        {
            var user = _userService.Login(username, password);
            if (user != null)
            {
                HttpContext.Session.SetString("UserID", user.UserID.ToString());
                return RedirectToAction("List", "Book");
            }

            ViewBag.Error = "Invalid username or password";
            return View();
        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Login");
        }
    }
}

