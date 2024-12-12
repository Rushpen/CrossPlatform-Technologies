using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Gadelshin_Lab1.Data;
using Gadelshin_Lab1.Models;
using Humanizer.Localisation;
using static System.Reflection.Metadata.BlobBuilder;

namespace Gadelshin_Lab1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly Gadelshin_Lab1Context _context;

        public AuthorsController(Gadelshin_Lab1Context context)
        {
            _context = context;
        }

        // GET: api/Authors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Author>>> GetAuthor()
        {
           var author =  await _context.Author
                .Select(a => new
                {
                    Id = a.Id,
                    Title = a.Name,
                    Genre = a.Biography,
                    PublishedYear = a.DateOfBirth,
                    Biography = a.Biography,
                    Books = a.Books.Select(b => b.Title).ToList()
                })
                .ToListAsync();
            return Ok(author);
        }

        // GET: api/Authors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Author>> GetAuthor(int id)
        {
            var author = await _context.Author
                .Where(a => a.Id == id)
                .Select(a => new
                {
                    Id = a.Id,
                    Title = a.Name,
                    Genre = a.Biography,
                    PublishedYear = a.DateOfBirth,
                    Biography = a.Biography,
                    Books = a.Books.Select(b => b.Title).ToList()
                }
                ).ToListAsync();

            if (author == null)
                return NotFound();

            return Ok(author);
        }

        // GET: api/Authors/{id}/details
        [HttpGet("{id}/details")]
        public async Task<IActionResult> GetAuthorDetails(int id)
        {
            var author = await _context.Author
                .Include(a => a.Books)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (author == null)
                return NotFound();

            return Ok(author.GetFullInfo());
        }

        // POST: api/Authors
        [HttpPost]
        public async Task<ActionResult<Author>> PostAuthor(Author author)
        {
            _context.Author.Add(author);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAuthor", new { id = author.Id }, author);
        }

        // POST: api/Authors/id/add-book
        [HttpPost("{id}/add-books")]
        public async Task<IActionResult> AddAuthorsToBook(int id, [FromBody] List<int> bookIds)
        {
            var author = await _context.Author
                .Include(b => b.Books)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (author == null)
                return NotFound($"Author with ID {id} not found.");

            var books = await _context.Book
                .Where(b => bookIds.Contains(b.Id))
                .ToListAsync();

            if (books == null || books.Count == 0)
                return NotFound("No valid books found with the provided IDs.");

            foreach (var book in books)
            {
                if (!author.Books.Contains(book))
                    author.Books.Add(book);
            }

            await _context.SaveChangesAsync();

            return Ok($"Books successfully added to the book with ID {id}.");
        }

        // PUT: api/Authors/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAuthor(int id, Author author)
        {
            if (id != author.Id)
                return BadRequest();

            _context.Entry(author).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
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

        // PUT: api/Authors/id/update-book
        [HttpPut("{id}/update-books")]
        public async Task<IActionResult> PutAuthorsToBook(int id, [FromBody] List<int> bookIds)
        {
            var author = await _context.Author
                .Include(b => b.Books)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (author == null)
                return NotFound($"Author with ID {id} not found.");

            var books = await _context.Book
                .Where(b => bookIds.Contains(b.Id))
                .ToListAsync();

            if (books == null || books.Count == 0)
                return NotFound("No valid books found with the provided IDs.");

            author.Books.Clear();
            author.Books.AddRange(books);

            await _context.SaveChangesAsync();

            return Ok($"Books for the author with ID {id} were successfully updated.");
        }

        // DELETE: api/Authors/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            var author = await _context.Author.FindAsync(id);
            if (author == null)
            {
                return NotFound();
            }

            _context.Author.Remove(author);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AuthorExists(int id)
        {
            return _context.Author.Any(e => e.Id == id);
        }
    }
}
