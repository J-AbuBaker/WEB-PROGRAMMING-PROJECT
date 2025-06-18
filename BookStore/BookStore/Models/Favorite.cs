using System.ComponentModel.DataAnnotations;

namespace BookStore.Models
{
    public class Favorite
    {
        public int UserID { get; set; }
        public User User { get; set; }

        public int BookID { get; set; }
        public Book Book { get; set; }
    }
}
