using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookStore.Models;
using BookStore.ViewModels;

namespace BookStore.Controllers
{
    public class ProfileController : Controller
    {
        private readonly BookStoreContext context;

        public ProfileController(BookStoreContext ctx)
        {
            context = ctx;
        }

        public IActionResult Index()
        {
            var userIdString = HttpContext.Session.GetString("UserID");

            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
            {
                return RedirectToAction("Index", "Login");
            }

            var user = context.Users
                .Include(u => u.Favorites)
                .ThenInclude(f => f.Book)
                .FirstOrDefault(u => u.UserID == userId);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ChangePassword(ChangePasswordViewModel model)
        {
            int userId = int.Parse(HttpContext.Session.GetString("UserID") ?? "0");
            var user = context.Users.FirstOrDefault(u => u.UserID == userId);

            if (user == null)
            {
                TempData["ErrorMessage"] = "User not found.";
                return RedirectToAction("ChangePassword");
            }

            if (user.Password != model.CurrentPassword)
            {
                TempData["ErrorMessage"] = "Current password is incorrect.";
                return RedirectToAction("ChangePassword");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            user.Password = model.NewPassword;
            context.SaveChanges();
            TempData["StatusMessage"] = "Password updated successfully.";
            return RedirectToAction("ChangePassword");
        }
    }
}

