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

    }
}

