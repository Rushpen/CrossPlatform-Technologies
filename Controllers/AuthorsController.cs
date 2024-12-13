using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Gadelshin_Lab1.Data;
using Gadelshin_Lab1.Models;
using Gadelshin_Lab1.Managers;
using Humanizer.Localisation;
using static System.Reflection.Metadata.BlobBuilder;

namespace Gadelshin_Lab1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly Gadelshin_Lab1Context _context;
        private readonly AuthorManager _authorManager;

        public AuthorsController(Gadelshin_Lab1Context context)
        {
            _context = context;
            _authorManager = new AuthorManager(context);
        }

        // GET: api/Authors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Author>>> GetAuthor()
        {
            var author = await _authorManager.GetAllAuthors();
            return Ok(author);
        }

        // GET: api/Authors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Author>> GetAuthor(int id)
        {
            var author = await _authorManager.GetAuthor(id);

            if (!author.Any())
                return NotFound();

            return Ok(author);
        }

        // GET: api/Authors/{id}/details
        [HttpGet("{id}/details")]
        public async Task<IActionResult> GetAuthorDetails(int id)
        {
            var details = await _authorManager.GetAuthorDetails(id);

            if (!details.Any())
                return NotFound();

            return Ok(details);
        }

        // POST: api/Authors
        [HttpPost]
        public async Task<ActionResult<Author>> PostAuthor(Author author)
        {
            var newAuthor = await _authorManager.PostAuthor(author);
            return CreatedAtAction("GetAuthor", new { id =newAuthor.Id }, newAuthor);
        }

        // POST: api/Authors/id/add-book
        [HttpPost("{id}/add-books")]
        public async Task<IActionResult> AddAuthorsToBook(int id, [FromBody] List<int> bookIds)
        {
            try
            {
                var message = await _authorManager.AddAuthorsToBook(id, bookIds);
                return Ok(message);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // PUT: api/Authors/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAuthor(int id, Author author)
        {
            try
            {
                author.Id = id;
                await _authorManager.UpdateAuthor(id, author);
                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // PUT: api/Authors/id/update-book
        [HttpPut("{id}/update-books")]
        public async Task<IActionResult> PutAuthorsToBook(int id, [FromBody] List<int> bookIds)
        {
            try
            {
                await _authorManager.UpdateAuthorsToBook(id, bookIds);
                return Ok($"Books updated for author with ID {id}.");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // DELETE: api/Authors/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            try
            {
                await _authorManager.DeleteAuthor(id);
                return Ok($"Author deleted with ID {id}.");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
