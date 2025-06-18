using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookStore.Models
{
    public class User
    {
        public int UserID { get; set; }

        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        // Navigation properties
        public List<Favorite> Favorites { get; set; } = new List<Favorite>();

        // ADD THIS: Navigation property for books authored by this user
        public List<Book> Books { get; set; } = new List<Book>();
    }
}
