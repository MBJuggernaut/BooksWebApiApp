using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly BooksDataContext context;

        public BooksController(BooksDataContext _context)
        {
            context = _context;
        }

        // GET: api/Books
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookDto>>> GetBooks()
        {
            return await context.Books.Include(b => b.Authors).Select(b => (BookDto)b).ToListAsync();
        }

        // GET: api/Books/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BookDto>> GetBook(int id)
        {
            var book = await context.Books.Include(b => b.Authors).FirstOrDefaultAsync(i => i.Id == id);

            if (book == null)
            {
                return NotFound();
            }

            return (BookDto)book;
        }

        // PUT: api/Books/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBook(int id, Book book)
        {
            if (id != book.Id)
            {
                return BadRequest();
            }

            try
            {
                var bookToChange = await context.Books.FindAsync(id);
                bookToChange.Name = book.Name;
                bookToChange.Authors = book.Authors;
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Books
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Book>> PostBook(Book book)
        {
            var authors = (List<Author>)book.Authors;
            for (int i = 0; i < authors.Count; i++)
            {
                if (context.Authors.Any(e => e.Name == authors[i].Name))
                {
                    string name = authors[i].Name;
                    authors[i] = context.Authors.FirstOrDefaultAsync(i => i.Name == name).Result;                   
                }
            }
            book.Authors = authors;


            context.Books.Add(book);
            await context.SaveChangesAsync();

            return CreatedAtAction("GetBook", new { id = book.Id }, book);
        }

        // DELETE: api/Books/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            context.Books.Remove(book);
            await context.SaveChangesAsync();

            return NoContent();
        }

        private bool BookExists(int id)
        {
            return context.Books.Any(e => e.Id == id);
        }
    }
}
