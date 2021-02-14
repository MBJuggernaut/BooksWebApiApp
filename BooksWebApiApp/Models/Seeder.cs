using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace WebApplication.Models
{
    public class Seeder
    {
        private BooksDataContext context;

        public Seeder(BooksDataContext context)
        {
            this.context = context;
        }

        public void Seed()
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            if (!context.Books.Any() && !context.Authors.Any())
            {
                Author author1 = new Author { Name = "Frank Herbert" };
                Author author2 = new Author { Name = "Brian Herbert" };
                Author author3 = new Author { Name = "Lev Tolstoy" };
                Book book1 = new Book { Name = "Dune", Authors = new List<Author>() { author1 } };
                Book book2 = new Book { Name = "Dune 2", Authors = new List<Author>() { author1, author2 } };
                Book book3 = new Book { Name = "War and Peace", Authors = new List<Author>() { author3 } };
                context.Authors.Add(author1);
                context.Authors.Add(author2);
                context.Authors.Add(author3);
                context.Books.Add(book1);
                context.Books.Add(book2);
                context.Books.Add(book3);

                context.SaveChanges();
            }
        }
    }
}
