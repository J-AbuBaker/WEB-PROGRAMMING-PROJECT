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

        private string _role = "User";
        public string Role
        {
            get => _role;
            set => _role = string.IsNullOrEmpty(value) ? "User" : value;
        }

        public bool IsAdmin { get; set; } = false;

        public List<Favorite> Favorites { get; set; } = new List<Favorite>();
        public List<Book> Books { get; set; } = new List<Book>();
    }
}
