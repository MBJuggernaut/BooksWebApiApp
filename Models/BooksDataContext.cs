using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace WebApplication.Models
{
    public class BooksDataContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }

        public BooksDataContext(DbContextOptions<BooksDataContext> options)
            : base(options)
        {
            Database.EnsureCreated();

            if (!Books.Any() && !Authors.Any())
                Seed();
        }

        private void Seed()
        {
            Author author1 = new Author { Name = "Frank Herbert" };
            Author author2 = new Author { Name = "Brian Herbert" };
            Author author3 = new Author { Name = "Lev Tolstoy" };
            Book book1 = new Book { Name = "Dune", Authors = new List<Author>() { author1 } };
            Book book2 = new Book { Name = "Dune 2", Authors = new List<Author>() { author1, author2 } };
            Book book3 = new Book { Name = "War and Peace", Authors = new List<Author>() { author3 } };
            Authors.Add(author1);
            Authors.Add(author2);
            Authors.Add(author3);
            Books.Add(book1);
            Books.Add(book2);
            Books.Add(book3);

            SaveChanges();
        }
    }
}
