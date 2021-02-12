using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace WebApplication.Models
{
    public class BooksDataContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<AuthorBook> AuthorBooks { get; set; }

        public BooksDataContext(DbContextOptions<BooksDataContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AuthorBook>()
                         .HasKey(e => new { e.BookId, e.AuthorId });

            modelBuilder.Entity<AuthorBook>().HasOne(x => x.Book).WithMany(x => x.AuthorBooks).HasForeignKey(bc => bc.BookId);
            modelBuilder.Entity<AuthorBook>().HasOne(x => x.Author).WithMany(x => x.AuthorBooks).HasForeignKey(bc => bc.AuthorId);


            modelBuilder.Entity<Book>()
       .HasData(
         new Book { Id = 1, Name = "Dune" },
         new Book { Id = 2, Name = "Dune 2" },
         new Book { Id = 3, Name = "War and Peace" }
       );
            modelBuilder.Entity<Author>()
         .HasData(
           new Author { Id = 1, Name = "Frank Herbert" },
           new Author { Id = 2, Name = "Brian Herbert" },
           new Author { Id = 3, Name = "Lev Tolstoy" }
         );
            modelBuilder.Entity<AuthorBook>()
         .HasData(

            new AuthorBook { BookId = 1, AuthorId = 1 },
            new AuthorBook { BookId = 2, AuthorId = 1 },
            new AuthorBook { BookId = 2, AuthorId = 2 },
            new AuthorBook { BookId = 3, AuthorId = 3 }
         );
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
