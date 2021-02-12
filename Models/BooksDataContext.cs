using Microsoft.EntityFrameworkCore;

namespace WebApplication.Models
{
    public class BooksDataContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }

        public BooksDataContext(DbContextOptions<BooksDataContext> options)
            : base(options)
        {

        }
    }
}
