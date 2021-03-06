﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.Description;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly BooksDataContext context;

        public AuthorsController(BooksDataContext _context)
        {
            this.context = _context;
        }

        // GET: api/Authors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuthorDto>>> GetAuthors()
        {
            return await context.Authors.Include(b => b.Books).Select(b => (AuthorDto)b).ToListAsync();
        }

        // GET: api/Authors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AuthorDto>> GetAuthor(int id)
        {
            var author = await context.Authors.Include(b => b.Books).FirstOrDefaultAsync(i => i.Id == id);

            if (author == null)
            {
                return NotFound();
            }

            return (AuthorDto)author;
        }

        [Route("{id:int}/bookscount")]
        [ResponseType(typeof(int))]
        public async Task<ActionResult> GetAuthorBookCount(int id)
        {
            var author = await context.Authors.Include(b => b.Books).FirstOrDefaultAsync(i => i.Id == id);
            var booksCount = author.Books.Count();

            if (author == null)
            {
                return NotFound();
            }

            return Ok(booksCount);
        }

        // PUT: api/Authors/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAuthor(int id, Author author)
        {
            if (id != author.Id)
            {
                return BadRequest();
            }

            try
            {
                var authorToChange = await context.Authors.FindAsync(id);
                authorToChange.Name = author.Name;
                authorToChange.Books = GetBooks(author);
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AuthorExists(id))
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

        // POST: api/Authors
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Author>> PostAuthor(Author author)
        {

            author.Books = GetBooks(author);
            context.Authors.Add(author);
            await context.SaveChangesAsync();

            return CreatedAtAction("GetAuthor", new { id = author.Id }, author);
        }

        // DELETE: api/Authors/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            var author = await context.Authors.FindAsync(id);
            if (author == null)
            {
                return NotFound();
            }

            context.Authors.Remove(author);
            await context.SaveChangesAsync();

            return NoContent();
        }

        private bool AuthorExists(int id)
        {
            return context.Authors.Any(e => e.Id == id);
        }

        private List<Book> GetBooks(Author author)
        {
            var books = (List<Book>)author.Books;
            for (int i = 0; i < books.Count; i++)
            {
                string name = books[i].Name;
                var bookWithSuchName = context.Books.FirstOrDefaultAsync(i => i.Name == name).Result;
                if (bookWithSuchName != null)
                {
                    books[i] = bookWithSuchName;
                }
            }
            return books;
        }
    }
}
