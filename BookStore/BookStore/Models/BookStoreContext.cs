using Microsoft.EntityFrameworkCore;
using System;
using BookStore.Models;

namespace BookStore.Models
{
    public class BookStoreContext : DbContext
    {
        public BookStoreContext(DbContextOptions<BookStoreContext> options)
            : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Favorite> Favorites { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Composite key for many-to-many Favorite table
            modelBuilder.Entity<Favorite>()
                .HasKey(f => new { f.UserID, f.BookID });

            // Configure Favorite → User relationship (Restrict to avoid multiple cascade paths)
            modelBuilder.Entity<Favorite>()
                .HasOne(f => f.User)
                .WithMany(u => u.Favorites)
                .HasForeignKey(f => f.UserID)
                .OnDelete(DeleteBehavior.Restrict); // Fixes cascade conflict

            // Configure Favorite → Book relationship
            modelBuilder.Entity<Favorite>()
                .HasOne(f => f.Book)
                .WithMany(b => b.Favorites)
                .HasForeignKey(f => f.BookID)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure Book → User relationship
            modelBuilder.Entity<Book>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(b => b.UserID)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure precision for decimal price
            modelBuilder.Entity<Book>()
                .Property(b => b.Price)
                .HasPrecision(10, 2);

            // Seed Users
            modelBuilder.Entity<User>().HasData(
                new User { UserID = 1, Username = "admin", Password = "admin123" },
                new User { UserID = 2, Username = "alice", Password = "alicepwd" },
                new User { UserID = 3, Username = "bob", Password = "bob123" },
                new User { UserID = 4, Username = "charlie", Password = "charlie123" },
                new User { UserID = 5, Username = "diana", Password = "diana123" },
                new User { UserID = 6, Username = "eve", Password = "evepass" },
                new User { UserID = 7, Username = "frank", Password = "frankpass" },
                new User { UserID = 8, Username = "grace", Password = "gracepass" }
            );

            // Seed Books (all must include UserID)
            modelBuilder.Entity<Book>().HasData(
                new Book { BookID = 1, Title = "Clean Code", Author = "Robert C. Martin", Price = 45.00m, ISBN = "978-0132350884", PublishDate = new DateTime(2008, 8, 1), Genre = "Software Engineering", Description = "A Handbook of Agile Software Craftsmanship", CoverImageUrl = "https://images-na.ssl-images-amazon.com/images/I/41jEbK-jG+L.jpg", StockQuantity = 10, Rating = 4.8f, IsFeatured = true, UserID = 3 },
                new Book { BookID = 2, Title = "The Pragmatic Programmer", Author = "Andrew Hunt, David Thomas", Price = 50.00m, ISBN = "978-0201616224", PublishDate = new DateTime(1999, 10, 20), Genre = "Programming", Description = "Your Journey to Mastery", CoverImageUrl = "https://images-na.ssl-images-amazon.com/images/I/51a3ZQK9sQL.SX404_BO1,204,203,200.jpg", StockQuantity = 12, Rating = 4.7f, IsFeatured = false, UserID = 7 },
                new Book { BookID = 3, Title = "Refactoring", Author = "Martin Fowler", Price = 48.00m, ISBN = "978-0134757599", PublishDate = new DateTime(2018, 11, 20), Genre = "Software Development", Description = "Improving the Design of Existing Code", CoverImageUrl = "https://www.thoughtworks.com/insights/books/refactoring2", StockQuantity = 9, Rating = 4.6f, IsFeatured = true, UserID = 2 },
                new Book { BookID = 4, Title = "Design Patterns", Author = "Erich Gamma et al.", Price = 55.00m, ISBN = "978-0201633610", PublishDate = new DateTime(1994, 10, 31), Genre = "Software Engineering", Description = "Elements of Reusable Object-Oriented Software", CoverImageUrl = "https://images-na.ssl-images-amazon.com/images/I/41uPjEenkFL.SX379_BO1,204,203,200.jpg", StockQuantity = 6, Rating = 4.5f, IsFeatured = false, UserID = 5 },
                new Book { BookID = 5, Title = "Domain-Driven Design", Author = "Eric Evans", Price = 60.00m, ISBN = "978-0321125217", PublishDate = new DateTime(2003, 8, 30), Genre = "Architecture", Description = "Tackling Complexity in the Heart of Software", CoverImageUrl = "https://images-na.ssl-images-amazon.com/images/I/51uGf5E3QvL.jpg", StockQuantity = 7, Rating = 4.4f, IsFeatured = true, UserID = 8 },
                new Book { BookID = 6, Title = "Head First Design Patterns", Author = "Eric Freeman", Price = 40.00m, ISBN = "978-0596007126", PublishDate = new DateTime(2004, 10, 25), Genre = "Design", Description = "Brain‑friendly guide to design patterns", CoverImageUrl = "https://images-na.ssl-images-amazon.com/images/I/518FqJvR9aL.jpg", StockQuantity = 8, Rating = 4.7f, IsFeatured = false, UserID = 1 },
                new Book { BookID = 7, Title = "Code Complete", Author = "Steve McConnell", Price = 52.00m, ISBN = "978-0735619678", PublishDate = new DateTime(2004, 6, 9), Genre = "Programming", Description = "A practical handbook of software construction", CoverImageUrl = "https://images-na.ssl-images-amazon.com/images/I/41EwefVCNZL.jpg", StockQuantity = 11, Rating = 4.8f, IsFeatured = true, UserID = 4 },
                new Book { BookID = 8, Title = "Working Effectively with Legacy Code", Author = "Michael Feathers", Price = 47.00m, ISBN = "978-0131177055", PublishDate = new DateTime(2004, 9, 22), Genre = "Maintenance", Description = "Strategies for managing legacy systems", CoverImageUrl = "https://images-na.ssl-images-amazon.com/images/I/51YH3pAhSbL.SX404_BO1,204,203,200.jpg", StockQuantity = 5, Rating = 4.3f, IsFeatured = false, UserID = 6 },
                new Book { BookID = 9, Title = "Patterns of Enterprise Application Architecture", Author = "Martin Fowler", Price = 53.00m, ISBN = "978-0321127426", PublishDate = new DateTime(2002, 11, 15), Genre = "Architecture", Description = "Catalog of enterprise architectural patterns", CoverImageUrl = "https://images-na.ssl-images-amazon.com/images/I/41Ts4oWv6kL.jpg", StockQuantity = 6, Rating = 4.4f, IsFeatured = true, UserID = 3 },
                new Book { BookID = 10, Title = "Introduction to Algorithms", Author = "Cormen, Leiserson, Rivest, Stein", Price = 65.00m, ISBN = "978-0262033848", PublishDate = new DateTime(2009, 7, 31), Genre = "Algorithms", Description = "Comprehensive guide to modern algorithms", CoverImageUrl = "https://images-na.ssl-images-amazon.com/images/I/41oWgjDmsIL.SX379_BO1,204,203,200.jpg", StockQuantity = 10, Rating = 4.6f, IsFeatured = false, UserID = 7 },
                new Book { BookID = 11, Title = "Structure and Interpretation of Computer Programs", Author = "Harold Abelson & Gerald Jay Sussman", Price = 46.00m, ISBN = "978-0262510875", PublishDate = new DateTime(1996, 7, 25), Genre = "Computer Science", Description = "Classic text in computer programming", CoverImageUrl = "https://images-na.ssl-images-amazon.com/images/I/41fEeB0Ml0L.jpg", StockQuantity = 4, Rating = 4.2f, IsFeatured = true, UserID = 2 },
                new Book { BookID = 12, Title = "The Mythical Man-Month", Author = "Frederick P. Brooks Jr.", Price = 38.00m, ISBN = "978-0201835953", PublishDate = new DateTime(1995, 8, 12), Genre = "Project Management", Description = "Essays on software engineering", CoverImageUrl = "https://images-na.ssl-images-amazon.com/images/I/51qL4oX3Y3L.jpg", StockQuantity = 9, Rating = 4.1f, IsFeatured = false, UserID = 5 },
                new Book { BookID = 13, Title = "Continuous Delivery", Author = "Jez Humble", Price = 49.00m, ISBN = "978-0321601919", PublishDate = new DateTime(2010, 7, 27), Genre = "DevOps", Description = "Reliable software releases through automation", CoverImageUrl = "https://images-na.ssl-images-amazon.com/images/I/41lygW8gMsL.jpg", StockQuantity = 6, Rating = 4.5f, IsFeatured = false, UserID = 8 },
                new Book { BookID = 14, Title = "Soft Skills", Author = "John Sonmez", Price = 35.00m, ISBN = "978-1617292392", PublishDate = new DateTime(2014, 12, 28), Genre = "Career", Description = "Guide to a balanced software developer life", CoverImageUrl = "https://images-na.ssl-images-amazon.com/images/I/41+7Zh6PTmL.jpg", StockQuantity = 7, Rating = 4.3f, IsFeatured = true, UserID = 1 },
                new Book { BookID = 15, Title = "The C++ Programming Language", Author = "Bjarne Stroustrup", Price = 66.00m, ISBN = "978-0321958327", PublishDate = new DateTime(2013, 5, 19), Genre = "Programming", Description = "Definitive guide to C++ by its creator", CoverImageUrl = "https://images.thenile.io/r1000/9780321958327.jpg", StockQuantity = 10, Rating = 4.7f, IsFeatured = false, UserID = 4 }
            );


            // Seed Favorites
            modelBuilder.Entity<Favorite>().HasData(
                new Favorite { UserID = 1, BookID = 1 },
                new Favorite { UserID = 1, BookID = 2 },
                new Favorite { UserID = 2, BookID = 3 }
            );
        }
    }
}
