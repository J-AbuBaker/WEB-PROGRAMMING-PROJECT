using Microsoft.AspNetCore.Mvc;
using BookStore.Models;
using BookStore.ViewModels;

namespace BookStore.Controllers
{
    public class UserController : Controller
    {
        private readonly BookStoreContext context;

        public UserController(BookStoreContext ctx)
        {
            context = ctx;
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var user = context.Users.Find(id);
            if (user == null) return NotFound();

            var favoriteBookIds = context.Favorites
                .Where(f => f.UserID == id)
                .Select(f => f.BookID)
                .ToList();

            var favoriteBooks = context.Books
                .Where(b => favoriteBookIds.Contains(b.BookID))
                .ToList();

            ViewBag.FavoriteBooks = favoriteBooks;

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
