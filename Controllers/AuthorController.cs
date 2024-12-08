using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Gadelshin_Lab1.Models;

namespace Gadelshin_Lab1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private static List<Author> authors = new List<Author>();

        // GET: /api/authors
        [HttpGet]
        public IEnumerable<Author> Get()
        {
            return authors;
        }

        // GET: /api/authors/{id}/details
        [HttpGet("{id}/details")]
        public IActionResult GetAuthorDetails(int id)
        {
            var author = authors.FirstOrDefault(a => a.Id == id);
            if (author == null) return NotFound();

            return Ok(author.GetFullInfo());
        }

        // GET: /api/authors/most-books
        [HttpGet("most-books")]
        public IActionResult GetAuthorWithMostBooks()
        {
            var author = authors.OrderByDescending(a => a.GetBookCount()).FirstOrDefault();
            if (author == null) return NotFound();

            return Ok(author);
        }

        // POST: /api/authors
        [HttpPost]
        public IActionResult Create([FromBody] Author author)
        {
            authors.Add(author);
            return CreatedAtAction(nameof(Get), new { id = author.Id }, author);
        }

        // PUT: /api/authors/{id}
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Author author)
        {
            var existingAuthor = authors.FirstOrDefault(a => a.Id == id);
            if (existingAuthor == null) return NotFound();

            existingAuthor.Name = author.Name;
            existingAuthor.DateOfBirth = author.DateOfBirth;
            existingAuthor.Biography = author.Biography;
            existingAuthor.Books = author.Books;

            return NoContent();
        }

        // DELETE: /api/authors/{id}
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var author = authors.FirstOrDefault(a => a.Id == id);
            if (author == null) return NotFound();

            authors.Remove(author);
            return NoContent();
        }
    }
}
