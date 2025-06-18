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
    }
}

