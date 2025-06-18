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

    }
}

