using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Gadelshin_Lab1.Data;
using Gadelshin_Lab1.Models;
using static System.Reflection.Metadata.BlobBuilder;

namespace Gadelshin_Lab1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly Gadelshin_Lab1Context _context;

        public BooksController(Gadelshin_Lab1Context context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetBook()
        {
            var books = await _context.Book
                .Select(b => new
                {
                    Id = b.Id,
                    Title = b.Title,
                    Genre = b.Genre,
                    PublishedYear = b.PublishedYear,
                    AuthorName = b.Author != null ? b.Author.Name : "Unknown",
                    isBorrow = b.User != null ? "Yes" : "No"
                }
                )
                .ToListAsync();

            return Ok(books);
        }

        // GET: /api/books/filter?isModern=true
        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<Book>>> GetFilteredBooks([FromQuery] bool? isModern)
        {
            var books = await _context.Book.ToListAsync();

            if (isModern.HasValue)
            {
                if (isModern.Value)
                {
                    books = books.Where(b => b.IsModern()).ToList();
                }
                else
                {
                    books = books.Where(b => b.IsClassic()).ToList();
                }
            }
            if (!books.Any())
            {
                return NotFound("No books match the specified criteria.");
            }
            return Ok(books);
        }

        [HttpGet("by-author/{id}")]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooksByAuthor(int id)
        {
            var books = await _context.Book
                .Where(b => b.AuthorId == id)
                .Select(b => new
                {
                    Id = b.Id,
                    Title = b.Title,
                    Genre = b.Genre,
                    PublishedYear = b.PublishedYear
                }
                ).ToListAsync();

            if (books == null || books.Count == 0)
            {
                return NotFound($"Books by author '{id}' not found.");
            }

            return Ok(books);
        }

        // GET: api/Books/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBook(int id)
        {
            var book = await _context.Book
                .Where(b=>b.Id == id)
                .Select(b => new
                {
                    Id = b.Id,
                    Title = b.Title,
                    Genre = b.Genre,
                    PublishedYear = b.PublishedYear,
                    AuthorName = b.Author != null ? b.Author.Name : "Unknown"
                }
                ).ToListAsync();

            if (book == null)
            {
                return NotFound();
            }

            return Ok(book);
        }

        // POST: api/Books
        [HttpPost]
        public async Task<ActionResult<Book>> PostBook(Book book)
        {
            _context.Book.Add(book);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBook", new { id = book.Id }, book);
        }

        // PUT: /api/books/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBook(int id, Book book)
        {
            if (id != book.Id)
            {
                return BadRequest();
            }

            _context.Entry(book).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
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

        // DELETE: api/Books/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _context.Book.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            _context.Book.Remove(book);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BookExists(int id)
        {
            return _context.Book.Any(e => e.Id == id);
        }
    }
}
