using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Gadelshin_Lab1.Models;

namespace Gadelshin_Lab1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private static List<Book> books = new List<Book>();

        //GET
        [HttpGet]
        public IEnumerable<Book> Get()
        {
            return books;
        }

        // GET: /api/books/available
        [HttpGet("available")]
        public IEnumerable<Book> GetAvailableBooks()
        {
            return books.Where(b => b.IsAvailable());
        }

        // GET: /api/books/by-author/{authorId}
        [HttpGet("by-author/{authorId}")]
        public IEnumerable<Book> GetBooksByAuthor(int authorId)
        {
            return books.Where(b => b.AuthorId == authorId);
        }

        // POST: /api/books
        [HttpPost]
        public IActionResult Create([FromBody] Book book)
        {
            books.Add(book);
            return CreatedAtAction(nameof(Get), new { id = book.Id }, book);
        }

        // PUT: /api/books/{id}
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Book book)
        {
            var existingBook = books.FirstOrDefault(b => b.Id == id);
            if (existingBook == null) return NotFound();

            existingBook.Title = book.Title;
            existingBook.Genre = book.Genre;
            existingBook.PublishedYear = book.PublishedYear;
            existingBook.AuthorId = book.AuthorId;

            return NoContent();
        }

        // DELETE: /api/books/{id}
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var book = books.FirstOrDefault(b => b.Id == id);
            if (book == null) return NotFound();

            books.Remove(book);
            return NoContent();
        }
    }
}
