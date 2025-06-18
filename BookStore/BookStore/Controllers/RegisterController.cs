using Microsoft.AspNetCore.Mvc;
using BookStore.Models;

namespace BookStore.Controllers
{
    public class RegisterController : Controller
    {
        private readonly BookStoreContext context;

        public RegisterController(BookStoreContext ctx)
        {
            context = ctx;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(string username, string password, string confirmPassword)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                ViewBag.Error = "Username and password are required.";
                return View();
            }

            if (password != confirmPassword)
            {
                ViewBag.Error = "Passwords do not match!";
                return View();
            }

            bool userExists = context.Users.Any(u => u.Username == username);
            if (userExists)
            {
                ViewBag.Error = "Username already taken.";
                return View();
            }

            var user = new User
            {
                Username = username,
                Password = password // Note: Should hash passwords in production
            };

            context.Users.Add(user);
            context.SaveChanges();

            return RedirectToAction("Index", "Login");
        }
    }
}

