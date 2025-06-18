using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookStore.Models
{

    public class Book
    {
     public int BookID { get; set; }   
      [Required, StringLength(100)]
        public string Title { get; set; }
          [Required, Range(0.01, 1000)]
        public decimal Price { get; set; }
        [Required, RegularExpression(@"\d{3}-\d{10}", ErrorMessage = "Invalid ISBN format")]
        public string ISBN { get; set; }
        public DateTime PublishDate { get; set; }
        public string Genre { get; set; }
       [DataType(DataType.MultilineText)] 
       public string Description { get; set; }
       public string CoverImageUrl { get; set; }
       public int StockQuantity { get; set; }  
       public float Rating { get; set; }  
       public bool IsFeatured { get; set; }
      public DateTime CreatedAt { get; set; } = DateTime.UtcNow;  
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
// Required foreign key for Author
        public int UserID { get; set; }
    // Optional navigation property
        public User? User { get; set; }
        public List<Favorite> Favorites { get; set; } = new();

    }
}

