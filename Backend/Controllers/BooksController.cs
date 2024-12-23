using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Gadelshin_Lab1.Data;
using Gadelshin_Lab1.Models;
using static System.Reflection.Metadata.BlobBuilder;
using Gadelshin_Lab1.Managers;
using Microsoft.AspNetCore.Authorization;

namespace Gadelshin_Lab1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly Gadelshin_Lab1Context _context;
        private readonly BookManager _bookManager;

        public BooksController(Gadelshin_Lab1Context context)
        {
            _context = context;
            _bookManager = new BookManager(context);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetBook()
        {
            var books = await _bookManager.GetBook();
            return Ok(books);
        }

        // GET: /api/books/filter?isModern
        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<Book>>> GetFilteredBooks([FromQuery] string? bookType)
        {
            try
            {
                var books = await _bookManager.GetFilteredBooks(bookType);

                if (!books.Any())
                    return NotFound("No books match the specified criteria.");

                return Ok(books);
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("by-author/{id}")]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooksByAuthor(int id)
        {
            var books = await _bookManager.GetBooksByAuthor(id);

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
            var book = await _bookManager.GetBook(id);
            if (book == null)
                return NotFound();
            return Ok(book);
        }

        // POST: api/Books
        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<ActionResult<Book>> PostBook([FromBody] Book bookInput)
        {
            var newBook = await _bookManager.PostBook(bookInput);
            _context.Book.Add(newBook);
            return CreatedAtAction(nameof(GetBook), new { id = newBook.Id }, newBook);
        }

        // POST: api/Books/id/add-authors
        [Authorize(Roles = "admin")]
        [HttpPost("{id}/add-authors")]
        public async Task<IActionResult> AddAuthorsToBook(int id, [FromBody] List<int> authorIds)
        {
            try
            {
                var message = await _bookManager.AddAuthorsToBook(id, authorIds);
                return Ok(message);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // PUT: /api/books/{id}
        [Authorize(Roles = "admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBook(int id, Book book)
        {
            try
            {
                book.Id = id;
                await _bookManager.UpdateBook(id, book);
                return Ok($"Book with ID {id} was updated");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize(Roles = "admin")]
        [HttpPut("{id}/update-authors")]
        public async Task<IActionResult> UpdateAuthorsForBook(int id, [FromBody] List<int> authorIds)
        {
            try
            {
                await _bookManager.UpdateAuthorsForBook(id, authorIds);
                return Ok($"Author updated for book with ID {id}.");
            }

            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        // DELETE: api/Books/5
        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            try
            {
                await _bookManager.DeleteBook(id);
                return Ok($"Book deleted with ID {id}.");
            }

            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    }
}
