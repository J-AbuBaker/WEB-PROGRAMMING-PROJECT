using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookStore.Models;

namespace BookStore.Controllers
{
    public class BookController : Controller
    {
        private readonly BookStoreContext context;

        public BookController(BookStoreContext ctx)
        {
            context = ctx;
        }

        [HttpGet]
        public IActionResult List()
        {
            var books = context.Books.Include(b => b.Favorites).ToList();
            ViewBag.Users = context.Users.ToList();
            return View(books);
        }
        
        [HttpGet]
        public IActionResult Details(int id)
        {
            var book = context.Books
                .Include(b => b.Favorites)
                .FirstOrDefault(b => b.BookID == id);

            if (book == null) return NotFound();
            return View(book);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var book = context.Books.Find(id);
            if (book == null) return NotFound();
            return View(book);
        }

        [HttpPost]
        public IActionResult Edit(Book book)
        {
            if (ModelState.IsValid)
            {
                var existingBook = context.Books.AsNoTracking().FirstOrDefault(b => b.BookID == book.BookID);
                if (existingBook == null) return NotFound();

                book.UserID = existingBook.UserID; // Preserve UserID!
                book.CreatedAt = existingBook.CreatedAt;
                book.UpdatedAt = DateTime.UtcNow;

                context.Books.Update(book);
                context.SaveChanges();
                return RedirectToAction("List");
            }

            return View(book);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var book = context.Books.Find(id);
            if (book == null) return NotFound();
            return View(book);
        }
        [HttpPost]
        public IActionResult Delete(Book book)
        {
            context.Books.Remove(book);
            context.SaveChanges();
            return RedirectToAction("List");
        }
        
        [HttpGet]
        public IActionResult Add() => View();

        [HttpPost]
        public IActionResult Add(Book book)
        {
            var userIDString = HttpContext.Session.GetString("UserID");
            if (!int.TryParse(userIDString, out int userID))
                return BadRequest("User ID missing or invalid.");

            book.UserID = userID;
            book.CreatedAt = DateTime.UtcNow;
            book.UpdatedAt = DateTime.UtcNow;

            if (ModelState.IsValid)
            {
                context.Books.Add(book);
                context.SaveChanges();
                return RedirectToAction("List");
            }
            return View(book);
        }

        [HttpGet]
        public IActionResult Search(string title, string author)
        {
            var books = context.Books.AsQueryable();

            if (!string.IsNullOrWhiteSpace(title))
                books = books.Where(b => b.Title.Contains(title));

            if (!string.IsNullOrWhiteSpace(author))
                books = books.Where(b => b.Author.Contains(author));

            ViewBag.Users = context.Users.ToList();
            return View("List", books.ToList());
        }

        [HttpPost]
        public IActionResult AddToFavorites(int bookId)
        {
            var userIDString = HttpContext.Session.GetString("UserID");
            if (!int.TryParse(userIDString, out int userID))
                return Unauthorized();

            var alreadyFavorite = context.Favorites.Any(f => f.UserID == userID && f.BookID == bookId);
            if (!alreadyFavorite)
            {
                var favorite = new Favorite { UserID = userID, BookID = bookId };
                context.Favorites.Add(favorite);
                context.SaveChanges();
            }

            return RedirectToAction("List");
        }

        [HttpPost]
        public IActionResult RemoveFromFavorites(int bookId)
        {
            var userIDString = HttpContext.Session.GetString("UserID");
            if (!int.TryParse(userIDString, out int userID))
                return Unauthorized();

            var favorite = context.Favorites.FirstOrDefault(f => f.UserID == userID && f.BookID == bookId);
            if (favorite != null)
            {
                context.Favorites.Remove(favorite);
                context.SaveChanges();
            }

            return RedirectToAction("List");
        }
    }
}

