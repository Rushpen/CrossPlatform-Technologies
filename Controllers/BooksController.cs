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
                    Authors = b.Authors.Select(b => b.Name).ToList(),
                    isBorrow = b.User != null ? "Yes" : "No"
                }
                )
                .ToListAsync();

            return Ok(books);
        }

        // GET: /api/books/filter?isModern
        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<Book>>> GetFilteredBooks([FromQuery] string? bookType)
        {
            var books = await _context.Book
                .Include(b => b.Authors)
                .Include(b => b.User)
                .ToListAsync();

            if (!string.IsNullOrEmpty(bookType))
            {
                books = books.Where(b => b.GetBookType().ToString().Equals(bookType, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            var result = books.Select(b => new
            {
                Id = b.Id,
                Title = b.Title,
                Genre = b.Genre,
                PublishedYear = b.PublishedYear,
                Authors = b.Authors.Select(a => a.Name).ToList(),
                isBorrow = b.User != null ? "Yes" : "No"
            }).ToList();

            if (!result.Any())
            {
                return NotFound("No books match the specified criteria.");
            }

            return Ok(result);
        }

        [HttpGet("by-author/{id}")]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooksByAuthor(int id)
        {
            var books = await _context.Book
                .Where(b => b.Authors.Any(a => a.Id == id))
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
                    Authors = b.Authors.Select(b => b.Name).ToList(),
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
        public async Task<ActionResult<Book>> PostBook([FromBody] Book bookInput)
        {
            if (bookInput == null || string.IsNullOrEmpty(bookInput.Title) ||
                string.IsNullOrEmpty(bookInput.Genre) || bookInput.PublishedYear <= 0)
            {
                return BadRequest("Invalid book data. Please provide Title, Genre, and Published Year.");
            }
            var newBook = new Book
            {
                Title = bookInput.Title,
                Genre = bookInput.Genre,
                PublishedYear = bookInput.PublishedYear,
                Authors = new List<Author>()
            };

            _context.Book.Add(newBook);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBook), new { id = newBook.Id }, newBook);
        }

        // POST: api/Books/id/add-authors
        [HttpPost("{id}/add-authors")]
        public async Task<IActionResult> AddAuthorsToBook(int id, [FromBody] List<int> authorIds)
        {
            var book = await _context.Book
                .Include(b => b.Authors)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (book == null)
            {
                return NotFound($"Book with ID {id} not found.");
            }

            var authors = await _context.Author
                .Where(a => authorIds.Contains(a.Id))
                .ToListAsync();

            if (authors == null || authors.Count == 0)
            {
                return NotFound("No valid authors found with the provided IDs.");
            }

            foreach (var author in authors)
            {
                if (!book.Authors.Contains(author))
                {
                    book.Authors.Add(author);
                }
            }

            await _context.SaveChangesAsync();

            return Ok($"Authors successfully added to the book with ID {id}.");
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

        [HttpPut("{id}/update-authors")]
        public async Task<IActionResult> UpdateAuthorsForBook(int id, [FromBody] List<int> authorIds)
        {
            var book = await _context.Book
                .Include(b => b.Authors)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (book == null)
                return NotFound($"Book with ID {id} not found.");

            var authors = await _context.Author
                .Where(a => authorIds.Contains(a.Id))
                .ToListAsync();

            if (authors == null || authors.Count == 0)
                return NotFound("No valid authors found with the provided IDs.");

            book.Authors.Clear();
            book.Authors.AddRange(authors);

            await _context.SaveChangesAsync();

            return Ok($"Authors for the book with ID {id} were successfully updated.");
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
